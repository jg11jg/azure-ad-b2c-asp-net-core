using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using TestApp;
using TestApp.Infrastructure;
using AuthenticationFailedContext = Microsoft.AspNetCore.Authentication.JwtBearer.AuthenticationFailedContext;
using MessageReceivedContext = Microsoft.AspNetCore.Authentication.JwtBearer.MessageReceivedContext;
using TokenValidatedContext = Microsoft.AspNetCore.Authentication.JwtBearer.TokenValidatedContext;

namespace TestService
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<B2CAuthenticationOptions>(configuration.GetSection("Authentication:AzureAd"));
            services.Configure<B2CPolicies>(configuration.GetSection("Authentication:AzureAd:B2C"));

            //services.Configure<TestServiceOptions>(configuration.GetSection("TestServiceOptions"));
            //services.AddTransient<TestServiceProxy>();

            services.AddMvc(options => options.Filters.Add(typeof(ReauthenticationRequiredFilter)));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();

            ConfigureAuthentication(services);

            var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
//                .RequireAssertion((Func<AuthorizationHandlerContext, bool>) Handler1)
                //.RequireClaim("postalCode", "e143")
//                .RequireClaim("scp", "read_values")
                .RequireClaim("http://schemas.microsoft.com/identity/claims/scope",  "read_values" )
                //    .RequireAssertion((Func<AuthorizationHandlerContext, bool>)Handler2)
               .Build()
                ;

            services.AddAuthorization(options =>
                {
                    options.AddPolicy("ReadValuesPolicy", policy); 
                }


            );

            //options.AddPolicy("ReadValuesPolicy", config => config.RequireClaim("http://schemas.microsoft.com/identity/claims/scope", new[] { "read_values" })));


        }

        private bool Handler1(AuthorizationHandlerContext authorizationHandlerContext)
        {
            Console.WriteLine(authorizationHandlerContext);
            return true;
        }

        private bool Handler2(AuthorizationHandlerContext authorizationHandlerContext)
        { 
            Console.WriteLine(authorizationHandlerContext);
            return true;
        }

        private void WhatsGoingOnHere(AuthorizationPolicyBuilder config)
        {
            config.RequireClaim("http://schemas.microsoft.com/identity/claims/scope", new[] { "read_values" });
            //config.RequireClaim("scp", new[] {"read_values"});
        }

        private static void ConfigureAuthentication(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();


            var authOptions = serviceProvider.GetService<IOptions<B2CAuthenticationOptions>>();
            var b2cPolicies = serviceProvider.GetService<IOptions<B2CPolicies>>();

            var distributedCache = serviceProvider.GetService<IDistributedCache>();
            services.AddSingleton(distributedCache);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    //options.DefaultScheme = Constants.OpenIdConnectAuthenticationScheme;
                    options.DefaultChallengeScheme = Constants.OpenIdConnectAuthenticationScheme;
                   // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddCookie()
                .AddOpenIdConnect(Constants.OpenIdConnectAuthenticationScheme, options =>
                {
                    options.Authority = authOptions.Value.Authority;
                    options.ClientId = authOptions.Value.ClientId;
                    options.ClientSecret = authOptions.Value.ClientSecret;
                    options.SignedOutRedirectUri = authOptions.Value.PostLogoutRedirectUri;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.ConfigurationManager = new PolicyConfigurationManager(authOptions.Value.Authority,
                        new[]
                        {
                            b2cPolicies.Value.SignInOrSignUpPolicy, b2cPolicies.Value.EditProfilePolicy,
                            b2cPolicies.Value.ResetPasswordPolicy
                        });

                    options.Events =
                        CreateOpenIdConnectEventHandlers(authOptions.Value, b2cPolicies.Value, distributedCache);

                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name"
                    };

                    // it will fall back on using DefaultSignInScheme if not set
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    // we have to set these scope that will be used in /authorize request
                    // (otherwise the /token request will not return access and refresh tokens)
                    options.Scope.Add("offline_access");
                    options.Scope.Add($"{authOptions.Value.ApiIdentifier}/read_values");

                    // this can be used if the middleware redeems the authorization code
                    //options.SaveTokens = true;

                })
                .AddJwtBearer(options =>
                {
                    options.MetadataAddress =
                        $"{authOptions.Value.Authority}/.well-known/openid-configuration?p={"B2C_1_SiUpIn"}";
                    options.Audience = authOptions.Value.ClientId;
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = OnAuthenticationFailed,
                        OnChallenge = OnChallenge,
                        OnMessageReceived = OnMessageReceived,
                        OnTokenValidated = OnTokenValidated
                    };
                });

        }

        private static Task OnTokenValidated(TokenValidatedContext arg)
        {
            Console.WriteLine(arg);
            return Task.FromResult(0);
        }

        private static Task OnMessageReceived(MessageReceivedContext arg)
        {
            Console.WriteLine(arg);
            return Task.FromResult(0);
        }

        private static Task OnChallenge(JwtBearerChallengeContext arg)
        {
            Console.WriteLine(arg);
            return Task.FromResult(0);
        }

        private static Task OnAuthenticationFailed(AuthenticationFailedContext arg)
        {
            Console.WriteLine(arg);
            return Task.FromResult(0);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        private static OpenIdConnectEvents CreateOpenIdConnectEventHandlers(B2CAuthenticationOptions authOptions, B2CPolicies policies, IDistributedCache distributedCache)
        {
            return new OpenIdConnectEvents
            {
                OnRedirectToIdentityProvider = context => SetIssuerAddressAsync(context, policies.SignInOrSignUpPolicy),
                OnRedirectToIdentityProviderForSignOut = context => SetIssuerAddressForSignOutAsync(context, policies.SignInOrSignUpPolicy),
                OnTokenValidated = OnTokenValidated,
                OnUserInformationReceived = OnUserInformationReceived,
            OnAuthorizationCodeReceived = async context =>
                {
                    try
                    {
                        var principal = context.Principal;

                        var policy = principal.FindFirst(Constants.AcrClaimType).Value;
                        string authority = authOptions.GetAuthority(policy);

                        var userTokenCache = new DistributedTokenCache(distributedCache, principal.FindFirst(Constants.ObjectIdClaimType).Value).GetMSALCache();
                        var client = new ConfidentialClientApplication(authOptions.ClientId,
                            authority,
                            "https://app", // it's not really needed
                            new ClientCredential(authOptions.ClientSecret),
                            userTokenCache,
                            null);

                        var result = await client.AcquireTokenByAuthorizationCodeAsync(context.TokenEndpointRequest.Code,
                            new[] { $"{authOptions.ApiIdentifier}/read_values" });

                        context.HandleCodeRedemption(result.AccessToken, result.IdToken);
                    }
                    catch (Exception ex)
                    {
                        context.Fail(ex);
                    }
                },
                OnAuthenticationFailed = context =>
                {
                    context.Fail(context.Exception);
                    return Task.FromResult(0);
                },
                OnMessageReceived = context =>
                {
                    if (!string.IsNullOrEmpty(context.ProtocolMessage.Error) &&
                        !string.IsNullOrEmpty(context.ProtocolMessage.ErrorDescription))
                    {
                        if (context.ProtocolMessage.ErrorDescription.StartsWith("AADB2C90091")) // cancel profile editing
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/");
                        }
                        else if (context.ProtocolMessage.ErrorDescription.StartsWith("AADB2C90118")) // forgot password
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Account/ResetPassword");
                        }
                    }

                    return Task.FromResult(0);
                }
            };
        }

        private static Task OnUserInformationReceived(UserInformationReceivedContext arg)
        {
            Console.WriteLine(arg);
            return Task.FromResult(0);
        }

        private static Task OnTokenValidated(Microsoft.AspNetCore.Authentication.OpenIdConnect.TokenValidatedContext arg)
        {
            Console.WriteLine(arg);
            Console.WriteLine("ok so we have authenticated user - now ensure they have profile in database.");
            return Task.FromResult(0);
        }

        private static async Task SetIssuerAddressAsync(RedirectContext context, string defaultPolicy)
        {
            var configuration = await GetOpenIdConnectConfigurationAsync(context, defaultPolicy);
            context.ProtocolMessage.IssuerAddress = configuration.AuthorizationEndpoint;
        }

        private static async Task SetIssuerAddressForSignOutAsync(RedirectContext context, string defaultPolicy)
        {
            var configuration = await GetOpenIdConnectConfigurationAsync(context, defaultPolicy);
            context.ProtocolMessage.IssuerAddress = configuration.EndSessionEndpoint;
        }

        private static Task<OpenIdConnectConfiguration> GetOpenIdConnectConfigurationAsync(RedirectContext context, string defaultPolicy)
        {
            var manager = (PolicyConfigurationManager)context.Options.ConfigurationManager;
            var policy = context.Properties.Items.ContainsKey(Constants.B2CPolicy) ? context.Properties.Items[Constants.B2CPolicy] : defaultPolicy;

            return manager.GetConfigurationByPolicyAsync(CancellationToken.None, policy);
        }
    }
}

//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using TestApp;

//namespace TestService
//{
//    public class Startup
//    {
//        private readonly IConfiguration configuration;

//        public Startup(IConfiguration configuration)
//        {
//            this.configuration = configuration;
//        }
        
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication:AzureAd"));

//            var serviceProvider = services.BuildServiceProvider();
//            var authOptions = serviceProvider.GetService<IOptions<AuthenticationOptions>>();
            
//            services.AddAuthentication(options =>
//            {
//                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
////                options.DefaultChallengeScheme = Constants.OpenIdConnectAuthenticationScheme;
//            }) // sets both authenticate and challenge default schemes
//                .AddJwtBearer(options =>
//                {
//                    options.MetadataAddress = $"{authOptions.Value.Authority}/.well-known/openid-configuration?p={authOptions.Value.SignInOrSignUpPolicy}";
//                    options.Audience = authOptions.Value.Audience;
//                    options.Events = new JwtBearerEvents
//                    {
//                        OnAuthenticationFailed = OnAuthenticationFailed,
//                        OnChallenge = OnChallenge,
//                        OnMessageReceived = OnMessageReceived,
//                        OnTokenValidated = OnTokenValidated
//                    };
//                });

//            services.AddMvc();

//            services.AddAuthorization(options =>
//                options.AddPolicy("ReadValuesPolicy", config => config.RequireClaim("http://schemas.microsoft.com/identity/claims/scope", new[] { "read_values" })));
//        }

//        private Task OnTokenValidated(TokenValidatedContext arg)
//        {
//            Console.WriteLine(arg);
//            return Task.FromResult(0);
//        }

//        private Task OnMessageReceived(MessageReceivedContext arg)
//        {
//            Console.WriteLine(arg);
//            return Task.FromResult(0);
//        }

//        private Task OnChallenge(JwtBearerChallengeContext arg)
//        {
//            Console.WriteLine(arg);
//            return Task.FromResult(0);
//        }

//        private Task OnAuthenticationFailed(AuthenticationFailedContext arg)
//        {
//            Console.WriteLine(arg);
//            return Task.FromResult(0);
//        }

//        public void Configure(IApplicationBuilder app)
//        {
//            app.UseAuthentication();
//            app.UseMvc();
//        }
//    }
//}

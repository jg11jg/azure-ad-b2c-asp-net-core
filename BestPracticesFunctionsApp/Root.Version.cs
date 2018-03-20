using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace BestPracticeFunctionApp
{
    public static class Version
    {
        [FunctionName("Version")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, TraceWriter log)
        {
            var dic = new Dictionary<string, string>();

            var directoryInfo =
                new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            var buildFactsDir = Path.Combine(directoryInfo.Parent.FullName, "BuildFacts");

            var buildDateStr = "Unknown";

            try
            {
                using (var stream = File.Open(Path.Combine(buildFactsDir, "BuildDate.txt"), FileMode.Open))
                {

                    var contents = new StreamReader(stream).ReadToEnd().Trim();
                    var buildDate = DateTime.Parse(contents);
                    buildDateStr = buildDate.ToString("dd-MMM-yyyy HH:mm:ss");
                }
            }
            catch
            {
                //
            }

            dic.Add("Build Time (UTC)", buildDateStr);

            var buildNumber = "Unknown";

            try
            {
                using (var stream = File.Open(Path.Combine(buildFactsDir, "VstsBuildNumber.txt"), FileMode.Open))
                {
                    buildNumber = new StreamReader(stream).ReadToEnd().Trim();
                }
            }
            catch
            {
                //
            }

            dic.Add("Build Number", buildNumber);

            var commitHash = "Unknown";

            try
            {
                using (var stream = File.Open(Path.Combine(buildFactsDir, "CommitHash.txt"), FileMode.Open))
                {
                    commitHash = new StreamReader(stream).ReadToEnd().Trim();
                }
            }
            catch
            {
                //
            }

            dic.Add("Commit Hash", commitHash);

            return new OkObjectResult(dic);
        }
    }
}
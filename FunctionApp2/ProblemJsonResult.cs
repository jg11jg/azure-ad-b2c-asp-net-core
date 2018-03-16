using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CalorieCruncher
{
    public class ProblemJsonResult : JsonResult
    {
        public ProblemJsonResult(int statusCode, string type, string title, string detail, string instance)
            : base(new Dictionary<string, string>()
            {
                {"type", type},
                {"title", title},
                {"detail", detail},
                {"instance", instance}
            })
        {
            this.StatusCode = statusCode;
        }

        public Dictionary<string, string> Dic => this.Value as Dictionary<string, string>;
    }
}

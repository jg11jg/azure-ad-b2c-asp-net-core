using System;
using System.Collections.Generic;
using System.Text;
using CalorieCruncher;

namespace CalCrunch.Utils
{
    public class ProblemJsonException : Exception
    {
        public readonly ProblemJsonResult ProblemJsonResult;
        public ProblemJsonException(int statusCode, string type, string title, string detail, string instance)
        {
            ProblemJsonResult = new ProblemJsonResult(statusCode, type, title, detail, instance);
        }
    }
}

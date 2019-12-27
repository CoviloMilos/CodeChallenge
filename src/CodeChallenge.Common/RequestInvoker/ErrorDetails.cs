using System;
using Newtonsoft.Json;

namespace CodeChallenge.Common.RequestInvoker
{
    public class ErrorDetails
    {
        public string Origin { get; }
        public string Type { get; }
        public string Code { get; }
        public string Detail { get; }
        public string StatusCode { get; }
        public DateTime ExceptionDate { get; set; }

        public ErrorDetails (string origin, string type, string code, string detail, string statusCode) {
            Origin = origin;
            Type = type;
            Code = code;
            Detail = detail;
            StatusCode = statusCode;
            ExceptionDate = DateTime.Now;
        }

        public override string ToString () {
            return JsonConvert.SerializeObject (this);
        }
    }
}
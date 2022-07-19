using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class AppException
    {
        public AppException(int statusCode, string mesasge, string details = null)
        {
            StatusCode = statusCode;
            Mesasge = mesasge;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Mesasge { get; set; }
        public string Details { get; set; }
    }
}

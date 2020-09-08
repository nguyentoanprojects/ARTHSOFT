using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARTHSOFT.Models
{
    public class Result
    {
        public bool Status { get; set; }
        public String Error { get; set; }

        public Result()
        {
            Status = true;
            Error = String.Empty;
        }
    }
}
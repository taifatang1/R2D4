using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace R2D4Web.Clients.Models
{
    public class ExecutionModel
    {
        public bool IsValid { get; set; } = false;
        public List<string> ErrorMessages { get; set; }

        public ExecutionModel()
        {
            IsValid = false;
            ErrorMessages = new List<string>();
        }
    }
}
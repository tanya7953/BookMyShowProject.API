using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookMyShowProject.Contract
{
    public class APIResponse<T> where T : class
    {
        public T data { get; set; }
        public Error Error { get; set; }
        public HttpStatusCode Status { get; set; }
    }
    public class Error
    {
        public string errorMessage { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CoreCommon.Model
{
    public class ResponseApi<T>
    {
        public HttpStatusCode code { get; set; }


        public T data { get; set; }
        public string message { get; set; }

    }
}

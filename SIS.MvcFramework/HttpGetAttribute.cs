using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP;

namespace SIS.MvcFramework
{
    
    public class HttpGetAttribute : HttpMethodAttribute
    {
        public HttpGetAttribute()
        {

        }
        public HttpGetAttribute(string url)

            : base(url)
        {
            
        }

        public override HttpMethodType Type => HttpMethodType.Get;
    }
}

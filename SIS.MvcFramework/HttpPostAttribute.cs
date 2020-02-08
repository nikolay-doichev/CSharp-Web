using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP;

namespace SIS.MvcFramework
{
    public class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute()
        {

        }

        public HttpPostAttribute(string url)

            : base(url)
        {
        }

        public override HttpMethodType Type => HttpMethodType.Post;
    }
}

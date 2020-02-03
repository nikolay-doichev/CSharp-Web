using SIS.HTTP;
using SIS.HTTP.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace SulsApp.Controllers
{
    public class HomeController
    {
        public HttpResponse Index(HttpRequest request)
        {
            return new HtmlResponse("<h1>Hello!</h1>");
        }
    }
}

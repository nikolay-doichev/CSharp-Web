﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Response
{
    public class HtmlResponse : HttpResponse
    {
        public HtmlResponse(string html)
        {
            this.StatusCode = HttpResponseCode.Ok;
            byte[] byteData = Encoding.UTF8.GetBytes(html);
            this.Body = byteData;
            this.Headers.Add(new Header("Content-Type", "text/html"));
            this.Headers.Add(new Header("Content-Lenght", this.Body.Length.ToString()));
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using SIS.HTTP;
using SIS.MvcFramework;
using SIS.HTTP.Response;

namespace DemoApp
{
    public class StartUp : IMvcApplication
    {
        public void Configure(IList<Route> routeTable)
        {

            routeTable.Add(new Route(HttpMethodType.Get, "/", Index));
            routeTable.Add(new Route(HttpMethodType.Post, "/Tweets/Create", CreateTweet));
            routeTable.Add(new Route(HttpMethodType.Get, "/favicon.ico", FavIcon));
        }

        public void ConfigureServices()
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();
        }


        private static HttpResponse FavIcon(HttpRequest request)
        {
            var byteContent = File.ReadAllBytes("wwwroot/favicon.ico");
            return new FileResponse(byteContent, "image/x-icon");
        }
        public static HttpResponse Index(HttpRequest request)
        {
            var username = request.SessionData.ContainsKey("Username") ? request.SessionData["Username"] : "Anonymous";


            var db = new ApplicationDbContext();
            var tweets = db.Tweets.Select(x => new
            {
                x.CreateOn,
                x.Creator,
                x.Content
            }).ToList();

            StringBuilder html = new StringBuilder();
            html.Append("<table><tr>Date<th></th><th>Creator</th><th>Content</th></tr>");
            foreach (var tweet in tweets)
            {
                html.Append($"<tr><td>{tweet.CreateOn}</td><td>{tweet.Creator}</td><td>{tweet.Content}</td></tr>");
            }
            html.Append("</table>");
            html.Append($"<form action='/Tweets/Create' method='post'><input name='creator' /> <br /><textarea name='tweetName'></textarea><br /><input type='submit' /></form>");

            return new HtmlResponse(html.ToString());
        }

        public static HttpResponse CreateTweet(HttpRequest request)
        {
            var db = new ApplicationDbContext();
            db.Tweets.Add(new Tweet
            {
                CreateOn = DateTime.UtcNow,
                Creator = request.FormData["creator"],
                Content = request.FormData["tweetName"]
            });

            db.SaveChanges();

            return new RedirectResponse("/");
        }
    }
}

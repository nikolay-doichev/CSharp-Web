namespace SharedTrip.App.Controllers
{
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            return this.View();
        }
        [HttpGet("/Home/Index")]
        public HttpResponse IndexFullPage()
        {
            return this.Index();
        }
    }
}
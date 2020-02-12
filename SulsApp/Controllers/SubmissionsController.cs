using SIS.HTTP;
using SIS.MvcFramework;
using SulsApp.ViewModels.Submissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SulsApp.Controllers
{
    public class SubmissionsController : Controller
    {
        private readonly ApplicationDbContext db;

        public SubmissionsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public HttpResponse Create(string id)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var problem = this.db.Problems
                .Where(x => x.Id == id)
                .Select(x => new CreateFormViewModel
                {
                    Name = x.Name,
                    ProblemId = x.Id,
                }).FirstOrDefault();
            if (problem == null)
            {
                return this.Error("Problem not found!");
            }

            return this.View(problem);
        }
    }
}

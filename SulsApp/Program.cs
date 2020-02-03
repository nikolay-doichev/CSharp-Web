using SIS.HTTP;
using SIS.MvcFramework;
using SulsApp.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SulsApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           await  WebHost.StartAsync(new StartUp());
        }
    }
}

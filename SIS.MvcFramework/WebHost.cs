using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using SIS.HTTP;
using SIS.HTTP.Logging;
using SIS.HTTP.Response;

namespace SIS.MvcFramework
{
    public static class WebHost
    {
        public static async Task StartAsync(IMvcApplication application)
        {
            var routeTable = new List<Route>();
            IserviceCollection serviceCollection = new ServiceCollection();

            serviceCollection.Add<ILogger, ConsoleLogger>();


            application.ConfigureServices(serviceCollection);
            application.Configure(routeTable);
            AutoRegisterStaticFilesRoute(routeTable);
            AutoRegisterActionRoutes(routeTable, application, serviceCollection);

            var logger = serviceCollection.CreateInstance<ILogger>();
            logger.Log("Registered routes:");
            foreach (var route in routeTable)
            {
                logger.Log(route.ToString());
            }
            logger.Log(string.Empty);
            logger.Log("Request:");

            var httpServer = new HttpServer(80, routeTable, logger);
            await httpServer.StartAsync();
        }

        private static void AutoRegisterActionRoutes(List<Route> routeTable, IMvcApplication application, IserviceCollection serviceCollection)
        {
            var controllers = application.GetType().Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Controller)) && !type.IsAbstract);

            foreach (var controller in controllers)
            {
                var action = controller.GetMethods()
                    .Where(x => !x.IsSpecialName &&
                                !x.IsConstructor &&
                                x.IsPublic &&
                                x.DeclaringType == controller);

                foreach (var method in action)
                {
                    var url = "/" + controller.Name.Replace("Controller", string.Empty) + "/" + method.Name;
                    var attribute = method
                        .GetCustomAttributes()
                        .FirstOrDefault(x => x.GetType()
                        .IsSubclassOf(typeof(HttpMethodAttribute)))
                        as HttpMethodAttribute;

                    var httpActionType = HttpMethodType.Get;
                    if (attribute != null)
                    {
                        httpActionType = attribute.Type;
                        if (attribute.Url != null)
                        {
                            url = attribute.Url;
                        }
                    }

                    routeTable.Add(new Route(httpActionType, url, (request) => InvokeAction(request, serviceCollection, controller, method)
                    ));
                }
            }
        }

        private static HttpResponse InvokeAction(HttpRequest request, IserviceCollection serviceCollection, Type controllerType, MethodInfo actionMethod)
        {
            var controller = serviceCollection.CreateInstance(controllerType) as Controller;
            controller.Request = request;

            var actionParameterValues = new List<object>();
            var actionParameters = actionMethod.GetParameters();
            foreach (var parameter in actionParameters)
            {
                var value = Convert.ChangeType(
                    GetValueFromRequest(request, parameter.Name), 
                    parameter.ParameterType);

                if (value == null && parameter.ParameterType != typeof(String))
                {
                    var parameterValue = Activator.CreateInstance(parameter.ParameterType);
                    foreach (var property in parameter.ParameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        var propertyValue = GetValueFromRequest(request, property.Name);
                        property.SetValue(parameterValue, Convert.ChangeType(propertyValue, property.PropertyType));
                    }
                    actionParameterValues.Add(parameterValue);
                }

                else
                {
                    actionParameterValues.Add(value);
                }

            }

            var respone = actionMethod.Invoke(controller, actionParameterValues.ToArray()) as HttpResponse;

            return respone;
        }

        private static object GetValueFromRequest(HttpRequest request, string parameterName)
        {
            object value = null;
            parameterName = parameterName.ToLower();
            if (request.QueryData.Any(x => x.Key.ToLower() == parameterName))
            {
                value = request.QueryData.FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
            }
            else if (request.FormData.Any(x => x.Key.ToLower() == parameterName))
            {
                value = request.FormData.FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
            }

            return value;
        }

        private static void AutoRegisterStaticFilesRoute(IList<Route> routeTable)
        {
            var staticFiles = Directory.GetFiles("wwwroot", "*", SearchOption.AllDirectories);
            foreach (var staticFile in staticFiles)
            {
                var path = staticFile.Replace("wwwroot", string.Empty).Replace("\\", "/");
                routeTable.Add(new Route(HttpMethodType.Get, path, (request) =>
                {
                    var fileInfo = new FileInfo(staticFile);
                    var contentType = fileInfo.Extension switch
                    {
                        ".css" => "text/css",
                        ".html" => "text/html",
                        ".js" => "text/javascript",
                        ".ico" => "image/x-icon",
                        ".jpg" => "image/jpeg",
                        ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        _ => "text/plain",
                    };

                    return new FileResponse(File.ReadAllBytes(staticFile), contentType);
                }));
            }

        }
    }
}

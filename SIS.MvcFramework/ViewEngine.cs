using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SIS.MvcFramework
{
    public partial class ViewEngine : IViewEngine
    {
        public string GetHtml(string templateHtml, object model)
        {
            var methodCode = PrepareCSharpCode(templateHtml);
            var code = @$"using.System
                             using System.Text;
                             using System.Linq;
                             using System.Collection.Generic;
                             using SIS.MvcFramework;
                             namespace AppViewNamespace
                                {{
                                    public class AppViewCode : IView
                                        {{
                                             public string GetHtml(object model)
                                                {{
                                                     var html = new StringBuilder();
                                                     {methodCode}
                                                     return html.ToString();
                                                }}
                                        }}
                                }}";
            IView view = GetInstanceFromCode(code, model);
            string html = view.GetHtml(model);
            return html;
        }

        private IView GetInstanceFromCode(string code, object model)
        {
            var compilation =  CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(Object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(model.GetType().Assembly.Location));

            var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();

            foreach (var library in libraries)
            {
                compilation = compilation.AddReferences(
                    MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

            using var memoryStream = new MemoryStream();
            compilation.Emit(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var assemblyByteArray = memoryStream.ToArray();
            var assembly = Assembly.Load(assemblyByteArray);
            var type = assembly.GetType("AppViewNamespace.AppViewCode");
            var instance = Activator.CreateInstance(type) as IView;

            return instance;

        }

        private string PrepareCSharpCode(string templateHtml)
        {
            StringBuilder cSharpCode = new StringBuilder();
            StringReader reader = new StringReader(templateHtml);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                cSharpCode.AppendLine($"html.AppendLine(@\"{line.Replace("\"", "\"\"")}\");");
            }
            return cSharpCode.ToString();
        }
    }
}

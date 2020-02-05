using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SIS.MvcFramework.Tests
{
    public class ViewEngineTest
    {
        [Theory]
        [InlineData("OnlyHtmlView")]
        [InlineData("ForForeachView")]
        [InlineData("ViewModelView")]
        public void GetHtmlTest(string testName)
        {
            var viewModel = new TestViewModel()
            {
                Name = "Niki",
                Year = 2020,
                Number = new List<int> { 1, 10, 100, 1000, 10000}
            };

            var viewtContent = File.ReadAllText($"ViewTests/{testName}.html");
            var expectedResultContent = File.ReadAllText($"ViewTests/{testName}.Expected.html");

            IViewEngine viewEngine = new ViewEngine();
            var actualResult = viewEngine.GetHtml(viewtContent, viewModel);

            Assert.Equal(expectedResultContent, actualResult);
        }       
        
    }
}

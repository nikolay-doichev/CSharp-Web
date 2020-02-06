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
        public void TestGetHtml(string testName)
        {
            var viewModel = new TestViewModel()
            {
                Name = "Niki",
                Year = 2020,
                Numbers = new List<int> { 1, 10, 100, 1000, 10000}
            };

            var viewtContent = File.ReadAllText($"ViewTests/{testName}.html");
            var expectedResultContent = File.ReadAllText($"ViewTests/{testName}.Expected.html");

            IViewEngine viewEngine = new ViewEngine();
            var actualResult = viewEngine.GetHtml(viewtContent, viewModel);

            Assert.Equal(expectedResultContent, actualResult);
        }

        [Fact]
        public void TestGetHtmlWithTemplateModel()
        {
            var viewModel = new List<int> { 1, 2, 3 };

            var viewtContent = @"
@foreach(var num in Model)
{
<p>@num</p>
}";
            var expectedResultContent = @"
<p>1</p>
<p>2</p>
<p>3</p>
";
            IViewEngine viewEngine = new ViewEngine();
            var actualResult = viewEngine.GetHtml(viewtContent, viewModel);

            Assert.Equal(expectedResultContent, actualResult);
        }


    }
}

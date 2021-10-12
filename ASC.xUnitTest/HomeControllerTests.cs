using ASC.Models.Configuration;
using ASC.Utilities;
using ASC.WebApp.Controllers;
using ASC.xUnitTest.UtilitiesTest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Xunit;
using Xunit.Sdk;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace ASC.xUnitTest
{
    public class HomeControllerTests
    {
        private readonly Mock<IOptions<ApplicationSettings>> _optionsMock;
        private readonly Mock<HttpContext> _mockHttpcontext;
        public HomeControllerTests()
        {
            // Create an instance of Mock IOptions
            _optionsMock = new Mock<IOptions<ApplicationSettings>>();
            // Set IOptions<> Values property to return ApplicationSettings object
            _optionsMock.Setup(ap => ap.Value).Returns(new ApplicationSettings
            {
                ApplicationTitle = "ASC"
            });
            _mockHttpcontext = new Mock<HttpContext>();
            //set fakesession to httpcontext session
            _mockHttpcontext.Setup(x => x.Session).Returns(new FakeSession());
        }
        [Fact]
        public void HomeController_Index_View_Test()
        {
            // Home controller instantiated with Mock IOptions<> object
            var homecontroller = new HomeController(_optionsMock.Object);
            homecontroller.ControllerContext.HttpContext = _mockHttpcontext.Object;
            //assert return viewresult
            Assert.IsInstanceOfType(homecontroller.Index(),typeof(ViewResult));
        }
        [Fact]
        public void HomeController_Index_NoModel_Test()
        {
            var controller = new HomeController(_optionsMock.Object);
            controller.ControllerContext.HttpContext = _mockHttpcontext.Object;
            // Assert Model for Null
            Assert.IsNull((controller.Index() as ViewResult).ViewData.Model);
        }
        [Fact]
        public void HomeController_Index_Validation_Test()
        {
            var controller = new HomeController(_optionsMock.Object);
            controller.ControllerContext.HttpContext = _mockHttpcontext.Object;
            // Assert ModelState Error Count to 0
            Assert.AreEqual(0, (controller.Index() as ViewResult).ViewData.ModelState.ErrorCount);
            
        }
        [Fact]
        public void Homecontroller_Index_Session_Test()
        {
            var controller = new HomeController(_optionsMock.Object);
            controller.ControllerContext.HttpContext = _mockHttpcontext.Object;
            controller.Index();
            //session value with key  "Test"
            Assert.IsNull(controller.HttpContext.Session.GetSession<ApplicationSettings>("Test"));
        }
    }
}

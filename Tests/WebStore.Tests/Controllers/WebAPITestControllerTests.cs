using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Interfaces.Api;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebAPITestControllerTests
    {
        //private class TestValueService : IValuesService
        //{
        //    public IEnumerable<string> Get() { throw new NotImplementedException(); }
        //    public async Task<IEnumerable<string>> GetAsync() { throw new NotImplementedException(); }
        //    public string Get(int id) { throw new NotImplementedException(); }
        //    public async Task<string> GetAsync(int id) { throw new NotImplementedException(); }
        //    public Uri Post(string value) { throw new NotImplementedException(); }
        //    public async Task<Uri> PostAsync(string value) { throw new NotImplementedException(); }
        //    public HttpStatusCode Put(int id, string value) { throw new NotImplementedException(); }
        //    public async Task<HttpStatusCode> PutAsync(int id, string value) { throw new NotImplementedException(); }
        //    public HttpStatusCode Delete(int id) { throw new NotImplementedException(); }
        //    public async Task<HttpStatusCode> DeletAsync(int id) { throw new NotImplementedException(); }
        //}

        [TestMethod]
        public async Task Index_Returns_View_With_Values()
        {
            var expected_values = new[] {"1", "2", "3"};

            var value_service = new Mock<IValuesService>();
            value_service.Setup(service => service.GetAsync()).ReturnsAsync(expected_values);

            var controller = new WebAPITestController(value_service.Object);

            var result = await controller.Index();

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Model);

            Assert.Equal(expected_values.Length, model.Count());
            
            value_service.Verify(service => service.GetAsync());
            value_service.VerifyNoOtherCalls();
        }
    }
}

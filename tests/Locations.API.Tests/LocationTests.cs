using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestContext = License.API.Tests.Setup.TestContext;

namespace License.API.Tests
{
    [TestClass]
    public class LocationTests : IDisposable
    {
        private readonly TestContext _context;

        public LocationTests()
        {
            _context = new TestContext();
        }
        [TestMethod]
        public async Task GetLocationsTest()
        {
            var response =  await _context.Client.GetAsync(new Uri($"/api/location", UriKind.Relative));
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = response.Content.ReadAsStringAsync().Result;
            var resultModel = JsonConvert.DeserializeObject<List<Location>>(result);
            Assert.AreEqual(resultModel.Count, 3);
            Assert.AreEqual(resultModel[0].Id, 0);
            Assert.AreEqual(resultModel[1].Id, 1);
            Assert.AreEqual(resultModel[2].Id, 2);
        }

        #region IDisposable Support
        private bool _disposed = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        [TestCleanup]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

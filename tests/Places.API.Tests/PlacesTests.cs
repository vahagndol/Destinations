using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestContext = Places.API.Tests.Setup.TestContext;

namespace Places.API.Tests
{
    [TestClass]
    public class PlacesTests : IDisposable
    {
        private readonly TestContext _context;

        public PlacesTests()
        {
            _context = new TestContext();
        }
        [TestMethod]
        public async Task GetPlacesTest()
        {
            var response = await _context.Client.GetAsync(new Uri($"/api/place", UriKind.Relative));
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = response.Content.ReadAsStringAsync().Result;
            var resultModel = JsonConvert.DeserializeObject<List<Place>>(result);
            Assert.AreEqual(resultModel.Count, 9);
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

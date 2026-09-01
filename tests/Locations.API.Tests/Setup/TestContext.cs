using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Locations.API.Helper;

namespace Locations.API.Tests.Setup
{
    public class TestContext : IDisposable
    {
        private TestServer _server;
        public HttpClient Client { get; private set; }

        public TestContext()
        {
            SetUpClient();
        }

        private void SetUpClient()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.Testing.json", optional: false) 
                .AddEnvironmentVariables()
                .Build();

            _server = new TestServer(new WebHostBuilder()
                .UseConfiguration(config)
                .ConfigureServices(services =>
                {
                    services.AddLogging();
                    services.AddScoped<IContextReader<Location>, LocationContextReader<Location>>();
                    services.AddScoped<IApplicationDbContext<Location>, ApplicationDbContext<Location>>();
                    services.AddScoped<IRepository<Location>, Repository<Location>>();
                    services.AddScoped<IEntityService<Location>, EntityService<Location>>();
                    services.AddMvc(options => options.EnableEndpointRouting = false);
                })
                .Configure(app =>
                {
                    app.UseMvc();
                }));

            Client = _server.CreateClient();
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    _server?.Dispose();
                    Client?.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.
                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

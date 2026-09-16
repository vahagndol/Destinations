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
using Places.API.Helper;

namespace Places.API.Tests.Setup
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
            // Load test configuration using modern ConfigurationBuilder
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.Testing.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            // Create test server with modern minimal hosting setup
            var hostBuilder = new WebHostBuilder()
                .UseConfiguration(config)
                .ConfigureServices(services =>
                {
                    // Logging
                    services.AddLogging();

                    // Register domain services
                    services.AddSingleton<IContextReader<Place>, PlaceContextReader<Place>>();
                    services.AddSingleton<IApplicationDbContext<Place>, ApplicationDbContext<Place>>();
                    services.AddScoped<IRepository<Place>, Repository<Place>>();
                    services.AddScoped<IEntityService<Place>, PlaceService>();

                    // Controllers with modern routing
                    // Ensure controllers from the API assembly are discovered by tests
                    services.AddControllers()
                            .AddApplicationPart(typeof(Places.API.Controllers.PlaceController).Assembly);
                })
                .Configure(app =>
                {
                    // Modern middleware pipeline for testing
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapControllers());
                });

            _server = new TestServer(hostBuilder);
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

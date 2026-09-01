using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // Add the http clients to call our APIs
            builder.Services.AddHttpClient("locations", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["LocationsUrl"] ?? "https://localhost:5001");
            });

            builder.Services.AddHttpClient("places", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["PlacesUrl"] ?? "https://localhost:5002");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllers();
            app.MapRazorPages();
            app.MapGet("/", () => Results.Redirect("/index.html")).WithName("root");

            app.Run();
        }
    }
}

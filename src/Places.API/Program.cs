using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Places.API.Helper;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.AddConsole();

// DI registrations. The reader must be singleton: it is consumed by the singleton context below,
// and a singleton cannot depend on a scoped service (Development's scope validation fails at startup).
builder.Services.AddSingleton<IContextReader<Place>, PlaceContextReader<Place>>();
// Keep ApplicationDbContext as singleton if it intentionally holds application-wide in-memory data
builder.Services.AddSingleton<IApplicationDbContext<Place>, ApplicationDbContext<Place>>();
builder.Services.AddScoped<IRepository<Place>, Repository<Place>>();
builder.Services.AddScoped<IEntityService<Place>, PlaceService>();

// Controllers with System.Text.Json (default JSON serializer)
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Places API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Places API V1"));

app.MapControllers();

app.Run();

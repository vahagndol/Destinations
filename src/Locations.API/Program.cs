using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Locations.API.Helper;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.AddConsole();

// DI registrations (scoped for request safety)
builder.Services.AddScoped<IContextReader<Location>, LocationContextReader<Location>>();
builder.Services.AddScoped<IApplicationDbContext<Location>, ApplicationDbContext<Location>>();
builder.Services.AddScoped<IRepository<Location>, Repository<Location>>();
builder.Services.AddScoped<IEntityService<Location>, EntityService<Location>>();

// Controllers with System.Text.Json (default JSON serializer)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Locations API", Version = "v1" });
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
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Locations API V1"));

app.MapControllers();

app.Run();

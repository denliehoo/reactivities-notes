// this file is always the starting point of our app
using API.Extensions;
using Application.Activities;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Refactored other services (except AddControllers) to ApplicationServiceExtensions.cs for cleaner Program.cs
// entry point any more services in the future will be in that file
builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

// using means that once we are done with this method, anything inside it
// will be destroyed / cleaned from memory
// here, we migrate the database
// in API directory, dotnet watch
// or do this instead(recommended)" dotnet watch --no-hot-reload 
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An unexpected error occured");
}

app.Run();

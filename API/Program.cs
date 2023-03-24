// this file is always the starting point of our app
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Application.Activities;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(opt =>
{   // with this, all our APIs now require authentication
    // however, we still want our login to be able to login w/o a token since
    // token is generated during login. We can use [AllowAnonymous] in
    // our relevant AccountController methods to ensure that no authentication is needed to send the API
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});
// Refactored other services (except AddControllers) to ApplicationServiceExtensions.cs for cleaner Program.cs
// entry point any more services in the future will be in that file
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication(); // authentication always comes before authorization

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat"); // the route that they redirect to when they conenct to the chat hub is /chat

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
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An unexpected error occured");
}

app.Run();

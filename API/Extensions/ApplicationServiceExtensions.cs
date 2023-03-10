using Application.Activities;
using Application.Core;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        // this refers to where we use this method
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services
                .AddDbContext<DataContext>(opt =>
                {
                    opt
                        .UseSqlite(config
                            .GetConnectionString("DefaultConnection"));
                });

            // allows us to call our APIs from localhost:3000 which is our react app
            // without this, we will get  CORS errors
            services
                .AddCors(opt =>
                {
                    opt
                        .AddPolicy("CorsPolicy",
                        policy =>
                        {
                            policy
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .WithOrigins("http://localhost:3000");
                        });
                });
            services.AddMediatR(typeof(List.Handler));
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Create>();
            // this 2 lines below makes the IUSerAccessor and UserAccessor
            // available to be injected into our Application Handlers
            // i.e. the CRUD handlers in Application/Activities
            services.AddHttpContextAccessor();
            services.AddScoped<IUserAccessor, UserAccessor>();

            return services;
        }
    }
}

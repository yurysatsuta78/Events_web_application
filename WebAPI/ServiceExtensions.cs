using Application.Interfaces.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Application.Services;
using Infrastructure.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;
using Domain.Interfaces;
using Infrastructure;
using Application.UseCases.Auth.Register.Validators;
using System.Reflection;
using WebAPI.Filters;

namespace WebAPI
{
    public static class ServiceExtensions
    {
        public static void AddRepositories(this IServiceCollection services) 
        {
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<IParticipantsRepository, ParticipantsRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddServices(this IServiceCollection services) 
        {
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<AuthOptions>();
        }

        public static void AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var handlerInterface = typeof(IInputPort);

            var handlerTypes = assembly.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface &&
                               handlerInterface.IsAssignableFrom(type));

            foreach (var handler in handlerTypes)
            {
                var interfaces = handler.GetInterfaces()
                    .Where(i => i != handlerInterface && handlerInterface.IsAssignableFrom(i));

                foreach (var @interface in interfaces)
                {
                    services.AddScoped(@interface, handler);
                }
            }
        }

        public static void AddAutoMapperProfiles(this IServiceCollection services) 
        {
            services.AddAutoMapper(typeof(GetEventProfile).Assembly);
        }

        public static void AddApiAuthentication(this IServiceCollection services) 
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireParticipantRole", policy => policy.RequireRole("Participant"));
            });
        }

        public static void AddValidators(this IServiceCollection services) 
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        }

        public static void AddFilters(this IServiceCollection services) 
        {
            services.AddScoped(typeof(ParticipantRequestFilter<>));
        }
    }
}

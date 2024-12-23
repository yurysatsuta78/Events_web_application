using Application.Interfaces.Repositories;
using Application.Interfaces.UnitsOfWork;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.UnitsOfWork;
using Infrastructure.AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Application;
using Application.Services;
using Infrastructure.Auth;
using Application.Interfaces.Services;
using FluentValidation;
using Application.DTOValidators.Participant;
using FluentValidation.AspNetCore;

namespace WebAPI
{
    public static class ServiceExtensions
    {
        public static void AddRepositories(this IServiceCollection services) 
        {
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<IImagesRepository, ImagesRepository>();
            services.AddScoped<IParticipantsRepository, ParticipantsRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IEventParticipantUOW, EventParticipantUOW>();
            services.AddScoped<IParticipantRoleUOW, ParticipantRoleUOW>();
        }

        public static void AddServices(this IServiceCollection services) 
        {
            services.AddScoped<EventsService>();
            services.AddScoped<ImageService>();
            services.AddScoped<ParticipantsService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<AuthOptions>();
        }

        public static void AddAutoMapperProfiles(this IServiceCollection services) 
        {
            services.AddAutoMapper(typeof(EventProfile).Assembly);
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
            services.AddValidatorsFromAssemblyContaining<CreateParticipantDtoValidator>();
        }
    }
}

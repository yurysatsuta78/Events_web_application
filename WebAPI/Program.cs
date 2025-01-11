using Application.UseCases.Events.Create;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebAPI;
using WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<EventsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(true);
});


services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddApiAuthentication();
services.AddRepositories();
services.AddHandlersFromAssembly(typeof(CreateEventHandler).Assembly);
services.AddServices();
services.AddAutoMapperProfiles();
services.AddValidators();
services.AddFilters();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<JwtCookieMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

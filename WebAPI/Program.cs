using Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebAPI;
using WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<EventsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddApiAuthentication();
services.AddRepositories();
services.AddServices();
services.AddAutoMapperProfiles();
services.AddValidators();

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

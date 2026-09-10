using Application;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Api.Extensions;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddApiServices(builder.Configuration);

            var app = builder.Build();

            app.UseExceptionHandler();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseStaticFiles();

            app.UseCors("FrontendPolicy");

            app.UseHttpsRedirection();

            app.UseRateLimiter();

            app.UseAuthentication();
            app.UseAuthorization();

            await app.SeedDatabaseAsync(builder.Configuration);

            app.MapControllers();

            app.Run();
        }
    }
}
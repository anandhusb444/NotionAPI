
using Microsoft.EntityFrameworkCore;
using NotionAPI.Context;
using NotionAPI.Services;
using Scalar.AspNetCore;

namespace NotionAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddDbContext<NotionData>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddOpenApi();

            builder.Services.AddScoped<IuserService, UserService>();

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("ReactPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });


            var app = builder.Build();

            app.UseCors("ReactPolicy");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference("/scalar/v1");

            }
            app.MapGet("/", () => Results.Redirect("/scalar/v1"));
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Books;
using Services.Books.Interfaces;

namespace Web.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddDbContext<LibraryDbContext>(options => options
                .UseSqlite(configuration.GetConnectionString("BookRepositoryConnectionString")));
    
    public static IApplicationBuilder UseAnyCors(
        this IApplicationBuilder app)
        => app
            .UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    
    public static IServiceCollection ConfigureApiOptions(
        this IServiceCollection services)
    {
        services.AddTransient<IBooksBusinessService, BooksBusinessService>();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value is { Errors.Count: > 0 })
                    .Select(e => new
                    {
                        Field = e.Key,
                        Errors = e.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                    });

                return new BadRequestObjectResult(new { Errors = errors });
            };
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddTransient<IBooksBusinessService, BooksBusinessService>();
        
        return services;
    }
}
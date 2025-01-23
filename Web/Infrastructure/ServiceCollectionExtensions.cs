using Data;
using Microsoft.EntityFrameworkCore;

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
}
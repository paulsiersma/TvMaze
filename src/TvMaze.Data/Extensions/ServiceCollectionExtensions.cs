using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TvMaze.Data.Repositories;

namespace TvMaze.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddDbContext<TvMazeDbContext>(options =>
                options.UseInMemoryDatabase("TvMazeScrapedData"));

            services.AddScoped<IShowAndCastRepository, ShowAndCastRepository>();

            return services;
        }
    }
}
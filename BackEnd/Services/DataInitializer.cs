using BackEnd.Data;
using BackEnd.WebScrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DataInitializer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task SeedAsync()
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            var games = await context.Games.ToListAsync();
            if (games.Count != 0)
            {
                return;
            }
            var details = await GamesScrapper.GetPageDetails("https://newzoo.com/insights/rankings/top-20-core-pc-games/");
            var result = GamesScrapper.AssignData(details);

            await context.Games.AddRangeAsync(result);

            await context.SaveChangesAsync();
        }
    }
}

using BackEnd.Data;
using BackEnd.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamFinderDTO;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<SearchResult>>> Search(SearchTerm searchTerm)
        {
            var query = searchTerm.Query;

            var sessionResults = await _context.Sessions.Include(s => s.SessionPlayers)
                                                            .ThenInclude(sp => sp.Player)
                                                        .Include(s => s.Game)
                                                        .Where(s =>
                                                            s.Title.Contains(query) ||
                                                            s.Game.Name.Contains(query)
                                                        )
                                                        .ToListAsync();

            var playerResults = await _context.Players.Include(p => p.SessionPlayers)
                                                        .ThenInclude(sp => sp.Session)
                                                      .Where(p =>
                                                        p.UserName.Contains(query)
                                                       )
                                                      .ToListAsync();

            var results = sessionResults.Select(s => new SearchResult
            {
                Type = SearchResultType.Session,
                Session = s.MapSessionResponse()
            })
            .Concat(playerResults.Select(p => new SearchResult
            {
                Type = SearchResultType.Player,
                Player = p.MapPlayerResponse()
            }));

            return results.ToList();
        }

    }
}

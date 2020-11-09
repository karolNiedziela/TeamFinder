using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Infrastructure;
using BackEnd.WebScrapper;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamFinderDTO;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<GameDTO>>> GetGamesAsync()
        {
            var games = await _context.Games.Select(g => g.MapGame()).ToListAsync();

            return games;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<GameDTO>> GetAsync(string name)
        {
            var game = await _context.Games.SingleOrDefaultAsync(g => g.Name == name);

            if (game == null)
            {
                return NotFound();
            }

            var result = game.MapGame();

            return result;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GameDTO>> PutGameAsync(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if(!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<GameDTO>> PostGameAsync(Game input)
        {
            var game = new Game
            {
                Name = input.Name,
                Publisher = input.Publisher
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            var result = game.MapGame();

            return CreatedAtAction(nameof(GetAsync), new { name = game.Name }, result);
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}

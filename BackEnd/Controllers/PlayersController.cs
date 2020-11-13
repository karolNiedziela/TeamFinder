using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Infrastructure;
using TeamFinderDTO;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerResponse>>> GetPlayersAsync()
        {
            var players = await _context.Players.Include(p => p.SessionPlayers)
                                                    .ThenInclude(sp => sp.Session)
                                                 .Include(p => p.PlayerGames)
                                                    .ThenInclude(pg => pg.Game)
                                                 .Select(p => p.MapPlayerResponse())
                                                 .ToListAsync();

            return players;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PlayerResponse>> GetPlayerAsync(int id)
        {
            var player = await _context.Players.Include(p => p.SessionPlayers)
                                                    .ThenInclude(sp => sp.Session)
                                               .Include(p => p.PlayerGames)
                                                    .ThenInclude(pg => pg.Game)
                                               .SingleOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            return player.MapPlayerResponse();
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<PlayerResponse>> GetPlayerAsync(string username)
        {
            var player = await _context.Players.Include(p => p.SessionPlayers)
                                                    .ThenInclude(sp => sp.Session)
                                               .Include(p => p.PlayerGames)
                                                    .ThenInclude(pg => pg.Game)
                                               .SingleOrDefaultAsync(p => p.UserName == username);

            if (player == null)
            {
                return NotFound();
            }

            return player.MapPlayerResponse();
        }

        [HttpGet("{id}/games")]
        public async Task<ActionResult<List<PlayerResponse>>> GetGamesAsync(int id)
        {
            var games = await _context.Players.Include(p => p.PlayerGames)
                                                .ThenInclude(pg => pg.Game)
                                              .Where(p => p.Id == id)
                                              .Select(p => p.MapPlayerResponse())
                                              .ToListAsync();

            return games;
        }

        [HttpGet("{id}/sessions")]
        public async Task<ActionResult<List<SessionResponse>>> GetSessionsAsync(int id)
        {
            var sessions = await _context.Sessions.Include(s => s.SessionPlayers)
                                                    .ThenInclude(sp => sp.Player)
                                                  .Include(s => s.Game)
                                                  .Where(s => s.SessionPlayers.Any(sp => sp.Player.Id == id))
                                                  .Select(s => s.MapSessionResponse())
                                                  .ToListAsync();
            return sessions;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
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
        public async Task<ActionResult<PlayerResponse>> PostPlayerAsync(PlayerDTO input)
        {
            var existingPlayer = await _context.Players.Where(p => p.UserName == input.UserName).FirstOrDefaultAsync();

            if (existingPlayer != null)
            {
                return Conflict(input);
            }

            var player = new Player
            {
                UserName = input.UserName,
                Age = input.Age,
                Info = input.Info
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            var result = player.MapPlayerResponse();

            return CreatedAtAction(nameof(GetPlayerAsync), new { username = player.UserName }, result);
          
        }

        [HttpPost("{id}/sessions/{sessionId}")]
        public async Task<ActionResult<PlayerResponse>> AddSessionAsync(int id, int sessionId)
        {
            var player = await _context.Players.Include(p => p.SessionPlayers)
                                                    .ThenInclude(sp => sp.Session)
                                               .SingleOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(sessionId);

            if (session == null)
            {
                return BadRequest();
            }

            player.SessionPlayers.Add(new SessionPlayer
            {
                PlayerId = player.Id,
                SessionId = sessionId
            });

            await _context.SaveChangesAsync();

            var result = player.MapPlayerResponse();

            return result;
        }

        [HttpPost("{id}/games/{gameId}")]
        public async Task<ActionResult<PlayerResponse>> AddGameAsync(int id, int gameId)
        {
            var player = await _context.Players.Include(p => p.PlayerGames)
                                                    .ThenInclude(pg => pg.Game)
                                               .SingleOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(gameId);

            if (game == null)
            {
                return BadRequest();
            }

            await _context.SaveChangesAsync();

            var result = player.MapPlayerResponse();

            return result;
        }

        [HttpDelete("{id}/sessions/{sessionId}")]
        public async Task<IActionResult> RemoveSessionAsync(int id, int sessionId)
        {
            var player = await _context.Players.Include(p => p.SessionPlayers)
                                               .SingleOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(sessionId);
            
            if(session == null)
            {
                return BadRequest();
            }

            var sessionPlayer = player.SessionPlayers.FirstOrDefault(sp => sp.SessionId == sessionId);
            player.SessionPlayers.Remove(sessionPlayer);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}/games/{gameId}")]
        public async Task<IActionResult> RemoveGameAsync(string username, int gameId)
        {
            var player = await _context.Players.Include(p => p.PlayerGames)
                                               .SingleOrDefaultAsync(p => p.UserName == username);

            if (player == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(gameId);

            if (game == null)
            {
                return BadRequest();
            }

            var playerGame = await _context.PlayerGames.FirstOrDefaultAsync(pg => pg.GameId == gameId);
            player.PlayerGames.Remove(playerGame);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var loader = new PlayerLoader();

            using (var stream = file.OpenReadStream())
            {
                await loader.LoadDataAsync(stream, _context);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }


        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}

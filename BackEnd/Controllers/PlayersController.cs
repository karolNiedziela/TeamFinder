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
    public class PlayersController : ControllerBase
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

        [HttpGet("{username}/games")]
        public async Task<ActionResult<List<PlayerResponse>>> GetGamesAsync(string username)
        {
            var games = await _context.Players.Include(p => p.PlayerGames)
                                                .ThenInclude(pg => pg.Game)
                                              .Where(p => p.UserName == username)
                                              .Select(p => p.MapPlayerResponse())
                                              .ToListAsync();

            return games;
        }

        [HttpGet("{username}/sessions")]
        public async Task<ActionResult<List<SessionResponse>>> GetSessionsAsync(string username)
        {
            var sessions = await _context.Sessions.Include(s => s.SessionPlayers)
                                                    .ThenInclude(sp => sp.Player)
                                                  .Where(s => s.SessionPlayers.Any(sp => sp.Player.UserName == username))
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

        [HttpPost("{username}/session/{sessionId}")]
        public async Task<ActionResult<PlayerResponse>> AddSessionAsync(string username, int sessionId)
        {
            var player = await _context.Players.Include(p => p.SessionPlayers)
                                                    .ThenInclude(sp => sp.Session)
                                               .SingleOrDefaultAsync(p => p.UserName == username);

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

        [HttpPost("{username}/game/{gameId}")]
        public async Task<ActionResult<PlayerResponse>> AddGameAsync(string username, int gameId)
        {
            var player = await _context.Players.Include(p => p.PlayerGames)
                                                    .ThenInclude(pg => pg.Game)
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

            await _context.SaveChangesAsync();

            var result = player.MapPlayerResponse();

            return result;
        }

        [HttpDelete("{username}/session/{sessionId}")]
        public async Task<IActionResult> RemoveSessionAsync(string username, int sessionId)
        {
            var player = await _context.Players.Include(p => p.SessionPlayers)
                                               .SingleOrDefaultAsync(p => p.UserName == username);

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

        [HttpDelete("{username}/game/{gameId}")]
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

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}

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
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SessionResponse>>> GetSessionsAsync()
        {
            var sessions = await _context.Sessions.Include(s => s.SessionPlayers)
                                                        .ThenInclude(sp => sp.Player)
                                                  .Select(s => s.MapSessionResponse())
                                                  .ToListAsync();

            return sessions;
                                                  
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SessionResponse>> GetSessionAsync(int id)
        {
            var session = await _context.Sessions.Include(s => s.SessionPlayers)
                                                        .ThenInclude(sp => sp.Player)
                                                 .Include(s => s.Game)
                                                 .SingleOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                return NotFound();
            }

            var result = session.MapSessionResponse();

            return result;
        }

        // PUT: api/Sessions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSession(int id, Session session)
        {
            if (id != session.Id)
            {
                return BadRequest();
            }

            _context.Entry(session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
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

        // POST: api/Sessions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SessionResponse>> PostSessionAsync(Session input)
        {
            var session = new Session
            {
                Title = input.Title,
                StartTime = input.StartTime,
                EndTime = input.StartTime,
                GameId = input.GameId
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            var result = session.MapSessionResponse();

            return CreatedAtAction(nameof(GetSessionAsync), new { id = session.Id }, result);
        }

        // DELETE: api/Sessions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Session>> DeleteSession(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return session;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var loader = new SessionLoader();

            using (var stream = file.OpenReadStream())
            {
                await loader.LoadDataAsync(stream, _context);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.UserName)
                .IsUnique();

            modelBuilder.Entity<PlayerGame>()
                .HasKey(pg => new { pg.GameId, pg.PlayerId });

            modelBuilder.Entity<SessionPlayer>()
                .HasKey(sp => new { sp.PlayerId, sp.SessionId });
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<PlayerGame> PlayerGames { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<SessionPlayer> SessionPlayers { get; set; }
    }
}

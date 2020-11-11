using BackEnd.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamFinderDTO;

namespace BackEnd.Infrastructure
{
    public static class EntityExtensions
    {
        public static SessionResponse MapSessionResponse(this Session session) =>
            new SessionResponse
            {
                Id = session.Id,
                Title = session.Title,
                StartTime = session.StartTime?.LocalDateTime,
                EndTime = session.EndTime?.LocalDateTime,
                Players = session.SessionPlayers?
                                  .Select(sp => new PlayerDTO
                                  {
                                      Id = sp.PlayerId,
                                      UserName = sp.Player.UserName,
                                      Age = sp.Player.Age,
                                      Info = sp.Player.Info
                                  })
                                  .ToList(),
                GameId = session.GameId,
                Game = new GameDTO
                {
                    Id = session?.GameId ?? 0,
                    Name = session.Game?.Name,
                    Publisher = session.Game?.Publisher
                }
            };

        public static PlayerResponse MapPlayerResponse(this Player player) =>
            new PlayerResponse
            {
                Id = player.Id,
                UserName = player.UserName,
                Age = player.Age,
                Info = player.Info,
                Games = player.PlayerGames?
                              .Select(pg => new GameDTO
                              {
                                  Id = pg.GameId,
                                  Name = pg.Game.Name,
                                  Publisher = pg.Game.Publisher
                              })
                              .ToList(),
                SessionResponses = player.SessionPlayers?
                              .Select(sp => new SessionDTO
                              {
                                  Id = sp.SessionId,
                                  Title = sp.Session.Title,
                                  StartTime = sp.Session.StartTime,
                                  EndTime = sp.Session.EndTime
                              })
                              .ToList()             
            };

        public static GameDTO MapGame(this Game game) =>
            new GameDTO
            {
                Id = game.Id,
                Name = game.Name,
                Publisher = game.Publisher
            };
    }
}

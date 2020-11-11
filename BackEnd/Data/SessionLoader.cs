using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Data
{
    public class SessionLoader : DataLoader
    {
        public override async Task LoadDataAsync(Stream fileStream, ApplicationDbContext context)
        {
            var addedSessions = new Dictionary<string, Session>();

            var array = await JToken.LoadAsync(new JsonTextReader(new StreamReader(fileStream)));

            var sessions = array.ToObject<List<ImportSession>>();

            foreach (var session in sessions)
            {
                if (!addedSessions.ContainsKey(session.Title))
                {
                    var thisSession = new Session { Title = session.Title, StartTime = session.StartTime, EndTime = session.EndTime, GameId = session.GameId };
                    context.Sessions.Add(thisSession);
                    addedSessions.Add(thisSession.Title, thisSession);
                }

            }
        }

        private class ImportSession
        {
            public string Title { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }

            public int GameId { get; set; }

            public List<SessionPlayer> SessionPlayers { get; set; }


        }
    }
}

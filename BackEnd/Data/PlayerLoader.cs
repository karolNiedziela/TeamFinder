using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Data
{
    public class PlayerLoader : DataLoader
    {
        public override async Task LoadDataAsync(Stream fileStream, ApplicationDbContext context)
        {
            var addedPlayers = new Dictionary<string, Player>();

            var array = await JToken.LoadAsync(new JsonTextReader(new StreamReader(fileStream)));

            var players = array.ToObject<List<ImportPlayer>>();

            foreach (var player in players)
            {
                if(!addedPlayers.ContainsKey(player.UserName))
                {
                    var thisPlayer = new Player { UserName = player.UserName, Age = player.Age, Info = player.Info };
                    context.Players.Add(thisPlayer);
                    addedPlayers.Add(thisPlayer.UserName, thisPlayer);
                }
            }
        }

        private class ImportPlayer
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public int Age { get; set; }

            public string Info { get; set; }
        }
    }

    
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TeamFinderDTO;

namespace BackEnd.Data
{
    public class Player : PlayerDTO
    {
        public ICollection<PlayerGame> PlayerGames { get; set; }

        public ICollection<SessionPlayer> SessionPlayers { get; set; }
    }
}

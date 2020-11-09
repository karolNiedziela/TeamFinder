using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Data
{
    public class PlayerGame
    {
        public Player Player { get; set; }
        
        public int PlayerId { get; set; }

        public Game Game { get; set; }

        public int GameId { get; set; }
    }
}

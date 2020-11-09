using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TeamFinderDTO;

namespace BackEnd.Data
{
    public class Session : SessionDTO
    {
        public ICollection<SessionPlayer> SessionPlayers { get; set; }

        public Game Game { get; set; }
        
    }
}

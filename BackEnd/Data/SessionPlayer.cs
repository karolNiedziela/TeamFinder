using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Data
{
    public class SessionPlayer
    {
        public Player Player { get; set; }
        
        public int PlayerId { get; set; }

        public Session Session { get; set; }

        public int SessionId { get; set; }
    }
}

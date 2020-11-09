using System;
using System.Collections.Generic;
using System.Text;
using TeamFinderDTO;

namespace TeamFinderDTO
{
    public class SessionResponse : SessionDTO
    {
        public GameDTO Game { get; set; }

        public List<PlayerDTO> Players { get; set; }
    }
}

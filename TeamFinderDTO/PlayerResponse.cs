using System;
using System.Collections.Generic;
using System.Text;

namespace TeamFinderDTO
{
    public class PlayerResponse : PlayerDTO
    {
        public List<GameDTO> Games { get; set; }
        public List<SessionDTO> SessionResponses { get; set; }
    }
}

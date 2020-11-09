using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeamFinderDTO
{
    public class GameDTO
    {
        public int Id { get; set; }

        [StringLength(2000)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Publisher { get; set; }
    }
}

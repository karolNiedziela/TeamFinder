using FrontEnd.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TeamFinderDTO;

namespace FrontEnd.Pages.Models
{
    public class Session : SessionDTO
    {
        [StartTime(ErrorMessage = "Invalid date. Date must be between today and max seven days from today." )]
        public new DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Choose game.")]
        public new int GameId { get; set; }
    }
}

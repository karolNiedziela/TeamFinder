using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeamFinderDTO
{
    public class PlayerDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [StringLength(4000)]
        public string Info { get; set; }
    }
}

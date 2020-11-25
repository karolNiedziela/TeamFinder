using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeamFinderDTO
{
    public class SessionDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string Title { get; set; }

        [Required]
        public DateTimeOffset? StartTime { get; set; }
        
        public DateTimeOffset? EndTime { get; set; }

        public TimeSpan Duration => EndTime?.Subtract(StartTime ?? DateTimeOffset.MinValue) ?? TimeSpan.Zero;

        [Required]
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int MembersLimit { get; set; }

        [Required]
        public int GameId { get; set; }

    }
}

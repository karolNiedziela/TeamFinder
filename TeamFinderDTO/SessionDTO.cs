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

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public TimeSpan Duration => EndTime?.Subtract(StartTime ?? DateTimeOffset.MinValue) ?? TimeSpan.Zero;

        public int? GameId { get; set; }
    }
}

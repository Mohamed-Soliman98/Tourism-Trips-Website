using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitys
{
    public class TripImage
    {
        public Guid Id { get; set; }

        public Guid TripId { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string? AltText { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsCover { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Trip Trip { get; set; } = null!;
    }
}

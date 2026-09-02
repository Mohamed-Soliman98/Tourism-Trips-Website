using Domain.Entitys;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Trip
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public TripStatus Status { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
        public int Duration { get; set; }
        public DurationUnit DurationUnit { get; set; }
        public string? PickupLocation { get; set; }
        public string Currency { get; set; } = "EUR";
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal? OldPrice { get; set; }
        public bool IsPriceFrom { get; set; }
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? OgImage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public Guid DestinationId { get; set; }
        public Destination Destination { get; set; } = null!;
        public Guid TourTypeId { get; set; }
        public TourType TourType { get; set; } = null!;
        public ICollection<TripImage> Images { get; set; } = new List<TripImage>();
        public ICollection<TripItineraryItem> ItineraryItems { get; set; }= new List<TripItineraryItem>();
        public ICollection<TripInclude> Includes { get; set; }= new List<TripInclude>();
        public ICollection<TripExclude> Excludes { get; set; }= new List<TripExclude>();
        public ICollection<FAQ> FAQs { get; set; } = new List<FAQ>();
        public ICollection<TripTranslation> Translations { get; set; }= new List<TripTranslation>();
        public ICollection<BookingInquiry> BookingInquiries { get; set; } = new List<BookingInquiry>();
    }
}

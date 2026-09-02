using Domain.Entity;
using Domain.Entitys;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public DbSet<Trip> Trips => Set<Trip>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Destination> Destinations => Set<Destination>();
        public DbSet<TourType> TourTypes => Set<TourType>();
        public DbSet<TripImage> TripImages => Set<TripImage>();
        public DbSet<TripItineraryItem> TripItineraryItems => Set<TripItineraryItem>();
        public DbSet<TripInclude> TripIncludes => Set<TripInclude>();
        public DbSet<TripExclude> TripExcludes => Set<TripExclude>();
        public DbSet<TripTranslation> TripTranslations => Set<TripTranslation>();
        public DbSet<FAQ> FAQs => Set<FAQ>();
        public DbSet<FAQTranslation> FAQTranslations => Set<FAQTranslation>();
        public DbSet<BookingInquiry> BookingInquiries => Set<BookingInquiry>();
        public DbSet<Testimonial> Testimonials => Set<Testimonial>();
        public DbSet<Banner> Banners => Set<Banner>();
        public DbSet<CMSSection> CMSSections => Set<CMSSection>();
        public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();


    }
}

using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork
    {

        ITripRepository Trips { get; }
        ICategoryRepository Categories { get; }
        IDestinationRepository Destinations { get; }
        ITourTypeRepository TourTypes { get; }
        IBookingInquiryRepository BookingInquiries { get; }
        ITripImageRepository TripImages { get; }
        ITripTranslationRepository TripTranslations { get; }
        ITripIncludeRepository TripIncludes { get; }
        ITripExcludeRepository TripExcludes { get; }
        ITripItineraryItemRepository TripItineraryItems { get; }
        IFAQRepository FAQs { get; }
        IFAQTranslationRepository FAQTranslations { get; }
        ITestimonialRepository Testimonials { get; }
        IBannerRepository Banners { get; }
        ICMSSectionRepository CMSSections { get; }
        ISiteSettingRepository SiteSettings { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}

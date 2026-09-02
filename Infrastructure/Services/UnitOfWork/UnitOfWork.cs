using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext dbContext;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(AppDbContext app, IServiceProvider serviceProvider)   
        {
            dbContext = app;
            _serviceProvider = serviceProvider;
        }

        public ITripRepository Trips => _serviceProvider.GetRequiredService<ITripRepository>();
        public ICategoryRepository Categories => _serviceProvider.GetRequiredService<ICategoryRepository>();
        public IDestinationRepository Destinations => _serviceProvider.GetRequiredService<IDestinationRepository>();
        public ITourTypeRepository TourTypes => _serviceProvider.GetRequiredService<ITourTypeRepository>();
        public IBookingInquiryRepository BookingInquiries => _serviceProvider.GetRequiredService<IBookingInquiryRepository>();
        public ITripImageRepository TripImages => _serviceProvider.GetRequiredService<ITripImageRepository>();
        public ITripTranslationRepository TripTranslations => _serviceProvider.GetRequiredService<ITripTranslationRepository>();
        public ITripIncludeRepository TripIncludes => _serviceProvider.GetRequiredService<ITripIncludeRepository>();
        public ITripExcludeRepository TripExcludes => _serviceProvider.GetRequiredService<ITripExcludeRepository>();
        public ITripItineraryItemRepository TripItineraryItems => _serviceProvider.GetRequiredService<ITripItineraryItemRepository>();
        public IFAQRepository FAQs => _serviceProvider.GetRequiredService<IFAQRepository>();
        public IFAQTranslationRepository FAQTranslations => _serviceProvider.GetRequiredService<IFAQTranslationRepository>();
        public ITestimonialRepository Testimonials => _serviceProvider.GetRequiredService<ITestimonialRepository>();
        public IBannerRepository Banners => _serviceProvider.GetRequiredService<IBannerRepository>();
        public ICMSSectionRepository CMSSections => _serviceProvider.GetRequiredService<ICMSSectionRepository>();
        public ISiteSettingRepository SiteSettings => _serviceProvider.GetRequiredService<ISiteSettingRepository>();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
           return await dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}

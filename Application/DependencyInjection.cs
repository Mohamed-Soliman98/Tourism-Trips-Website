using Application.Interfaces.BookingInquiries;
using Application.Interfaces.Banners;
using Application.Interfaces.Categories;
using Application.Interfaces.CMSSections;
using Application.Interfaces.Dashboard;
using Application.Interfaces.Destinations;
using Application.Interfaces.Media;
using Application.Interfaces.SiteSettings;
using Application.Interfaces.Testimonials;
using Application.Interfaces.TourTypes;
using Application.Interfaces.TripExcludes;
using Application.Interfaces.TripFAQs;
using Application.Interfaces.TripImages;
using Application.Interfaces.TripIncludes;
using Application.Interfaces.Trips;
using Application.Interfaces.TripTranslations;
using Application.Services.BookingInquiries;
using Application.Services.Banners;
using Application.Services.Categories;
using Application.Services.CMSSections;
using Application.Services.Dashboard;
using Application.Services.Destinations;
using Application.Services.Media;
using Application.Services.SiteSettings;
using Application.Services.Testimonials;
using Application.Services.TourTypes;
using Application.Services.TripExcludes;
using Application.Services.TripFAQs;
using Application.Services.TripImages;
using Application.Services.TripIncludes;
using Application.Services.Trips;
using Application.Services.TripTranslations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Validators
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            // Trip Use Case Services
            services.AddScoped<ICreateTripService, CreateTripService>();
            services.AddScoped<IGetTripByIdService, GetTripByIdService>();
            services.AddScoped<IGetTripBySlugService, GetTripBySlugService>();
            services.AddScoped<IGetPublicTripsService, GetPublicTripsService>();
            services.AddScoped<IGetAdminTripsService, GetAdminTripsService>();
            services.AddScoped<IUpdateTripService, UpdateTripService>();
            services.AddScoped<IDeleteTripService, DeleteTripService>();
            services.AddScoped<IPublishTripService, PublishTripService>();
            services.AddScoped<IUnpublishTripService, UnpublishTripService>();
            services.AddScoped<IDuplicateTripService, DuplicateTripService>();

            // Category Use Case Services
            services.AddScoped<ICreateCategoryService, CreateCategoryService>();
            services.AddScoped<IGetCategoriesService, GetCategoriesService>();
            services.AddScoped<IGetCategoryByIdService, GetCategoryByIdService>();
            services.AddScoped<IUpdateCategoryService, UpdateCategoryService>();
            services.AddScoped<IDeleteCategoryService, DeleteCategoryService>();
            services.AddScoped<IGetPublicCategoriesService, GetPublicCategoriesService>();

            // Destination Use Case Services
            services.AddScoped<ICreateDestinationService, CreateDestinationService>();
            services.AddScoped<IGetDestinationsService, GetDestinationsService>();
            services.AddScoped<IGetDestinationByIdService, GetDestinationByIdService>();
            services.AddScoped<IUpdateDestinationService, UpdateDestinationService>();
            services.AddScoped<IDeleteDestinationService, DeleteDestinationService>();
            services.AddScoped<IGetPublicDestinationsService, GetPublicDestinationsService>();

            // TourType Use Case Services
            services.AddScoped<ICreateTourTypeService, CreateTourTypeService>();
            services.AddScoped<IGetTourTypesService, GetTourTypesService>();
            services.AddScoped<IGetTourTypeByIdService, GetTourTypeByIdService>();
            services.AddScoped<IUpdateTourTypeService, UpdateTourTypeService>();
            services.AddScoped<IDeleteTourTypeService, DeleteTourTypeService>();
            services.AddScoped<IGetPublicTourTypesService, GetPublicTourTypesService>();

            // Booking Inquiry Use Case Services
            services.AddScoped<ICreateBookingInquiryService, CreateBookingInquiryService>();
            services.AddScoped<IGetBookingInquiriesService, GetBookingInquiriesService>();
            services.AddScoped<IGetBookingInquiryByIdService, GetBookingInquiryByIdService>();
            services.AddScoped<IUpdateBookingInquiryService, UpdateBookingInquiryService>();
            services.AddScoped<IDeleteBookingInquiryService, DeleteBookingInquiryService>();

            services.AddScoped<IUploadMediaService, UploadMediaService>();

            // TripImage Use Case Services
            services.AddScoped<IAddTripImageService, AddTripImageService>();
            services.AddScoped<IGetTripGalleryService, GetTripGalleryService>();
            services.AddScoped<IUpdateTripImageService, UpdateTripImageService>();
            services.AddScoped<IDeleteTripImageService, DeleteTripImageService>();

            // Trip Translation Use Case Services
            services.AddScoped<IAddTripTranslationService, AddTripTranslationService>();
            services.AddScoped<IGetTripTranslationsService, GetTripTranslationsService>();
            services.AddScoped<IGetTripTranslationByIdService, GetTripTranslationByIdService>();
            services.AddScoped<IUpdateTripTranslationService, UpdateTripTranslationService>();
            services.AddScoped<IDeleteTripTranslationService, DeleteTripTranslationService>();

            // Trip Include Use Case Services
            services.AddScoped<IAddTripIncludeService, AddTripIncludeService>();
            services.AddScoped<IGetTripIncludesService, GetTripIncludesService>();
            services.AddScoped<IGetTripIncludeByIdService, GetTripIncludeByIdService>();
            services.AddScoped<IUpdateTripIncludeService, UpdateTripIncludeService>();
            services.AddScoped<IDeleteTripIncludeService, DeleteTripIncludeService>();

            // Trip Exclude Use Case Services
            services.AddScoped<IAddTripExcludeService, AddTripExcludeService>();
            services.AddScoped<IGetTripExcludesService, GetTripExcludesService>();
            services.AddScoped<IGetTripExcludeByIdService, GetTripExcludeByIdService>();
            services.AddScoped<IUpdateTripExcludeService, UpdateTripExcludeService>();
            services.AddScoped<IDeleteTripExcludeService, DeleteTripExcludeService>();

            // Trip FAQ Use Case Services
            services.AddScoped<IAddTripFAQService, AddTripFAQService>();
            services.AddScoped<IGetTripFAQsService, GetTripFAQsService>();
            services.AddScoped<IGetTripFAQByIdService, GetTripFAQByIdService>();
            services.AddScoped<IUpdateTripFAQService, UpdateTripFAQService>();
            services.AddScoped<IDeleteTripFAQService, DeleteTripFAQService>();

            // Testimonial Use Case Services
            services.AddScoped<ICreateTestimonialService, CreateTestimonialService>();
            services.AddScoped<IGetTestimonialsService, GetTestimonialsService>();
            services.AddScoped<IGetTestimonialByIdService, GetTestimonialByIdService>();
            services.AddScoped<IUpdateTestimonialService, UpdateTestimonialService>();
            services.AddScoped<IDeleteTestimonialService, DeleteTestimonialService>();

            // Banner Use Case Services
            services.AddScoped<ICreateBannerService, CreateBannerService>();
            services.AddScoped<IGetBannersService, GetBannersService>();
            services.AddScoped<IGetBannerByIdService, GetBannerByIdService>();
            services.AddScoped<IUpdateBannerService, UpdateBannerService>();
            services.AddScoped<IDeleteBannerService, DeleteBannerService>();
            services.AddScoped<IGetPublicBannersService, GetPublicBannersService>();

            // CMS Section Use Case Services
            services.AddScoped<ICreateCMSSectionService, CreateCMSSectionService>();
            services.AddScoped<IGetCMSSectionsService, GetCMSSectionsService>();
            services.AddScoped<IGetCMSSectionByIdService, GetCMSSectionByIdService>();
            services.AddScoped<IUpdateCMSSectionService, UpdateCMSSectionService>();
            services.AddScoped<IDeleteCMSSectionService, DeleteCMSSectionService>();
            services.AddScoped<IGetPublicCMSSectionsService, GetPublicCMSSectionsService>();

            // Site Settings Use Case Services
            services.AddScoped<IGetSiteSettingsService, GetSiteSettingsService>();
            services.AddScoped<ICreateSiteSettingsService, CreateSiteSettingsService>();
            services.AddScoped<IUpdateSiteSettingsService, UpdateSiteSettingsService>();
            services.AddScoped<IGetPublicSiteSettingsService, GetPublicSiteSettingsService>();

            // Dashboard Use Case Services
            services.AddScoped<IGetDashboardSummaryService, GetDashboardSummaryService>();

            return services;
        }
    }
}

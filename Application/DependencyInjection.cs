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
using Application.Interfaces.TripHighlights;
using Application.Interfaces.TripImages;
using Application.Interfaces.TripIncludes;
using Application.Interfaces.TripItineraryItems;
using Application.Interfaces.Auth;
using Application.Interfaces.Trips;
using Application.Services.Auth;
using Application.Interfaces.TripTranslations;
using Application.Interfaces.TripWhatToBrings;
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
using Application.Services.TripHighlights;
using Application.Services.TripImages;
using Application.Services.TripIncludes;
using Application.Services.TripItineraryItems;
using Application.Services.Trips;
using Application.Services.TripTranslations;
using Application.Services.TripWhatToBrings;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddScoped<ICreateTripService, CreateTripService>();
            services.AddScoped<IGetTripByIdService, GetTripByIdService>();
            services.AddScoped<IGetTripBySlugService, GetTripBySlugService>();
            services.AddScoped<IGetPublicTripsService, GetPublicTripsService>();
            services.AddScoped<IGetAdminTripsService, GetAdminTripsService>();
            services.AddScoped<IGetAdminTripByIdService, GetAdminTripByIdService>();
            services.AddScoped<IUpdateTripService, UpdateTripService>();
            services.AddScoped<IDeleteTripService, DeleteTripService>();
            services.AddScoped<IPublishTripService, PublishTripService>();
            services.AddScoped<IUnpublishTripService, UnpublishTripService>();
            services.AddScoped<IDuplicateTripService, DuplicateTripService>();

            services.AddScoped<ICreateCategoryService, CreateCategoryService>();
            services.AddScoped<IGetCategoriesService, GetCategoriesService>();
            services.AddScoped<IGetCategoryByIdService, GetCategoryByIdService>();
            services.AddScoped<IUpdateCategoryService, UpdateCategoryService>();
            services.AddScoped<IDeleteCategoryService, DeleteCategoryService>();
            services.AddScoped<IGetPublicCategoriesService, GetPublicCategoriesService>();

            services.AddScoped<ICreateDestinationService, CreateDestinationService>();
            services.AddScoped<IGetDestinationsService, GetDestinationsService>();
            services.AddScoped<IGetDestinationByIdService, GetDestinationByIdService>();
            services.AddScoped<IUpdateDestinationService, UpdateDestinationService>();
            services.AddScoped<IDeleteDestinationService, DeleteDestinationService>();
            services.AddScoped<IGetPublicDestinationsService, GetPublicDestinationsService>();

            services.AddScoped<ICreateTourTypeService, CreateTourTypeService>();
            services.AddScoped<IGetTourTypesService, GetTourTypesService>();
            services.AddScoped<IGetTourTypeByIdService, GetTourTypeByIdService>();
            services.AddScoped<IUpdateTourTypeService, UpdateTourTypeService>();
            services.AddScoped<IDeleteTourTypeService, DeleteTourTypeService>();
            services.AddScoped<IGetPublicTourTypesService, GetPublicTourTypesService>();

            services.AddScoped<ICreateBookingInquiryService, CreateBookingInquiryService>();
            services.AddScoped<IGetBookingInquiriesService, GetBookingInquiriesService>();
            services.AddScoped<IGetBookingInquiryByIdService, GetBookingInquiryByIdService>();
            services.AddScoped<IUpdateBookingInquiryService, UpdateBookingInquiryService>();
            services.AddScoped<IDeleteBookingInquiryService, DeleteBookingInquiryService>();

            services.AddScoped<IUploadMediaService, UploadMediaService>();

            services.AddScoped<IAddTripImageService, AddTripImageService>();
            services.AddScoped<IGetTripGalleryService, GetTripGalleryService>();
            services.AddScoped<IUpdateTripImageService, UpdateTripImageService>();
            services.AddScoped<IDeleteTripImageService, DeleteTripImageService>();

            services.AddScoped<IAddTripTranslationService, AddTripTranslationService>();
            services.AddScoped<IGetTripTranslationsService, GetTripTranslationsService>();
            services.AddScoped<IGetTripTranslationByIdService, GetTripTranslationByIdService>();
            services.AddScoped<IUpdateTripTranslationService, UpdateTripTranslationService>();
            services.AddScoped<IDeleteTripTranslationService, DeleteTripTranslationService>();

            services.AddScoped<IAddTripIncludeService, AddTripIncludeService>();
            services.AddScoped<IGetTripIncludesService, GetTripIncludesService>();
            services.AddScoped<IGetTripIncludeByIdService, GetTripIncludeByIdService>();
            services.AddScoped<IUpdateTripIncludeService, UpdateTripIncludeService>();
            services.AddScoped<IDeleteTripIncludeService, DeleteTripIncludeService>();

            services.AddScoped<IAddTripExcludeService, AddTripExcludeService>();
            services.AddScoped<IGetTripExcludesService, GetTripExcludesService>();
            services.AddScoped<IGetTripExcludeByIdService, GetTripExcludeByIdService>();
            services.AddScoped<IUpdateTripExcludeService, UpdateTripExcludeService>();
            services.AddScoped<IDeleteTripExcludeService, DeleteTripExcludeService>();

            services.AddScoped<IAddTripHighlightService, AddTripHighlightService>();
            services.AddScoped<IGetTripHighlightsService, GetTripHighlightsService>();
            services.AddScoped<IGetTripHighlightByIdService, GetTripHighlightByIdService>();
            services.AddScoped<IUpdateTripHighlightService, UpdateTripHighlightService>();
            services.AddScoped<IDeleteTripHighlightService, DeleteTripHighlightService>();

            services.AddScoped<IAddTripWhatToBringService, AddTripWhatToBringService>();
            services.AddScoped<IGetTripWhatToBringsService, GetTripWhatToBringsService>();
            services.AddScoped<IGetTripWhatToBringByIdService, GetTripWhatToBringByIdService>();
            services.AddScoped<IUpdateTripWhatToBringService, UpdateTripWhatToBringService>();
            services.AddScoped<IDeleteTripWhatToBringService, DeleteTripWhatToBringService>();

            services.AddScoped<IAddTripItineraryItemService, AddTripItineraryItemService>();
            services.AddScoped<IGetTripItineraryItemsService, GetTripItineraryItemsService>();
            services.AddScoped<IGetTripItineraryItemByIdService, GetTripItineraryItemByIdService>();
            services.AddScoped<IUpdateTripItineraryItemService, UpdateTripItineraryItemService>();
            services.AddScoped<IDeleteTripItineraryItemService, DeleteTripItineraryItemService>();

            services.AddScoped<IAddTripFAQService, AddTripFAQService>();
            services.AddScoped<IGetTripFAQsService, GetTripFAQsService>();
            services.AddScoped<IGetTripFAQByIdService, GetTripFAQByIdService>();
            services.AddScoped<IUpdateTripFAQService, UpdateTripFAQService>();
            services.AddScoped<IDeleteTripFAQService, DeleteTripFAQService>();

            services.AddScoped<ICreateTestimonialService, CreateTestimonialService>();
            services.AddScoped<IGetTestimonialsService, GetTestimonialsService>();
            services.AddScoped<IGetTestimonialByIdService, GetTestimonialByIdService>();
            services.AddScoped<IUpdateTestimonialService, UpdateTestimonialService>();
            services.AddScoped<IDeleteTestimonialService, DeleteTestimonialService>();

            services.AddScoped<ICreateBannerService, CreateBannerService>();
            services.AddScoped<IGetBannersService, GetBannersService>();
            services.AddScoped<IGetBannerByIdService, GetBannerByIdService>();
            services.AddScoped<IUpdateBannerService, UpdateBannerService>();
            services.AddScoped<IDeleteBannerService, DeleteBannerService>();
            services.AddScoped<IGetPublicBannersService, GetPublicBannersService>();

            services.AddScoped<ICreateCMSSectionService, CreateCMSSectionService>();
            services.AddScoped<IGetCMSSectionsService, GetCMSSectionsService>();
            services.AddScoped<IGetCMSSectionByIdService, GetCMSSectionByIdService>();
            services.AddScoped<IUpdateCMSSectionService, UpdateCMSSectionService>();
            services.AddScoped<IDeleteCMSSectionService, DeleteCMSSectionService>();
            services.AddScoped<IGetPublicCMSSectionsService, GetPublicCMSSectionsService>();

            services.AddScoped<IGetSiteSettingsService, GetSiteSettingsService>();
            services.AddScoped<ICreateSiteSettingsService, CreateSiteSettingsService>();
            services.AddScoped<IUpdateSiteSettingsService, UpdateSiteSettingsService>();
            services.AddScoped<IGetPublicSiteSettingsService, GetPublicSiteSettingsService>();

            services.AddScoped<IGetDashboardSummaryService, GetDashboardSummaryService>();

            services.AddScoped<ILogoutService, LogoutService>();

            return services;
        }
    }
}

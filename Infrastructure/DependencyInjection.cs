using Application.Interfaces.Auth;
using Application.Interfaces.CurrentUser;
using Application.Interfaces.Repositories;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services.Auth;
using Infrastructure.Services.CurrentUser;
using Infrastructure.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                // Lockout
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<JwtOptions>(configuration.GetSection("JWT"));

            var jwtOptions = configuration.GetSection("JWT").Get<JwtOptions>()
                ?? throw new InvalidOperationException("JWT options are not configured in appsettings.json.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key)),
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDestinationRepository, DestinationRepository>();
            services.AddScoped<ITourTypeRepository, TourTypeRepository>();
            services.AddScoped<IBookingInquiryRepository, BookingInquiryRepository>();
            services.AddScoped<ITripImageRepository, TripImageRepository>();
            services.AddScoped<ITripTranslationRepository, TripTranslationRepository>();
            services.AddScoped<ITripIncludeRepository, TripIncludeRepository>();
            services.AddScoped<ITripExcludeRepository, TripExcludeRepository>();
            services.AddScoped<ITripItineraryItemRepository, TripItineraryItemRepository>();
            services.AddScoped<ITripHighlightRepository, TripHighlightRepository>();
            services.AddScoped<ITripWhatToBringRepository, TripWhatToBringRepository>();
            services.AddScoped<IFAQRepository, FAQRepository>();
            services.AddScoped<IFAQTranslationRepository, FAQTranslationRepository>();
            services.AddScoped<ITestimonialRepository, TestimonialRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<ICMSSectionRepository, CMSSectionRepository>();
            services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IAdminProfileService, AdminProfileService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IChangePasswordService, ChangePasswordService>();
            services.AddScoped<IAddAdminService, AddAdminService>();
            services.AddScoped<IGetAdminsService, GetAdminsService>();
            services.AddScoped<Application.Interfaces.Storage.IFileStorageService, Infrastructure.Services.Storage.LocalFileStorageService>();
            services.AddScoped<Application.Interfaces.IUnitOfWork.IUnitOfWork, Infrastructure.Services.UnitOfWork.UnitOfWork>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
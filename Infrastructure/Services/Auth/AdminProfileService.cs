using Application.DTOs.Auth;
using Application.Interfaces.Auth;
using Application.Interfaces.CurrentUser;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Auth
{
    public sealed class AdminProfileService : IAdminProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public AdminProfileService( UserManager<ApplicationUser> userManager,ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<AdminProfileDto> GetProfileAsync(CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync( _currentUserService.UserId.ToString());

            if (user is null)
            {
                throw new KeyNotFoundException("Admin profile not found.");
            }

            return new AdminProfileDto
            (
              FullName: user.FullName,
              Email: user.Email!,
              PhoneNumber: user.PhoneNumber
            );
        }
    }
}

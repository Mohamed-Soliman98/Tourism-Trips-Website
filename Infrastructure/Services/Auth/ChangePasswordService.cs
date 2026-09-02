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
    public sealed class ChangePasswordService : IChangePasswordService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public ChangePasswordService( UserManager<ApplicationUser> userManager,ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto,CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId.ToString());

            if (user is null)
            {
                throw new KeyNotFoundException("Admin user not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user,dto.CurrentPassword,dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ",result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }
        }
    }
}

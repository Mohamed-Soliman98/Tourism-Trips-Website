using Application.DTOs.Auth;
using Application.Interfaces.Auth;
using Domain.Enum;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Auth
{
    public sealed class AddAdminService : IAddAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AddAdminService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AdminCreatedDto> AddAdminAsync(CreateAdminRequestDto dto, CancellationToken cancellationToken)
        {
            
            var existing = await _userManager.FindByEmailAsync(dto.Email);

            if (existing is not null)
            {
                throw new InvalidOperationException("An account with this email already exists.");
            }

            var user = new ApplicationUser
            {
                Id           = Guid.NewGuid(),
                UserName     = dto.Email,
                Email        = dto.Email,
                FullName     = dto.FullName,
                EmailConfirmed = true
            };

            
            var createResult = await _userManager.CreateAsync(user, dto.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

    
            var roleResult = await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

            return new AdminCreatedDto(
                Id:       user.Id,
                FullName: user.FullName,
                Email:    user.Email!,
                Role:     UserRole.Admin.ToString()
            );
        }
    }
}

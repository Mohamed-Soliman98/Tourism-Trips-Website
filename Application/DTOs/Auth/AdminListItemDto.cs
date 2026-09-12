using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public record AdminListItemDto
    (
       Guid Id,
       string FullName,
       string Email,
       string Role
    );
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public sealed record AuthResultDto
    (
      string Token,
      DateTime ExpiresAt,
      string Email,
      string Role
    );
}

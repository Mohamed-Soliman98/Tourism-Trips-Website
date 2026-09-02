using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Auth
{
    public interface ITokenService
    {
        (string token, DateTime expiresOn) GenerateToken(Guid userId,string email,IEnumerable<string> roles);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace IncoMasterAPIService.Interfaces
{
    public interface IAuthenticationService
    {
        // Authenticate with token
        Task<string> JwtAuthenticate(string email, SecureString password);
    }
}

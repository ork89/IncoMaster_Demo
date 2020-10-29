using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUserData
    {
        Task<IList<UserModel>> GetUsersFromDB();
    }
}

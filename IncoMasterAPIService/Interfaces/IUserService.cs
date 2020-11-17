using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IncoMasterAPIService.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> LoginUserAsync(string email, string password);
        Task<List<UserModel>> GetAllAsync();
        Task<UserModel> GetByIdAsync(string id);
        Task<UserModel> GetByIdWithCategoriesAsync(string id);
        Task<UserModel> CreateAsync(UserModel user);
        Task UpdateAsync(string id, UserModel student);
        Task DeleteAsync(string id);
    }
}

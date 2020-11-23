using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IncoMasterAPIService.Interfaces
{
    public interface ICategoriesService
    {
        Task<List<CategoriesModel>> GetAllAsync();
        Task<CategoriesModel> GetByIdAsync(string id);
        Task<CategoriesModel> CreateAsync(CategoriesModel category);
        Task UpdateAsync(CategoriesModel category);
        Task DeleteAsync(string id);
    }
}

using Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IncoMasterAPIService.Services
{
    public class CategoriesService
    {
        private readonly IMongoCollection<CategoriesModel> _categories;

        public CategoriesService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DbName);
            _categories = database.GetCollection<CategoriesModel>(settings.CategoriesCollectionName);
        }

        public async Task<List<CategoriesModel>> GetAllAsync()
        {
            return await _categories.Find(s => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<CategoriesModel> GetByIdAsync(string id)
        {
            return await _categories.Find<CategoriesModel>(c => c.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<CategoriesModel> CreateAsync(CategoriesModel course)
        {
            await _categories.InsertOneAsync(course).ConfigureAwait(false);
            return course;
        }

        public async Task UpdateAsync(string id, CategoriesModel course)
        {
            await _categories.ReplaceOneAsync(c => c.Id == id, course).ConfigureAwait(false);
        }

        public async Task DeleteAsync(string id)
        {
            await _categories.DeleteOneAsync(c => c.Id == id).ConfigureAwait(false);
        }
    }
}

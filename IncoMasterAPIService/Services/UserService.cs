using HelperClasses;
using IncoMasterAPIService.Interfaces;
using Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;

namespace IncoMasterAPIService.Services
{
    public class UserService: IUserService
    {
        private readonly IMongoCollection<UserModel> _users;
        private readonly IMongoCollection<CategoriesModel> _categories;
        private readonly IMongoDatabase _mongoDb;
        private readonly string _dbName;
        private MongoClient _mongoClient;

        public UserService(IMongoDBSettings settings)
        {
            var client = _mongoClient ?? new MongoClient(settings.ConnectionString);
            var _dbName = client.GetDatabase(settings.DbName);
            _users = _dbName.GetCollection<UserModel>(settings.UsersCollectionName);
            _categories = _dbName.GetCollection<CategoriesModel>(settings.CategoriesCollectionName);
        }

        public async Task<UserModel> LoginUserAsync(string email, string password)
        {
            var credential = MongoCredential.CreateCredential(_dbName, email, password);
            var settings = new MongoClientSettings
            {
                Credential = credential
            };

            _mongoClient = new MongoClient(settings);

            var user = await _users.Find(u => u.Email == email && u.Password == password).FirstOrDefaultAsync().ConfigureAwait(false);

            return user;
        }

        public bool AuthenticateUser(string email, SecureString password)
        {
            var credential = MongoCredential.CreateCredential(_dbName, email, password);
            var settings = new MongoClientSettings
            {
                Credential = credential
            };

            _mongoClient = new MongoClient(settings);
            return _mongoClient != null;
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            return await _users.Find(s => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<UserModel> GetByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<UserModel> GetByIdWithCategoriesAsync(string id)
        {
            var user = await GetByIdAsync(id);
            if (user.Income != null && user.Income.Count > 0)
                user.IncomeList = await _categories.Find<CategoriesModel>(c => user.Income.Contains(c.Id)).ToListAsync().ConfigureAwait(false);
            
            if (user.Expenses != null && user.Expenses.Count > 0)
                user.ExpensesList = await _categories.Find<CategoriesModel>(c => user.Expenses.Contains(c.Id)).ToListAsync().ConfigureAwait(false);
            
            if (user.Savings != null && user.Savings.Count > 0)
                user.SavingsList = await _categories.Find<CategoriesModel>(c => user.Savings.Contains(c.Id)).ToListAsync().ConfigureAwait(false);
            
            if (user.Loans != null && user.Loans.Count > 0)
                user.LoansList = await _categories.Find<CategoriesModel>(c => user.Loans.Contains(c.Id)).ToListAsync().ConfigureAwait(false);

            return user;
        }

        public async Task<UserModel> CreateAsync(UserModel user)
        {
            await _users.InsertOneAsync(user).ConfigureAwait(false);
            return user;
        }

        public async Task UpdateAsync(string id, UserModel user)
        {
            await _users.ReplaceOneAsync(u => u.Id == id, user).ConfigureAwait(false);
        }

        public async Task InsertCategoryAsync(string categoryId, string categoryType, string userId)
        {
            var filter = Builders<UserModel>.Filter.Eq(x => x.Id, userId);
            var update = Builders<UserModel>.Update.Push(categoryType, categoryId);

            await _users.UpdateOneAsync(filter, update);
        }

        public async Task<bool> DeleteCategoryAsync(string categoryId, string categoryType, string userId)
        {
            var filter = Builders<UserModel>.Filter.Eq(x => x.Id, userId);
            var update = Builders<UserModel>.Update.Pull(categoryType, categoryId);

            var result = await _users.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task DeleteAsync(string id)
        {
            await _users.DeleteOneAsync(u => u.Id == id).ConfigureAwait(false);
        }
    }

}

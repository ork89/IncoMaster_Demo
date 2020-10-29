using Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IncoMasterAPIService.Services
{
    //public class UserService
    //{
    //    private readonly IMongoCollection<UserModel> _users;
    //    private readonly IMongoCollection<CategoriesModel> _categories;

    //    public UserService(IMongoDBSettings settings)
    //    {
    //        var mongoClient = new MongoClient(settings.ConnectionString);
    //        var db = mongoClient.GetDatabase(settings.DbName);

    //        _users = db.GetCollection<UserModel>(settings.UsersCollectionName);
    //        _categories = db.GetCollection<CategoriesModel>(settings.CategoriesCollectionName);
    //    }

    //    public async Task<List<UserModel>> GetUsers() => await _users.Find<UserModel>(usr => true).ToListAsync();

    //    public async Task<UserModel> GetUserByID(string id) => await _users.Find<UserModel>(user => user.Id == id).FirstOrDefaultAsync();

    //    public async Task<UserModel> GetUserByIDWithCategories(string id)
    //    {
    //        var user = await GetUserByID(id);
    //        if (user.Income != null && user.Income.Count > 0)
    //            user.IncomeList = await _categories.Find<CategoriesModel>(c => user.Income.Contains(c.Id)).ToListAsync();

    //        return user;
    //    }

    //    public async Task<UserModel> AddNewUser(UserModel user)
    //    {
    //        await _users.InsertOneAsync(user);
    //        return user;
    //    }

    //    public async Task UpdateUser(string id, UserModel user) => await _users.ReplaceOneAsync(u => u.Id == id, user);

    //    //public async Task RemoveUserFromDB(UserModel user) => await _users.DeleteOneAsync(u => u.Id == user.Id);
    //    public async Task RemoveUserByIDFromDB(string id) => await _users.DeleteOneAsync(user => user.Id == id);
    //}

    public class UserService
    {
        private readonly IMongoCollection<UserModel> _users;

        public UserService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DbName);
            _users = database.GetCollection<UserModel>(settings.UsersCollectionName);
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            return await _users.Find(s => true).ToListAsync();
        }

        public async Task<UserModel> GetByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<UserModel> CreateAsync(UserModel user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task UpdateAsync(string id, UserModel student)
        {
            await _users.ReplaceOneAsync(u => u.Id == id, student);
        }

        public async Task DeleteAsync(string id)
        {
            await _users.DeleteOneAsync(u => u.Id == id);
        }
    }

}

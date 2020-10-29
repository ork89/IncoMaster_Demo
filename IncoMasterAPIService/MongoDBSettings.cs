using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IncoMasterAPIService
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public string UsersCollectionName { get; set; }
        public string CategoriesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DbName { get; set; }
    }

    public interface IMongoDBSettings
    {
        public string UsersCollectionName { get; set; }
        public string CategoriesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DbName { get; set; }
    }
}

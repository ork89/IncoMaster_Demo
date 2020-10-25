using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Models
{
    public class Users
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string AccountType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Balance { get; set; }
    }
}

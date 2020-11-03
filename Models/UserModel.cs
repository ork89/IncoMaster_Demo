using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string AccountType { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Balance { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Income { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Expenses { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Savings { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Loans { get; set; }


        [BsonIgnore]
        public List<CategoriesModel> IncomeList { get; set; }

        [BsonIgnore]
        public List<CategoriesModel> ExpensesList { get; set; }

        [BsonIgnore]
        public List<CategoriesModel> SavingsList { get; set; }

        [BsonIgnore]
        public List<CategoriesModel> LoansList { get; set; }
    }
}

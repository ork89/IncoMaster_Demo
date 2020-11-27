using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcService.Common;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreGrpcClient
{
    public class CoreGrpcClient
    {
        private static UserModel _loggeduser;
        private static GrpcChannel _channel = GrpcChannel.ForAddress("https://localhost:5001");
        private static UserManagementService.UserManagementServiceClient userMngClient =
            new UserManagementService.UserManagementServiceClient(_channel);
        private static CategoryManagementService.CategoryManagementServiceClient categoryMngClient =
            new CategoryManagementService.CategoryManagementServiceClient(_channel);

        public CoreGrpcClient()
        {

        }

        public static async Task<UserModel> LoginUser(string email, string password)
        {
            var response = await userMngClient.LoginUserAsync(new LoginUserRequest { Email = email, Password = password });

            return GetLoggedUserDetails(response.User);
        }

        private static UserModel GetLoggedUserDetails(User user)
        {
            //TODO: Add Try-Catch and or throw exception
            if (user == null) return new UserModel();

            _loggeduser = new UserModel
            {
                Id = user.Id,
                AccountType = user.AccountType,
                FirstName = user.FName,
                LastName = user.LName,
                Balance = user.Balance,
            };

            CategoriesModel userCategory;
            InitializeUserLists(_loggeduser);

            foreach (var income in user.Income)
            {
                _loggeduser.Income.Add(income.Id);
                userCategory = new CategoriesModel
                {
                    Id = income.Id,
                    Category = income.Category_,
                    Title = income.Title,
                    Amount = income.Amount,
                    SubmitDate = income.SubmitDate.ToDateTime()
                };
                _loggeduser.IncomeList.Add(userCategory);
            }
            foreach (var expense in user.Expenses)
            {
                _loggeduser.Expenses.Add(expense.Id);
                userCategory = new CategoriesModel
                {
                    Id = expense.Id,
                    Category = expense.Category_,
                    Title = expense.Title,
                    Amount = expense.Amount,
                    SubmitDate = expense.SubmitDate.ToDateTime()
                };
                _loggeduser.ExpensesList.Add(userCategory);
            }
            foreach (var savings in user.Savings)
            {
                _loggeduser.Savings.Add(savings.Id);
                userCategory = new CategoriesModel
                {
                    Id = savings.Id,
                    Category = savings.Category_,
                    Title = savings.Title,
                    Amount = savings.Amount,
                    SubmitDate = savings.SubmitDate.ToDateTime()
                };
                _loggeduser.SavingsList.Add(userCategory);
            }
            foreach (var loan in user.Loans)
            {
                _loggeduser.Loans.Add(loan.Id);
                userCategory = new CategoriesModel
                {
                    Id = loan.Id,
                    Category = loan.Category_,
                    Title = loan.Title,
                    Amount = loan.Amount,
                    SubmitDate = loan.SubmitDate.ToDateTime()
                };
                _loggeduser.LoansList.Add(userCategory);
            }

            return _loggeduser;
        }

        private static UserModel InitializeUserLists(UserModel loggeduser)
        {
            loggeduser.Income = new List<string>();
            loggeduser.Expenses = new List<string>();
            loggeduser.Savings = new List<string>();
            loggeduser.Loans = new List<string>();

            loggeduser.IncomeList = new List<CategoriesModel>();
            loggeduser.ExpensesList = new List<CategoriesModel>();
            loggeduser.SavingsList = new List<CategoriesModel>();
            loggeduser.LoansList = new List<CategoriesModel>();

            return loggeduser;
        }

        public static async Task<string> AddOrUpdateUser(UserModel newUser)
        {
            var replay = await userMngClient.AddOrUpdateUserAsync(new AddOrUpdateUserRequest
            {
                User = new User
                {
                    FName = newUser.FirstName,
                    LName = newUser.LastName,
                    Email = newUser.Email,
                    Password = newUser.Password,
                    AccountType = newUser.AccountType,
                    Balance = newUser.Balance,
                }
            });

            return replay.Error;
        }

        public static async Task<string> AddCategory(CategoriesModel category, string userId)
        {
            var categoryReplay = await categoryMngClient.AddNewCategoryAsync(new AddNewCategoryRequest
            {
                Category = new SingleCategory
                {
                    Category = category.Category,
                    Title = category.Title,
                    Amount = category.Amount,
                    SubmitDate = DateTime.SpecifyKind(category.SubmitDate, DateTimeKind.Utc).ToTimestamp()
                }
            });

            if (!string.IsNullOrEmpty(categoryReplay.Error))
                return categoryReplay.Error;

            var InsertReplay = await userMngClient.InsertCategoryAsync(new InsertCategoryRequest
            {
                CategoryId = categoryReplay.CategoryId,
                CategoryType = category.Category,
                UserId = userId
            });

            return InsertReplay.Error;
        }

        public static async Task<string> UpdateCategory(CategoriesModel category, string id)
        {
            var replay = await categoryMngClient.UpdateCategoryAsync(new UpdateCategoryRequest
            {
                Category = new SingleCategory
                {
                    Id = category.Id,
                    Category = category.Category,
                    Title = category.Title,
                    Amount = category.Amount,
                    SubmitDate = DateTime.SpecifyKind(category.SubmitDate, DateTimeKind.Utc).ToTimestamp()
                }
            });

            return replay.Error;
        }

        public static async Task<string> DeleteCategory(string categoryId, string CategoryType, string userId)
        {
            var categoryReplay = await categoryMngClient.DeleteCategoryAsync(new DeleteCategoryRequest { Id = categoryId });

            if (!string.IsNullOrEmpty(categoryReplay.Error))
                return categoryReplay.Error;

            var deleteReplay = await userMngClient.DeleteCategoryFromUserAsync(new DeleteCategoryFromUserRequest
            {
                UserId = userId,
                CategoryId = categoryId,
                CategoryType = CategoryType
            });

            return deleteReplay.Error;
        }
    }
}

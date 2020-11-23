﻿using Google.Protobuf.WellKnownTypes;
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
        
        public CoreGrpcClient() { }

        //TODO: Delete all mock data after enabling user to insert his own data
        public static UserModel GenerateMockDataForUser(User user)
        {
            _loggeduser = new UserModel();
            CategoriesModel userCategory;
            var rnd = new Random();
            //double balance = rnd.NextDouble() * (11000 - 1500) + 1500;

            _loggeduser.FirstName = user.FName;
            _loggeduser.LastName = user.LName;
            _loggeduser.Balance = user.Balance;
            _loggeduser.AccountType = user.AccountType;

            InitializeUserLists(_loggeduser);

            foreach (var income in user.Income)
            {
                _loggeduser.Income.Add(income.Id);
                userCategory = new CategoriesModel
                {
                    Id = income.Id,
                    Category = income.Category_,
                    Title = income.Title,
                    Amount = rnd.Next(750, 6000), // Mock Data
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
                    Amount = rnd.Next(150, 3000), // Mock Data
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
                    Amount = rnd.Next(50, 10000), // Mock Data
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
                    Amount = rnd.Next(1000, 5000), // Mock Data
                    SubmitDate = loan.SubmitDate.ToDateTime()
                };
                _loggeduser.LoansList.Add(userCategory);
            }

            return _loggeduser;
        }

        public static async Task<UserModel> GetUsersRequest(string id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var userMngClient = new UserManagementService.UserManagementServiceClient(channel);

            Console.WriteLine("gRPC client is runing");

            var reply = await userMngClient.GetUserAsync(new GetUserRequest() { Id = id });
            if (reply.User != null)
            {
                _loggeduser = GenerateMockDataForUser(reply.User);
            }

            Console.WriteLine($"User first name: {reply.User.FName}\nUser last name: {reply.User.LName}");

            if (reply.Error != null && !string.IsNullOrWhiteSpace(reply.Error))
                Console.WriteLine($"Reply: {reply.Error}");

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
      
        public static async Task<UserModel> LoginUser(string email, string password)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var userMngClient = new UserManagementService.UserManagementServiceClient(channel);
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

        public static async Task<string> AddOrUpdateUser(UserModel newUser)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var userMngClient = new UserManagementService.UserManagementServiceClient(channel);

            var replay = await userMngClient.AddOrUpdateUserAsync(new AddOrUpdateUserRequest { User = new User 
                                                                                                            {   
                                                                                                                FName =  newUser.FirstName,
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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var categoryMngClient = new CategoryManagementService.CategoryManagementServiceClient(channel);
            var userMngClient = new UserManagementService.UserManagementServiceClient(channel);

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
            
            var InsertReplay = await userMngClient.InsertCategoryAsync(new InsertCategoryRequest {
                CategoryId = categoryReplay.CategoryId,
                CategoryType = category.Category,
                UserId = userId
            });

            return InsertReplay.Error;
        }

        public static async Task<string> UpdateCategory(CategoriesModel category, string id)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var categoryMngClient = new CategoryManagementService.CategoryManagementServiceClient(channel);

            var replay = await categoryMngClient.UpdateCategoryAsync(new UpdateCategoryRequest { Category = new SingleCategory { 
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
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var categoryMngClient = new CategoryManagementService.CategoryManagementServiceClient(channel);
            var userMngClient = new UserManagementService.UserManagementServiceClient(channel);

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

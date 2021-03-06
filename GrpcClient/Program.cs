﻿using Grpc.Net.Client;
using GrpcService.Common;
using Models;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrpcConsoleClient
{
    public class Program
    {
        private static UserModel _user;        

        public static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var userMngClient = new UserManagementService.UserManagementServiceClient(channel);

            _user = await GetUsersRequest(userMngClient);
            Console.ReadLine();
        }

        protected static async Task<UserModel> GetUsersRequest(UserManagementService.UserManagementServiceClient userMngClient)
        {
            Console.WriteLine("gRPC client is runing");
            UserModel loggeduser = new UserModel();
            var id = "5f987797d65f2130f0c61b7e";

            var reply = await userMngClient.GetUserAsync(new GetUserRequest() { Id = id });
            if (reply.User != null)
            {
                loggeduser.FirstName = reply.User.FName;
                loggeduser.LastName = reply.User.LName;
                loggeduser.Balance = reply.User.Balance;
                loggeduser.AccountType = reply.User.AccountType;
            }

            Console.WriteLine($"User first name: {reply.User.FName}\nUser last name: {reply.User.LName}");
            
            if (reply.Error != null && !string.IsNullOrWhiteSpace(reply.Error))
                Console.WriteLine($"Reply: {reply.Error}");

            return loggeduser;
        }
    }
}

using Google.Protobuf;
using Grpc.Net.Client;
using GrpcService.Common;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5000");
            var userMngClient = new UserManagementService.UserManagementServiceClient(channel);
            await GetUsersRequest(userMngClient);
        }

        private async static Task GetUsersRequest(UserManagementService.UserManagementServiceClient userMngClient)
        {
            var response = userMngClient.GetUsersFromDB(new GrpcService.Common.GetUsersRequest() { });
            while (await response.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
            {
                var currentUser = response.ResponseStream.Current.User;
                User loggedUser = new User
                {
                    Id = currentUser.Id,
                    AccountType = currentUser.AccountType,
                    FName = currentUser.FName,
                    LName = currentUser.LName,
                    Balance = currentUser.Balance
                };

                //Console.WriteLine($"ID: {currentUser.Id}\n" +
                //    $"Account Type: {currentUser.AccountType}\n" +
                //    $"First Name: {currentUser.FName}\n" +
                //    $"Last Name: {currentUser.LName}\n" +
                //    $"Balance: { currentUser.Balance}\n");
            }
        }
    }
}

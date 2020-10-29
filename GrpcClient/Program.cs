using Grpc.Net.Client;
using GrpcService.Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrpcClient
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // This switch must be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var userMngClient = new UserManagementService.UserManagementServiceClient(channel);

            await GetUsersRequest(userMngClient);
        }

        private async static Task GetUsersRequest(UserManagementService.UserManagementServiceClient userMngClient)
        {
            var response = userMngClient.GetUserAsync(new GetUserRequest() { });

            while(true)
            {
                Console.Write("Enter UserID: ");
                var id = Console.ReadLine();
                if (id == "exit") break;

                var reply = await userMngClient.GetUserAsync(new GetUserRequest() { Id = id });

                if (reply.User != null)
                    Console.WriteLine($"Reply: {reply.User}");

                if(reply.Error != null)
                    Console.WriteLine($"Reply: {reply.Error}");
            }

            Console.WriteLine("Exiting");


            //while (await response.ResponseAsync.MoveNext(new System.Threading.CancellationToken()))
            //{
            //    var currentUser = response.ResponseStream.Current.User;
            //    User loggedUser = new User
            //    {
            //        Id = currentUser.Id,
            //        AccountType = currentUser.AccountType,
            //        FName = currentUser.FName,
            //        LName = currentUser.LName,
            //        Balance = currentUser.Balance,
            //    };

            //    Console.WriteLine($"ID: {currentUser.Id}\n" +
            //        $"Account Type: {currentUser.AccountType}\n" +
            //        $"First Name: {currentUser.FName}\n" +
            //        $"Last Name: {currentUser.LName}\n" +
            //        $"Balance: { currentUser.Balance}\n");
            //}
        }
    }
}

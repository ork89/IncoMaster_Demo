using System.Threading.Tasks;
using Grpc.Core;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using GrpcService;
using Microsoft.Extensions.Logging;
using GrpcService.Protos;

namespace gRPC.GrpcService.Services
{
    public class UserService : UserManagementService.UserManagementServiceBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UserService> _logger;

        private MongoClient mongoClient = null;
        private IMongoDatabase mongoDB = null;
        private IMongoCollection<BsonDocument> mongoCollection = null;

        public UserService(IConfiguration config, ILogger<UserService> logger)
        {
            _config = config;
            _logger = logger;
            mongoClient = new MongoClient(_config["mongoDB"]);
            mongoDB = mongoClient.GetDatabase("IncoMaster");
            mongoCollection = mongoDB.GetCollection<BsonDocument>("Users");
        }

        public async override Task<AddNewUserResponse> AddNewUserToDB(AddNewUserRequest request, ServerCallContext context)
        {
            var user = request.NewUser;
            var newUser = AddNewUser(user);

            return new AddNewUserResponse
            {
                NewUser = newUser
            };
        }

        private User AddNewUser(User user)
        {
            //TODO: Change to dictionaty to insert all the relavent fileds for the User document.
            BsonDocument doc = new BsonDocument("UserId", user.Id)
                                                .Add("FirstName", user.FName)
                                                .Add("LastName", user.LName);
            mongoCollection.InsertOne(doc);

            var id = doc.GetValue("_id").ToString();
            user.Id = id;

            return user;
        }

        public override async Task GetUsersFromDB(GetUsersRequest request, IServerStreamWriter<GetUsersResponse> responseStream, ServerCallContext context)
        {
            //Don't filter out any user.
            var filter = new FilterDefinitionBuilder<BsonDocument>().Empty;
            var result = mongoCollection.Find(filter);
            var count = 0;

            foreach (var user in result.ToList())
            {
                await responseStream.WriteAsync(new GetUsersResponse
                {
                    User = new User
                    {
                        Id = user.GetValue("_id").ToString(),
                        FName = user.GetValue("FirstName").ToString(),
                        LName = user.GetValue("LastName").ToString(),
                    }
                });
                count++;
            }

            _logger.LogInformation($"\nFound {count} users.\n");
        }
    }
}

using System;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using GrpcService.Common;
using IncoMasterAPIService.Services;

namespace IncoMasterAPIService.Controllers
{
    public class UserGrpcController : UserManagementService.UserManagementServiceBase
    {
        private readonly UserService _UserService;
        private readonly IMapper _mapper;

        public UserGrpcController(UserService UserService, IMapper mapper)
        {
            _UserService = UserService;
            _mapper = mapper;
        }

        public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            try
            {
                if (request.Id != null)
                {
                    var user = await _UserService.GetByIdAsync(request.Id);
                    return new GetUserResponse
                    {
                        User = _mapper.Map<User>(user)
                    };
                }
                else
                {
                    return new GetUserResponse
                    {
                        Error = "User ID is null or empty"
                    };
                }
            }
            catch (Exception ex)
            {

                return new GetUserResponse
                { Error = $"{ ex.Message }" };
            };

        }
    }
}

using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService.Common;
using IncoMasterAPIService.Services;
using Models;

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

        public override async Task<LoginUserResponse> LoginUser(LoginUserRequest request, ServerCallContext context)
        {
            try
            {
                LoginUserResponse response = new LoginUserResponse();
                var user = await _UserService.LoginUserAsync(request.Email, request.Password);

                if (user != null)
                {
                    var userToLogin = await _UserService.GetByIdWithCategoriesAsync(user.Id);                    
                    response.User = _mapper.Map<User>(userToLogin);
                }

                return response;
            }
            catch (Exception exc)
            {

                return new LoginUserResponse { Error = $"Login Faild {exc.Message}" };
            }
        }

        public override async Task<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest request, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.Password) )
                    return new AuthenticateUserResponse { Error = "Authentication Faild" };

                var auth = _UserService.AuthenticateUser(request.Email, new NetworkCredential("", request.Password).SecurePassword);
                return new AuthenticateUserResponse { Success = true };
            }
            catch (Exception ex)
            {
                return new AuthenticateUserResponse { Error = "Authentication Faild"};
            }
        }

        public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            try
            {
                if (request.Id != null)
                {
                    var user = await _UserService.GetByIdWithCategoriesAsync(request.Id);

                    GetUserResponse response = new GetUserResponse();
                    response.User = _mapper.Map<User>(user);

                    return response;
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

        public override async Task<AddOrUpdateUserResponse> AddOrUpdateUser(AddOrUpdateUserRequest request, ServerCallContext context)
        {
            try
            {
                if (request.User != null)
                {
                    var newUser = _mapper.Map<UserModel>(request.User);
                    var result = await _UserService.CreateAsync(newUser);

                    return new AddOrUpdateUserResponse
                    {
                        Success = true
                    };
                }
                else
                {
                    return new AddOrUpdateUserResponse { Error = "Unable to register user" };
                }
            }
            catch (Exception ex)
            {
                return new AddOrUpdateUserResponse { Error = $"{ex.Message}" };
            }
        }
    }
}

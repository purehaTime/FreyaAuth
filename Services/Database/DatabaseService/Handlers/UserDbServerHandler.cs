using ConfigHelper.Configs;
using DatabaseService.Interfaces.Handlers;
using DatabaseService.Interfaces.Services;
using Google.Protobuf.WellKnownTypes;
using GrpcHelper.Proto.Common;
using GrpcHelper.Proto.Db;
using Microsoft.Extensions.Options;
using Models.Mapping;
using Models.Models;
using MongoDB.Bson;

namespace DatabaseService.Handlers;

public class UserDbServerHandler : IUserDbServerHandler
{
    private readonly IUserService _userService;
    private readonly JwtSettings _jwtOption;
    
    public UserDbServerHandler(IUserService userService, IOptions<JwtSettings> jwtOption)
    {
        _userService = userService;
        _jwtOption = jwtOption.Value;
    }
    
    public async Task<RefreshTokenResponse> GetRefreshToken(Id request)
    {
        var objectId = ObjectId.Parse(request.ObjectId);
        var user = await _userService.GetUser(objectId);
        if (user.User != null)
        {
            return new RefreshTokenResponse()
            {
                Token = user.User.RefreshToken,
                Expired = user.User.RefreshTokenExpire?.ToTimestamp()
            };
        }
        
        return new RefreshTokenResponse()
        {
            Error = user.Error?.ToGrpcData()
        };
    }

    public async Task<Status> UpdateRefreshToken(UpdateTokenRequest request)
    {
        var objectId = ObjectId.Parse(request.UserId);
        var expired = DateTime.UtcNow.AddMinutes(_jwtOption.RefreshExpires);
        var result = await _userService.UpdateToken(objectId, request.Token, expired);
        
        if (result.Updated)
        {
            return new Status
            {
                Complete = true
            };
        }
        
        return new Status
        {
            Complete = false,
            Error = result.Error?.ToGrpcData()
        };
    }

    public async Task<UserModelResponse> GetUserByEmail(EmailRequest request)
    {
        var finder = new FindUser
        {
            Email = request.Email
        };
        var user = await _userService.FindUser(finder);
        var response = new UserModelResponse
        {
            User = user.User?.ToGrpcData(),
            Error = user.Error?.ToGrpcData(),
        };

        return response;
    }

    public async Task<Status> CreateUser(UserModel request)
    {
        var user = request.ToDbModel();
        var result = await _userService.CreateUser(user);
        return new Status()
        {
            Error = result.Error?.ToGrpcData(),
            Complete = result.Created
        };
    }
}
using Google.Protobuf.WellKnownTypes;
using GrpcHelper.Proto.Db;
using MongoDB.Bson;
using User = Models.DbModels.User;

namespace Models.Mapping;

public static class UserMapping
{
    public static UserModel ToGrpcData(this User user)
    {
        return new UserModel
        {
            Id = user.Id.ToString(),
            Created = user.Created.ToTimestamp(),
            Email = user.Email,
            Locked = user.Locked,
            Role = (Roles)user.Role,
            Verified = user.Verified,
            LockedDate = user.LockedDate?.ToTimestamp(),
            PasswordChanged = user.PasswordChanged.ToTimestamp(),
            PasswordHash = user.PasswordHash,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpire = user.RefreshTokenExpire?.ToTimestamp(),
            VerificationToken = user.VerificationToken,
            VerificationTokenExpire = user.VerificationTokenExpire?.ToTimestamp()
        };
    }
    
    public static User ToDbModel(this UserModel user)
    {
        return new User
        {
            Id = string.IsNullOrWhiteSpace(user.Id) ? ObjectId.GenerateNewId() : ObjectId.Parse(user.Id),
            Created = user.Created.ToDateTime(),
            Email = user.Email,
            Locked = user.Locked,
            Role = (Enums.Roles)user.Role,
            Verified = user.Verified,
            LockedDate = user.LockedDate?.ToDateTime(),
            PasswordChanged = user.PasswordChanged.ToDateTime(),
            PasswordHash = user.PasswordHash,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpire = user.RefreshTokenExpire?.ToDateTime(),
            VerificationToken = user.VerificationToken,
            VerificationTokenExpire = user.VerificationTokenExpire?.ToDateTime()
        };
    }
}
using Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.DbModels;

public class User
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool Verified { get; set; }
    public OAuthType OAuthType { get; set; }
    public bool UseF2A { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Created { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime PasswordChanged { get; set; }
    public bool Locked { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? LockedDate { get; set; }
    public Roles Role { get; set; }
    public string RefreshToken { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? RefreshTokenExpire { get; set; }
    public string VerificationToken { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? VerificationTokenExpire { get; set; }
}
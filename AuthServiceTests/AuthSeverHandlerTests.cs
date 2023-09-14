using AuthService.Interfaces;
using AuthService.Services;
using AutoFixture;
using FluentAssertions;
using GrpcHelper.Clients.Interfaces;
using GrpcHelper.Proto.Auth;
using GrpcHelper.Proto.Common;
using NSubstitute;

namespace AuthServiceTests;

[TestFixture]
public class AuthServerHandlerTests
{
    private IUserDatabaseClient _userDbClientMock;
    private IJwtTokenService _jwtTokenService;
    
    private IAuthSeverHandler _handler;

    private Fixture _fuxture = new();
    
    [SetUp]
    public void Setup()
    {
        _userDbClientMock = Substitute.For<IUserDatabaseClient>();
        _jwtTokenService = Substitute.For<IJwtTokenService>();

        _handler = new AuthServerHandler(_userDbClientMock, null);
    }

    [Test]
    public void Should_UpdateToken()
    {
        var token = _fuxture.Create<string>();
        var err = _fuxture.Create<Error>();
        var userId = _fuxture.Create<string>();
        var email = _fuxture.Create<string>();

        var request = _fuxture.Create<TokenRequest>();

        _jwtTokenService.VerifyToken(request.AuthToken).Returns(Task.FromResult(false));
        _jwtTokenService.GetUserData(request.AuthToken).Returns((userId, email));
        _userDbClientMock.GetRefreshToken(userId).ReturnsForAnyArgs(Task.FromResult((token, DateTime.Now, err)));
        
        
        var result = _handler.UpdateToken(request).GetAwaiter().GetResult();

        result.Should().NotBeNull();
        result.Error.Should().NotBeNull();
    }
}
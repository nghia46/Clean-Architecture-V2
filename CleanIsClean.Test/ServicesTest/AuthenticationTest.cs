using System.Linq.Expressions;
using CleanIsClean.Application.Services;
using CleanIsClean.Domain.ViewModels;
using CleanIsClean.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Telerik.JustMock;
using Newtonsoft.Json;

namespace CleanIsClean.Test.ServicesTest;
[TestClass]
public class AuthenticationTest
{
    // Define TestContext property
    public TestContext? TestContext { get; set; }
    [TestMethod]
    public async Task LoginGenerateRefreshToken_ReturnsValidJwtToken()
    {
        // Arrange
        var loginView = new LoginView
        {
            Email = "test@example2.com",
            Password = "password1234"
        };

        var users = new List<User>
    {
        new() {
            Id = Guid.NewGuid(),
            Username = "testUser",
            Email = "test@example.com",
            Password = "password"
        },
        new() {
            Id = Guid.NewGuid(),
            Username = "testUser2",
            Email = "test@example2.com",
            Password = "password2"
        }
    };

        var role = new Role
        {
            RoleId = Guid.NewGuid(),
            RoleName = "User" // Assign the role name here
        };

        var userRoles = new List<UserRole>
    {
        new()
        {
            UserRoleId = Guid.NewGuid(),
            UserId = users[0].Id,
            RoleId = role.RoleId // Use the role ID here
        },
        new()
        {
            UserRoleId = Guid.NewGuid(),
            UserId = users[1].Id,
            RoleId = role.RoleId // Use the role ID here
        },
    };

        string? Issuer = "https://TestIssuer.com";
        string? Audience = "https://TestAudience.com";
        string? Key = "f9f32ffd678c3f71738411850f8af8b1fc3f6b13b60817d86f370e0232fce69bc4b418315c8224c99e8180b84c0f7b12d20ba6545b7a38d6a5e184159d3d9a41";

        var userRepositoryMock = Mock.Create<IRepository<User>>();
        var userRoleRepositoryMock = Mock.Create<IRepository<UserRole>>();
        var roleRepositoryMock = Mock.Create<IRepository<Role>>();

        var configurationMock = Mock.Create<IConfiguration>();

        Mock.Arrange(() => userRepositoryMock.Get(
            Arg.IsAny<Expression<Func<User, bool>>>(),
            Arg.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
            Arg.IsAny<string>()))
            .Returns(Task.FromResult<IEnumerable<User>>(users.Cast<User>()));

        Mock.Arrange(() => userRoleRepositoryMock.Get(
            Arg.IsAny<Expression<Func<UserRole, bool>>>(),
            Arg.IsAny<Func<IQueryable<UserRole>, IOrderedQueryable<UserRole>>>(),
            Arg.IsAny<string>()))
            .Returns(Task.FromResult<IEnumerable<UserRole>>(userRoles.Cast<UserRole>()));

        Mock.Arrange(() => roleRepositoryMock.Get(
            Arg.IsAny<Expression<Func<Role, bool>>>(),
            Arg.IsAny<Func<IQueryable<Role>, IOrderedQueryable<Role>>>(),
            Arg.IsAny<string>()))
            .Returns(Task.FromResult<IEnumerable<Role>>(new List<Role> { role })); // Return the role here

        Mock.Arrange(() => configurationMock["Jwt:Issuer"]).Returns(Issuer);
        Mock.Arrange(() => configurationMock["Jwt:Audience"]).Returns(Audience);
        Mock.Arrange(() => configurationMock["Jwt:Key"]).Returns(Key);

        var authenticationService = new AuthenticationService(userRepositoryMock, userRoleRepositoryMock, configurationMock);
        // Act
        string? token = await authenticationService.Login(loginView);
        TestContext?.WriteLine(token);
        // Assert
        Assert.IsNotNull(token);
    }

    [TestMethod]
    public async Task Register_AddUserAsync_ValidUser_UserAddedWithUserRole()
    {
        // Arrange
        
        // Act
        
        // Assert

    }
}
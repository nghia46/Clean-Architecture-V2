using System.Linq.Expressions;
using CleanIsClean.Application.Services;
using CleanIsClean.Domain.ViewModels;
using CleanIsClean.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Cryptography;

namespace CleanIsClean.Test.Services;
[TestClass]
public class AuthenticationTest
{
    // Define TestContext property
    public TestContext? TestContext { get; set; }
    [TestMethod]
    public async Task GenerateRefreshToken_GeneratesRandom32ByteArray_ReturnsBase64String()
    {
        // Arrange
        var userRepositoryMock = new Mock<IRepository<User>>();
        var userRoleRepositoryMock = new Mock<IRepository<UserRole>>();
        var configurationMock = new Mock<IConfiguration>();


        var authenticationService = new AuthenticationService(userRepositoryMock.Object, userRoleRepositoryMock.Object, configurationMock.Object);

        // Act
        LoginView loginView = new LoginView
        {
            Email = "test@example.com",
            Password = "12345"
        };
        var user = new User
        {
            Username = "testUser",
            Email = "test@example.com",
            Password = "password"
        };

        userRepositoryMock.Setup(p => p.Get(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
            It.IsAny<string>()))
           .ReturnsAsync(new List<User> { user });

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = 1
        };

        userRoleRepositoryMock.Setup(repo => repo.Get(
                   It.IsAny<Expression<Func<UserRole, bool>>>(), // Accept any filter
                   It.IsAny<Func<IQueryable<UserRole>, IOrderedQueryable<UserRole>>>(), // Accept any orderBy
                   It.IsAny<string>())) // Accept any includeProperties
               .ReturnsAsync(new List<UserRole> { userRole });

        configurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns("issuer");
        configurationMock.SetupGet(x => x["Jwt:Audience"]).Returns("audience");
        var key = new byte[64]; // 64 bytes = 512 bits
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(key);
        }
        var base64Key = Convert.ToBase64String(key);
        configurationMock.SetupGet(x => x["Jwt:Key"]).Returns(base64Key);
        
        var token = await authenticationService.Login(loginView);

        TestContext?.WriteLine("Generated Token: " + token);
        // Assert
        Assert.IsNotNull(token);
        // Add more assertions if needed

    }
    [TestMethod]
    public async Task Register_AddUserAsync_ValidUser_UserAddedWithUserRole()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testUser",
            Email = "testUser@gmail.com",
            Password = "testPassword",
            FullName = "Test User Full Name",
        };

        var role = new Role
        {
            RoleId = 1,
            RoleName = "User",
        };

        var userRepositoryMock = new Mock<IRepository<User>>();
        userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                          .Returns(Task.CompletedTask);

        var userRoleRepositoryMock = new Mock<IRepository<UserRole>>();
        userRoleRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<UserRole>()))
                              .Returns(Task.CompletedTask);

        var roleRepositoryMock = new Mock<IRepository<Role>>();
        roleRepositoryMock.Setup(repo => repo.Get(
            It.IsAny<Expression<Func<Role, bool>>>(),
            It.IsAny<Func<IQueryable<Role>, IOrderedQueryable<Role>>>(),
            It.IsAny<string>()))
            .ReturnsAsync(new[] { role });

        var userService = new UserService(userRepositoryMock.Object, userRoleRepositoryMock.Object, roleRepositoryMock.Object);

        // Act
        await userService.AddUserAsync(user);

        // Assert
        userRepositoryMock.Verify(repo => repo.AddAsync(user), Times.Once);

        // Add verification for role existence and user role assignment
        userRoleRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<UserRole>()), Times.Once);
    }
}
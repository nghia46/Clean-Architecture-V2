using CleanIsClean.Application.Services;
using CleanIsClean.Domain.Interfaces;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Telerik.JustMock;

namespace CleanIsClean.Test.ServicesTest;
[TestClass]
public class UserServiceTest
{
    public TestContext? TestContext { get; set; }
    [TestMethod]
    public async Task GetUserById_ReturnUser()
    {
        // Arrange
        List<User?> users =
        [
        new() {
            Id = Guid.NewGuid(),
            Username = "testUser1",
            Email = "testUser1@gmail.com",
            Password = "testPassword1",
            FullName = "Test User 1 Full Name",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new() {
            Id = Guid.NewGuid(),
            Username = "testUser2",
            Email = "testUser2@gmail.com",
            Password = "testPassword2",
            FullName = "Test User 2 Full Name",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }
    ];

        var userRepositoryMock = Mock.Create<IRepository<User>>();
        var userRoleRepositoryMock = Mock.Create<IRepository<UserRole>>();
        var roleRepositoryMock = Mock.Create<IRepository<Role>>();
        Mock.Arrange(() => userRepositoryMock.Get(
            Arg.IsAny<Expression<Func<User, bool>>>(),
            Arg.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
            Arg.IsAny<string>()))
            .Returns(Task.FromResult<IEnumerable<User>>(users.Cast<User>()));

        var userService = new UserService(userRepositoryMock, userRoleRepositoryMock, roleRepositoryMock);

        // Act
        Guid? userId = users.FirstOrDefault()?.Id;
        User? user = await userService.GetUserByIdAsync(userId);

        string jsonExpectedUser = JsonConvert.SerializeObject(users.FirstOrDefault());
        string jsonActualUser = JsonConvert.SerializeObject(user);
        TestContext?.WriteLine("Expected User: " + jsonExpectedUser);
        TestContext?.WriteLine("Actual User: " + jsonActualUser);
        // Assert
        Assert.IsNotNull(user); // Ensure that the user is not null
        Assert.AreEqual(jsonExpectedUser, jsonActualUser); // Check if the retrieved user has the expected username
    }

    [TestMethod]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        // Arrange
        List<User> expectedUsers =
    [
        new() {
            Id = Guid.NewGuid(),
            Username = "testUser1",
            Email = "testUser1@gmail.com",
            Password = "testPassword1",
            FullName = "Test User 1 Full Name",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        },
        new() {
            Id = Guid.NewGuid(),
            Username = "testUser2",
            Email = "testUser2@gmail.com",
            Password = "testPassword2",
            FullName = "Test User 2 Full Name",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow

        },
            new() {
            Id = Guid.NewGuid(),
            Username = "testUser3",
            Email = "testUser3@gmail.com",
            Password = "testPassword3",
            FullName = "Test User 3 Full Name",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow

        }
    ];

        var userRepositoryMock = Mock.Create<IRepository<User>>();
        var userRoleRepositoryMock = Mock.Create<IRepository<UserRole>>();
        var roleRepositoryMock = Mock.Create<IRepository<Role>>();
        Mock.Arrange(() => userRepositoryMock.Get(
            Arg.IsAny<Expression<Func<User, bool>>>(),
            Arg.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
            Arg.IsAny<string>()
            ))
            .Returns(Task.FromResult<IEnumerable<User>>(expectedUsers));

        var userService = new UserService(userRepositoryMock, userRoleRepositoryMock, roleRepositoryMock);

        // Act
        var actualUsers = await userService.GetAllUsersAsync();

        string jsonExpectedUsers = JsonConvert.SerializeObject(expectedUsers);
        string jsonActualUsers = JsonConvert.SerializeObject(actualUsers);

        TestContext?.WriteLine("Expected Users: " + jsonExpectedUsers);
        TestContext?.WriteLine("Actual Users: " + jsonActualUsers);
        // Assert
        Assert.AreEqual(jsonExpectedUsers, jsonActualUsers);
    }

}


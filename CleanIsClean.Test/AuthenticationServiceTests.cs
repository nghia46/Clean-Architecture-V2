using CleanIsClean.Application.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

[TestClass]
public class AuthenticationServiceTests
{
    [TestMethod]
    public void GenerateRefreshToken_ShouldReturnValidToken()
    {
        // Arrange
        var authenticationService = new AuthenticationService();

        // Act
        var refreshToken = authenticationService.GenerateRefreshToken();

        // Assert
        Assert.IsNotNull(refreshToken);
        Assert.IsTrue(refreshToken.Length > 0);
    }
}
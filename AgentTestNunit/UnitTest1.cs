using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using AgentCheckListApi.Model;
using AgentCheckListApi.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Moq;
using Moq.Protected;

namespace AgentTestNunit;

public class Tests
{


    private Mock<MongoDbService<Organization>> _mongoDbServiceMock;
    private Mock<IOptions<JWT>> _optionsMock;
    private Mock<IMongoCollection<User>> _usersMock;
    private Mock<IMongoCollection<Permission>> _permissionsMock;
    private AuthService _authService;

    // inject those services 
    //  _mongoDbServiceMock.Setup(m => m.GetCollection<User>("users")).Returns(_usersMock.Object);
    public Tests(IOptions<JWT> options, AuthService authService)
    {
        _optionsMock.Setup(m => m.Value).Returns(options.Value);
        _mongoDbServiceMock.Setup(m => m.GetCollection<User>("users")).Returns(_usersMock.Object);
        _mongoDbServiceMock.Setup(m => m.GetCollection<Permission>("permissions")).Returns(_permissionsMock.Object);
        _mongoDbServiceMock.Setup(m => m.GetCollection<Organization>("organizations")).Returns(_mongoDbServiceMock.Object.GetCollection<Organization>("organizations"));
        _mongoDbServiceMock.Setup(m => m.GetCollection<User>("users")).Returns(_mongoDbServiceMock.Object.GetCollection<User>("users"));
        _mongoDbServiceMock.Setup(m => m.GetCollection<Permission>("permissions")).Returns(_mongoDbServiceMock.Object.GetCollection<Permission>("permissions"));

    }


    [Test]
    public void Authenticate_UserNotFound_ReturnsFailure()
    {
        var userName = "testUser";
        var password = "testPassword";

        var userList = new List<User>().AsQueryable();
        _usersMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(userList.Provider);
        _usersMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userList.Expression);
        _usersMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userList.ElementType);
        _usersMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(userList.GetEnumerator());

        // Act
        var result = _authService.Authnticate(userName, password);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.AreEqual("User Not Found", result.Message);
    }


    [Test]
    public void authenticate_with_incorrect_password_returns_failure()
    {
        // Arrange
        var authServiceMock = new Mock<IAuthService>();
        authServiceMock.Setup(x => x.Authnticate("existingUser", "wrongPassword"))
                       .Returns(new ServiceResult { Success = false });

        // Act
        var result = authServiceMock.Object.Authnticate("existingUser", "wrongPassword");

        // Assert
        Assert.IsFalse(result.Success);
    }
    // Correct credentials provide access to system features
    [Test]
    public void authenticate_with_correct_credentials_returns_success()
    {
        // Arrange
        var authServiceMock = new Mock<IAuthService>();
        authServiceMock.Setup(x => x.Authnticate("correctUser", "correctPassword")).Returns(new ServiceResult { Data = "asdas", Success = true });

        // Act
        var result = authServiceMock.Object.Authnticate("correctUser", "correctPassword");

        // Assert
        Assert.IsTrue(result.Success);
        authServiceMock.Verify(x => x.Authnticate("correctUser", "correctPassword"), Times.Once);
    }

}
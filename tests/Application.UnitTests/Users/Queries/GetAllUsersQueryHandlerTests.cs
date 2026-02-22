using Application.Users.Queries.GetAllUsers;
using Domain.Aggregates.Users;
using Domain.Aggregates.Users.Repositories;
using Moq;

namespace Application.UnitTests.Users.Queries;

[TestClass]
public class GetAllUsersQueryHandlerTests
{
    private Mock<IUserRepository> _mockUserRepository = null!;
    private GetAllUsersQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _handler = new GetAllUsersQueryHandler(_mockUserRepository.Object);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnListOfUsers_WhenUsersExist()
    {
        // Arrange
        var expectedUsers = new List<UserRoot>
        {
            new UserRoot
            {
                Id = 1,
                Name = "Juan Pérez",
                Email = "juan.perez@example.com",
                Role = "Administrator",
                IsActive = true
            },
            new UserRoot
            {
                Id = 2,
                Name = "María García",
                Email = "maria.garcia@example.com",
                Role = "Developer",
                IsActive = true
            }
        };

        _mockUserRepository
            .Setup(repo => repo.GetAllUsersAsync())
            .ReturnsAsync(expectedUsers);

        var query = new GetAllUsersQuery();

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Juan Pérez", result[0].Name);
        Assert.AreEqual("juan.perez@example.com", result[0].Email);
        Assert.AreEqual("Administrator", result[0].Role);
        Assert.IsTrue(result[0].IsActive);
        
        _mockUserRepository.Verify(repo => repo.GetAllUsersAsync(), Times.Once);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _mockUserRepository
            .Setup(repo => repo.GetAllUsersAsync())
            .ReturnsAsync(new List<UserRoot>());

        var query = new GetAllUsersQuery();

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
        
        _mockUserRepository.Verify(repo => repo.GetAllUsersAsync(), Times.Once);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldMapUserRootToDto_Correctly()
    {
        // Arrange
        var userRoot = new UserRoot
        {
            Id = 10,
            Name = "Test User",
            Email = "test@example.com",
            Role = "Tester",
            IsActive = false
        };

        _mockUserRepository
            .Setup(repo => repo.GetAllUsersAsync())
            .ReturnsAsync(new List<UserRoot> { userRoot });

        var query = new GetAllUsersQuery();

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        Assert.AreEqual(1, result.Count);
        var dto = result[0];
        Assert.AreEqual(userRoot.Id, dto.Id);
        Assert.AreEqual(userRoot.Name, dto.Name);
        Assert.AreEqual(userRoot.Email, dto.Email);
        Assert.AreEqual(userRoot.Role, dto.Role);
        Assert.AreEqual(userRoot.IsActive, dto.IsActive);
    }

    [TestMethod]
    public async Task HandleAsync_ShouldCallRepository_OnlyOnce()
    {
        // Arrange
        _mockUserRepository
            .Setup(repo => repo.GetAllUsersAsync())
            .ReturnsAsync(new List<UserRoot>());

        var query = new GetAllUsersQuery();

        // Act
        await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        _mockUserRepository.Verify(repo => repo.GetAllUsersAsync(), Times.Once);
        _mockUserRepository.VerifyNoOtherCalls();
    }
}

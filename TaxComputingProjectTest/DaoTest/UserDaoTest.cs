using Microsoft.EntityFrameworkCore;
using Moq;
using TaxComputingProject.Dao;
using TaxComputingProject.DBContext;
using TaxComputingProject.Model;
using TaxComputingProject.Services;

namespace TaxComputingProjectTest.DaoTest;

public class UserDaoTest
{
    private readonly IQueryable<User> _users = new List<User>
    {
        new()
        {
            Id = 1, Email = "Tom@email.com", PasswordHash = null, PasswordSalt = null, VerificationToken = "",
            VerifiedAt = null
        },
        new()
        {
            Id = 2, Email = "Amy@email.com", PasswordHash = null, PasswordSalt = null, VerificationToken = "",
            VerifiedAt = null
        },
        new()
        {
            Id = 3, Email = "Bob@email.com", PasswordHash = null, PasswordSalt = null, VerificationToken = "",
            VerifiedAt = null
        },
    }.AsQueryable();

    [Fact]
    public void Should_Return_True_When_User_Exist()
    {
        var mockSet = new Mock<DbSet<User>>();
        mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(_users.Provider);
        mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(_users.Expression);
        mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(_users.ElementType);
        mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => _users.GetEnumerator());
        var mockContext = new Mock<DataContext>();
        mockContext.Setup(dataContext => dataContext.Users).Returns(mockSet.Object);
        var userDao = new UserDaoImpl(mockContext.Object);
        Assert.True(userDao.FindUserByEmail("Tom@email.com"));
        Assert.False(userDao.FindUserByEmail("Lucas@email.com"));
    }
    
}
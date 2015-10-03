using NSubstitute;
using TraditionalLayering;
using TraditionalLayering.Model;
using TraditionalLayering.Repository;
using TraditionalLayering.Service;
using Xunit;

namespace TraditionalLayering_Tests
{
    public class When_getting_a_user 
    {
        [Fact]
        public void Should_return_null_if_no_user_found() 
        {
            var repository = Substitute.For<IAccountRepository>();
            repository.GetLoggedInUser(2).Returns(x => null);

            var target = new AccountService(repository);

            var user = target.GetLoggedInUser(2);

            Assert.Null(user);
        }

        [Fact]
        public void Should_return_user() {
            var repository = Substitute.For<IAccountRepository>();
            repository.GetLoggedInUser(1).Returns(
                new Person {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    EmailAddress = "john.smith@gmail.com"
                });

            var target = new AccountService(repository);

            var user = target.GetLoggedInUser(1);

            Assert.Equal("John", user.FirstName);
        }
    }

}

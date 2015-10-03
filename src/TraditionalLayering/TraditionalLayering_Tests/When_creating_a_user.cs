using NSubstitute;
using TraditionalLayering;
using TraditionalLayering.Model;
using TraditionalLayering.Repository;
using TraditionalLayering.Service;
using Xunit;

namespace TraditionalLayering_Tests 
{

    public class When_creating_a_user 
    {
        [Fact]
        public void Should_return_error()
        {
            var repository = Substitute.For<IAccountRepository>();
            repository.GetUserByEmail(Arg.Any<string>()).Returns(new Person());

            var target = new AccountService(repository);

            var result = target.Create(new Person());
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Should_return_no_errors() 
        {
            var repository = Substitute.For<IAccountRepository>();
            repository.Create(Arg.Any<Person>());

            var newPerson = new Person {
                FirstName = "Jane",
                LastName = "Doe",
                EmailAddress = "Jane.Doe@gmail.com"
            };

            var target = new AccountService(repository);

            var result = target.Create(newPerson);

            Assert.Empty(result);

        }
    }
}
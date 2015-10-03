using TraditionalLayering.Model;

namespace TraditionalLayering.Repository {
    public interface IAccountRepository
    {
        Person GetLoggedInUser(int id);

        Person GetUserByEmail(string emailAddress);

        void Create(Person person);
    }
}
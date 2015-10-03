using System.Collections.Generic;
using TraditionalLayering.Model;

namespace TraditionalLayering.Service {
    public interface IAccountService
    {
        Person GetLoggedInUser(int id);

        List<string> Create(Person person);
    }
}
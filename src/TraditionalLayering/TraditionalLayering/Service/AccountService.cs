using System.Collections.Generic;
using TraditionalLayering.Model;
using TraditionalLayering.Repository;

namespace TraditionalLayering.Service {
    public class AccountService:IAccountService
    {
        private readonly IAccountRepository accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public Person GetLoggedInUser(int id)
        {
            //Hit another layer just because the service isnt responsible for retrieving data
            return this.accountRepository.GetLoggedInUser(id);
        }

        public List<string> Create(Person person)
        {
            var errorList = new List<string>();
            var existingPerson = this.accountRepository.GetUserByEmail(person.EmailAddress);

            if (existingPerson != null)
            {
                errorList.Add("User already exists");
                return errorList;
            }

            //Other business logic that might do checks and return errors

            this.accountRepository.Create(person);

            return errorList;  
        }
    }
}
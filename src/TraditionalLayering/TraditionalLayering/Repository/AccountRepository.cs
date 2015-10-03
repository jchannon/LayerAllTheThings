using System.Collections.Generic;
using System.Linq;
using TraditionalLayering.Model;

namespace TraditionalLayering.Repository {
    public class AccountRepository : IAccountRepository
    {
        //Our DB
        public static List<Person> data = new List<Person>()
        { 
            { new Person{ Id = 1, FirstName = "Jim", LastName = "Parsons", EmailAddress = "jim@parsons.com" } }, 
            { new Person{ Id = 2, FirstName = "Fred", LastName = "Smith", EmailAddress = "fred@smith.com" } }, 
            { new Person{ Id = 3,FirstName = "Bob", LastName = "Hope", EmailAddress = "bob@hope.com" } }, 
            { new Person{ Id = 4, FirstName = "Bernard", LastName = "Targarian", EmailAddress = "bernard@targarian.com" } }, 
            { new Person{ Id = 5, FirstName = "Troy", LastName = "Vega", EmailAddress = "troy@vega.com" } }
        };


        public Person GetLoggedInUser(int id)
        {
            return data.FirstOrDefault(x => x.Id == id);
        }

        public Person GetUserByEmail(string emailAddress)
        {
            return data.FirstOrDefault(x => x.EmailAddress == emailAddress);
        }

        public void Create(Person person) {
            var maxId = data.Max(x => x.Id);
            person.Id = maxId + 1;
            data.Add(person);
        }
        
    }
}
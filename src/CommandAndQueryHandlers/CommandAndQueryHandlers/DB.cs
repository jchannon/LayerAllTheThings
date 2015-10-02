using System.Collections.Generic;

namespace QueryHandler
{
    public static class DB
    {
        //Our DB
        public static List<User> Data = new List<User>()
        { 
            new User{ Id = 1, FirstName = "Jim", LastName = "Parsons", EmailAddress = "jim@parsons.com" }, 
            new User{ Id = 2, FirstName = "Fred", LastName = "Smith", EmailAddress = "fred@smith.com" }, 
            new User{ Id = 3, FirstName = "Bob", LastName = "Hope", EmailAddress = "bob@hope.com" }, 
            new User{ Id = 4, FirstName = "Bernard", LastName = "Targarian", EmailAddress = "bernard@targarian.com" }, 
            new User{ Id = 5, FirstName = "Troy", LastName = "Vega", EmailAddress = "troy@vega.com" } 
        };
    }
}

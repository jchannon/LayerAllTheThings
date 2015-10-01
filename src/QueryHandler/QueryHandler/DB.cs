using System.Collections.Generic;

namespace QueryHandler
{
    public static class DB
    {
        //Our DB
        public static Dictionary<int, User> Data = new Dictionary<int, User>()
        { 
            { 1, new User{ FirstName = "Jim", LastName = "Parsons", EmailAddress = "jim@parsons.com" } }, 
            { 2, new User{ FirstName = "Fred", LastName = "Smith", EmailAddress = "fred@smith.com" } }, 
            { 3, new User{ FirstName = "Bob", LastName = "Hope", EmailAddress = "bob@hope.com" } }, 
            { 4, new User{ FirstName = "Bernard", LastName = "Targarian", EmailAddress = "bernard@targarian.com" } }, 
            { 5, new User{ FirstName = "Troy", LastName = "Vega", EmailAddress = "troy@vega.com" } }
        };
    }
}

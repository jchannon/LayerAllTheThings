using System.Linq;

namespace QueryHandler
{
    public class UserQueryHandler : IHandleQueries<IQuery<Person>, Person>
    {
        public Person Handle(IQuery<Person> query)
        {
            var userQuery = query as UserQuery;
            return DB.Data.FirstOrDefault(x => x.Key == userQuery.UserId).Value;
        }
    }
}

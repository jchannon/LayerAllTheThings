using System.Linq;
using System;

namespace QueryHandler
{
    public class UserQueryHandler : IHandleQueries<IQuery<User>, User>
    {
        public User Handle(IQuery<User> query)
        {
            var userQuery = query as UserQuery;
            return DB.Data.FirstOrDefault(x => x.Key == userQuery.UserId).Value;
        }

        public bool CanHandle(Type queryType)
        {
            return queryType == typeof(UserQuery);
        }
    }
}

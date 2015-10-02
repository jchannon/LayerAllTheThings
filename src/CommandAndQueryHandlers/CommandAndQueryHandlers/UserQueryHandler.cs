using System.Linq;
using System;

namespace QueryHandler
{
    public class UserQueryHandler : IQueryHandler<IQuery<User>, User>
    {
        public User Handle(IQuery<User> query)
        {
            var userQuery = query as UserQuery;
            var user = DB.Data.FirstOrDefault(x => x.Id == userQuery.UserId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            return user;
        }

        public bool CanHandle(Type queryType)
        {
            return queryType == typeof(UserQuery);
        }
    }
}

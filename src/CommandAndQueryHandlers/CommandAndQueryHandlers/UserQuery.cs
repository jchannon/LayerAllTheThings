namespace QueryHandler
{
    public class UserQuery : IQuery<User>
    {
        public int UserId { get; private set; }

        public UserQuery(int userId)
        {
            UserId = userId;
        }
    }
}

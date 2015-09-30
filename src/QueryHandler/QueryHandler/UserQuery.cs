namespace QueryHandler
{
    public class UserQuery : IQuery<Person>
    {
        public int UserId { get; private set; }

        public UserQuery(int userId)
        {
            UserId = userId;
        }
    }
}

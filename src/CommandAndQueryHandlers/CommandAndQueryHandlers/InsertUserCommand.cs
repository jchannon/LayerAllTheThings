namespace QueryHandler
{
    public class InsertUserCommand : ICommand<int>
    {
        public User User { get; private set; }

        public InsertUserCommand(User user)
        {
            this.User = user;
        }
    }
}

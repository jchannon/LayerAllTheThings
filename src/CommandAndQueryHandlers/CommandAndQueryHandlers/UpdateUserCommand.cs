namespace QueryHandler
{
    public class UpdateUserCommand : ICommand<int>
    {
        public User User { get; private set; }

        public UpdateUserCommand(User user)
        {
            this.User = user;
        }
    }
}

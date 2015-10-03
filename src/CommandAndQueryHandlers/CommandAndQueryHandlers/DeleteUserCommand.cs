namespace QueryHandler
{
    public class DeleteUserCommand : ICommand<int>
    {
        public int UserId { get; set; }

        public DeleteUserCommand(int userId)
        {
            this.UserId = userId;
        }
    }
}

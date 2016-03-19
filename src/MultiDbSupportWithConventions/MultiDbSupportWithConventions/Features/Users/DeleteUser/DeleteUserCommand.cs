using MediatR;

namespace MultiDbSupportWithConventions
{
    public class DeleteUserCommand : IRequest<int>
    {
        public DeleteUserCommand(int id)
        {
            this.Id = id;
        }

        public int Id { get; private set; }
    }
}


namespace MultiDbSupportWithConventions.Features.Users.GetUser
{
    using MediatR;

    public class GetUserQuery : IRequest<User>
    {
        public GetUserQuery(int userId)
        {
            this.UserId = userId;
        }

        public int UserId { get; private set; }
    }
}
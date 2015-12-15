namespace MultiDbSupportWithConventions.Features.Users
{
    using MediatR;

    public class UserInputModel : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
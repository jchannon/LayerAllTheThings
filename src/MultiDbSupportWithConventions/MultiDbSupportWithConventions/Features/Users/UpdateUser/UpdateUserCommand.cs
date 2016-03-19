using MediatR;

namespace MultiDbSupportWithConventions.Features.Users.UpdateUser
{
    public class UpdateUserCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName {get;set;}
        public string Email { get; set;}
    }
}
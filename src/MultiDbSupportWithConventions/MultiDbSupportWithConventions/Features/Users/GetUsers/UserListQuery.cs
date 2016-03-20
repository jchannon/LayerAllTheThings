namespace MultiDbSupportWithConventions.Features.Users.GetUsers
{
    using System.Collections.Generic;

    using MediatR;

    public class UserListQuery : IRequest<IEnumerable<User>>
    {
    }
}
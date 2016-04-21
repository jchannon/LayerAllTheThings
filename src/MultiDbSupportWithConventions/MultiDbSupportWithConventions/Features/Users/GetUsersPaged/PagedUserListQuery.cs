using System;
using MediatR;
using System.Collections.Generic;
using MultiDbSupportWithConventions.Features.Users;

namespace MultiDbSupportWithConventions
{
    public class PagedUserListQuery: IRequest<IEnumerable<User>>
    {
        
    }
}


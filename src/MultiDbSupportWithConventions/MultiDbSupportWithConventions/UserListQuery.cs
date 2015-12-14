namespace MultiDbSupportWithConventions
{
    using System.Collections.Generic;

    using MediatR;

    public class UserListQuery : IRequest<IEnumerable<User>>
    {
        public UserListQuery(int customerId)
        {
            this.CustomerId = customerId;
        }

        //Just an example of having parameters for queries
        public int CustomerId { get; private set; }
    }
}
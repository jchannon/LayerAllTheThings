using MultiDbSupportWithConventions.Features.Users;

namespace MultiDbSupportWithConventions.Features.Users.DataAccess
{
    public interface IGetUserByIdQuery
    {
        User Execute(int id);
    }
}
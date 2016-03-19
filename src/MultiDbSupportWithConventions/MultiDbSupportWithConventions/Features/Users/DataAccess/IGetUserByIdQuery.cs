using MultiDbSupportWithConventions.Features.Users;

namespace MultiDbSupportWithConventions.Features.Users.DataAccess
{
    public interface IGetUserByIdQuery
    {
        User GetUserById(int id);
    }
}
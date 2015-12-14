namespace MultiDbSupportWithConventions
{
    using System.Data;

    public interface IDbConnectionProvider
    {
        IDbConnection GetConnection();
    }
}

namespace MultiDbSupport
{
    using System.Data;

    public interface IDbConnectionProvider
    {
        IDbConnection GetConnection();
    }
}

namespace DbConnectionAndCommands
{
    using System.Data;

    public interface IDbConnectionProvider
    {
        IDbConnection GetConnection();
    }
}

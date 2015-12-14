namespace MultiDbSupport
{
    using System.Configuration;
    using System.Data;

    using Npgsql;

    public class PostgresConnectionProvider : IDbConnectionProvider
    {
        private readonly string connectionString;

        public PostgresConnectionProvider()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["mydb"].ConnectionString;
        }

        public IDbConnection GetConnection()
        {
            var connection = new NpgsqlConnection(this.connectionString);
            connection.Open();
            return connection;
        }
    }
}
namespace DbConnectionAndCommands
{
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Implement the IDbConnectionProvider interface.
    /// </summary>
    public class SqlServerConnectionProvider : IDbConnectionProvider
    {
        private readonly string connectionString;

        public SqlServerConnectionProvider()
        {
            this.connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString;
        }

        /// <summary>
        /// Return an open SqlConnection to the VQ database. 
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            var connection = new SqlConnection(this.connectionString);
            connection.Open();
            return connection;
        }
    }
}

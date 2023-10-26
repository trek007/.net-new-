using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DoneRightAddressAPI.DAL.DBInteractions
{
    public class Connection
    {
        #region Fields
        public static IConfigurationRoot Configuration { get; set; }
        SqlConnection sqlConnection = null;
        #endregion
        #region Methods
        /// <summary>
        /// Get Connection String From App-setting.
        /// </summary>
        /// <returns>Return Connection String.</returns>
        public static string GetDBConnection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            return Configuration.GetSection("ConnectionString").Value;
        }
        /// <summary>
        /// Open Database Connection.
        /// </summary>
        /// <returns>Return Open SQL Connection.</returns>
        public SqlConnection OpenConnection()
        {
            sqlConnection = new SqlConnection(GetDBConnection());
            if (sqlConnection.State == ConnectionState.Closed)
            {
                SqlConnection.ClearAllPools();
                sqlConnection.Open();
            }
            return sqlConnection;
        }
        /// <summary>
        /// Close Database Connection.
        /// </summary>
        public void CloseConnection()
        {
            sqlConnection.Close();
            sqlConnection.Dispose();
            SqlConnection.ClearAllPools();
        }
        #endregion
    }
}

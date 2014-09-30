using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Diagnose
{
    public class Database
    {
        /// <summary>
        /// Checks to see if a connection can be established to a
        /// MySQL database, either via an EntityFramework DBContext
        /// connectionstring or through a regular mysql connection string
        /// from the config file.
        /// </summary>
        /// <param name="key">The key to look for in the config file.</param>
        /// <param name="type">The type of the check to run.</param>
        /// <param name="source">The source of the configuration value to get the connectionstring from.</param>
        /// <returns></returns>
        public static Check MySQL(string key, Check.Type type, Check.Source source = Check.Source.AppSettings)
        {
            Check status = new Check(key, type);
            if (source == Check.Source.AppSettings)
            {
                // Get the value of the configuration from the source.
                string value = string.Empty;
                if (source == Check.Source.AppSettings)
                {
                    if (!Util.CheckConfigKey(key))
                        return status.Fail("Could not find app setting with the key " + key);

                    value = Util.GetConfigValue(key);
                }
                else if (source == Check.Source.ConnectionStrings)
                {
                    if (!Util.CheckConnectionStringKey(key))
                        return status.Fail("Could not find connection string with the key " + key);

                    value = Util.GetConnectionStringValue(key);
                }

                // Determine the connection string based on the database type.
                string connectionString = GetConnectionString(value, type);

                // Try connecting to the mysql database
                using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
                    TryOpenConnection(connection, ref status);
                    if (!status.pass)
                        return status;
                }
            }

            return status.Pass();
        }

        /// <summary>
        /// Checks to see if a connection can be established to a
        /// MSSQL database, either via an EntityFramework DBContext
        /// connectionstring or through a regular mysql connection string
        /// from the config file.
        /// </summary>
        /// <param name="key">The key to look for in the config file.</param>
        /// <param name="type">The type of the check to run.</param>
        /// <param name="source">The source of the configuration value to get the connectionstring from.</param>
        /// <returns></returns>
        public static Check MSSQL(string key, Check.Type type, Check.Source source = Check.Source.AppSettings)
        {
            Check status = new Check(key, type);
            if (source == Check.Source.AppSettings)
            {
                // Get the value of the configuration from the source.
                string value = string.Empty;

                // Determine the connection string based on the database type.
                string connectionString = GetConnectionString(value, type);

                // Try connecting to the MSSQL database
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    TryOpenConnection(connection, ref status);
                    if (!status.pass)
                        return status;
                }
            }

            return status.Pass();
        }

        /// <summary>
        /// Get the connection string from a static source or from an EF
        /// DB context string.
        /// </summary>
        /// <param name="connectionString">The connectionstring from config to check.</param>
        /// <param name="type">The type (EFContext or regular ConnectionString)</param>
        private static string GetConnectionString(string connectionString, Check.Type type) {
            if (type == Check.Type.EFContext)
            {
                var context = new System.Data.Entity.DbContext(connectionString);
                return context.Database.Connection.ConnectionString;
            }
            else if (type == Check.Type.ConnectionString)
            {
                return connectionString;
            }
            return connectionString;
        }

        /// <summary>
        /// Tries to establish a connection using the specified
        /// DB connection.
        /// </summary>
        /// <param name="connection">The mysql/mssql connection to connect to.</param>
        /// <param name="status">The status of the current check.</param>
        private static void TryOpenConnection(DbConnection connection, ref Check status)
        {
            try
            {
                connection.Open();
                status.pass = true;
            }
            catch (Exception ex)
            {
                status.Fail("Could not connect to the MySQL database " + connection.Database.ToString() + ". " + ex.Message);
                connection.Dispose();
            }
        }
    }
}

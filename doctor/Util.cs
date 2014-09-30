using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor
{
    public class Util
    {
        /// <summary>
        /// Checks to see if an appsettings key is present in the app/web.config file.
        /// </summary>
        /// <param name="key">The name of the key to check for.</param>
        public static bool CheckConfigKey(string key)
        {
            // Check if the config value exists
            string value = GetConfigValue(key);
            if (value == null)
                return false;

            return true;
        }

        /// <summary>
        /// Gets a config value from the app.config file.
        /// </summary>
        /// <param name="key">The name of the key to get.</param>
        public static string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Checks to see if the specified connection string exists
        /// in the config file.
        /// </summary>
        /// <param name="key">The name of the connection string to check for.</param>
        public static bool CheckConnectionStringKey(string key)
        {
            // Check if the config value exists
            string value = GetConnectionStringValue(key);
            if (value == null)
                return false;

            return true;
        }

        /// <summary>
        /// Gets the value of a connection string from the config file.
        /// </summary>
        /// <param name="key">The name of the connection string to get the value for.</param>
        public static string GetConnectionStringValue(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
    }
}

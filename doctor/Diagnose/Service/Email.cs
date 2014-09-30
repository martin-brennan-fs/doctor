using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doctor.Diagnose.Service
{
    public class Email
    {
        /// <summary>
        /// Checks to see if the mandrill service is
        /// contactable from the API key used.
        /// </summary>
        /// <param name="key">The app setting key to get the api key from.</param>
        /// <param name="type">The type of the check.</param>
        /// <param name="source">The source of the api key.</param
        public static Check Mandrill(string key, Check.Type type, Check.Source source = Check.Source.AppSettings)
        {
            Check status = new Check("Mandrill", type);

            if (source == Check.Source.AppSettings)
            {
                // Get the value of the configuration from the source.
                string value = string.Empty;
                if (!Util.CheckConfigKey(key))
                    return status.Fail("Could not find app setting with the key " + key);

                value = Util.GetConfigValue(key);

                // Set up the mandrill api.
                Mandrill.MandrillApi api = new global::Mandrill.MandrillApi(value);

                // Try and send the ping command to the mandrill api to see
                // if a connection can be made with the api key.
                try
                {
                    api.Ping();
                }
                catch (Exception ex)
                {
                    return status.Fail("Could not establish a connection to the Mandrill service. " + ex.Message);
                }
            }

            return status.Pass();
        }
    }
}

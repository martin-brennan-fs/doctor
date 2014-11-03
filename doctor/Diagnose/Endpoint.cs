using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Doctor.Diagnose
{
    public class Endpoint
    {
        public static Check HTTP(string name, string url)
        {
            Check status = new Check(name, Check.Type.Endpoint);

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "HEAD";
                request.Timeout = 1000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                int statusCode = (int)response.StatusCode;
                if (statusCode >= 500)
                {
                    return status.Fail("The url could be reached, but a status code in the 500 range was returned.");
                }
                else if (statusCode < 500)
                {
                    return status.Pass();
                }
            }
            catch (Exception ex)
            {
                return status.Fail("Could not connect to the url. Message: {0}", ex.Message.Replace(url, "*******"));
            }

            return status.Pass();
        }
    }
}

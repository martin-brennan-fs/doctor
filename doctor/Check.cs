using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor
{
    public class Check
    {
        public Check() { }
        public Check(string name, Type type) { 
            this.pass = false;
            this.message = "Check passed successfully.";
            this.watch = new System.Diagnostics.Stopwatch();
            this.watch.Start();
            this.name = name;
            this.type = type;
        }
        public Type type { get; set; }
        public string name { get; set; }
        public bool pass { get; set; }
        public string message { get; set; }
        public System.Diagnostics.Stopwatch watch { get; set; }
        public long ms { get { return this.watch.ElapsedMilliseconds; } }

        /// <summary>
        /// Makes the check pass, stopping the timer
        /// in the process and setting the fail message.
        /// </summary>
        public Check Fail(string message)
        {
            this.watch.Stop();
            this.pass = false;
            this.message = message;
            return this;
        }

        /// <summary>
        /// Makes the check pass, stopping the timer
        /// in the process.
        /// </summary>
        public Check Pass()
        {
            this.watch.Stop();
            this.pass = true;
            return this;
        }

        /// <summary>
        /// The type of the check being performed.
        /// </summary>
        public enum Type
        {
            ConnectionString = 1,
            Configuration = 2,
            Service = 3,
            EFContext = 4
        }

        /// <summary>
        /// The source of the configuration for the check.
        /// </summary>
        public enum Source
        {
            AppSettings = 1,
            ConnectionStrings = 2
        }
    }
}

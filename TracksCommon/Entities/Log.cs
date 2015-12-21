using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracksCommon.Entities
{
    public class Log
    {
        public readonly string type;
        public readonly string message;

        private Log(string type, string message)
        {
            this.type = type;
            this.message = message;
        }

        public static Log Info(string message)
        {
            return new Log("Info", message);
        }

        public static Log Error(string message)
        {
            return new Log("Error", message);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP.MicrosoftMQ
{
    public class MessageEntity
    {
        // IP address for message come from.
        public string Source { get; set; } = "";

        public DateTime DateTime { get; set; } = DateTime.Now;

        public string TypeName { get; set; } = "";


        public string Body { get; set; } = "";
    }
}

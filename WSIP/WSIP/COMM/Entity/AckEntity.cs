using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP.COMM
{
    public class AckEntity
    {
        public string SendFrom = "";
        public AckState Ack = AckState.Unknown;
        public DateTime Occur = DateTime.Now;
        public DateTime LastAck = new DateTime(2000, 1, 1);
    }
}

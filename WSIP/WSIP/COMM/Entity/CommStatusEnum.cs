using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP.COMM
{
    public enum CommStatus
    {
        Unknown,
        Offline,
        OnlineMonitor,
        OnlineControl,
    }

    public enum CommResult
    {
        NotAssign,
        Approve,
        Reject
    }

    public enum AckState
    {
        Unknown,
        Query,
        ACK,
    }
}

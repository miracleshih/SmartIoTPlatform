using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP.COMM
{
    public class COMMRequest
    {
        public CommStatus Current = CommStatus.Unknown;
        public CommStatus ChangeTo = CommStatus.Unknown;
        public CommResult Result = CommResult.NotAssign;    // For request is not assign, another side decide the approve or reject.
        public string Reason = "";

    }
}

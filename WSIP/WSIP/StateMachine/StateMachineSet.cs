using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSIP.StateMachine;

namespace WSIP
{
    public partial class StateReport
    {
        public string StateReportSet(StateEnum status)
        {
            SendState(status);

            throw new NotImplementedException();
        }
    }
}

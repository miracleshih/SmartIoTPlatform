using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP.StateMachine
{
    public class StateEntity
    {
        public string ActionCode = "StateReport";
        public StateEnum State = StateEnum.INIT;
    }
}

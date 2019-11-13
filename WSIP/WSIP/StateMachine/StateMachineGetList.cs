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
        public List<string> GetStateEnumList()
        {
            List<string> enumList = new List<string>();
            foreach (StateEnum val in Enum.GetValues(typeof(StateEnum)))
            {
                enumList.Add(val.ToString());
            }
            return enumList;
        }

        public StateEnum GetCurrentState()
        {
            throw new NotImplementedException();
        }


    }
}

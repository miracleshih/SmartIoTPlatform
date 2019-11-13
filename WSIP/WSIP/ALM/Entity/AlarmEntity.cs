using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP.ALM
{
    public class AlarmEntity : WAction
    {
        public int AlarmLevel = 0;
        public AlarmEnum AlarmType = AlarmEnum.Null;

        public AlarmEntity()
        {
            // Action = "S"
        }
    }
}

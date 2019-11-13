using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S2F123 : WAction
    {
        public int RoutineId;
        
        public S2F123()
        {
            ActionCode = this.GetType().ToString();
        }
    }
}

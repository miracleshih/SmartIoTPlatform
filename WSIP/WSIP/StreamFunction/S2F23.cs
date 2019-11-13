using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S2F23 : WAction
    {
        public int RoutineId;
        public int Period;
        public bool Enable;
        public List<string> TagId;
        
        public S2F23()
        {
            ActionCode = this.GetType().ToString();
        }
    }
}

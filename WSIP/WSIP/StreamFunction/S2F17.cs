using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S2F17 : WAction
    {
        public S2F17()
        {
            ActionCode = this.GetType().ToString();
        }
    }
}

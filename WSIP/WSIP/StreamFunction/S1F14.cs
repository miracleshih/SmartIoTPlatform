using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S1F14 : WAction
    {
        public string AckEqp;

        public S1F14()
        {
            ActionCode = this.GetType().ToString();
        }
    }

}

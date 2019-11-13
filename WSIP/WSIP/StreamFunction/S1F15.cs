using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S1F15 : WAction
    {
        public S1F15()
        {
            ActionCode = this.GetType().ToString();
        }
    }

}

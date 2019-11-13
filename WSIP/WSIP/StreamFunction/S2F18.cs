using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S2F18 : WAction
    {
        public DateTime DateTime;

        public S2F18()
        {
            ActionCode = this.GetType().ToString();
        }
    }
}

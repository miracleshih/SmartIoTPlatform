using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S1F11 : WAction
    {
        public List<string> TagId;

        public S1F11()
        {
            ActionCode = this.GetType().ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S2F13 : WAction
    {
        public List<string> TagId;

        public S2F13()
        {
            ActionCode = this.GetType().ToString();
        }
    }
}

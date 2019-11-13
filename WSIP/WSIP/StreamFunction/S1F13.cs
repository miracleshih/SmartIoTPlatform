using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S1F13 : WAction
    {
        public string Command;

        public S1F13()
        {
            ActionCode = this.GetType().ToString();
        }
    }

}

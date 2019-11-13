using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S1F2 : WAction
    {
        public string SoftwareName;
        public string SoftwareVersion;

        public S1F2()
        {
            ActionCode = this.GetType().ToString();
        }
    }
}

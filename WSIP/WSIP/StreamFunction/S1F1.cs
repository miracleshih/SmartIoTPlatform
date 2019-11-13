using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WSIP.COMM;

namespace WSIP
{
    public class S1F1 : WAction
    {
        // [XmlElement("Action")]
        //[XmlText]
        public string INFO;

        public S1F1()
        {
            ActionCode = this.GetType().ToString();
        }
    }
}

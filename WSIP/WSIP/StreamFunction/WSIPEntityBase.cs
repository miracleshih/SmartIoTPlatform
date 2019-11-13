using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WSIP;

namespace WSIP
{
    public class ActionBase
    {

    }

    public class WAction
    {

        [XmlAttribute("VendorName")]
        public string VendorName = "";

        [XmlAttribute("ServiceID")]
        public string ServiceID;

        [XmlAttribute("Action")]
        public string ActionCode = "";


        public WAction()
        {
            VendorName = Vendor.VendorName;
            ServiceID = Vendor.ServiceID;
        }
    }

}

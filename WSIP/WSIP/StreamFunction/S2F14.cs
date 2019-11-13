using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S2F14 : WAction
    {
        public List<TagID> TagIdList;

        public S2F14()
        {
            ActionCode = this.GetType().ToString();
        }
    }
}

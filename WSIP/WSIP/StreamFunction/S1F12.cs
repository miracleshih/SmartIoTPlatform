using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSIP
{
    public class S1F12 : WAction
    {
        public List<TagID> TagIdList;

        public S1F12()
        {
            ActionCode = this.GetType().ToString();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP.RMS
{
    public enum RecipeSourceEnum
    {
        Unknown,    // Unknown recipe source. Initial state in entity.
        Server,     // Version control server or recipe storage.
        Machine,    // The recipe used in machine.
        UserDefine, // User define recipe source.
    }

}

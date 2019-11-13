using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSIP
{
    public class Walsin
    {
    }

    public static class APP
    {
        public static string Version = $"WSIP_{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major}.{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor}";
    }


    public static class Vendor
    {
        public static string VendorName = "";

        public static string ServiceID = "";

        public static void Profile(string VenderName, string machineID)
        {
            VendorName = VenderName;
            ServiceID = machineID;
        }

    }


}

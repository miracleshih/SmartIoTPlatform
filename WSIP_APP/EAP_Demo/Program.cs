using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WSIP;
using WSIP.ALM;
using WSIP.COMM;
using WSIP.MicrosoftMQ;
using WSIP.RMS;
using WSIP.RS232;
using WSIP.StateMachine;


namespace EAP_Demo
{
    class Program
    {
        static void Main(string[] args)
        {

            System.Threading.Thread.Sleep(5000);

            CustomerEquipment customerApp = new CustomerEquipment();

            System.Threading.Thread.Sleep(2000);

            customerApp.ShutdownMachine();
        }

        static char[] schar = new char[] { '\n', '\r' };
        public static string RS232Income = "";
        public static bool ReceiveRS232(object mesg)
        {
            if (mesg is null)
                return false;
            if(mesg.GetType() == typeof(string))
            {

            }
            else if (mesg.GetType() == typeof(byte[]))
            {
                byte[] data = (byte[])mesg;
                string str = System.Text.Encoding.Default.GetString(data);
                RS232Income += str;
                string[] sline = RS232Income.Split(schar);
                Console.WriteLine("Get data");
                RS232Income = "";
            }

            return true;
        }

    }
}

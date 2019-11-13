using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSIP;
using WSIP.MicrosoftMQ;
using WSIP.RS232;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            MSMQ mq = new MSMQ("queue1", "queue2");


            WRS232 rs232 = new WRS232("COM6", 115200, Mytest);

            System.Threading.Thread.Sleep(5000);
        }

        public static bool Mytest(object mesg)
        {
            if (mesg is null)
                return false;
            if(mesg.GetType() == typeof(byte[]))
            {
                byte[] data = (byte[])mesg;
                string str = data.ByteToString();
                if(str.Length > 0)
                    Console.WriteLine($"Data: {str}");
            }
            return true;
        }
    }
}

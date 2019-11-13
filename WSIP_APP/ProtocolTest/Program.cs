using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using WSIP;

namespace ProtocolTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Register vendor and equipment id.
            Vendor.Profile("Vendor001", "EQ001");

            // Get MSMQ object.
            MSMQ mqtest = new MSMQ(new List<string> { @".\private$\IoTTemplate" }, new List<string> { @".\private$\IoTTemplate" });

            mqtest.NotifyRegister((_receiveNotify)ReceiveMQ);
            mqtest.StartReceive();

            S1F1 s1f1 = new S1F1() { INFO = "IDLE, test S1F1 action." };
            S1F2 s1f2 = new S1F2() { SoftwareName = "EAPSoftware", SoftwareVersion = "V1.0.0" };

            mqtest.Send(s1f1);
            mqtest.Send(s1f2);

            string strs1f1 = s1f1.SerializeObject();
            string strs1f2 = s1f2.SerializeObject();
            Console.WriteLine($"OUT: {strs1f1} {strs1f2}");

            System.Threading.Thread.Sleep(1000);
        }

        public static bool ReceiveMQ(object mesg)
        {
            if (mesg is null || mesg.GetType() != typeof(Message))
                return false;

            Message mq = (Message)mesg;

            if(mq.Label == typeof(S1F1).ToString())
            {
                Console.WriteLine($"Get MQ S1F1 data: {mq.Label}");

            }
            if (mq.Label == typeof(S1F2).ToString())
            {
                Console.WriteLine($"Get MQ S1F2 data: {mq.Label}");

            }

            return true;
        }
    }
}

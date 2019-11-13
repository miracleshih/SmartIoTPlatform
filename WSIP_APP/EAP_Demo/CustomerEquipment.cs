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

namespace EAP_Demo
{
    public class CustomerEquipment
    {
        public const string Vendor = "Customer Name";
        public const string MachineID = "EAP_0001";
        public const string Queue_Read = @".\private$\IoTTemplate";
        public const string Queue_Write = @".\private$\IoTTemplate";

        public string COMPort = "COM6";

        Alarm WalsinAlarm;
        // static MSMQ WalsinMSMQ;
        Recipe WalsinRecipe;
        StateReport WalsinST;
        COMMU WalsinComm;
        IConnect Platform = null;


        private string _LastError = "";
        public string LastError
        {
            get
            {
                return _LastError;
            }
            set
            {
                _LastError = value;

                if (_LastError.StartsWith("ERROR"))
                    Console.WriteLine(_LastError);
            }
        }

        SerialPort serial;


        public CustomerEquipment()
        {
            // if notify function is not required, set to null.
            if (!InitialWSIP((_receiveNotify)ReceiveByCustomer))        
            {
                Console.WriteLine("Walsin service initialize failed.");
                return;
            }

            // Swtich to online monitor.
            WalsinComm.OnlineControl();

            Console.WriteLine("EAP send customer equipment recipe to host.");
            SendRecipe();       // Send customer recipe if it is required.

            Console.WriteLine("EAP send APP start alarm to host.");
            WalsinAlarm.SendAlarm(AlarmEnum.APPStart);        // Send app start alram to host.

            // Customer code in here.
            serial = new SerialPort(COMPort, 115200);
            serial.DataReceived += Serial_DataReceived;
            try
            {
                serial.Open();
                serial.DiscardInBuffer();
                serial.DiscardOutBuffer();
                WalsinST.GoToRUNNING();
            }
            catch (Exception)
            {
                Console.WriteLine($"ERROR: EAP device port {serial.PortName} cannot open. Send fatal error to host.");
                WalsinAlarm.SendAlarm(AlarmEnum.FatalError);        // test operation error alram.
                WalsinComm.Offline();
                WalsinST.GoToDOWN();
                return;
            }
        }

        public bool InitialWSIP(_receiveNotify notify)
        {
            Console.WriteLine($"Hello Walsin! Version: {WSIP.APP.Version}");

            WSIP.Vendor.Profile(Vendor, MachineID);

            try
            {
                MSMQ mq = new MSMQ(Queue_Read, Queue_Write);
                mq.StartReceive();
                Platform = mq;
                WalsinComm = new COMMU(Platform);
                WalsinST = new StateReport(Platform);
                WalsinAlarm = new Alarm(Platform);
                WalsinRecipe = new Recipe(Platform);

                WalsinComm.OnlineMonitor();

                WalsinST.GoToIDLE();

                bool isError = false;
                if (isError)
                {
                    WalsinAlarm.SendAlarm(AlarmEnum.FatalError);
                    WalsinST.GoToDOWN();
                    return false;
                }

                if(!(notify is null))
                    Platform.NotifyRegister(notify);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Walsin platform initial failed.\n  {ex.ToString()}");
            }



            return true;
        }

        public bool ReceiveByCustomer(object mesg)
        {
            LastError = "";

            if (mesg is null || mesg.GetType() != typeof(Message))
                return false;

            Message message = (Message)mesg;

            if (message.Label == typeof(CustomerRecipeEntity).ToString())
            {   // If receive entity type is CustomerRecipeEntity
                try
                {
                    message.Formatter = new XmlMessageFormatter(new Type[] { typeof(CustomerRecipeEntity) });
                    CustomerRecipeEntity CurrentRecipe = (CustomerRecipeEntity)message.Body;
                    Console.WriteLine($"CustomerRecipeEntity: EAP get entity {CurrentRecipe.MachineID} return");
                }
                catch (Exception ex)
                {
                    LastError = $"ERROR: {ex.ToString()}";
                }

                return true;
            }
            //else if(message.Label == typeof(xxxx).ToString())
            //{     // You can put your entity type in here if more than one entity type need to process.
            //}

            LastError = "WARN: Mismatch message type in Recipe. Ignore.";
            return false;
        }

        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!serial.IsOpen)
                return;

            try
            {
                for (; ; )
                {
                    string str = serial.ReadLine();
                    if (str is null || str.Length <= 0)
                        return;
                    Match mt = Regex.Match(str, @"([\d]+)[\s]*cm");
                    if (mt.Groups.Count == 2)
                    {
                        int val = int.Parse(mt.Groups[1].Value.ToString());
                        Platform.Send(new EAPEntity() { Distance = val });
                    }
                }
            }
            catch (Exception)
            { }

        }

        public bool SendRecipe()
        {
            // Send one recipe to host to backup.
            CustomerRecipeEntity entity = new CustomerRecipeEntity()
            {
                MachineID = MachineID,
                TAG = "Tag001",
                RecipeFrom = RecipeSourceEnum.Machine,
                RecipeVersion = "0.0.1",
                RecipeContent = "Temperation=30,Speed=40,Width=10,Height=30",
                RecipeDescription = "Adjust speed recipe and test.",

                Recipe_1 = 5,
                Recipe_2 = "Customer's recipe 2",
            };
            WalsinRecipe.SendRecipe(entity);

            return true;
        }

        public void ShutdownMachine()
        {
            WalsinComm.Offline();
        }
    }
}

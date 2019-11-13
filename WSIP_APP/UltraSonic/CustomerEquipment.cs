using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Threading;
using WSIP;
using WSIP.ALM;
using WSIP.COMM;
using WSIP.MicrosoftMQ;
using WSIP.RMS;
using WSIP.StateMachine;

namespace UltraSonic
{
    public class CustomerEquipment: INotifyPropertyChanged
    {

        public const string Vendor = "Customer Name";
        public const string MachineID = "EAP_0001";
        public const string Queue_Read = @".\private$\queue1_out";
        public const string Queue_Write = @".\private$\queue1_in";


        public const int MaxSensorRecords = 400;

        // public string COMPort = "COM6";

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

        int _TotalIncome = 0;
        public int TotalIncome
        {
            get { return _TotalIncome; }
            set
            {
                _TotalIncome = value;
                OnPropertyChanged("TotalIncome");
            }
        }

        string _openPortButton = "Open";
        public string openPortButton
        {
            get { return _openPortButton; }
            set
            {
                _openPortButton = value;
                OnPropertyChanged("openPortButton");
            }
        }

        int _comSelect = 0;
        public int comSelect
        {
            get { return _comSelect; }
            set
            {
                _comSelect = value;
                OnPropertyChanged("comSelect");
            }
        }

        private int _incomeAverage = 0;
        public int incomeAverage
        {
            get
            {
                return _incomeAverage;
            }
            set
            {
                _incomeAverage = value;
                OnPropertyChanged("incomeAverage");
            }
        }



        private ChartValues<double> _Values1 = new ChartValues<double>();
        private ChartValues<double> _Values2 = new ChartValues<double>();
        private ChartValues<double> _Values3 = new ChartValues<double>();
        public ChartValues<double> Values1
        {
            get { return _Values1; }
            set
            {
                _Values1 = value;
                OnPropertyChanged("Values1");
            }
        }
        public ChartValues<double> Values2
        {
            get
            {
                return _Values2;
            }
            set
            {
                _Values2 = value;
                OnPropertyChanged("Values2");
            }
        }
        public ChartValues<double> Values3
        {
            get
            {
                return _Values3;
            }
            set
            {
                _Values3 = value;
                OnPropertyChanged("Values3");
            }
        }

        ObservableCollection<string> _comlist = new ObservableCollection<string>();
        public ObservableCollection<string> comlist
        {
            get
            {
                return _comlist;
            }
            set
            {
                _comlist = value;
                OnPropertyChanged("comlist");
            }
        }

        SerialPort serial = new SerialPort();
        DispatcherTimer _timer = new DispatcherTimer();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public CustomerEquipment()
        {
            //// if notify function is not required, set to null.
            if (!InitialWSIP((_receiveNotify)ReceiveByCustomer))
            {
                Console.WriteLine("Walsin service initialize failed.");
                return;
            }

            //List<string> mqsend = new List<string>() { @".\private$\queue5_in", @".\private$\queue5_out" };
            //IConnect Platform1 = new MSMQ(null, mqsend);
            //Platform1.Send(new GyroEntity { GyroX = 10});

            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += _timer_Tick;

            GetPortList();

            //// Swtich to online monitor.
            //WalsinComm.OnlineControl();
            //Console.WriteLine("EAP send customer equipment recipe to host.");
            //Console.WriteLine("EAP send APP start alarm to host.");
            //WalsinAlarm.SendAlarm(AlarmEnum.APPStart);        // Send app start alram to host.
            //// Customer code in here.
            //serial = new SerialPort(COMPort, 115200);
            //serial.DataReceived += Serial_DataReceived;
            //try
            //{
            //    serial.Open();
            //    serial.DiscardInBuffer();
            //    serial.DiscardOutBuffer();
            //    WalsinST.GoToRUNNING();
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine($"ERROR: EAP device port {serial.PortName} cannot open. Send fatal error to host.");
            //    WalsinAlarm.SendAlarm(AlarmEnum.FatalError);        // test operation error alram.
            //    WalsinComm.Offline();
            //    WalsinST.GoToDOWN();
            //    return;
            //}
        }

        public bool OpenPort()
        {
            if (comSelect < 0 || comSelect >= comlist.Count)
            {
                openPortButton = "Open";
                return false;
            }

            string xport = comlist[comSelect];
            if (xport is null || xport.Length <= 0)
            {
                openPortButton = "Open";
                return false;
            }

            if (openPortButton == "Open")
            {
                // string xport = cmbPort.Text;
                try
                {
                    serial.PortName = xport;
                    serial.BaudRate = 115200;
                    serial.DataReceived += Serial_DataReceived;
                    serial.Open();
                    openPortButton = "Close";

                    _timer.Start();
                }
                catch (Exception ex)
                {
                    LastError = $"ERROR: Port {xport} cannot open.";
                    return false;
                }
            }
            else
            {
                try
                {
                    openPortButton = "Open";
                    serial.Close();
                    _timer.Stop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.ToString()}");
                }

            }


            return true;
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            incomeAverage = TotalIncome;
            TotalIncome = 0;
        }


        public void GetPortList()
        {
            List<string> ComList = System.IO.Ports.SerialPort.GetPortNames().ToList<string>();
            for (int i = 0; i < ComList.Count; i++)
                comlist.Add(ComList[i]);

            if (comlist.Count > 0)
                comSelect = 0;
        }


        public bool InitialWSIP(_receiveNotify notify)
        {
            Console.WriteLine($"Hello Walsin! Version: {WSIP.APP.Version}");

            WSIP.Vendor.Profile(Vendor, MachineID);

            try
            {
                Platform = new MSMQ(Queue_Read, Queue_Write);
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

            if (mesg.GetType() != typeof(Message))
                return false;

            Message message = (Message)mesg;

            if (message.Label == typeof(GyroEntity).ToString())
            {   // If receive entity type is CustomerRecipeEntity
                try
                {
                    message.Formatter = new XmlMessageFormatter(new Type[] { typeof(GyroEntity) });
                    GyroEntity CurrentRecipe = (GyroEntity)message.Body;
                    Console.WriteLine($"CustomerRecipeEntity: EAP get entity {CurrentRecipe.GyroX} return");
                }
                catch (Exception ex)
                {
                    LastError = $"ERROR: {ex.ToString()}";
                }

                return true;
            }
            if (message.Label == typeof(UltraSonicEntity).ToString())
            {     // You can put your entity type in here if more than one entity type need to process.
            }

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
                        GetAndSendValue(val);
                        TotalIncome++;
                        continue;
                    }
                    mt = Regex.Match(str, @"([\d]+)%([\d]+)%([\d]+)");
                    if (mt.Groups.Count == 4)
                    {
                        int x = int.Parse(mt.Groups[1].Value.ToString());
                        int y = int.Parse(mt.Groups[2].Value.ToString());
                        int z = int.Parse(mt.Groups[3].Value.ToString());
                        GetAndSendValue(x, y, z);
                        TotalIncome++;
                        continue;
                    }
                }

            }
            catch (Exception)
            { }

        }

        public bool GetAndSendValue(int val)
        {
            Platform.Send(new UltraSonicEntity()
            {
                Function = 0,
                Distance = val
            });
            Values1.Add(val);
            Values2.Add(0);
            Values3.Add(0);
            if (Values1.Count > MaxSensorRecords)
            {
                Values1.RemoveAt(0);
                Values2.RemoveAt(0);
                Values3.RemoveAt(0);
            }

            return true;
        }

        public bool GetAndSendValue(int grx, int gry, int grz)
        {
            Platform.Send(new GyroEntity()
            {
                GyroX = grx,
                GyroY = gry,
                GyroZ = grz,
            });
            Values1.Add(grx);
            Values2.Add(gry);
            Values3.Add(grz);
            if (Values1.Count > MaxSensorRecords)
            {
                Values1.RemoveAt(0);
                Values2.RemoveAt(0);
                Values3.RemoveAt(0);
            }
            return true;
        }

        public void ShutdownMachine()
        {
            WalsinComm.Offline();
        }
    }
}

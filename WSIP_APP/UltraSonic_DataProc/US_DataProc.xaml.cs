using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Windows;
using UltraSonic;
using WSIP;
using WSIP.MicrosoftMQ;

namespace UltraSonic_DataProc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class US_DataProc : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public const int MaxSensorRecords = 200;       // unit: sample count.
        public const int MaxIndicatorRec = 200;         // unit: sample count.
        public const int AnalysisWindow = 5;           // unit: sample count.
        public const int Indicator_DistanceChange = 10; // unit: cm
        public const int Alarm_DistanceChange = 20;     // unit: cm
        public const int Indicator_ReportPeriod = 2000; // unit: ms.

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

        
        int _DataInCount = 0;
        public int DataInCount
        {
            get { return _DataInCount; }
            set
            {
                _DataInCount = value;
                OnPropertyChanged("DataInCount");
            }
        }

        
        int _DataIndicatorCount = 0;
        public int DataIndicatorCount
        {
            get { return _DataIndicatorCount; }
            set
            {
                _DataIndicatorCount = value;
                OnPropertyChanged("DataIndicatorCount");
            }
        }

        private ChartValues<int> _Values1 = new ChartValues<int>();
        public ChartValues<int> Values1
        {
            get { return _Values1; }
            set
            {
                _Values1 = value;
                OnPropertyChanged("Values1");
            }
        }

        private ChartValues<int> _Values2 = new ChartValues<int>();
        public ChartValues<int> Values2
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

        private ChartValues<int> _Values3 = new ChartValues<int>();
        public ChartValues<int> Values3
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


        int alarm = 0;

        private ChartValues<int> _Values_alarm = new ChartValues<int>();
        public ChartValues<int> Values_alarm
        {
            get
            {
                return _Values_alarm;
            }
            set
            {
                _Values_alarm = value;
                OnPropertyChanged("Values_alarm");
            }
        }


        MSMQ MQ_DataProcess;    // = new MSMQ(
                //@".\private$\queue2_out",
                //@".\private$\queue1_out");

        MSMQ MQ_Indicator = new MSMQ(
            @".\private$\queue3_out",
            @".\private$\queue2_out");

        MSMQ MQ_FDC = new MSMQ(
            @".\private$\queue4_out",
            @".\private$\queue3_out");


        public US_DataProc()
        {
            InitializeComponent();

            // MQ_DataProcess.ClearInQueue();
            MQ_DataProcess = new MSMQ(
                @".\private$\queue2_out",
                @".\private$\queue1_out");
            MQ_DataProcess.NotifyRegister((_receiveNotify)ReceiveSensorData);
            MQ_DataProcess.StartReceive();

            this.DataContext = this;
        }


        List<int> avgList = new List<int>();
        Stopwatch stp = Stopwatch.StartNew();
        int LastReportDistance = 0;
        int LastEventDistant = 0;

        private bool ReceiveSensorData(object mesg)
        {
            LastError = "";

            if (mesg.GetType() != typeof(Message))
                return false;

            Message message = (Message)mesg;

            if (message.Label == typeof(UltraSonicEntity).ToString())
            {
                DataInCount++;
                message.Formatter = new XmlMessageFormatter(new Type[] { typeof(UltraSonicEntity) });
                UltraSonicEntity entity = (UltraSonicEntity)message.Body;
                ProcessUltraSonic(entity);
                return true;
            }
            else if (message.Label == typeof(GyroEntity).ToString())
            {
                DataInCount++;
                message.Formatter = new XmlMessageFormatter(new Type[] { typeof(GyroEntity) });
                GyroEntity entity = (GyroEntity)message.Body;
                ProcessGyro(entity);
            }

            return true;
        }

        public void ProcessUltraSonic(UltraSonicEntity entity)
        {
            Values1.Add(entity.Distance);
            if (Values1.Count > MaxSensorRecords)
                Values1.RemoveAt(0);
            avgList.Add(entity.Distance);
            if (avgList.Count > AnalysisWindow)
                avgList.RemoveAt(0);

            List<int> avgcp = avgList.ToList();
            avgcp.Sort();
            int median = avgcp[avgcp.Count / 2];   //  (int)avgList.Average();
            Values2.Add(median);
            if (Values2.Count > MaxSensorRecords)
                Values2.RemoveAt(0);

            if (Math.Abs(median - LastReportDistance) > Indicator_DistanceChange || stp.ElapsedMilliseconds > Indicator_ReportPeriod)
            {
                MQ_Indicator.Send(new UltraSonicEntity() { Distance = median });

                DataIndicatorCount++;
                Values3.Add(median);
                if (Values3.Count > 1 && Math.Abs(median - LastEventDistant) > Alarm_DistanceChange)
                {
                    if (alarm > 50)
                        alarm = 0;
                    else
                        alarm = 100;

                    MQ_FDC.Send(new UltraSonicEntity() { Distance = alarm });
                }
                Values_alarm.Add(alarm);
                LastEventDistant = median;

                if (Values3.Count > MaxIndicatorRec)
                {
                    Values3.RemoveAt(0);
                    Values_alarm.RemoveAt(0);
                }

                LastReportDistance = median;
                stp.Restart();
            }

        }

        public void ProcessGyro(GyroEntity entity)
        {
            Values1.Add(entity.GyroX);
            if (Values1.Count > MaxSensorRecords)
                Values1.RemoveAt(0);
            avgList.Add(entity.GyroX);
            if (avgList.Count > AnalysisWindow)
                avgList.RemoveAt(0);

            List<int> avgcp = avgList.ToList();
            avgcp.Sort();
            int median = avgcp[avgcp.Count / 2];   //  (int)avgList.Average();
            Values2.Add(median);
            if (Values2.Count > MaxSensorRecords)
                Values2.RemoveAt(0);

            if (Math.Abs(median - LastReportDistance) > Indicator_DistanceChange || stp.ElapsedMilliseconds > Indicator_ReportPeriod)
            {
                MQ_Indicator.Send(new UltraSonicEntity() { GyroX = median });

                DataIndicatorCount++;
                Values3.Add(median);
                if (Values3.Count > 1 && Math.Abs(median - LastEventDistant) > Alarm_DistanceChange)
                {
                    if (alarm > 50)
                        alarm = 0;
                    else
                        alarm = 100;

                    MQ_FDC.Send(new UltraSonicEntity() { GyroX = alarm });
                }
                Values_alarm.Add(alarm);
                LastEventDistant = median;

                if (Values3.Count > MaxIndicatorRec)
                {
                    Values3.RemoveAt(0);
                    Values_alarm.RemoveAt(0);
                }

                LastReportDistance = median;
                stp.Restart();
            }

        }






    }
}

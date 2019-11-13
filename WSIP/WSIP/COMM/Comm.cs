using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using WSIP.COMM;

namespace WSIP
{
    public partial class COMMU
    {
        const int IntervalAck = 1000;
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

        private CommStatus commStatus = CommStatus.Offline; 

        /// <summary>
        /// Initial Recipe management service.
        /// </summary>
        public COMMU(IConnect platf)
        {
            Platform = platf;
            Platform.NotifyRegister((_receiveNotify)ReceiveAck);
            Platform.NotifyRegister((_receiveNotify)ReceiveCOMM);
        }

        public bool SendCOMMStatus(COMMRequest status)
        {
            if (Platform is null)
            {
                LastError = "ERROR: Platform is null";
                return false;
            }

            return Platform.Send(status);
        }

        public bool Offline()
        {
            Console.WriteLine("Change communication to Offline");

            //throw new NotImplementedException();
            return SendCOMMStatus(new COMMRequest() {
                Current = commStatus,
                ChangeTo = CommStatus.Offline,
                Reason = $"{Vendor.VendorName} - {Vendor.ServiceID} change from {commStatus.ToString()} to Offline"
            });
        }

        public bool OnlineMonitor()
        {
            Console.WriteLine("Change communication to OnlineMonitor");
            return SendCOMMStatus(new COMMRequest()
            {
                Current = commStatus,
                ChangeTo = CommStatus.OnlineMonitor,
                Reason = $"{Vendor.VendorName} - {Vendor.ServiceID} change from {commStatus.ToString()} to Online Monitor"
            });
        }

        public bool OnlineControl()
        {
            Console.WriteLine("Change communication to OnlineControl");
            return SendCOMMStatus(new COMMRequest()
            {
                Current = commStatus,
                ChangeTo = CommStatus.OnlineControl,
                Reason = $"{Vendor.VendorName} - {Vendor.ServiceID} change from {commStatus.ToString()} to Online Control"
            });
        }



    }
}

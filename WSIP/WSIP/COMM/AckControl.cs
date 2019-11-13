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
        AckEntity CurrentAck = new AckEntity();

        DateTime LastReceiveAck = new DateTime(2000, 1, 1);

        bool isAcking = false;

        public bool EnableAck()
        {
            // throw new NotImplementedException();
            Task.Factory.StartNew(DoAcking);

            return false;
        }

        private bool DoAcking()
        {
            if (Platform is null)
            {
                return false;
            }

            isAcking = true;

            for (; isAcking;)
            {
                SendAck();

                System.Threading.Thread.Sleep(IntervalAck);
            }
            isAcking = false;
            return true;
        }

        public bool DisableAck()
        {
            isAcking = false;

            return true;
        }

        public bool SendAck()
        {
            if (Platform is null)
            {
                LastError = "ERROR: Platform is null";
                return false;
            }

            AckEntity entity = new AckEntity() {
                Ack = AckState.ACK,
                LastAck = LastReceiveAck,
                SendFrom = Vendor.ServiceID };

            Platform.Send(entity);

            LastReceiveAck = DateTime.Now;

            return true;
        }

        private bool ReceiveAck(object mesg)
        {
            if (mesg is null || mesg.GetType() != typeof(Message))
                return false;
            Message message = (Message)mesg;

            LastError = "";
            if (Platform is null)
            {
                LastError = "ERROR: Null platform in StateMachine";
                return false;
            }

            if (message.Label != typeof(AckEntity).ToString())
            {
                LastError = "WARN: Mismatch message type in Recipe. Ignore.";
                return false;
            }

            try
            {
                message.Formatter = new XmlMessageFormatter(new Type[] { typeof(AckEntity) });
                CurrentAck = (AckEntity)message.Body;
                Console.WriteLine($"ACK: Get Ack from {CurrentAck.SendFrom} Content: {CurrentAck.Ack}");

                if (CurrentAck.Ack == AckState.ACK)
                {
                    // Get return from host.
                }
                else if(CurrentAck.Ack == AckState.Query)
                {
                    SendAck();
                }
                else
                {
                    LastError = $"ERROR: Uknown Ack state: {CurrentAck.Ack.ToString()} from {CurrentAck.SendFrom}";
                }

                LastReceiveAck = DateTime.Now;
            }
            catch (Exception ex)
            {
                LastError = $"ERROR: {ex.ToString()}";
            }

            return true;
        }

        private bool ReceiveCOMM(object mesg)
        {
            LastError = "";

            if (mesg.GetType() != typeof(Message))
                return false;

            Message message = (Message)mesg;

            if (Platform is null)
            {
                LastError = "ERROR: Null platform in StateMachine";
                return false;
            }

            if (message.Label != typeof(COMMRequest).ToString())
            {
                LastError = "WARN: Mismatch message type in Recipe. Ignore.";
                return false;
            }

            try
            {
                message.Formatter = new XmlMessageFormatter(new Type[] { typeof(COMMRequest) });
                COMMRequest CurrentCOMM = (COMMRequest)message.Body;
                Console.WriteLine($"COMM: Host get communication status {CurrentCOMM.ChangeTo}");
                CurrentCOMM.Current = CurrentCOMM.ChangeTo;
            }
            catch (Exception ex)
            {
                LastError = $"ERROR: {ex.ToString()}";
            }

            return true;
        }

    }
}

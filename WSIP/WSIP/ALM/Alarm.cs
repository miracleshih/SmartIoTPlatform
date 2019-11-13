using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using WSIP.ALM;

namespace WSIP
{
    public partial class Alarm
    {
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


        /// <summary>
        /// Initial alarm service.
        /// </summary>
        public Alarm(IConnect platf)
        {
            Platform = platf;
            Platform.NotifyRegister((_receiveNotify)ReceiveAlarm);
        }

        public bool SendAlarm(AlarmEnum alarm)
        {
            if (Platform is null)
                return false;

            AlarmEntity entity = new AlarmEntity()
            {
                AlarmLevel = 0,
                AlarmType = alarm,
            };
            Platform.Send(entity);
            return true;
        }

        private bool ReceiveAlarm(object mesg)
        {
            LastError = "";

            if (mesg is null || mesg.GetType() != typeof(Message))
                return false;
            Message message = (Message)mesg;

            if (Platform is null)
            {
                LastError = "ERROR: Null platform in StateMachine";
                return false;
            }

            if (message.Label != typeof(AlarmEntity).ToString())
            {
                LastError = "WARN: Mismatch message type in Alarm. Ignore.";
                return false;
            }

            try
            {
                message.Formatter = new XmlMessageFormatter(new Type[] { typeof(AlarmEntity) });
                AlarmEntity entity = (AlarmEntity)message.Body;

                Console.WriteLine($"Alarm: Host get alarm entity {entity.AlarmType.ToString()}");
                // Need to discuss later.
                //throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                LastError = $"ERROR: {ex.ToString()}";
            }

            return true;
        }

    }
}

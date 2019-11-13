using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using WSIP.StateMachine;

namespace WSIP
{
    public partial class StateReport
    {
        IConnect Platform = null;

        StateEnum CurrentState = StateEnum.INIT;

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
                if(_LastError.StartsWith("ERROR"))
                    Console.WriteLine(_LastError);
            }
        }

        /// <summary>
        /// Initial device/equipment's state machine.
        ///   Basic state is: IDLE, RUNNING, DOWN.
        ///   More detail please refer to StateMachineEnum in Entity.
        /// </summary>
        public StateReport(IConnect platf)
        {
            Platform = platf;
            Platform.NotifyRegister((_receiveNotify)ReceiveState);
        }


        public bool GoToIDLE()
        {
            Console.WriteLine("Change machine to IDLE state");
            return SendState(StateEnum.IDLE);
        }

        public bool GoToRUNNING()
        {
            Console.WriteLine("Change machine to RUNNING state");
            return SendState(StateEnum.RUN);
        }

        public bool GoToDOWN()
        {
            Console.WriteLine("Change machine to DOWN state");
            return SendState(StateEnum.DOWN);
        }


        public string SetCurrentState(StateEnum currentState)
        {
            throw new NotImplementedException();
        }

        private bool SendState(StateEnum status)
        {
            if (Platform is null)
                return false;

            StateEntity entity = new StateEntity()
            {
                State = status,
            };

            Platform.Send(entity);
            return true;
        }

        private bool ReceiveState(object mesg)
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

            if (message.Label != typeof(StateEntity).ToString())
            {
                return false;
            }

            try
            {
                // Console.WriteLine($"Message type: {message.Body.ToString()}");
                message.Formatter = new XmlMessageFormatter(new Type[] { typeof(StateEntity) });
                StateEntity entity = (StateEntity)message.Body;
                CurrentState = entity.State;
                Console.WriteLine($"ST: Host receive machine state: {entity.State.ToString()}");
            }
            catch(Exception ex)
            {
                LastError = $"ERROR: {ex.ToString()}";
            }

            return true;
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using WSIP.RMS;

namespace WSIP
{
    public partial class Recipe
    {
        IConnect Platform = null;

        RecipeEntity CurrentRecipe = new RecipeEntity();

        private string _LastError = "";
        public string LastError {
            get {
                return _LastError;
            }
            set {
                _LastError = value;

                if (_LastError.StartsWith("ERROR"))
                    Console.WriteLine(_LastError);
            }
        }

        /// <summary>
        /// Initial Recipe management service.
        /// </summary>
        public Recipe(IConnect platf)
        {
            Platform = platf;
            Platform.NotifyRegister((_receiveNotify)ReceiveRecipe);
        }


        public bool SendRecipe<T>(T entity)
        {
            if (Platform is null)
            {
                LastError = "ERROR: Platform is null";
                return false;
            }

            Platform.Send(entity);
            return true;
        }

        private bool ReceiveRecipe(object mesg)
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

            if (message.Label != typeof(RecipeEntity).ToString())
            {
                LastError = "WARN: Mismatch message type in Recipe. Ignore.";
                return false;
            }

            try
            {
                message.Formatter = new XmlMessageFormatter(new Type[] { typeof(RecipeEntity) });
                CurrentRecipe = (RecipeEntity)message.Body;
                Console.WriteLine($"Recipe: Get entity {CurrentRecipe.MachineID}");
            }
            catch (Exception ex)
            {
                LastError = $"ERROR: {ex.ToString()}";
            }

            return true;
        }



    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraSonic
{
    public class QueuePath : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _PathFrom = "";
        public string PathFrom
        {
            get
            {
                return _PathFrom;
            }
            set
            {
                _PathFrom = value;
                OnPropertyChanged("PathFrom");
            }
        }

        private List<string> _PathTo = new List<string>();
        public List<string> PathTo
        {
            get
            {
                return _PathTo;
            }
            set
            {
                _PathTo = value;
                OnPropertyChanged("PathTo");
            }
        }


        public override string ToString()
        {
            string str = PathFrom + " -->";
            for (int i = 0; i < PathTo.Count; i++)
                str += "\n    " + PathTo[i];

            return str;
        }


    }
}

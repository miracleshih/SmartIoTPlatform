using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RoutingManager
{
    public partial class RoutingManagerUI : Window, INotifyPropertyChanged
    {

        private List<string> _listQueue_in = new List<string>();
        public List<string> listQueue_in
        {
            get
            {
                return _listQueue_in;
            }
            set
            {
                _listQueue_in = value;
                OnPropertyChanged("listQueue_in");
            }
        }

        private List<string> _listQueue_out = new List<string>();
        public List<string> listQueue_out
        {
            get
            {
                return _listQueue_out;
            }
            set
            {
                _listQueue_out = value;
                OnPropertyChanged("listQueue_out");
            }
        }

        private ObservableCollection<string> _listLink = new ObservableCollection<string>();
        public ObservableCollection<string> listLink
        {
            get
            {
                return _listLink;
            }
            set
            {
                _listLink = value;
                OnPropertyChanged("listLink");
            }
        }


        


        private Image _placeImage = new Image();
        public Image placeImage
        {
            get
            {
                return _placeImage;
            }
            set
            {
                _placeImage = value;
                OnPropertyChanged("placeImage");
            }
        }


        MessageQueue[] QueueList = MessageQueue.GetPrivateQueuesByMachine(".");

        List<QueueItem> TotalQueueList = new List<QueueItem>();

        ObservableCollection<QueueItem> QItems = new ObservableCollection<QueueItem>();
        ObservableCollection<QueueLink> _QLink = new ObservableCollection<QueueLink>();
        public ObservableCollection<QueueLink> QLink
        {
            get { return _QLink; }
            set
            {
                _QLink = value;
                OnPropertyChanged("QLink");
            }
        }

    }
}

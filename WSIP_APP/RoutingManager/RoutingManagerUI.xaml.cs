using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RoutingManager
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class RoutingManagerUI : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Line newLine = new Line();

        public RoutingManagerUI()
        {
            InitializeComponent();

            MessageQueue[] QueueList = MessageQueue.GetPrivateQueuesByMachine(".");
            List<string> lst_in = new List<string>();
            List<string> lst_out = new List<string>();
            foreach (MessageQueue queueItem in QueueList)
            {
                lst_in.Add(queueItem.QueueName);
                lst_out.Add(queueItem.QueueName);
                //if (Regex.IsMatch(queueItem.QueueName, @"_in$"))
                //    lst_in.Add(queueItem.QueueName);
                //if (Regex.IsMatch(queueItem.QueueName, @"_out$"))
                //    lst_out.Add(queueItem.QueueName);
            }
            listQueue_in = lst_in;
            listQueue_out = lst_out;

            this.DataContext = this;
      
            PRMlaceAllQueue();

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectQ1.SelectedItems.Count <= 0 || selectQ2.SelectedItems.Count <= 0)
                return;

            TotalQueueList.Clear();

            for (int i = 0; i < selectQ1.SelectedItems.Count; i++)
            {
                AddIntoNodeList(selectQ1.SelectedItems[i].ToString());
            }
            for (int j = 0; j < selectQ2.SelectedItems.Count; j++)
            {
                AddIntoNodeList(selectQ2.SelectedItems[j].ToString());
            }

            // QLink.Clear();
            ObservableCollection<QueueLink> links = new ObservableCollection<QueueLink>();
            listLink.Clear();

            for (int i=0; i<selectQ1.SelectedItems.Count; i++)
            {
                for(int j=0; j<selectQ2.SelectedItems.Count; j++)
                {
                    List<QueueItem> src = TotalQueueList.Where( x => x.QueueName == selectQ1.SelectedItems[i].ToString()).ToList();
                    List<QueueItem> dst = TotalQueueList.Where(x => x.QueueName == selectQ2.SelectedItems[j].ToString()).ToList();
                    QueueLink link = new QueueLink() {
                        msMQFrom = src[0],
                        msMQTo = dst[0] };

                    links.Add(link);
                    listLink.Add(link.ToString());
                }
            }

            QLink = links;

            RefreshNodeInLink();

            PRMlaceAllQueue();
        }



    }
}

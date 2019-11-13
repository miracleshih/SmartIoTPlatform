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
using WSIP.MicrosoftMQ;

namespace UltraSonic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public const int AnalysisWindow = 5;

        CustomerEquipment equipment; // = new CustomerEquipment();

        public MainWindow()
        {
            InitializeComponent();

            equipment = new CustomerEquipment();

            this.DataContext = equipment;   // this;
        }


        private void btnLink_Click(object sender, RoutedEventArgs e)
        {
            if(sender == btnSendMq)
            {
                Random rnd = new Random();
                equipment.GetAndSendValue(rnd.Next(50, 300));
            }
            else if(sender == btnOpenPort)
            {
                equipment.OpenPort();
            }
        }

    }
}

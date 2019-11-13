using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace RoutingManager
{
    public class QueueItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string _QueueName = "";
        public string QueueName
        {
            get { return _QueueName; }
            set
            {
                _QueueName = value;
                RefreshIcon();
                OnPropertyChanged("QueueName");
            }
        }
        int _X;
        public int X
        {
            get { return _X; }
            set
            {
                _X = value;
                RefreshIcon();
                OnPropertyChanged("X");
            }
        }
        int _Y;
        public int Y
        {
            get { return _Y; }
            set
            {
                _Y = value;
                RefreshIcon();
                OnPropertyChanged("Y");
            }
        }

        TextBlock _textBlock = new TextBlock();
        public TextBlock textBlock
        {
            get { return _textBlock; }
            set
            {
                _textBlock = value;
                OnPropertyChanged("textBlock");
            }
        }


        public void RefreshIcon()
        {
            textBlock.Text = QueueName;
            textBlock.Foreground = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(textBlock, X);
            Canvas.SetTop(textBlock, Y);
            OnPropertyChanged("textBlock");
        }
    }
}

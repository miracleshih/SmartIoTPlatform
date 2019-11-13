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
using System.Windows.Media;
using System.Windows.Shapes;

namespace RoutingManager
{
    public class QueueLink : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public QueueItem msMQFrom;
        public QueueItem msMQTo;

        private Line _QLine = new Line();
        public Line QLine
        {
            get { return _QLine; }
            set
            {
                _QLine = value;
                OnPropertyChanged("QLine");
            }
        }

        Line _lineBlock = new Line();
        public Line lineBlock
        {
            get { return _lineBlock; }
            set
            {
                _lineBlock = value;
                OnPropertyChanged("lineBlock");
            }
        }

        ObservableCollection<Line> _LineList = new ObservableCollection<Line>();
        public ObservableCollection<Line> LineList
        {
            get { return _LineList; }
            set
            {
                _LineList = value;
                OnPropertyChanged("LineList");
            }
        }


        public void RefreshIcon(QueueItem item1, QueueItem item2)
        {
            const double HeadWidth = 10;
            const double HeadHeight = 10;
            double theta = Math.Atan2(item1.Y - item2.Y, item1.X - item2.X);    // y1 - y2, x1 - x2);
            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);

            Point pt3 = new Point(
            /*x2*/item2.X + (HeadWidth * cost - HeadHeight * sint),
            /*y2*/item2.Y + (HeadWidth * sint + HeadHeight * cost));

            Point pt4 = new Point(
                /*x2*/item2.X + (HeadWidth * cost + HeadHeight * sint),
                /*y2*/item2.Y - (HeadHeight * cost - HeadWidth * sint));

            Line lineObj = new Line();
            lineObj.Stroke = new SolidColorBrush(Colors.Blue);
            lineObj.X1 = item1.X;   // x1;
            lineObj.X2 = item2.X;   // x2;
            lineObj.Y1 = item1.Y;   // y1;
            lineObj.Y2 = item2.Y;   // y2;
            lineObj.StrokeThickness = 1;

            Line line1 = new Line()
            {
                Stroke = new SolidColorBrush(Colors.Blue),
                X1 = item2.X,   // x2,
                Y1 = item2.Y,   // y2,
                X2 = pt3.X,
                Y2 = pt3.Y,
                StrokeThickness = 1
            };
            Line line2 = new Line()
            {
                Stroke = new SolidColorBrush(Colors.Blue),
                X1 = item2.X,   // x2,
                Y1 = item2.Y,   // y2,
                X2 = pt4.X,
                Y2 = pt4.Y,
                StrokeThickness = 1
            };

            ObservableCollection<Line> lines = new ObservableCollection<Line>();
            lines.Add(lineObj);
            lines.Add(line1);
            lines.Add(line2);
            LineList = lines;
        }

        public void DrawLines(ref Canvas canvas)
        {
            RefreshIcon(msMQFrom, msMQTo);

            for (int i=0; i<LineList.Count; i++)
            {
                canvas.Children.Add(LineList[i]);
            }
        }

        public override string ToString()
        {
            string strRet = $"{msMQFrom.QueueName} to {msMQTo.QueueName}";

            return strRet;
        }
    }
}

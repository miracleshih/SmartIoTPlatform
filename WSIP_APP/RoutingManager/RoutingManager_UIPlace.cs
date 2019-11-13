using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RoutingManager
{
    public partial class RoutingManagerUI : Window, INotifyPropertyChanged
    {

        // private void RMLine(Canvas canvas, double x1, double y1, double x2, double y2, Color color)
        private void RMLine(Canvas canvas, QueueItem item1, QueueItem item2, Color color)
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
            lineObj.Stroke = new SolidColorBrush(color);
            lineObj.X1 = item1.X;   // x1;
            lineObj.X2 = item2.X;   // x2;
            lineObj.Y1 = item1.Y;   // y1;
            lineObj.Y2 = item2.Y;   // y2;
            lineObj.StrokeThickness = 1;

            Line line1 = new Line() {
                Stroke = new SolidColorBrush(color),
                X1 = item2.X,   // x2,
                Y1 = item2.Y,   // y2,
                X2 = pt3.X,
                Y2 = pt3.Y,
                StrokeThickness = 1
            };
            Line line2 = new Line()
            {
                Stroke = new SolidColorBrush(color),
                X1 = item2.X,   // x2,
                Y1 = item2.Y,   // y2,
                X2 = pt4.X,
                Y2 = pt4.Y,
                StrokeThickness = 1
            };

            canvas.Children.Add(lineObj);
            canvas.Children.Add(line1);
            canvas.Children.Add(line2);

        }

        private void RMText(Canvas canvas, double x, double y, string text, Color color)
        {
            TextBlock textBlock = new TextBlock();

            textBlock.Text = text;

            textBlock.Foreground = new SolidColorBrush(color);

            Canvas.SetLeft(textBlock, x);

            Canvas.SetTop(textBlock, y);

            canvas.Children.Add(textBlock);
        }

        public void RefreshNodeInLink()
        {
            TotalQueueList.Clear();

            for (int i = 0; i < QLink.Count; i++)
            {
                List<QueueItem> item = TotalQueueList.Where(x => x.QueueName == QLink[i].msMQFrom.QueueName).ToList();
                if (item is null || item.Count <= 0)
                {
                    TotalQueueList.Add(QLink[i].msMQFrom);
                }
            }
            for (int i = 0; i < QLink.Count; i++)
            {
                List<QueueItem> item = TotalQueueList.Where(x => x.QueueName == QLink[i].msMQTo.QueueName).ToList();
                if (item is null || item.Count <= 0)
                {
                    TotalQueueList.Add(QLink[i].msMQTo);
                }
            }
        }

        public bool AddIntoNodeList(string queuename)
        {
            int cnt = TotalQueueList.Where(x => x.QueueName == queuename).ToList().Count;
            if (cnt > 0)
                return false;

            TotalQueueList.Add(new QueueItem() { QueueName = queuename});
            return true;
        }

        private void PRMlaceAllQueue()
        {
            const int centerX = 300;
            const int centerY = 300;
            const double radius = 200;

            if (TotalQueueList.Count <= 0)
                return;

            picPlace.Children.Clear();

            double angle = 2 * Math.PI / TotalQueueList.Count;

            QItems.Clear();

            for (int i=0; i< TotalQueueList.Count; i++)
            {
                double off_x = centerX + radius * Math.Sin(0.1 + angle * i);
                double off_y = centerY + radius * Math.Cos(0.1 + angle * i);


                //QueueItem item = new QueueItem()
                //{
                //    QueueName = TotalQueueList[i].QueueName,
                //    X = (int)off_x,
                //    Y = (int)off_y,
                //};
                TotalQueueList[i].X = (int)off_x;
                TotalQueueList[i].Y = (int)off_y;

                QItems.Add(TotalQueueList[i]);  // item);

                // RMText(picPlace, off_x, off_y, QueueList[i].QueueName, Colors.Red);
                picPlace.Children.Add(TotalQueueList[i].textBlock); // item.textBlock);
            }

            Point startPoint = new Point(50, 50);
            for(int i=0; i< QLink.Count; i++)
            {
                QLink[i].DrawLines(ref picPlace);
            }

            // picPlace.Children.Add(newLine);

        }


    }
}

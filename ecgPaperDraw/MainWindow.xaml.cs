using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace ecgPaperDraw
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //myLine.X1 = 0;
            //myLine.X2 = 800;
            //myLine.Y1 = 0;
            //myLine.Y2 = 0;
            //myLine.HorizontalAlignment = HorizontalAlignment.Center;
            //myLine.VerticalAlignment = VerticalAlignment.Center;
            myCanvas.Width = this.Width;
            myCanvas.Height = this.Height;
            for (int x = 0; x < myCanvas.Width; x += 15)
            {
                //myCanvas.DrawLine(Brushes.Pink, 1, 0, x, myCanvas.Width - x, 0);// It's so cool.
                myCanvas.DrawLine(Brushes.Pink, 1, x, 0, x, this.Height);
                myCanvas.DrawLine(Brushes.Pink, 1, 0, x, this.Width, x);
            }
            for (int x = 0; x < myCanvas.Width; x += 75)
            {
                myCanvas.DrawLine(Brushes.Pink, 3, x, 0, x, this.Height);
                myCanvas.DrawLine(Brushes.Pink, 3, 0, x, this.Width, x);
            }
        }
    }
    public static class CanvasExt
    {
        public static void DrawLine(this Canvas g, Brush brush, double width, double x1, double y1, double x2, double y2)
        {
            var line = new Line();
            line.StrokeThickness = width;
            line.Stroke = brush;
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            g.Children.Add(line);
        }
    }
    //ref : https://social.msdn.microsoft.com/Forums/en-US/a1c67f63-3d3e-40a7-8853-adc634ee4b6a/drawing-multiple-lines-from-a-loop?forum=wpf
}

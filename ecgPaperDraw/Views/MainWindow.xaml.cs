using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using ecgPaperDraw.ViewModels;
using MahApps.Metro.Controls;

namespace ecgPaperDraw
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        PaperViewModel paperVM;
        public MainWindow()
        {
            InitializeComponent();
            paperVM = new PaperViewModel();
            this.DataContext = paperVM;

            myCanvas.Width = this.Width;
            myCanvas.Height = this.Height;
            double Thickness;
            double paper2mv = paperVM.BigBlock * 4;
            for (double x = 0; x <= myCanvas.Width; x += paperVM.SmallBlock)
            {
                Thickness = 1;
                if(x % paperVM.BigBlock == 0)
                {
                    Thickness = 3;
                }
                myCanvas.DrawLine(Brushes.Pink, Thickness, x, 0, x, paper2mv);//直線
            }
            for (double y = 0; y <= paper2mv; y += paperVM.SmallBlock)
            {
                Thickness = 1;
                if (y % paperVM.BigBlock == 0)
                {
                    Thickness = 3;
                }
                myCanvas.DrawLine(Brushes.Pink, Thickness, 0, y, this.Width, y);//橫線
            }
            myCanvas.DrawSine(Brushes.Black, 3,0,paper2mv/2,paperVM.BigBlock);
        }

        private MetroWindow accentThemeTestWindow;

        private void ChangeAppStyleButtonClick(object sender, RoutedEventArgs e)
        {
            if (accentThemeTestWindow != null)
            {
                accentThemeTestWindow.Activate();
                return;
            }

            accentThemeTestWindow = new AccentStyleWindow();
            accentThemeTestWindow.Owner = this;
            accentThemeTestWindow.Closed += (o, args) => accentThemeTestWindow = null;
            accentThemeTestWindow.Left = this.Left + this.ActualWidth / 2.0;
            accentThemeTestWindow.Top = this.Top + this.ActualHeight / 2.0;
            accentThemeTestWindow.Show();
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
        public static void DrawSine(this Canvas g,Brush brush, double width ,double x0 = 0.0,double y0 = 180.0,double amplitude = 75.0,double frequency = 6.0,double phase = 0.0,double samples = 800.0)
        {
            var line = new Line();
            double x1, y1;
            double baseline = y0;

            for (int i = 1; i <= samples; i++)
            {
                double wt = amplitude * Math.Sin(2.0 * Math.PI * frequency * i / samples);
                x1 = i;
                y1 = baseline - wt;
                g.DrawLine(brush, width, x0, y0, x1, y1);
                x0 = x1;
                y0 = y1;
            }
        }
    }
}

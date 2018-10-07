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
            for (double x = 0; x < myCanvas.Width; x += paperVM.SmallBlock)
            {
                myCanvas.DrawLine(Brushes.Pink, 1, x, 0, x, this.Height);
                myCanvas.DrawLine(Brushes.Pink, 1, 0, x, this.Width, x);
            }
            for (double x = 0; x < myCanvas.Width; x += paperVM.BigBlock)
            {
                myCanvas.DrawLine(Brushes.Pink, 3, x, 0, x, this.Height);
                myCanvas.DrawLine(Brushes.Pink, 3, 0, x, this.Width, x);
            }
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
    }
}

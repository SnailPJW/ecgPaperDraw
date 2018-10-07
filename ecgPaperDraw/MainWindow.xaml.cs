using System;
using System.Collections.Generic;
using System.Globalization;//FormattedText
using System.IO;//File.ReadAllLines
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
using Microsoft.Win32;//FileDialog

namespace ecgPaperDraw
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        string strRawDataPath;
        string[] strRawArray;
        byte[] pointData,tempByteArray;
        int checkSum, iMin, iMax;
        int[] iEcgDataArray;
        char[] tempCharArray;

        public MainWindow()
        {
            InitializeComponent();
            strRawDataPath = String.Empty;
            //myLine.X1 = 0;
            //myLine.X2 = 800;
            //myLine.Y1 = 0;
            //myLine.Y2 = 0;
            //myLine.HorizontalAlignment = HorizontalAlignment.Center;
            //myLine.VerticalAlignment = VerticalAlignment.Center;
            //myCanvas.Width = this.Width;
            
            Label lblsticky = new Label();
            lblsticky.Content = "25 mm = 1 second";
            lblsticky.FontSize = 16;
            myCanvas.Children.Add(lblsticky);

            pointData = new byte[6];

            //Point[] P = new Point[]
            //{
            //    new Point { X = 0, Y = 10 },
            //    new Point { X = 20, Y = 200 },
            //    new Point { X = 40, Y = 130 },
            //    new Point { X = 60, Y = 40 },
            //    new Point { X = 80, Y = 255 },
            //    new Point { X = 100, Y = 346 },
            //    new Point { X = 120, Y = 157 },
            //    new Point { X = 140, Y = 298 },
            //    new Point { X = 160, Y = 199 }
            //};
            //for(int i = 0;i<P.Length-1;i++)
            //{
            //    myCanvas.DrawLine(Brushes.GreenYellow, 5, P[i].X, P[i].Y, P[i+1].X, P[i+1].Y);
            //}
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "Text Files|*.txt|All Files|*.*";
            fileDialog.DefaultExt = ".txt";
            Nullable<bool> dialogOk = fileDialog.ShowDialog();

            if(dialogOk == true)
            {
                strRawDataPath = fileDialog.FileName;
                strRawArray = File.ReadAllLines(strRawDataPath);
                StringBuilder sb = new StringBuilder();
                foreach(string str in strRawArray)
                {
                    sb.Append(str+Environment.NewLine);
                }
                tbxShowData.Text = sb.ToString();
                iEcgDataArray = new int[strRawArray.Length];
                DrawECGPaper();
            }
        }
        byte Chr2byte(char[] chr)
        {
            byte result = 0;
            int tmp;
            for(int i = 0; i < 2; i++)
            {
                result *= 16;
                tmp = (int)chr[i];
                if(tmp <= 57 && tmp >= 48)//0~9
                {
                    result += (byte)(tmp - 48);
                }
                else if(tmp <= 90 && tmp >= 65)//A~Z
                {
                    result += (byte)(tmp - 65 + 10);
                }
            }
            return result;
        }

        

        private void btnDrawEcg_Click(object sender, RoutedEventArgs e)
        {
            if(strRawArray == null)
            {
                MessageBox.Show("Please firstly press the load data button !");
                return;
            }
            StringBuilder sb = new StringBuilder();

            
            int ii = 0;
            foreach(string str in strRawArray)
            {
                tempCharArray = str.ToCharArray();
                tempByteArray = new byte[tempCharArray.Length];
                int k = 0;
                char[] chr = new char[2];
                for(int i = 0; i < tempCharArray.Length; i++)
                {
                    if(tempCharArray[i] == ',')
                    {
                        continue;
                    }
                    chr[0] = tempCharArray[i++];
                    chr[1] = tempCharArray[i];
                    tempByteArray[k++] = Chr2byte(chr);
                }
                Array.Resize(ref tempByteArray, k);
                iEcgDataArray[ii] = GetECGValue(tempByteArray);
                if(iEcgDataArray[ii] <= 0)
                {
                    continue;
                }
                sb.Append(string.Format("{1}{2}",BitConverter.ToString(tempByteArray)
                                                      ,iEcgDataArray[ii++]
                                                      ,Environment.NewLine));
            }
            Array.Resize(ref iEcgDataArray, ii);
            tbxShowDataReal.Text = sb.ToString();
            iMin = iMax = iEcgDataArray[0];
            for (ii = 0; ii < iEcgDataArray.Length; ii++)
            {
                if(iEcgDataArray[ii]<iMin)
                {
                    iMin = iEcgDataArray[ii];
                }
                else if (iEcgDataArray[ii] > iMax)
                {
                    iMax = iEcgDataArray[ii];
                }
            }

            DrawECG();
        }
        int GetECGValue(byte[] rxData)
        {
            int valueEcg = -1, i = 0;
            if(rxData[i] == 0x0D)
            {
                Array.Copy(rxData, i + 1, pointData, 0, 6);
                checkSum = (pointData[1] + pointData[2] + pointData[3]) & 0xFF;
                if(pointData[0] == 0x45 && pointData[5]== 0x0A && checkSum == pointData[4])
                {
                    valueEcg = pointData[1];
                    valueEcg = valueEcg * 256 + pointData[2];
                    valueEcg = valueEcg * 256 + pointData[3];
                    valueEcg /= 26840;//6710 * 4
                }
            }
            return valueEcg;
        }
        void DrawECG()
        {
            if(iEcgDataArray == null)
            {
                return;
            }
            double pnlH = this.Height;
            double px, py, px0, py0;
            double bot = pnlH * 9 / 10;
            double top = pnlH / 10;
            int dh = iMax - iMin;
            double dtb = top - bot;
            px0 = 0;
            py0 = (iEcgDataArray[0] - iMin) * dtb / dh + bot;
            for (int i = 0; i < iEcgDataArray.Length; i++)
            {
                px = 2 * i;
                py = (iEcgDataArray[i] - iMin) * dtb / dh + bot;
                myCanvas.DrawLine(Brushes.Black,2, px0, py0, px, py);
                px0 = px;
                py0 = py;
            }
            
        }
        void DrawECGPaper()
        {
            myCanvas.Width = iEcgDataArray.Length;
            for (int x = 0; x < myCanvas.Width; x += 15)//小格子
            {
                //myCanvas.DrawLine(Brushes.Pink, 1, 0, x, myCanvas.Width - x, 0);// It's so cool.
                myCanvas.DrawLine(Brushes.Pink, 1, x, 0, x, this.Height);//直線
                if (x > this.Height)
                {
                    continue;
                }
                myCanvas.DrawLine(Brushes.Pink, 1, 0, x, myCanvas.Width, x);//橫線
            }
            for (int x = 0; x < myCanvas.Width; x += 75)//大格子
            {
                myCanvas.DrawLine(Brushes.Pink, 3, x, 0, x, this.Height);//直線
                if (x > this.Height)
                {
                    continue;
                }
                myCanvas.DrawLine(Brushes.Pink, 3, 0, x, myCanvas.Width, x);//橫線
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
    //alan Check
}

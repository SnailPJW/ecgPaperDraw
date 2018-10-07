using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ecgPaperDraw.Models;

namespace ecgPaperDraw.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
    public class PaperViewModel : ViewModelBase
    {
        private Paper _paper;
        public PaperViewModel(double pixelsOfSmallBlock = 15.0)
        {
            _paper = new Paper();
            _paper.smallBlock = pixelsOfSmallBlock;
            _paper.bigBlock = _paper.smallBlock * 5;
        }
        public double SmallBlock
        {
            get { return _paper.smallBlock; }
            set { _paper.smallBlock = value; OnPropertyChanged(); }
        }
        public double BigBlock
        {
            get { return _paper.bigBlock; }
            set { _paper.bigBlock = value; OnPropertyChanged(); }
        }
    }
}

using System;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace WPF_Lab
{
    public class SelectedData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isSelected = false;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value; NotifyPropertyChanged("IsSelected");
            }
        }

        private Shape currentShape;

        public Shape CurrentShape
        {
            get { return currentShape; }
            set
            {
               currentShape = value; NotifyPropertyChanged("CurrentShape");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    public class LabAccessRule : INotifyPropertyChanged
    {
        private string _accessName;
        public string AccessName
        {
            get
            {
                return _accessName;
            }
            set
            {
                _accessName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AccessName"));
            }
        }


        private int _accessID;

        public event PropertyChangedEventHandler PropertyChanged;

        public int AccessID
        {
            get
            {
                return _accessID;
            }
            set
            {
                _accessID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AccessID"));

            }
        }

        public LabAccessRule(int id, string name)
        {
            AccessID = id;
            AccessName = name;
        }
    }
}

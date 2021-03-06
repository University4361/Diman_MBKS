﻿using Newtonsoft.Json;
using System.ComponentModel;

namespace lab_5
{
    public class AccessRule : INotifyPropertyChanged
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

        [JsonConstructor]
        public AccessRule(int id, string name)
        {
            AccessID = id;
            AccessName = name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace InvestmentOptions {
    public class Boolean : INotifyPropertyChanged {
        public String Name { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        bool _value;
        public bool Value {
            get { return _value; }
            set {
                _value = value;
                PropertyChanged(this, new PropertyChangedEventArgs(Name)); 
            }
        }

        public Boolean(String name) {
            Name = name;
        }

    }
}

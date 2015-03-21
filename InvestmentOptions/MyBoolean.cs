using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel; //yeah, because this is a viewModel!

namespace InvestmentOptions {
    public class MyBoolean : INotifyPropertyChanged {
        public String name;
        public event PropertyChangedEventHandler PropertyChanged;
        public List<InvestmentOption> optionsList;

        public bool _value;
        public bool value {
            get {
                return _value;
            }
            set {
                _value = value;
                PropertyChanged(this, new PropertyChangedEventArgs(name)); 
            }
        }

        public MyBoolean(String name) {
            this.name = name;
            //this.optionsList = optionsList;
            //I think really, this should be separated for GLOBALPARAMETERS MYBOOLEAN....
            //But for now, just leave it here. Can't hurt...
        }
    }
}

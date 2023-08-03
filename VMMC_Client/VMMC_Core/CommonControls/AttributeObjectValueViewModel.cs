using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VMMC_Core.CommonControls
{
    public class AttributeObjectValueViewModel : INotifyPropertyChanged
    {
        public VMMC_Core.AttributeObjectValue attributeObjectValue;
        public VMMC_Core.AttributeObjectValue AttributeObjectValue
        {
            get { return attributeObjectValue; }
            set
            {
                attributeObjectValue = value;
                OnAttributeObjectValuePropertyChanged("AttributeObjectValue");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnAttributeObjectValuePropertyChanged([CallerMemberName] string prop = "")
        {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

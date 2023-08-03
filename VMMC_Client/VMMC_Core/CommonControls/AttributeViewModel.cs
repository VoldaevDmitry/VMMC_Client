using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VMMC_Core.CommonControls
{
    public class AttributeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel> AttributeObjectValueViewModelCollection { get; set; }
        public ObservableCollection<VMMC_Core.AttributeObjectValue> AttributeObjectValuesCollection { get; set; }
        public bool IsEditMod { get; set; }


        public AttributeViewModel(ObservableCollection<VMMC_Core.AttributeObjectValue> attributeObjectValuesCollection)
        {
            AttributeObjectValuesCollection = attributeObjectValuesCollection;
            AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();
            if (attributeObjectValuesCollection != null)
            {
                foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                {
                    AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel() { AttributeObjectValue = aov });
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnAttributeViewModelPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}

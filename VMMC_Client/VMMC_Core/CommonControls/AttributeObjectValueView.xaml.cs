using System;
using System.Collections.Generic;
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

namespace VMMC_Core.CommonControls
{
    /// <summary>
    /// Логика взаимодействия для AttributeObjectValueView.xaml
    /// </summary>
    public partial class AttributeObjectValueView : UserControl
    {
        //public AttributeObjectValueViewModel attributeObjectValueViewModel { get; set; }  
        public AttributeObjectValueView()
        {
            InitializeComponent();
            

             //= new AttributeObjectValueViewModel();
             //this.DataContext = this.DataContext;
        }

        private void MultiSelectComboBox_FilterTextChanged(object sender, Sdl.MultiSelectComboBox.EventArgs.FilterTextChangedEventArgs e)
        {

            //this.MultiSelectComboBox.filte
            
        }
    }
}

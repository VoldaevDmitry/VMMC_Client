using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMMC_Core.Model.Attributes
{
    public class AttributeValue
    {
        public string StringValue { get; set; }
        public Decimal NumberValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public List<EnumAttributeValue> SelectedValues { get; set; }
        public List<EnumAttributeValue> AvailableValues { get; set; }

    }
}

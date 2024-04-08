using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Module
{
    public class ReportItem
    {
        public string DisplayValue { get; set; }
        public Core_Module.DbObject Item { get; set; }
        public int CountValue { get; set; }
    }
}

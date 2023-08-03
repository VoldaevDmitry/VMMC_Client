using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMMC_Core
{
    public class ReportItem
    {
        public string DisplayValue { get; set; }
        public VMMC_Core.DbObject Item { get; set; }
        public int CountValue { get; set; }
    }
}

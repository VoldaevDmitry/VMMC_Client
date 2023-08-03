using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMMC_Editor.Model
{
    public class DocumentClassRules
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public bool isHaveAtribute_DocumentNumber { get; set; }
        public bool isHaveAtribute_Date { get; set; }
        public bool isHaveAtribute_Validity { get; set; }
        public bool isHaveAtribute_WorkType { get; set; }
        public bool isHaveRelationship_DocumentSet { get; set; }
        public bool isHaveRelationship_DocumentAOSR { get; set; }
        public bool isHaveRelationship_DocumentAVK { get; set; }
        public bool isHaveRelationship_DocumentIS { get; set; }
        public bool isHaveRelationship_Material { get; set; }
        public bool isHaveRelationship_Organization { get; set; }
        public bool isHaveRelationship_Manufacturer { get; set; }
        public bool isHaveRelationship_Supplier { get; set; }
        public bool isHaveRelationship_Control { get; set; }
        public bool isHaveRelationship_SMR { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMMC_Editor.Model;
using VMMC_Core;

namespace VMMC_Editor
{
    public class DocumentClassRulesViewModel
    {
        public VMMC_Core.SessionInfo sessionInfo;
        //public string SQLServer;
        //public string SQLDataBase;
        public List<DocumentClassRules> documentClassRulesCollection { get; set; }
        public DocumentClassRulesViewModel(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;
            //SQLServer = sqlServer;
            //SQLDataBase = sqlDataBase;
            documentClassRulesCollection = getDocumentClassRules();
        }
        public List<DocumentClassRules> getDocumentClassRules()
        {
            List<DocumentClassRules> result = new List<DocumentClassRules>();
            result.Clear();
            var classes = new VMMC_Core.Class(sessionInfo).getDocumentClasses();
            foreach (Class cls in classes)
            {
                DocumentClassRules newDocumentClassRule = new DocumentClassRules
                {
                    ClassId = cls.ClassId,
                    ClassName = cls.ClassName
                };
                result.Add(newDocumentClassRule);
            }

            return result;
            }   
    }
}

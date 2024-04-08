﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core_Module
{
    public class DocumentCodePattern
    {
        public Core_Module.SessionInfo sessionInfo;
        public string Mask { get; set; }
        public string Pattern { get; set; }
        public Guid ProjectId { get; set; }
        public Class Class { get; set; }
        public Complekt Complekt { get; set; }
        public TreeItem Building { get; set; }
        public TreeItem System { get; set; }
        public TreeItem Subsystem { get; set; }
        public TreeItem Level { get; set; }
        public Revision Revision { get; set; }

        public DocumentCodePattern(Core_Module.SessionInfo session)
        {
            sessionInfo = session;

        }
        public List<Core_Module.DocumentCodePattern> GetDocumentCodePatternsList()
        {
            List<Core_Module.DocumentCodePattern> result = new List<DocumentCodePattern>();

            Core_Module.DocumentCodePattern rule1 = new DocumentCodePattern(sessionInfo)
            {
                Mask = "VMMC-XX-YYY-ZZ-QQQ-AA-BB",
                Pattern = @"^VMMC-\d{4}-\w{3}-\w{2}-\w{3}-\w{2}-\d{2}",
                ProjectId = Guid.Parse("FCE6ABA9-54A7-EB11-A1A0-00155D036519"), //ВММК
                Class = new Core_Module.Class(sessionInfo).getClass("3D - Модель")
            };
            Core_Module.DocumentCodePattern rule2 = new DocumentCodePattern(sessionInfo)
            {
                Mask = "VMMC-XX-YYY-ZZ-QQQ-AA-BB",
                Pattern = @"^VMMC-\d{4}-\w{3}-\w{2}-\d{3}-\w{2}-\d{2}",
                ProjectId = Guid.Parse("FCE6ABA9-54A7-EB11-A1A0-00155D036519"), //ВММК
                Class = new Core_Module.Class(sessionInfo).getClass("3D - Модель")
            };
            Core_Module.DocumentCodePattern rule3 = new DocumentCodePattern(sessionInfo)
            {
                Mask = "VMMC-XXXX-QQQ-ZZ-VV(VVV)-AA-BB",
                Pattern = @"^VMMC-\d{4}-\w{3}-\w{2}-\w{2}(w{3})-\w{2}-\d{2}",
                ProjectId = Guid.Parse("FCE6ABA9-54A7-EB11-A1A0-00155D036519"), //ВММК
                Class = new Core_Module.Class(sessionInfo).getClass("3D - Модель")
            };

            Core_Module.DocumentCodePattern rule4 = new DocumentCodePattern(sessionInfo)
            {
                Pattern = @"^ВММК-РД-\d{2}|(\d{1}.\d{2})-\w*|(РТП-2)-(\d{1}\.\d*)|(\d{2}\.\d*)|(\d{2}\.\d*\.\d*)|(\d{2},\d{2})|\d{2}|\d{1})-(\w{1}|\w{2}|(\w{1}-d{1}))-((\d*-\d*)|\d*.\d{1}|\d*)((_\s)|_|-|\s|\.)((И|и)зм(\.|-|_\s|_|\s)\d+)?",
                ProjectId = Guid.Parse("FCE6ABA9-54A7-EB11-A1A0-00155D036519"), //ВММК
                Class = new Core_Module.Class(sessionInfo).getClass("Рабочая документация")
            };

            return result;
        }

        public string getValidMask(string text) 
        {
            string result = "";
            List<Core_Module.DocumentCodePattern> patterns = GetDocumentCodePatternsList();
            foreach (Core_Module.DocumentCodePattern pattern in patterns) 
            {
                string reg = Regex.Replace(text, pattern.Pattern, String.Empty);
                if (reg != "")
                {
                    result = reg;
                    break;
                }
            }
            return result;
        }

    }
}

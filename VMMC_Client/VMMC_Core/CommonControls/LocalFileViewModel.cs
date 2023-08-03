using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMMC_Core.CommonControls
{
    public class LocalFileViewModel
    {
        private VMMC_Core.SessionInfo sessionInfo;
        public VMMC_Core.LocalFile LocalFile { get; set; }
        bool isReadOnly;
        public bool EditMode { get; set; }
        public LocalFileViewModel(VMMC_Core.LocalFile localFile, bool editMode, VMMC_Core.SessionInfo session)
        {

            LocalFile = localFile;
            EditMode = editMode;
            isReadOnly = !editMode;
            sessionInfo = session;
        }
    }
}

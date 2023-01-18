using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEH.WEB.File.Entity
{
    public enum UploadResultType
    {
         COMPLETED = 0, PROGRESS =1 , ERROR=2
    }
    public class UploadResult
    {
        public UploadResultType ResultType { get; set; }
        public string ErrorMsg { get; set; }

    }
}

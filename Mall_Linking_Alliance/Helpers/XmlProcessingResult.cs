using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall_Linking_Alliance.Helpers
{
    public class XmlProcessingResult
    {
        public bool Success { get; set; } = true;
        public bool HasErrors { get; set; } = false;
        public string Message { get; set; } = "";
        public int VoidedSkippedCount { get; set; }

    }
}

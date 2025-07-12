using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall_Linking_Alliance.Model
{
    public class TblLog
    {
        public int LogId { get; set; }
        public string LogDate { get; set; }
        public string LogLevel { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
    }
}

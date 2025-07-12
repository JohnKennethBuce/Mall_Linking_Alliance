using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall_Linking_Alliance.Model
{
    public class TblSettings
    {
        public int? TenantId { get; set; }
        public string TenantKey { get; set; }
        public int? TmId { get; set; }
        public string Doc { get; set; }
        public string BrowseDb { get; set; }
        public string SaveDb { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SweetSoft.APEM.Core.Security
{
    public class FunctionPermission
    {
        public long RoleId { set; get; }
        public string RoleName { set; get; }
        public bool AllowInsert { get; set; }
        public bool AllowUpdate { get; set; }
        public bool AllowDelete { get; set; }
    }
}

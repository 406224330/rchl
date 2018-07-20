using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCHL.Model
{
    public partial class GroupAuditOrderEntity
    {
        public string StatusName { get; set; }
        public string CashTypeName { get; set; }
        public AuditOrganizationEntity Audit { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCHL.Model
{
    public partial class AuditOrderEntity
    {
        public string StatusName { get; set; }
        public string CashTypeName { get; set; }
        /// <summary>
        /// 不是对应状态
        /// </summary>
        public int? NotStatus { get; set; }
        public AuditOrganizationEntity Audit { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCHL.Model
{
    public partial class InsuranceOrderEntity
    {
        public string StatusName { get; set; }

        public InsuranceCommpayEntity Insurance { get; set; }
    }
}

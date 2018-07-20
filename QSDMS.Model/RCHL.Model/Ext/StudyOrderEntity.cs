using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCHL.Model
{
    public partial class StudyOrderEntity
    {
        public string StudyTypeName { get; set; }
        public string StatusName { get; set; }

        public List<StudyOrderDetailEntity> DetailList { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RCHL.Model
{
    [Serializable]
    public class BaseModel
    {

        public string StartTime { get; set; }


        public string EndTime { get; set; }

        public string sidx { get; set; }

        public string sord { get; set; }
    }

    public class KeyValueEntity
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }

        public string Status { get; set; }

        public int SortNum { get; set; }
    }
}

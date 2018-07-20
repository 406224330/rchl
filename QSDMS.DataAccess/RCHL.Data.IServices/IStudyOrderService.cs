using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCHL.Data.IServices
{
    public interface IStudyOrderService<T, Q, P> : IDAL<T, Q, P>
    {
        string GetOrderNo();
    }
}

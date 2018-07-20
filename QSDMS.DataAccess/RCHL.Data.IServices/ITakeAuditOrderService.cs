using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCHL.Data.IServices
{
    public interface ITakeAuditOrderService<T, Q, P> : IDAL<T, Q, P>
    {
        string GetOrderNo();
    }
}

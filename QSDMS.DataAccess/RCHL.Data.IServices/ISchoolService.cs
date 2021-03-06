﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCHL.Data.IServices
{
    public interface ISchoolService<T, Q, P> : IDAL<T, Q, P>
    {
        T CheckLogin(string username, string pwd);
    }
}

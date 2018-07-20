using QSDMS.Application.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QSDMS.Application.Web.Areas.RchlManage.Controllers
{
    public class SelectController : BaseController
    {
        //
        // GET: /RchlManage/Select/

        public ActionResult SelectMember()
        {
            return View();
        }
        public ActionResult SelectTeacher()
        {
            return View();
        }


    }
}

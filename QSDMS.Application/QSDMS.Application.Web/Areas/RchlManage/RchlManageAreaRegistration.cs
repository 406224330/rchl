using System.Web.Mvc;

namespace QSDMS.Application.Web.Areas.RchlManage
{
    public class RchlManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RchlManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RchlManage_default",
                "RchlManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

using QSDMS.Util.WebControl;
using RCHL.Data.IServices.Report;
using RCHL.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCHL.Business.Report
{
    public class AuditReportBLL : BaseBLL<IAuditReportService<Pagination>>
    {
        /// <summary>
        /// 访问实例
        /// </summary>
        public static AuditReportBLL m_Instance = new AuditReportBLL();

        /// <summary>
        /// 访问实例
        /// </summary>
        public static AuditReportBLL Instance
        {
            get { return m_Instance; }
        }
        /// <summary>
        /// 缓存key
        /// </summary>
        public string cacheKey = "AuditReportCache";


        /// <summary>
        /// 构造方法
        /// </summary>


        public List<AuditCollectEntity> GetAuditCollectPageList(AuditCollectEntity para, ref Pagination pagination)
        {
            List<AuditCollectEntity> list = InstanceDAL.GetAuditCollectPageList(para, ref pagination);

            return list;
        }
        public List<AuditCollectEntity> GetAuditCollectList(AuditCollectEntity para)
        {
            List<AuditCollectEntity> list = InstanceDAL.GetAuditCollectList(para);

            return list;
        }
    }
}

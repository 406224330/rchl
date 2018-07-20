﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QSDMS.Util;
using QSDMS.Util.Extension;
using QSDMS.Util.WebControl;
using iFramework.Framework;
using RCHL.Model;
using RCHL.Business;
using QSDMS.Util.Excel;
using QSDMS.Application.Web.Controllers;
namespace QSDMS.Application.Web.Areas.RchlManage.Controllers
{
    public class WithDrivingOrderController : BaseController
    {
        //
        // GET: /RchlManage/WithDrivingOrder/

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            WithDrivingOrderEntity para = new WithDrivingOrderEntity();
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                var queryParam = queryJson.ToJObject();

                //类型
                if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
                {
                    var condition = queryParam["condition"].ToString().ToLower();
                    switch (condition)
                    {
                        case "drivingorderno":
                            para.DrivingOrderNo = queryParam["keyword"].ToString();
                            break;
                        case "membername":
                            para.MemberName = queryParam["keyword"].ToString();
                            break;
                        case "membermobile":
                            para.MemberMobile = queryParam["keyword"].ToString();
                            break;
                        case "schoolname":
                            para.SchoolName = queryParam["keyword"].ToString();
                            break;
                        case "teachername":
                            para.TeacherName = queryParam["keyword"].ToString();
                            break;
                    }
                }
                if (!queryParam["status"].IsEmpty())
                {
                    para.Status = int.Parse(queryParam["status"].ToString());
                }
            }

            var pageList = WithDrivingOrderBLL.Instance.GetPageList(para, ref pagination);
            if (pageList != null)
            {
                pageList.ForEach((o) =>
                {
                    if (o.Status != null)
                    {
                        o.StatusName = ((RCHL.Model.Enums.PaySatus)o.Status).ToString();
                    }
                    if (o.ServiceDate != null)
                    {
                        o.ServiceTime = Converter.ParseDateTime(o.ServiceDate).ToString("yyyy-MM-dd") + " " + o.ServiceTime;
                    }
                    if (o.IsBandCar != null)
                    {
                        o.IsBandCarName = o.IsBandCar == (int)RCHL.Model.Enums.IsBandType.带车 ? "个人提供" : "教练提供";
                    }
                });
            }
            var JsonData = new
            {
                rows = pageList,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };

            return Content(JsonData.ToJson());
        }


        /// <summary>
        /// 实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = WithDrivingOrderBLL.Instance.GetEntity(keyValue);

            return Content(data.ToJson());
        }
        /// <summary>
        /// 实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpPost]
        public ActionResult RemoveForm(string keyValue)
        {
            try
            {
                string[] keys = keyValue.Split(',');
                if (keys != null)
                {
                    bool flag = true;
                    foreach (var key in keys)
                    {
                        var entity = WithDrivingOrderBLL.Instance.GetEntity(key);
                        if (entity != null && (entity.Status != (int)RCHL.Model.Enums.PaySatus.待支付 && entity.Status != (int)RCHL.Model.Enums.PaySatus.已取消))
                        {
                            flag = false;
                            return Error("非待支付/已取消状态的订单不能删除操作");
                        }
                    }
                    if (flag)
                    {
                        foreach (var key in keys)
                        {
                            WithDrivingOrderBLL.Instance.Delete(key);
                        }
                    }
                }
                return Success("删除成功");
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "WithDrivingOrderController>>RemoveForm";
                new ExceptionHelper().LogException(ex);
                return Error("删除失败");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, WithDrivingOrderEntity entity)
        {
            try
            {
                if (keyValue != "")
                {
                    entity.DrivingOrderId = keyValue;
                    WithDrivingOrderBLL.Instance.Update(entity);
                }
                return Success("保存成功");
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "WithDrivingOrderController>>Register";
                new ExceptionHelper().LogException(ex);
                return Error("保存失败");
            }

        }

      
        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="queryJson"></param>
        public void ExportExcel(string queryJson)
        {
            string cacheKey = Request["cacheid"] as string;
            HttpRuntime.Cache[cacheKey + "-state"] = "processing";
            HttpRuntime.Cache[cacheKey + "-row"] = "0";
            try
            {

                WithDrivingOrderEntity para = new WithDrivingOrderEntity();
                if (!string.IsNullOrWhiteSpace(queryJson))
                {
                    //这里要url解码
                    var queryParam = Server.UrlDecode(queryJson).ToJObject();
                    //类型
                    if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
                    {
                        var condition = queryParam["condition"].ToString().ToLower();
                        switch (condition)
                        {
                            case "drivingorderno":
                                para.DrivingOrderNo = queryParam["keyword"].ToString();
                                break;
                            case "membername":
                                para.MemberName = queryParam["keyword"].ToString();
                                break;
                            case "membermobile":
                                para.MemberMobile = queryParam["keyword"].ToString();
                                break;
                            case "schoolname":
                                para.SchoolName = queryParam["keyword"].ToString();
                                break;
                            case "teachername":
                                para.TeacherName = queryParam["keyword"].ToString();
                                break;
                        }
                    }
                    if (!queryParam["status"].IsEmpty())
                    {
                        para.Status = int.Parse(queryParam["status"].ToString());
                    }
                }

                var list = WithDrivingOrderBLL.Instance.GetList(para);
                if (list != null)
                {
                    list.ForEach((o) =>
                    {
                        if (o.ServiceDate != null)
                        {
                            o.ServiceTime = Converter.ParseDateTime(o.ServiceDate).ToString("yyyy-MM-dd") + " " + o.ServiceTime;
                        }
                        if (o.Status != null)
                        {
                            o.StatusName = ((RCHL.Model.Enums.StudySubscribeStatus)o.Status).ToString();
                        }
                        if (o.IsBandCar != null)
                        {
                            o.IsBandCarName = o.IsBandCar == (int)RCHL.Model.Enums.IsBandType.带车 ? "个人提供" : "教练提供";
                        }
                    });

                    //设置导出格式
                    ExcelConfig excelconfig = new ExcelConfig();
                    excelconfig.Title = "陪驾订单";
                    excelconfig.TitleFont = "微软雅黑";
                    excelconfig.TitlePoint = 10;
                    excelconfig.FileName = "陪驾订单.xls";
                    excelconfig.IsAllSizeColumn = true;
                    //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                    List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                    excelconfig.ColumnEntity = listColumnEntity;
                    ColumnEntity columnentity = new ColumnEntity();
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "WithDrivingOrderNo", ExcelColumn = "订单号", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "MemberName", ExcelColumn = "学员用户名", Width = 15 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "MemberMobile", ExcelColumn = "联系方式", Width = 15 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ServiceTime", ExcelColumn = "预约时间", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Price", ExcelColumn = "费用", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "IsBandCarName", ExcelColumn = "带车信息", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "SchoolName", ExcelColumn = "预约驾校", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "TeacherName", ExcelColumn = "教练", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "StatusName", ExcelColumn = "状态", Width = 20 });
                    //需合并索引
                    //excelconfig.MergeRangeIndexArr = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };

                    //调用导出方法
                    ExcelHelper<WithDrivingOrderEntity>.ExcelDownload(list, excelconfig);
                    HttpRuntime.Cache[cacheKey + "-state"] = "done";
                }
            }
            catch (Exception)
            {
                HttpRuntime.Cache[cacheKey + "-state"] = "error";
            }
        }
    }
}

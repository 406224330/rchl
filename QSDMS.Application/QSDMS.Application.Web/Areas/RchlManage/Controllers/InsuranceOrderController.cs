﻿using iFramework.Framework;
using QSDMS.Application.Web.Controllers;
using RCHL.Business;
using RCHL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QSDMS.Util;
using QSDMS.Util.Extension;
using QSDMS.Util.WebControl;
using QSDMS.Util.Excel;
namespace QSDMS.Application.Web.Areas.RchlManage.Controllers
{
    public class InsuranceOrderController : BaseController
    {
        //
        // GET: /RchlManage/InsuranceOrder/

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
            InsuranceOrderEntity para = new InsuranceOrderEntity();
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                var queryParam = queryJson.ToJObject();

                //类型
                if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
                {
                    var condition = queryParam["condition"].ToString().ToLower();
                    switch (condition)
                    {
                        case "insuranceorderno":
                            para.InsuranceOrderNo = queryParam["keyword"].ToString();
                            break;
                        case "membername":
                            para.MemberName = queryParam["keyword"].ToString();
                            break;
                        case "mobile":
                            para.Mobile = queryParam["keyword"].ToString();
                            break;
                        case "carnum":
                            para.CarNum = queryParam["keyword"].ToString();
                            break;
                    }
                }
                if (!queryParam["status"].IsEmpty())
                {
                    para.Status = int.Parse(queryParam["status"].ToString());
                }
            }

            var pageList = InsuranceOrderBLL.Instance.GetPageList(para, ref pagination);
            if (pageList != null)
            {
                pageList.ForEach((o) =>
                {
                    if (o.Status != null)
                    {
                        o.StatusName = ((RCHL.Model.Enums.SubscribeStatus)o.Status).ToString();
                    }

                    if (o.ServiceDate != null)
                    {
                        o.ServiceTime = Converter.ParseDateTime(o.ServiceDate).ToString("yyyy-MM-dd") + " " + o.ServiceTime;
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
            var data = InsuranceOrderBLL.Instance.GetEntity(keyValue);

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
                        var entity = InsuranceOrderBLL.Instance.GetEntity(key);
                        if (entity != null && entity.Status != (int)RCHL.Model.Enums.SubscribeStatus.预约成功)
                        {
                            flag = false;
                            return Error("非预约成功状态的订单不能删除操作");
                        }
                    }
                    if (flag)
                    {
                        foreach (var key in keys)
                        {
                            InsuranceOrderBLL.Instance.Delete(key);
                        }
                    }
                }
                return Success("删除成功");
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "InsuranceOrderController>>RemoveForm";
                new ExceptionHelper().LogException(ex);
                return Error("删除失败");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string json)
        {
            try
            {
                return Success("保存成功");
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "InsuranceOrderController>>Register";
                new ExceptionHelper().LogException(ex);
                return Error("保存失败");
            }

        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Cancel(string keyValue)
        {
            try
            {
                string[] keys = keyValue.Split(',');
                if (keys != null)
                {
                    bool flag = true;
                    foreach (var key in keys)
                    {
                        var entity = InsuranceOrderBLL.Instance.GetEntity(key);
                        if (entity != null && entity.Status != (int)RCHL.Model.Enums.SubscribeStatus.预约成功)
                        {
                            flag = false;
                            return Error("非预约成功状态的订单不能取消操作");
                        }
                    }
                    if (flag)
                    {
                        foreach (var key in keys)
                        {
                            var entity = InsuranceOrderBLL.Instance.GetEntity(key);
                            if (entity != null)
                            {
                                entity.Status = (int)RCHL.Model.Enums.SubscribeStatus.已取消;
                                InsuranceOrderBLL.Instance.Update(entity);
                            }
                        }
                    }
                }
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "InsuranceOrderController>>Cancel";
                new ExceptionHelper().LogException(ex);
                return Error("操作失败");
            }

        }

        /// <summary>
        /// 导出EXCEL
        /// </summary>
        /// <param name="productOutId"></param>
        /// <param name="conditions"></param>
        public void ExportExcel(string queryJson)
        {
            string cacheKey = Request["cacheid"] as string;
            HttpRuntime.Cache[cacheKey + "-state"] = "processing";
            HttpRuntime.Cache[cacheKey + "-row"] = "0";
            try
            {

                InsuranceOrderEntity para = new InsuranceOrderEntity();
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
                            case "insuranceorderno":
                                para.InsuranceOrderNo = queryParam["keyword"].ToString();
                                break;
                            case "membername":
                                para.MemberName = queryParam["keyword"].ToString();
                                break;
                            case "mobile":
                                para.Mobile = queryParam["keyword"].ToString();
                                break;
                            case "CarNum":
                                para.CarNum = queryParam["keyword"].ToString();
                                break;
                        }
                    }
                    if (!queryParam["status"].IsEmpty())
                    {
                        para.Status = int.Parse(queryParam["status"].ToString());
                    }
                }

                var list = InsuranceOrderBLL.Instance.GetList(para);
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
                            o.StatusName = ((RCHL.Model.Enums.SubscribeStatus)o.Status).ToString();
                        }
                    });

                    //设置导出格式
                    ExcelConfig excelconfig = new ExcelConfig();
                    excelconfig.Title = "保险预约订单";
                    excelconfig.TitleFont = "微软雅黑";
                    excelconfig.TitlePoint = 10;
                    excelconfig.FileName = "保险预约订单.xls";
                    excelconfig.IsAllSizeColumn = true;
                    //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                    List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                    excelconfig.ColumnEntity = listColumnEntity;
                    ColumnEntity columnentity = new ColumnEntity();
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "InsuranceOrderNo", ExcelColumn = "订单号", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "UserName", ExcelColumn = "用户名", Width = 15 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Mobile", ExcelColumn = "联系方式", Width = 15 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "CarNum", ExcelColumn = "车牌号", Width = 15 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "CarFrameNum", ExcelColumn = "车架号", Width = 15 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ServiceTime", ExcelColumn = "预约时间", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "InsuranceName", ExcelColumn = "保险公司", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "StatusName", ExcelColumn = "状态", Width = 20 });
                    //需合并索引
                    //excelconfig.MergeRangeIndexArr = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };

                    //调用导出方法
                    ExcelHelper<InsuranceOrderEntity>.ExcelDownload(list, excelconfig);
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

﻿using QSDMS.Util;
using RCHL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QSDMS.Util;
using QSDMS.Util.Extension;
using RCHL.Business;
using QSDMS.Business;
using QSMS.API.BaiduMap;
using iFramework.Framework;
using Newtonsoft.Json;

namespace RCHL.WeiXinWeb.Controllers
{
    public class TakeAuditController : BaseController
    {
        private static object objLock = new object();
        //
        // GET: /TakeAudit/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Return()
        {
            return View();
        }
        public ActionResult VsAudit()
        {
            var notice = SettingsBLL.Instance.GetRemark("dsxz");
            ViewBag.Notice = notice;
            return View();
        }
        public ActionResult OrderSubmit()
        {
            return View();
        }
        /// <summary>
        /// 获取年检机构列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetList(string queryJson)
        {
            var result = new ReturnMessage(false) { Message = "加载列表失败!" };
            try
            {
                int type = 1;
                AuditOrganizationEntity para = new AuditOrganizationEntity();
                if (!string.IsNullOrWhiteSpace(queryJson))
                {
                    var queryParam = queryJson.ToJObject();
                    if (!queryParam["lat"].IsEmpty())
                    {
                        para.Lat = decimal.Parse(queryParam["lat"].ToString());
                    }
                    if (!queryParam["lng"].IsEmpty())
                    {
                        para.Lng = decimal.Parse(queryParam["lng"].ToString());
                    }
                    if (!queryParam["type"].IsEmpty())
                    {
                        type = int.Parse(queryParam["type"].ToString());
                    }
                }
                para.IsTake = 1;
                para.Status = (int)Model.Enums.UseStatus.启用;

                var pageList = AuditOrganizationBLL.Instance.GetList(para);
                if (pageList != null)
                {
                    pageList.ForEach((page) =>
                    {
                        if (page.FaceImage != null)
                        {
                            var imageHost = System.Configuration.ConfigurationManager.AppSettings["ImageHost"] == "" ? string.Format("http://{0}{1}", Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port) : System.Configuration.ConfigurationManager.AppSettings["ImageHost"];
                            page.FaceImage = imageHost + page.FaceImage;
                        }
                        if (page.ProvinceId != null)
                        {
                            page.ProvinceName = AreaBLL.Instance.GetEntity(page.ProvinceId).AreaName;
                        }
                        if (page.CityId != null)
                        {
                            page.CityName = AreaBLL.Instance.GetEntity(page.CityId).AreaName;
                        }
                        if (page.CountyId != null)
                        {
                            page.CountyName = AreaBLL.Instance.GetEntity(page.CountyId).AreaName;
                        }
                        page.AddressInfo = page.ProvinceName + page.CityName + page.CountyName + page.AddressInfo;

                        if (page.Lat != null && page.Lng != null && para.Lat != null && para.Lng != null)
                        {
                            page.HowLong = HarvenSin.GetDistance(
                                new Point2D()
                                {
                                    Lng = (double)para.Lng,
                                    Lat = (double)para.Lat
                                },
                                new Point2D()
                                {
                                    Lng = (double)page.Lng,
                                    Lat = (double)page.Lat
                                }).ToString("f2");
                        }
                        else
                        {
                            page.HowLong = "未知";
                        }
                    });
                }
                switch (type)
                {
                    case 1://按距离
                        pageList = pageList.OrderBy(i => i.HowLong).ThenBy(i => i.HowLong).ToList();
                        break;
                    case 2://按默认排序
                        pageList = pageList.OrderBy(i => i.MakeMoney).ThenBy(i => i.MakeMoney).ToList();
                        break;
                }
                result.IsSuccess = true;
                result.Message = "加载列表成功!";
                result.ResultData["List"] = pageList;

            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "TakeAuditController>>List";
                new ExceptionHelper().LogException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAuditModel(string id)
        {
            var result = new ReturnMessage(false) { Message = "获取对象失败!" };
            try
            {
                var data = AuditOrganizationBLL.Instance.GetEntity(id);
                if (data != null)
                {
                    if (data.FaceImage != null)
                    {
                        var imageHost = System.Configuration.ConfigurationManager.AppSettings["ImageHost"] == "" ? string.Format("http://{0}{1}", Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port) : System.Configuration.ConfigurationManager.AppSettings["ImageHost"];
                        data.FaceImage = imageHost + data.FaceImage;
                    }
                    //pic
                    var imageList = AttachmentPicBLL.Instance.GetList(new AttachmentPicEntity() { ObjectId = data.OrganizationId });
                    if (imageList != null)
                    {
                        data.AttachmentPic = imageList.OrderBy(i => i.SortNum).ThenBy(i => i.SortNum).ToList();
                        data.AttachmentPic.ForEach((o) =>
                        {
                            if (o.PicName != null)
                            {
                                var imageHost = System.Configuration.ConfigurationManager.AppSettings["ImageHost"] == "" ? string.Format("http://{0}{1}", Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port) : System.Configuration.ConfigurationManager.AppSettings["ImageHost"];
                                o.PicName = imageHost + o.PicName;
                            }
                        });
                    }

                    //address
                    if (data.ProvinceId != null)
                    {
                        data.ProvinceName = AreaBLL.Instance.GetEntity(data.ProvinceId).AreaName;
                    }
                    if (data.CityId != null)
                    {
                        data.CityName = AreaBLL.Instance.GetEntity(data.CityId).AreaName;
                    }
                    if (data.CountyId != null)
                    {
                        data.CountyName = AreaBLL.Instance.GetEntity(data.CountyId).AreaName;
                    }
                    data.AddressInfo = data.ProvinceName + data.CityName + data.CountyName + data.AddressInfo;
                }

                result.IsSuccess = true;
                result.Message = "获取成功!";
                result.ResultData["Data"] = data;

            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "TakeAuditController>>GetSchoolModel";
                new ExceptionHelper().LogException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateOrder(string data)
        {
            var result = new ReturnMessage(false) { Message = "创建订单失败!" };
            try
            {
                lock (objLock)
                {
                    var order = JsonConvert.DeserializeObject<TakeAuditOrderEntity>(data);
                    if (order == null)
                    {
                        return Json(result);
                    }
                    order.TakeAuditOrderId = Util.NewUpperGuid();
                    order.TakeAuditOrderNo = TakeAuditOrderBLL.Instance.GetOrderNo();
                    order.CreateTime = DateTime.Now;
                    order.Status = (int)Model.Enums.PaySatus.待支付;
                    order.MemberId = LoginUser == null ? "" : LoginUser.UserId;
                    TakeAuditOrderBLL.Instance.Add(order);
                    result.IsSuccess = true;
                    result.Message = "创建成功";

                    if (LoginUser != null)
                    {
                        //写消息
                        string servicetime = string.Format("{0} {1}", DateTime.Parse(order.ServiceDate.ToString()).ToString("yyyy-MM-dd"), order.ServiceTime);
                        string txt = "预约代审,检测机构：" + order.OrganizationName;
                        SendSysMessageBLL.SendMessage(RCHL.Model.Enums.MessageAlterType.代审预约提示, RCHL.Model.Enums.NoticeType.预约提醒, LoginUser.UserId,order.MemberName, servicetime, txt, order.TakeAuditOrderNo);

                        //送积分
                        GivePointBLL.GivePoint(RCHL.Model.Enums.OperationType.预约代审完成, LoginUser.UserId, double.Parse(order.Price.ToString()), order.TakeAuditOrderNo);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "TakeAuditController>>CreateOrder";
                new ExceptionHelper().LogException(ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 查询本人的代审订单
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMyTakeAudit(int status)
        {
            var result = new ReturnMessage(false) { Message = "获取失败!" };
            try
            {
                TakeAuditOrderEntity para = new TakeAuditOrderEntity();
                if (status != -1)
                {
                    para.Status = status;
                }
                para.MemberId = LoginUser.UserId;
                para.sidx = "CreateTime";
                para.sord = "desc";
                var list = TakeAuditOrderBLL.Instance.GetList(para);
                list.Foreach((o) =>
                {
                    if (o.Status != null)
                    {
                        o.StatusName = ((RCHL.Model.Enums.PaySatus)o.Status).ToString();
                    }
                    if (o.OrganizationId != null)
                    {
                        o.Audit = AuditOrganizationBLL.Instance.GetEntity(o.OrganizationId);
                        if (o.Audit != null)
                        {
                            if (o.Audit.ProvinceId != null)
                            {
                                o.Audit.ProvinceName = AreaBLL.Instance.GetEntity(o.Audit.ProvinceId).AreaName;
                            }
                            if (o.Audit.CityId != null)
                            {
                                o.Audit.CityName = AreaBLL.Instance.GetEntity(o.Audit.CityId).AreaName;
                            }
                            if (o.Audit.CountyId != null)
                            {
                                o.Audit.CountyName = AreaBLL.Instance.GetEntity(o.Audit.CountyId).AreaName;
                            }
                            o.Audit.AddressInfo = o.Audit.ProvinceName + o.Audit.CityName + o.Audit.CountyName + o.Audit.AddressInfo;

                        }
                    }
                });
                result.IsSuccess = true;
                result.Message = "获取成功";
                result.ResultData["List"] = list;
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "TakeAuditController>>GetMyAudit";
                new ExceptionHelper().LogException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Cancel(string id)
        {
            var result = new ReturnMessage(false) { Message = "操作失败!" };
            try
            {
                TakeAuditOrderEntity entity = new TakeAuditOrderEntity();
                entity.TakeAuditOrderId = id;
                entity.Status = (int)RCHL.Model.Enums.PaySatus.已取消;
                TakeAuditOrderBLL.Instance.Update(entity);
                result.IsSuccess = true;
                result.Message = "取消成功";

            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "TakeAuditController>>Cancel";
                new ExceptionHelper().LogException(ex);
            }
            return Json(result);
        }

    }
}

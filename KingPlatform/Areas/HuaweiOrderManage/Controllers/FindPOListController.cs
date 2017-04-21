using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Basic.Code;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;

namespace KingPlatform.Areas.HuaweiOrderManage.Controllers
{
    public class FindPOListController : ControllerBase
    {
        string findPOListurl = "https://api-beta.huawei.com:443/service/esupplier/findPoLineList/1.0.0/1";
        string url_token = "https://api-beta.huawei.com:443/oauth2/token";
        string key = "GHprAo4oNpiqPrre993DpW2KGy8a";
        string secury = "O4gunWWZ6pIfwAO94qcF0tcPFBwa";
        GetPOListParamBack getPOListParamBack = new GetPOListParamBack();

        /// <summary>
        /// 获取新POList
        /// </summary>
        /// <returns></returns>
        
        public ActionResult GetPOList()
        {
            //获取华为access_token
            string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));
            JavaScriptSerializer js = new JavaScriptSerializer();
            //定义接口json数据 获取新POList传入参数
            GetPOListParam param = new GetPOListParam("all", "all", "P", "all", "COL_TASK_STATUS", "huaweiPublishOrder", "P");
            string json = js.Serialize(param);
            string result = HttpMethods.HttpPost(findPOListurl, json, true, accessToken);
            result = result.Replace(":null", ":''");

            getPOListParamBack = js.Deserialize<GetPOListParamBack>(result);
            this.TempData["NewPOList"] = getPOListParamBack;
            var data = new
            {
                rows = getPOListParamBack.body.result,
                total = getPOListParamBack.body.pageVO.totalPages,
                page = getPOListParamBack.body.pageVO.curPage,
                records = getPOListParamBack.body.pageVO.totalRows
            };

            return Content(data.ToJson());
        }

        /// <summary>
        /// 获取取消POList
        /// </summary>
        /// <returns></returns>
        
        public ActionResult CancelPOList()
        {
            //获取华为access_token
            string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));
            JavaScriptSerializer js = new JavaScriptSerializer();
            //定义接口json数据 获取取消POList传入参数
            GetPOListParam param = new GetPOListParam("all", "all", "P", "all", "COL_TASK_STATUS", "huaweiNotifyCancelOrder", "P");
            string json = js.Serialize(param);
            string result = HttpMethods.HttpPost(findPOListurl, json, true, accessToken);
            result = result.Replace(":null", ":''");

            getPOListParamBack = js.Deserialize<GetPOListParamBack>(result);
            this.TempData["CancelPOList"] = getPOListParamBack;
            var data = new
            {
                rows = getPOListParamBack.body.result,
                total = getPOListParamBack.body.pageVO.totalPages,
                page = getPOListParamBack.body.pageVO.curPage,
                records = getPOListParamBack.body.pageVO.totalRows
            };

            return Content(data.ToJson());
        }

        /// <summary>
        /// 获取变更POList
        /// </summary>
        /// <returns></returns>
        
        public ActionResult ChangePOList()
        {
            //获取华为access_token
            string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));
            JavaScriptSerializer js = new JavaScriptSerializer();
            //定义接口json数据 获取取消POList传入参数
            GetPOListParam param = new GetPOListParam("all", "all", "P", "all", "COL_TASK_STATUS", "huaweiNotifyOrderChange", "P");
            string json = js.Serialize(param);
            string result = HttpMethods.HttpPost(findPOListurl, json, true, accessToken);
            result = result.Replace(":null", ":''");

            getPOListParamBack = js.Deserialize<GetPOListParamBack>(result);
            this.TempData["ChangePOList"] = getPOListParamBack;
            var data = new
            {
                rows = getPOListParamBack.body.result,
                total = getPOListParamBack.body.pageVO.totalPages,
                page = getPOListParamBack.body.pageVO.curPage,
                records = getPOListParamBack.body.pageVO.totalRows
            };

            return Content(data.ToJson());
        }

        /// <summary>
        /// 查询PO行变更明细
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult FindColTask(string keyValue)
        {
            string getPODetailsurl = "https://api-beta.huawei.com:443/service/esupplier/findColTaskList/1.0.0/1";
            //获取华为access_token
            string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));
            JavaScriptSerializer js = new JavaScriptSerializer();
            getPOListParamBack = new GetPOListParamBack();
            getPOListParamBack = this.TempData["ChangePOList"] as GetPOListParamBack;
            GetPODetails getPODetails = new GetPODetails();
            getPODetails.poNumber = getPOListParamBack.body.result[Int32.Parse(keyValue) - 1].poNumber;
            getPODetails.poLineNum = getPOListParamBack.body.result[Int32.Parse(keyValue) - 1].poLineNum;
            getPODetails.agentName = getPOListParamBack.body.result[Int32.Parse(keyValue) - 1].agentName;
            getPODetails.agentEmployeeNumber = getPOListParamBack.body.result[Int32.Parse(keyValue) - 1].agentEmployeeNumber;
            getPODetails.lineLocationId = getPOListParamBack.body.result[Int32.Parse(keyValue) - 1].lineLocationId;
            getPODetails.instanceId = getPOListParamBack.body.result[Int32.Parse(keyValue) - 1].instanceId;
            getPODetails.shipmentNum = getPOListParamBack.body.result[Int32.Parse(keyValue) - 1].shipmentNum;
            getPODetails.poReleaseId = getPOListParamBack.body.result[Int32.Parse(keyValue) - 1].poReleaseId;
            getPODetails.poHeaderId = getPOListParamBack.body.result[Int32.Parse(keyValue) - 1].poHeaderId;
            getPODetails.selectType = "0";
            string json = js.Serialize(getPODetails);
            string result = HttpMethods.HttpPost(getPODetailsurl, json, true, accessToken);
            result = result.Replace(":null", ":''");
            this.TempData["ChangePOList"] = getPOListParamBack;
            GetPODetailsBack getPODetailsBack = js.Deserialize<GetPODetailsBack>(result);

            return Content(getPODetailsBack.body.result[0].ToJson());

        }

        /// <summary>
        /// PO签返确认
        /// </summary>
        /// <param name="ids">id值，以#号隔开</param>
        /// <param name="operateType">接受还是驳回</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult SignBackPOList(string ids, string operateType, string types)
        {
            string signBackPOListUrl = "https://api-beta.huawei.com:443/service/esupplier/signBackPOList/1.0.0";
            ConfirmPO confirmPO = new ConfirmPO();
            confirmPO.operateType = operateType;
            string[] idsplit = ids.Split('#');
            getPOListParamBack = new GetPOListParamBack(); 
            if (types == "NewPOList")
            {
                getPOListParamBack = this.TempData["NewPOList"] as GetPOListParamBack;
            }
            else if (types == "CancelPOList")
            {
                getPOListParamBack = this.TempData["CancelPOList"] as GetPOListParamBack;
            }
            else if (types == "ChangePOList")
            {
                getPOListParamBack = this.TempData["ChangePOList"] as GetPOListParamBack;
            }

            for (int i = 0; i < idsplit.Length; i++)
            {
                ColTaskQueries colTaskQueries = new ColTaskQueries();
                colTaskQueries.poNum = getPOListParamBack.body.result[Int32.Parse(idsplit[i]) - 1].poNumber;
                colTaskQueries.poLineNum = getPOListParamBack.body.result[Int32.Parse(idsplit[i]) - 1].poLineNum;
                colTaskQueries.agentName = getPOListParamBack.body.result[Int32.Parse(idsplit[i]) - 1].agentName;
                colTaskQueries.businessType = "all";
                colTaskQueries.instanceId = getPOListParamBack.body.result[Int32.Parse(idsplit[i]) - 1].instanceId;
                colTaskQueries.poHeaderId = getPOListParamBack.body.result[Int32.Parse(idsplit[i]) - 1].poHeaderId;
                colTaskQueries.poReleaseId = getPOListParamBack.body.result[Int32.Parse(idsplit[i]) - 1].poReleaseId;
                colTaskQueries.lineLocationId = getPOListParamBack.body.result[Int32.Parse(idsplit[i]) - 1].lineLocationId;
                confirmPO.colTaskQueries.Add(colTaskQueries);
            }
            //获取华为access_token
            string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));
            JavaScriptSerializer js = new JavaScriptSerializer();
            //定义接口json数据
            string json = js.Serialize(confirmPO);

            string result = HttpMethods.HttpPost(signBackPOListUrl, json, true, accessToken);
            result = result.Replace(":null", ":''");
            ConfirmPOBack confirmPOBack = js.Deserialize<ConfirmPOBack>(result);
            string returnStr = "", passPOStr = "", nopassPOStr = "";
            for (int i = 0; i < idsplit.Length; i++)
            {
                if (confirmPOBack.body.data[i].code == "00000")
                {
                    passPOStr += confirmPOBack.body.data[i].poNum + "/";
                }
                else
                {
                    nopassPOStr += confirmPOBack.body.data[i].poNum + "/";
                }
            }
            if (passPOStr != "")
            {
                returnStr += "订单 " + passPOStr + " 处理成功！";
            }
            if (nopassPOStr != "")
            {
                returnStr += "/n订单 " + nopassPOStr + " 处理失败！";
            }
            return Success(returnStr);
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Cancel()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Change()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Warning()
        {
            return View();
        }


        #region GetPODetails 查询PO行变更明细入参
        /// <summary>
        /// 查询PO行变更明细参数
        /// </summary>
        public class GetPODetails
        {
            public string poNumber { get; set; }                    //PO号
            public string poLineNum { get; set; }                   //PO行号
            public string agentName { get; set; }                   //采购员名称
            public string agentEmployeeNumber { get; set; }         //采购员工号
            public long lineLocationId { get; set; }                //PO发运行ID
            public int instanceId { get; set; }                     //帐套标识
            public long shipmentNum { get; set; }                   //发运行号
            public long poReleaseId { get; set; }                   //PO释放ID
            public long poHeaderId { get; set; }                    //PO头ID
            public string selectType { get; set; }                  //选择类型  固定值：0
        }
        #endregion

        #region GetPODetailsBack 查询PO行变更明细出参
        /// <summary>
        /// 查询PO行变更明细参数
        /// </summary>
        public class GetPODetailsBack
        {
            public PODetailsBody body { get; set; }
        }
        public class PODetailsBody
        {
            public List<PODetailsResult> result { get; set; }
            public PODetailsBody()
            {
                result = new List<PODetailsResult>();
            }
            public PageVO pageVO { get; set; }
            public POConfig config { get; set; }
        }
        public class POConfig
        {
            public string config { get; set; }
        }
        public class PODetailsResult
        {
            public string poNumber { get; set; }                //PO号
            public string poLineNum { get; set; }               //PO行号
            public string changeColumnName { get; set; }        //变更字段
            public string changePreContent { get; set; }        //变更前内容
            public string changeAfterContent { get; set; }      //变更后内容
            public string openRemark { get; set; }              //变更理由
            public string memo { get; set; }                    //备注
            public string lineLocationId { get; set; }            //PO发运行ID
            public string poHeaderId { get; set; }                //PO头ID
        }
        #endregion

        #region ConfirmPO 新PO签返确认的入参
        public class ConfirmPO
        {
            public string operateType { get; set; }                     //接受还是驳回 "accept"是接受；"reject"是驳回
            public List<ColTaskQueries> colTaskQueries { get; set; }
            public ConfirmPO()
            {
                colTaskQueries = new List<ColTaskQueries>();
            }
        }

        public class ColTaskQueries
        {
            public string poNum { get; set; }                       //PO号
            public string poLineNum { get; set; }                   //PO行号
            public string agentName { get; set; }                   //采购员名称
            public string businessType { get; set; }                //PO待办类型 固定值：all
            public int instanceId { get; set; }                  //系统帐套标识
            public long poHeaderId { get; set; }                  //PO头ID
            public long poReleaseId { get; set; }                 //PO释放ID
            public long lineLocationId { get; set; }              //PO发运行ID
        }
        #endregion

        #region ConfirmPOBack 新PO签返确认的出参
        /// <summary>
        /// 新PO签返确认参数
        /// </summary>
        public class ConfirmPOBack
        {
            public POBackBody body { get; set; }
        }

        public class POBackBody
        {
            public string result { get; set; }
            public string code { get; set; }
            public bool success { get; set; }
            public bool failed { get; set; }
            public List<POBackData> data { get; set; }
            public POBackBody()
            {
                data = new List<POBackData>();
            }
        }

        public class POBackData
        {
            public string poNum { get; set; }               //PO号
            public string poLineNum { get; set; }           //PO行号
            public string code { get; set; }                //返回码 “00000”是成功，其他都是失败
            public string msg { get; set; }                 //返回信息描述
            public string taskIndex { get; set; }           //
        }
        #endregion

        #region GetPOListParam 查询新PO列表的入参
        /// <summary>
        /// 定义接口json数据
        /// </summary>
        public class GetPOListParam
        {
            public GetPOListParam()
            { }
            public GetPOListParam(string combo3, string combo2, string combo1, string shipmentStatus, string statusType, string colTaskOrPoStatus, string poSubType)
            {
                this.combo3 = combo3;
                this.combo2 = combo2;
                this.combo1 = combo1;
                this.shipmentStatus = shipmentStatus;
                this.statusType = statusType;
                this.colTaskOrPoStatus = colTaskOrPoStatus;
                this.poSubType = poSubType;
            }
            public string colTaskOrPoStatus { get; set; }       //任务单状态
            public string poSubType { get; set; }               //PO业务领域
            public string statusType { get; set; }              //状态类别
            public string shipmentStatus { get; set; }
            public string combo1 { get; set; }
            public string combo2 { get; set; }
            public string combo3 { get; set; }
        }
        #endregion

        #region GetPOListParamBack 查询新PO列表的出参
        /// <summary>
        /// POList
        /// </summary>
        public class GetPOListParamBack
        {
            public POListbody body { get; set; }
        }

        /// <summary>
        /// body
        /// </summary>
        public class POListbody
        {
            public List<POListResult> result { get; set; }
            public POListbody()
            {
                result = new List<POListResult>();
            }
            public PageVO pageVO { get; set; }
        }

        /// <summary>
        /// POList具体数据
        /// </summary>
        public class POListResult
        {
            public string vendorCode { get; set; }              //供应商编码
            public string vendorName { get; set; }              //供应商名称
            public string poNumber { get; set; }                //PO号
            public string poLineNum { get; set; }               //PO行号
            public string itemCode { get; set; }                //Item编码
            public string itemDescription { get; set; }         //Item描述
            public string needQuantity { get; set; }            //需求数量
            public string unitOfMeasure { get; set; }           //单位
            public string priceOverride { get; set; }           //单价
            public string currencyCode { get; set; }            //币种
            public string taxRate { get; set; }                 //税率
            public string publishDate { get; set; }             //订单下发日期
            public string shipToLocation { get; set; }          //收货地点
            public string termsMode { get; set; }               //付款方式
            public string issuOffice { get; set; }              //开票单位
            public string orgName { get; set; }                 //华为子公司
            public string itemRevision { get; set; }            //物料/服务编码版本
            public string promiseDate { get; set; }             //承诺日期
            public string expireDate { get; set; }              //订单有效日期
            public string dueQty { get; set; }                  //未交付数量
            public string remark { get; set; }                  //备注
            public string prNumber { get; set; }                //PR号
            public string billToLocation { get; set; }          //开票地址
            public string taskNum { get; set; }                 //任务令
            public string agentName { get; set; }               //采购员
            public string agentEmployeeNumber { get; set; }     //采购员
            public string carrierName { get; set; }             //发运方式
            public long shipmentNum { get; set; }             //发运行号
            public string unSendQuantity { get; set; }          //未交付数量
            public string paymentTerms { get; set; }            //支付条款
            public int instanceId { get; set; }              //华为系统内部标识用
            public long poHeaderId { get; set; }              //华为系统内部标识用
            public long poReleaseId { get; set; }             //华为系统内部标识用
            public long lineLocationId { get; set; }          //华为系统内部标识用
        }

        /// <summary>
        /// POList页码数据
        /// </summary>
        public class PageVO
        {
            public int startIndex { get; set; }
            public int curPage { get; set; }
            public int mysqlStartIndex { get; set; }
            public int endIndex { get; set; }
            public int resultMode { get; set; }
            public int totalPages { get; set; }
            public string tempProp { get; set; }
            public string orderBy { get; set; }
            public int pageSize { get; set; }
            public int totalRows { get; set; }
            public int mysqlEndIndex { get; set; }
            public string filterStr { get; set; }
        }
        #endregion
    }
}
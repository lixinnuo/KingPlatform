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
        string signBackPOListUrl = "https://api-beta.huawei.com:443/service/esupplier/signBackPOList/1.0.0";
        string url_token = "https://api-beta.huawei.com:443/oauth2/token";
        string key = "GHprAo4oNpiqPrre993DpW2KGy8a";
        string secury = "O4gunWWZ6pIfwAO94qcF0tcPFBwa";
        POList poList = new POList();

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

            poList = js.Deserialize<POList>(result);
            this.TempData["NewPOList"] = poList;
            var data = new
            {
                rows = poList.body.result,
                total = poList.body.pageVO.totalPages,
                page = poList.body.pageVO.curPage,
                records = poList.body.pageVO.totalRows
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

            poList = js.Deserialize<POList>(result);
            this.TempData["CancelPOList"] = poList;
            var data = new
            {
                rows = poList.body.result,
                total = poList.body.pageVO.totalPages,
                page = poList.body.pageVO.curPage,
                records = poList.body.pageVO.totalRows
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

            poList = js.Deserialize<POList>(result);
            this.TempData["ChangePOList"] = poList;
            var data = new
            {
                rows = poList.body.result,
                total = poList.body.pageVO.totalPages,
                page = poList.body.pageVO.curPage,
                records = poList.body.pageVO.totalRows
            };

            return Content(data.ToJson());
        }

        /// <summary>
        /// PO签返确认
        /// </summary>
        /// <param name="ids">id值，以#号隔开</param>
        /// <param name="operateType">接受还是驳回</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        public ActionResult SignBackPOList(string ids, string operateType, string types)
        {
            signBackPOList signBackPOList = new signBackPOList();
            signBackPOList.operateType = operateType;
            string[] idsplit = ids.Split('#');
            poList = new POList(); 
            if (types == "NewPOList")
            {
                poList = this.TempData["NewPOList"] as POList;
            }
            else if (types == "CancelPOList")
            {
                poList = this.TempData["CancelPOList"] as POList;
            }
            else if (types == "ChangePOList")
            {
                poList = this.TempData["ChangePOList"] as POList;
            }

            for (int i = 0; i < idsplit.Length; i++)
            {
                ColTaskQueries colTaskQueries = new ColTaskQueries();
                colTaskQueries.poNum = poList.body.result[Int32.Parse(idsplit[i]) - 1].poNumber;
                colTaskQueries.poLineNum = poList.body.result[Int32.Parse(idsplit[i]) - 1].poLineNum;
                colTaskQueries.agentName = poList.body.result[Int32.Parse(idsplit[i]) - 1].agentName;
                colTaskQueries.businessType = "all";
                colTaskQueries.instanceId = poList.body.result[Int32.Parse(idsplit[i]) - 1].instanceId;
                colTaskQueries.poHeaderId = poList.body.result[Int32.Parse(idsplit[i]) - 1].poHeaderId;
                colTaskQueries.poReleaseId = poList.body.result[Int32.Parse(idsplit[i]) - 1].poReleaseId;
                colTaskQueries.lineLocationId = poList.body.result[Int32.Parse(idsplit[i]) - 1].lineLocationId;
                signBackPOList.colTaskQueries.Add(colTaskQueries);
            }
            //获取华为access_token
            string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));
            JavaScriptSerializer js = new JavaScriptSerializer();
            //定义接口json数据
            string json = js.Serialize(poList);

            string result = HttpMethods.HttpPost(signBackPOListUrl, json, true, accessToken);
            result = result.Replace(":null", ":''");
            AcceptRejectResult acceptRejectResult = js.Deserialize<AcceptRejectResult>(result);
            if (acceptRejectResult.code == "00000")
            {
                return Success("处理成功！");
            }
            else
            {
                return Error("处理失败！");
            }
        }

        #region 定义AcceptRejectResult接口 新PO签返确认的出参
        /// <summary>
        /// 新PO签返确认参数
        /// </summary>
        public class AcceptRejectResult
        {
            public string poNum { get; set; }
            public string poLineNum { get; set; }
            public string code { get; set; }
            public string msg { get; set; }
        }
        #endregion

        #region 定义signBackPOList接口 新PO签返确认的入参
        public class signBackPOList
        {
            public string operateType { get; set; }
            public List<ColTaskQueries> colTaskQueries { get; set; }
            public signBackPOList()
            {
                colTaskQueries = new List<ColTaskQueries>();
            }
        }

        public class ColTaskQueries
        {
            public string poNum { get; set; }
            public string poLineNum { get; set; }
            public string agentName { get; set; }
            public string businessType { get; set; }
            public string instanceId { get; set; }
            public string poHeaderId { get; set; }
            public string poReleaseId { get; set; }
            public string lineLocationId { get; set; }
        }
        #endregion

        #region 定义POList接口 查询新PO列表的出参
        /// <summary>
        /// POList
        /// </summary>
        public class POList
        {
            public POListbody body { get; set; }
        }

        /// <summary>
        /// body
        /// </summary>
        public class POListbody
        {
            public List<result> result { get; set; }
            public POListbody()
            {
                result = new List<result>();
            }
            public pageVO pageVO { get; set; }
        }

        /// <summary>
        /// POList具体数据
        /// </summary>
        public class result
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
            public string carrierName { get; set; }             //发运方式
            public string shipmentNum { get; set; }             //发运行号
            public string unSendQuantity { get; set; }          //未交付数量
            public string paymentTerms { get; set; }            //支付条款
            public string instanceId { get; set; }              //华为系统内部标识用
            public string poHeaderId { get; set; }              //华为系统内部标识用
            public string poReleaseId { get; set; }             //华为系统内部标识用
            public string lineLocationId { get; set; }          //华为系统内部标识用
        }

        /// <summary>
        /// POList页码数据
        /// </summary>
        public class pageVO
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

        #region 定义GetPOListParam接口 查询新PO列表的入参
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
            public string colTaskOrPoStatus { get; set; }
            public string poSubType { get; set; }
            public string statusType { get; set; }
            public string shipmentStatus { get; set; }
            public string combo1 { get; set; }
            public string combo2 { get; set; }
            public string combo3 { get; set; }
        }
        #endregion
    }
}
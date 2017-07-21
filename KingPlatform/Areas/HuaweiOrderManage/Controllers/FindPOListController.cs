using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Basic.Code;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;

namespace KingPlatform.Areas.HuaweiOrderManage.Controllers
{
    public class FindPOListController : ControllerBase
    {
        /*string findPOListurl = "https://api-beta.huawei.com:443/service/esupplier/findPoLineList/1.0.0/";               //查询PO列表
        string findPOBoardUrl = "https://api-beta.huawei.com:443/service/esupplier/findProductionPOBoardData/1.0.0";    //查询PO看板
        string url_token = "https://api-beta.huawei.com:443/oauth2/token";                                              //查询华为access_token
        string getPODetailsurl = "https://api-beta.huawei.com:443/service/esupplier/findColTaskList/1.0.0/1";           //查询PO行变更明细
        string signBackPOListUrl = "https://api-beta.huawei.com:443/service/esupplier/signBackPOList/1.0.0";            //PO签返确认
        string onwayPOListUrl = "https://api-beta.huawei.com:443/service/esupplier/applyPromiseDateChange/1.0.0";       //供应商PO变更
        string getHWCorurl = "https://api-beta.huawei.com:443/service/esupplier/findOrganList/1.0.0";                   //华为子公司列表
        string key = "GHprAo4oNpiqPrre993DpW2KGy8a";                                                                    //系统键 测试平台
        string secury = "O4gunWWZ6pIfwAO94qcF0tcPFBwa";  */                                                               //系统值
        
        string findPOListurl = "https://openapi.huawei.com:443/service/esupplier/findPoLineList/1.0.0/";               //查询PO列表
        string findPOBoardUrl = "https://openapi.huawei.com:443/service/esupplier/findProductionPOBoardData/1.0.0";    //查询PO看板
        string url_token = "https://openapi.huawei.com:443/oauth2/token";                                              //查询华为access_token
        string getPODetailsurl = "https://openapi.huawei.com:443/service/esupplier/findColTaskList/1.0.0/1";           //查询PO行变更明细
        string signBackPOListUrl = "https://openapi.huawei.com:443/service/esupplier/signBackPOList/1.0.0";            //PO签返确认
        string onwayPOListUrl = "https://openapi.huawei.com:443/service/esupplier/applyPromiseDateChange/1.0.0";       //供应商PO变更
        string getHWCorurl = "https://openapi.huawei.com:443/service/esupplier/findOrganList/1.0.0";                   //华为子公司列表
        string key = "1W6F7vxypwbPu8ElV6csX6JeBm0a";                                                                    //系统键 正式平台
        string secury = "CbGf_TyvV_iCUUwntP0hQw1n_9sa";                                                                 //系统值
        
        GetPOListParamBack getPOListParamBack = new GetPOListParamBack();
        
        /// <summary>
        /// 查询PO看板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult FindPOBoard()
        {
            string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));      //获取华为access_token
            JavaScriptSerializer js = new JavaScriptSerializer();
            string result = HttpMethods.HttpPost(findPOBoardUrl, "{}", true, accessToken);
            result = result.Replace(":null", ":''");
            GetPOBoard getPOBoard = js.Deserialize<GetPOBoard>(result);
            return Content(getPOBoard.ToJson());
        }

        /// <summary>
        /// 获取POList  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult PostPOList(string poSubType = "P", string shipmentStatus = "all", string businessType = "all", string poStatus = "before_signe_back", string colTaskOrPoStatus = "huaweiPublishOrder", string statusType = "COL_TASK_STATUS", int page = 1, int rows = 20, string sidx = "publishDate", string sord = "asc")
        {
            string findPOListurlTrue = "";
            int getpage = page;
            if (getpage == 1)
            {
                string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));      //获取华为access_token
                JavaScriptSerializer js = new JavaScriptSerializer();
                //定义接口json数据 获取新POList传入参数
                GetPOListParam param = new GetPOListParam(poSubType, shipmentStatus, businessType, poStatus, colTaskOrPoStatus, statusType);
                string json = js.Serialize(param);
                for (int i = 0; ; i++)
                {
                    getpage = page + i;
                    findPOListurlTrue = findPOListurl + getpage.ToString(); //添加页码
                    string result = HttpMethods.HttpPost(findPOListurlTrue, json, true, accessToken);
                    result = result.Replace(":null", ":''");
                    GetPOListParamBack getPOListParamBack1 = new GetPOListParamBack();
                    getPOListParamBack1 = js.Deserialize<GetPOListParamBack>(result);
                    if (i == 0)
                    {
                        getPOListParamBack = getPOListParamBack1;
                    }
                    else
                    {
                        getPOListParamBack.result.AddRange(getPOListParamBack1.result);
                    }
                    if (getPOListParamBack1.result.Count < 100) break;
                }
            }
            else
            {
                getPOListParamBack = this.TempData[colTaskOrPoStatus] as GetPOListParamBack;            //获取储存的华为PO列表(对应colTaskOrPoStatus)
            }

            this.TempData[colTaskOrPoStatus] = getPOListParamBack;                                      //保存华为PO列表(对应colTaskOrPoStatus)

            if (getPOListParamBack.pageVO == null)
            {
                return Error("获取信息失败。");
            }
            else
            {
                POListResult[] poResult;
                POListResult[] poResult1 = getPOListParamBack.result.OrderByDescending(f => f.publishDate).ToArray();
                //Array.Sort(poResult1, (p1, p2) => p1.publishDate.CompareTo(p2.publishDate));
                //poResult1.OrderByDescending(f => f.publishDate);

                if (getPOListParamBack.result.Count > rows * page)
                {
                    poResult = new POListResult[rows];
                    Array.Copy(poResult1, rows * (page - 1), poResult, 0, rows);
                }
                else
                {
                    poResult = new POListResult[poResult1.Length - rows * (page - 1)];
                    Array.Copy(poResult1, rows * (page - 1), poResult, 0, poResult1.Length - rows * (page - 1));
                }

                var data = new
                {
                    rows = poResult,
                    total = Math.Ceiling(Convert.ToDouble(getPOListParamBack.pageVO.totalRows / (rows * 1.00))),
                    page = page,
                    records = getPOListParamBack.pageVO.totalRows
                };
                return Content(data.ToJson());
            }
        }

        /// <summary>
        /// 查询订单明细（根据订单号和行号）
        /// </summary>
        /// <param name="poNumber">订单号</param>
        /// <param name="poLineNum">行号</param>
        /// <param name="types"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPODetail(string poNumber, string poLineNum, string types)
        {
            int num = GetPOListNum(poNumber, poLineNum, types);
            if (num == -1)
            {
                return Error("获取信息失败。");
            }
            else
            {
                getPOListParamBack = this.TempData[types] as GetPOListParamBack;                        //获取储存的华为PO列表(对应types)
                this.TempData[types] = getPOListParamBack;                                              //保存华为PO列表(对应types)     
                return Content(getPOListParamBack.result[num].ToJson());
            }
        }

        /// <summary>
        /// 获取对应types的对应订单号、行号的数组id值
        /// </summary>
        /// <param name="poNumber">订单号</param>
        /// <param name="poLineNum">行号</param>
        /// <param name="types"></param>
        /// <returns></returns>
        public int GetPOListNum(string poNumber, string poLineNum, string types) {
            int num = -1;
            getPOListParamBack = this.TempData[types] as GetPOListParamBack;                        //获取储存的华为PO列表(对应types)
            this.TempData[types] = getPOListParamBack;                                              //保存华为PO列表(对应types)
            for (int j = 0; j < getPOListParamBack.result.Count; j++)                               //遍历getPOListParamBack列表
            {
                if (getPOListParamBack.result[j].poNumber == poNumber && getPOListParamBack.result[j].poLineNum == poLineNum)
                {
                    num = j;
                    break;
                }
            }
            return num;
        }

        /// <summary>
        /// 查询PO行变更明细
        /// </summary>
        /// <param name="poNumber">订单号</param>
        /// <param name="poLineNum">行号</param>
        /// <param name="types"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult FindColTask(string poNumber, string poLineNum, string types)
        {
            int num = GetPOListNum(poNumber, poLineNum, types);
            if (num == -1)
            {
                return Error("获取信息失败。");
            }
            else
            {
                //获取华为access_token
                string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));
                JavaScriptSerializer js = new JavaScriptSerializer();
                getPOListParamBack = this.TempData[types] as GetPOListParamBack;                        //获取储存的华为PO列表(对应types)
                this.TempData[types] = getPOListParamBack;                                              //保存华为PO列表(对应types)
                GetPODetails getPODetails = new GetPODetails();
                getPODetails.poNumber = getPOListParamBack.result[num].poNumber;
                getPODetails.poLineNum = getPOListParamBack.result[num].poLineNum;
                getPODetails.agentName = getPOListParamBack.result[num].agentName;
                getPODetails.agentEmployeeNumber = getPOListParamBack.result[num].agentEmployeeNumber;
                getPODetails.lineLocationId = getPOListParamBack.result[num].lineLocationId;
                getPODetails.instanceId = getPOListParamBack.result[num].instanceId;
                getPODetails.shipmentNum = getPOListParamBack.result[num].shipmentNum;
                getPODetails.poReleaseId = getPOListParamBack.result[num].poReleaseId;
                getPODetails.poHeaderId = getPOListParamBack.result[num].poHeaderId;
                getPODetails.selectType = "0";
                string json = js.Serialize(getPODetails);
                string result = HttpMethods.HttpPost(getPODetailsurl, json, true, accessToken);
                result = result.Replace(":null", ":''");
                GetPODetailsBack getPODetailsBack = js.Deserialize<GetPODetailsBack>(result);

                return Content(getPODetailsBack.result[0].ToJson());
            }
        }

        /// <summary>
        /// PO签返确认
        /// </summary>
        /// <param name="poNumber">订单号</param>
        /// <param name="poLineNum">行号</param>
        /// <param name="types"></param>
        /// <param name="operateType">接受还是驳回</param>
        /// <returns></returns>
        [HandlerAjaxOnly]
        public ActionResult SignBackPOList(string poNumber, string poLineNum, string operateType, string types)
        {
            ConfirmPO confirmPO = new ConfirmPO();
            confirmPO.operateType = operateType;
            string[] poNumberSplit = poNumber.Split('#');
            string[] poLineNumSplit = poLineNum.Split('#');
            getPOListParamBack = this.TempData[types] as GetPOListParamBack;                        //获取储存的华为PO列表(对应types)
            this.TempData[types] = getPOListParamBack;                                              //保存华为PO列表(对应types)

            for (int i = 0; i < poNumberSplit.Length; i++)
            {
                int num = GetPOListNum(poNumberSplit[i], poLineNumSplit[i], types);                 //获取对应数组id

                ColTaskQueries colTaskQueries = new ColTaskQueries();
                colTaskQueries.poNum = getPOListParamBack.result[num].poNumber;
                colTaskQueries.poLineNum = getPOListParamBack.result[num].poLineNum;
                colTaskQueries.agentName = getPOListParamBack.result[num].agentName;
                //colTaskQueries.businessType = "all";
                if (types == "huaweiPublishOrder")
                {
                    colTaskQueries.businessType = "new_po";                         //新PO  签返
                }
                else
                {
                    colTaskQueries.businessType = "po_cancel_notice";               //取消PO  签返
                }
                colTaskQueries.instanceId = getPOListParamBack.result[num].instanceId;
                colTaskQueries.poHeaderId = getPOListParamBack.result[num].poHeaderId;
                colTaskQueries.poReleaseId = getPOListParamBack.result[num].poReleaseId;
                colTaskQueries.lineLocationId = getPOListParamBack.result[num].lineLocationId;
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
            for (int i = 0; i < poNumberSplit.Length; i++)
            {
                if (confirmPOBack.data[i].code == "00000")
                {
                    passPOStr += confirmPOBack.data[i].poNum + "/";
                }
                else
                {
                    nopassPOStr += confirmPOBack.data[i].poNum + "/";
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

        /// <summary>
        /// 供应商PO变更
        /// </summary>
        /// <param name="ids">id值，以#号隔开</param>
        /// <param name="promiseDate">修改的日期，以#号隔开</param>
        /// <param name="types">类型</param>
        /// <returns></returns>
        [HandlerAjaxOnly]
        public ActionResult OnwayPOList(string poNumber, string poLineNum, string promiseDate, string promiseDateChangeReason, string types)
        {
            List<PoLinesAllVO> postOnwayPO = new List<PoLinesAllVO>();
            string[] poNumberSplit = poNumber.Split('#');
            string[] poLineNumSplit = poLineNum.Split('#');
            string[] dateSplit = promiseDate.Split('#');
            string[] reasonSplit = promiseDateChangeReason.Split('#');
            getPOListParamBack = this.TempData[types] as GetPOListParamBack;                        //获取储存的华为PO列表(对应types)
            this.TempData[types] = getPOListParamBack;                                              //保存华为PO列表(对应types)

            for (int i = 0; i < poNumberSplit.Length; i++)
            {
                int num = GetPOListNum(poNumberSplit[i], poLineNumSplit[i], types);                 //获取对应数组id
                PoLinesAllVO poLinesAllVO = new PoLinesAllVO();
                poLinesAllVO.instanceId = getPOListParamBack.result[num].instanceId;
                poLinesAllVO.poNumber = getPOListParamBack.result[num].poNumber;
                poLinesAllVO.lineLocationId = getPOListParamBack.result[num].lineLocationId;
                poLinesAllVO.promiseDateChangeReason = reasonSplit[i];
                poLinesAllVO.typeLookupCode = "2";
                poLinesAllVO.promiseDateStr = dateSplit[i];
                //poLinesAllVO.needQuantity = getPOListParamBack.result[num].needQuantity;
                postOnwayPO.Add(poLinesAllVO);
            }
            //获取华为access_token
            string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));
            JavaScriptSerializer js = new JavaScriptSerializer();
            //定义接口json数据
            string json = js.Serialize(postOnwayPO);

            string result = HttpMethods.HttpPost(onwayPOListUrl, json, true, accessToken);
            result = result.Replace(":null", ":''");
            List<POBackData> pOBackData = js.Deserialize<List<POBackData>>(result);
            string returnStr = "", passPOStr = "", nopassPOStr = "";
            for (int i = 0; i < poNumberSplit.Length; i++)
            {
                if (pOBackData[i].code == "00000")
                {
                    passPOStr += pOBackData[i].poNum + "/";
                }
                else
                {
                    nopassPOStr += pOBackData[i].poNum + "/";
                }
            }
            if (passPOStr != "")
            {
                returnStr += "订单 " + passPOStr + " 提交成功！";
            }
            if (nopassPOStr != "")
            {
                returnStr += "/n订单 " + nopassPOStr + " 提交失败！";
            }
            return Success(returnStr);
        }

        /// <summary>
        /// 华为子公司列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult HWCorList()
        {
            //获取华为access_token
            string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));
            JavaScriptSerializer js = new JavaScriptSerializer();
            //定义接口json数据 查询华为子公司列表入参
            GetHWCor getHWCor = new GetHWCor("zh_CN");
            string json = js.Serialize(getHWCor);
            string result = HttpMethods.HttpPost(getHWCorurl, json, true, accessToken);
            result = result.Replace(":null", ":''");
            GetHWCorBack getHWCorBack = js.Deserialize<GetHWCorBack>(result);
            return Content(getPOListParamBack.ToJson());
        }

        /// <summary>
        /// 导出HW订单pdf
        /// </summary>
        /// <param name="poNumber">订单号</param>
        /// <param name="poLineNum">行号</param>
        /// <param name="types">类型</param>
        /// <returns></returns>
        [HttpGet]
        public void ExportPOPDF(string poNumber, string poLineNum, string types)
        {
            int num = 0;    //定义匹配的序列号
            getPOListParamBack = this.TempData[types] as GetPOListParamBack;                        //获取储存的华为PO列表(对应types)
            this.TempData[types] = getPOListParamBack;                                              //保存华为PO列表(对应types)

            for (int j = 0; j < getPOListParamBack.result.Count; j++)           //遍历getPOListParamBack列表
            {
                if (getPOListParamBack.result[j].poNumber == poNumber && getPOListParamBack.result[j].poLineNum == poLineNum)
                {
                    num = j;
                    break;
                }
            }
            string filePath = "~/Resource/HWPO/pdf/";                                           //文件存放地址
            string fileName = iTextPDF.CreateHWPOPDF(getPOListParamBack.result[num].ToJson(), filePath);        //生产pdf文件，下载

            string filename = Server.UrlDecode(fileName);
            string filepath = Server.MapPath(filePath + fileName);
            if (FileDownHelper.FileExists(filepath))
            {
                FileDownHelper.DownLoadold(filepath, filename);
            }
        }

        [HttpGet]
        public virtual ActionResult News(string Second, string Third)
        {
            ViewData["Second"] = Second;
            ViewData["Third"] = Third;
            return View();
        }
        [HttpGet]
        public virtual ActionResult ChangeDetails()
        {
            return View();
        }
        [HttpGet]
        public virtual ActionResult PODetails()
        {
            return View();
        }


        #region GetHWCor 查询华为子公司列表入参
        public class GetHWCor
        {
            public GetHWCor()
            { }
            public GetHWCor(string lang)
            {
                this.lang = lang;
            }
            public string lang { get; set; }
        }
        #endregion

        #region GetHWCorBack 查询华为子公司列表出参
        public class GetHWCorBack
        {
            public List<HWCorBack> result { get; set; }
            public GetHWCorBack()
            {
                result = new List<HWCorBack>();
            }
            public string pageVO { get; set; }
        }

        public class HWCorBack
        {
            public long organizationId { get; set; }        //子公司Id
            public string companyCode { get; set; }         //子公司编号
            public string i18 { get; set; }                 //i18未知
            public string instanceId { get; set; }          //未知
            public string orgName { get; set; }             //子公司名字
            public string orgCode { get; set; }             //未知
            public string name { get; set; }                //未知
            public string orgNameZh { get; set; }           //子公司中文名字
            public string orgId { get; set; }               //子公司id
            public string orgNameEn { get; set; }           //子公司英文名字
        }
        #endregion

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
            public string shipmentNum { get; set; }                   //发运行号
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
            public List<PODetailsResult> result { get; set; }
            public GetPODetailsBack()
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

        #region ConfirmPO PO签返确认的入参
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

        #region ConfirmPOBack PO签返确认的出参
        /// <summary>
        /// 新PO签返确认参数
        /// </summary>
        public class ConfirmPOBack
        {
            public string result { get; set; }
            public string code { get; set; }
            public bool success { get; set; }
            public bool failed { get; set; }
            public List<POBackData> data { get; set; }
            public ConfirmPOBack()
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
            public bool success { get; set; }
            public string data { get; set; }
        }
        #endregion

        #region GetPOListParam 查询PO列表的入参
        /// <summary>
        /// 定义接口json数据
        /// </summary>
        public class GetPOListParam
        {
            public GetPOListParam()
            { }
            public GetPOListParam(string poSubType, string shipmentStatus, string businessType, string poStatus, string colTaskOrPoStatus, string statusType)
            {
                this.poSubType = poSubType;
                this.shipmentStatus = shipmentStatus;
                this.businessType = businessType;
                this.poStatus = poStatus;
                this.colTaskOrPoStatus = colTaskOrPoStatus;
                this.statusType = statusType;
            }
            
            public string poSubType { get; set; }               //PO业务领域
            public string shipmentStatus { get; set; }
            public string businessType { get; set; }
            public string poStatus { get; set; }
            public string colTaskOrPoStatus { get; set; }       //任务单状态
            public string statusType { get; set; }              //状态类别
        }
        #endregion

        #region GetPOListParamBack 查询PO列表的出参
        /// <summary>
        /// POList
        /// </summary>
        public class GetPOListParamBack
        {
            public List<POListResult> result { get; set; }
            public GetPOListParamBack()
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
            public string objectChangeContext { get; set; }      //修改内容
            public DateTime? pllaAttribute2 { get; set; }          //修改后的承诺日期
            public string poLineNum { get; set; }               //PO行号
            public string shipmentStatus { get; set; }          //订单状态
            public string itemCode { get; set; }                //Item编码
            public string itemDescription { get; set; }         //Item描述
            public int? quantity { get; set; }                  //需求数量
            public int? quantityReceived { get; set; }          //已收货数量
            public int? quantityCancelled { get; set; }          //取消数量
            public int? precision { get; set; }                 //保留小数位数
            public string unitOfMeasure { get; set; }           //单位
            public float? priceOverride { get; set; }           //单价
            public string currencyCode { get; set; }            //币种
            public float? taxRate { get; set; }                 //税率
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
            public string shipmentNum { get; set; }             //发运行号
            public string unSendQuantity { get; set; }          //未交付数量
            public string paymentTerms { get; set; }            //支付条款
            public string revisionNum { get; set; }             //版本号
            public string shipmentType { get; set; }            //PO Type
            public string sendVendorTelNum { get; set; }        //Phone
            public string sendVendorFax { get; set; }           //Fax
            public string businessMode { get; set; }            //businessMode
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

        #region GetPOBoard 查询PO看板出参
        public class GetPOBoard
        {
            public int expiredOrderQuantity { get; set; }               //新订单超三天未处理
            public int arrivalOnWeekOrderQuantity { get; set; }         //预计两周内到货
            public int vendorApplyCancelOrderQuantity { get; set; }     //待华为确认取消
            public int closedOrderQuantity { get; set; }                //订单已关闭
            public int vendorApplyQtyChangeOrderQuantity { get; set; }  //待华为确认数量变更
            public int intransitOrderQuantity { get; set; }             //在途订单
            public int cancelledOrderQuantity { get; set; }             //订单已取消
            public int changedOrderQuantity { get; set; }               //内容变更通知
            public int cancelledToConfirmedOrderQuantity { get; set; }  //订单取消确认
            public int newOrderQuantity { get; set; }                   //新订单
            public int deliveryTimeChangedOrderQuantity { get; set; }   //订单交期更改
            public int nonDeliveryOrderQuantity { get; set; }           //下单六个月未交货
            public int cancelOrderQuantity { get; set; }                //订单取消通知
            public int overdueOrderQuantity { get; set; }               //过期PO
            public int rmaBarterOrderQuantity { get; set; }             //RMA换货订单
            public int deliveryTimeChangedPendingOrderQuantity { get; set; }        //待华为确认交期更改 
            public int closed4Receving { get; set; }                    //已交货未关闭
        }
        #endregion

        #region PoLinesAllVO 供应商变更PO入参
        public class PoLinesAllVO
        {
            public int instanceId { get; set; }                     //帐套标识
            public string poNumber { get; set; }                    //PO号
            public long lineLocationId { get; set; }                //PO行ID
            public string promiseDateChangeReason { get; set; }     //修改原因
            public string typeLookupCode { get; set; }              //修改类型  “2”是修改交期 “1”是修改数量 “3”是既修改交期也修改数量
            public string promiseDateStr { get; set; }              //新的交期时间
            public int? needQuantity { get; set; }               //新的修改数量
        }
        #endregion
    }
}
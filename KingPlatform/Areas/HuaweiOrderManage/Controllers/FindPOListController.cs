using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Basic.Code;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using KingPlatform.Areas.HuaweiOrderManage.Models;

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
        public ActionResult PostPOList(string poSubType = "P", string shipmentStatus = "all", string businessType = "all", string poStatus = "before_signe_back", string colTaskOrPoStatus = "huaweiPublishOrder", string statusType = "COL_TASK_STATUS", string poTypes = "", int page = 1, int rows = 20, string sidx = "publishDate", string sord = "asc")
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
                getPOListParamBack = this.TempData[poTypes] as GetPOListParamBack;            //获取储存的华为PO列表(对应poTypes)
            }

            this.TempData[poTypes] = getPOListParamBack;                                      //保存华为PO列表(对应poTypes)

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
        /// <param name="poTypes"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPODetail(string poNumber, string poLineNum, string poTypes)
        {
            int num = GetPOListNum(poNumber, poLineNum, poTypes);
            if (num == -1)
            {
                return Error("获取信息失败。");
            }
            else
            {
                getPOListParamBack = this.TempData[poTypes] as GetPOListParamBack;                        //获取储存的华为PO列表(对应types)
                this.TempData[poTypes] = getPOListParamBack;                                              //保存华为PO列表(对应types)     
                return Content(getPOListParamBack.result[num].ToJson());
            }
        }

        /// <summary>
        /// 获取对应types的对应订单号、行号的数组id值
        /// </summary>
        /// <param name="poNumber">订单号</param>
        /// <param name="poLineNum">行号</param>
        /// <param name="poTypes"></param>
        /// <returns></returns>
        public int GetPOListNum(string poNumber, string poLineNum, string poTypes) {
            int num = -1;
            getPOListParamBack = this.TempData[poTypes] as GetPOListParamBack;                        //获取储存的华为PO列表(对应types)
            this.TempData[poTypes] = getPOListParamBack;                                              //保存华为PO列表(对应types)
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
        /// <param name="poTypes"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult FindColTask(string poNumber, string poLineNum, string poTypes)
        {
            int num = GetPOListNum(poNumber, poLineNum, poTypes);
            if (num == -1)
            {
                return Error("获取信息失败。");
            }
            else
            {
                //获取华为access_token
                string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));
                JavaScriptSerializer js = new JavaScriptSerializer();
                getPOListParamBack = this.TempData[poTypes] as GetPOListParamBack;                        //获取储存的华为PO列表(对应types)
                this.TempData[poTypes] = getPOListParamBack;                                              //保存华为PO列表(对应types)
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
                getPODetails.selectType = "1";
                string json = js.Serialize(getPODetails);
                string result = HttpMethods.HttpPost(getPODetailsurl, json, true, accessToken);
                result = result.Replace(":null", ":''");
                GetPODetailsBack getPODetailsBack = js.Deserialize<GetPODetailsBack>(result);

                return Content(getPODetailsBack.result.ToJson());
            }
        }

        /// <summary>
        /// PO签返确认
        /// </summary>
        /// <param name="poNumber">订单号</param>
        /// <param name="poLineNum">行号</param>
        /// <param name="poTypes"></param>
        /// <param name="operateType">接受还是驳回</param>
        /// <returns></returns>
        [HandlerAjaxOnly]
        public ActionResult SignBackPOList(string poNumber, string poLineNum, string operateType, string poTypes)
        {
            ConfirmPO confirmPO = new ConfirmPO();
            confirmPO.operateType = operateType;
            string[] poNumberSplit = poNumber.Split('#');
            string[] poLineNumSplit = poLineNum.Split('#');
            getPOListParamBack = this.TempData[poTypes] as GetPOListParamBack;                        //获取储存的华为PO列表(对应types)
            this.TempData[poTypes] = getPOListParamBack;                                              //保存华为PO列表(对应types)

            for (int i = 0; i < poNumberSplit.Length; i++)
            {
                int num = GetPOListNum(poNumberSplit[i], poLineNumSplit[i], poTypes);                 //获取对应数组id

                ColTaskQueries colTaskQueries = new ColTaskQueries();
                colTaskQueries.poNum = getPOListParamBack.result[num].poNumber;
                colTaskQueries.poLineNum = getPOListParamBack.result[num].poLineNum;
                colTaskQueries.agentName = getPOListParamBack.result[num].agentName;
                //colTaskQueries.businessType = "all";
                if (poTypes == "huaweiPublishOrder")
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
        /// <param name="poNumber">订单号</param>
        /// <param name="poLineNum">行号</param>
        /// <param name="promiseDate">修改的日期，以#号隔开</param>
        /// <param name="poTypes">类型</param>
        /// <returns></returns>
        [HandlerAjaxOnly]
        public ActionResult OnwayPOList(string poNumber, string poLineNum, string promiseDate, string promiseDateChangeReason, string poTypes)
        {
            List<PoLinesAllVO> postOnwayPO = new List<PoLinesAllVO>();
            string[] poNumberSplit = poNumber.Split('#');
            string[] poLineNumSplit = poLineNum.Split('#');
            string[] dateSplit = promiseDate.Split('#');
            string[] reasonSplit = promiseDateChangeReason.Split('#');
            getPOListParamBack = this.TempData[poTypes] as GetPOListParamBack;                        //获取储存的华为PO列表(对应types)
            this.TempData[poTypes] = getPOListParamBack;                                              //保存华为PO列表(对应types)

            for (int i = 0; i < poNumberSplit.Length; i++)
            {
                int num = GetPOListNum(poNumberSplit[i], poLineNumSplit[i], poTypes);                 //获取对应数组id
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
        /// <param name="poTypes">类型</param>
        /// <returns></returns>
        [HttpPost]
        public void ExportPOPDF(string poNumber, string poLineNum, string poTypes)
        {
            GetPOListParamBack getPOListParamBack1 = new GetPOListParamBack();                          //定义返回数据
            getPOListParamBack = this.TempData[poTypes] as GetPOListParamBack;                          //获取储存的华为PO列表(对应types)
            this.TempData[poTypes] = getPOListParamBack;                                                //保存华为PO列表(对应types)

            for (int j = 0; j < getPOListParamBack.result.Count; j++)                                   //遍历getPOListParamBack列表
            {
                if (getPOListParamBack.result[j].poNumber == poNumber)
                {
                    getPOListParamBack1.result.Add(getPOListParamBack.result[j]);
                }
            }
            string filePath = "~/Resource/Huawei/HWPO/pdf/";                                                           //文件存放地址
            string fileName = iTextPDF.CreateHWPOPDF(getPOListParamBack1.ToJson(), filePath);        //生产pdf文件，下载

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
    }
}
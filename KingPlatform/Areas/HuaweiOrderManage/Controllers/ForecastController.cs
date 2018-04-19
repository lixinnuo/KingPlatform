using Basic.Code;
using King.Domain.Entity.HuaweiOrderManage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KingPlatform.Areas.HuaweiOrderManage.Controllers
{
    public class ForecastController : ControllerBase
    {
        string url_token = "https://api-beta.huawei.com:443/oauth2/token";                                          //查询华为access_token
        string findForecastList = "https://api-beta.huawei.com:443/service/esupplier/findForecastList/1.0.0/1";      //查询预测列表
        string updateForecast = "https://api-beta.huawei.com:443/service/esupplier/updateForecast/1.0.0";           //回复供应能力
        string key = "gsNG1jYljmgHpnKr1YCXhkJ_Hb8a";                                                                //系统键 测试平台
        string secury = "gvxnmDREbvfD2qN8sy4waYYNYXka";                                                             //系统值

        /*string url_token = "https://openapi.huawei.com:443/oauth2/token";                                          //查询华为access_token
        string findForecastList = "https://openapi.huawei.com:443/service/esupplier/findForecastList/1.0.0";      //查询预测列表
        //string updateForecast = "https://openapi.huawei.com:443/service/esupplier/updateForecast/1.0.0";           //回复供应能力
        string key = "sWJPMfiPlP6f8zM1zgoYzlGzl6Aa";                                                                //系统键 正式平台
        string secury = "FHV0PYqc3sU3deyfyoApff38hZ0a"; */                                                            //系统值


        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult ForecastList(string suppItemCode, string itemCode, int orgId, string startTime, string endTime, string purchaseMode, string buyerName)
        {
            //查询预测列表入参
            HWForecastParam param = new HWForecastParam(); 
            param.suppItemCode = suppItemCode;
            param.itemCode = itemCode;
            param.orgId = orgId;
            param.startTime = startTime;
            param.endTime = endTime;
            param.purchaseMode = purchaseMode;
            param.buyerName = buyerName;

            //获取华为access_token
            string accessToken = HttpMethods.GetAccessToken(url_token, key, secury);      
            JavaScriptSerializer js = new JavaScriptSerializer();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var json = JsonConvert.SerializeObject(param, Formatting.Indented, jSetting);
            //webservice查询数据
            string result = HttpMethods.HttpPost(findForecastList, json, true, accessToken);
            result = result.Replace(":null", ":''");

            
            HWForecastEntity forecastList = new HWForecastEntity();
            forecastList = js.Deserialize<HWForecastEntity>(result);

            if (forecastList.result == "No record was found!")
            {
                return Error("未查询到数据！"); ;
            }
            else
            {
                var data = new
                {
                    rows = forecastList.data.result,
                    total = Math.Ceiling(Convert.ToDouble(forecastList.data.pageVO.totalRows)),
                    page = forecastList.data.pageVO.curPage,
                    records = forecastList.data.pageVO.totalRows
                };
                return Content(data.ToJson());
            }
        }
        
    }
}
using Basic.Code;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using King.Domain.Entity.HuaweiOrderManage;
using King.Application.HuaweiOrderManage;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace KingPlatform.Areas.HuaweiOrderManage.Controllers
{
    public class StockManageController : ControllerBase
    {
        private HWStockApp hwStockApp = new HWStockApp();
        /*string url_token = "https://api-beta.huawei.com:443/oauth2/token";                                              //查询华为access_token
        string importInventoryurl = "https://api-beta.huawei.com:443/service/esupplier/importInventory/1.0.0/1";             //库存明细接口
        string key = "CkAb2QO3G50NQZcrm2VYPycgEMga";                                                                    //系统键 测试平台
        string secury = "UEvjRaxRoggXXM2G1Y5izAk1b_ga";*/                                                                //系统值

        string url_token = "https://openapi.huawei.com:443/oauth2/token";                                              //查询华为access_token
        string importInventoryurl = "https://openapi.huawei.com:443/service/esupplier/importInventory/1.0.0/1";                 //库存明细接口
        string key = "CoQUc1M90PLv3PpMldpvwOX1HKIa";                                                                   //系统键 正式平台
        string secury = "JqkgDMDzbDlaTA9EFpkRB9veArsa";                                                                  //系统值

        /// <summary>
        /// 前台显示列表
        /// </summary>
        /// <param name="history">是否为历史记录</param>
        /// <returns></returns>
        public ActionResult GetStockJson(bool history = false, int page = 1, int rows = 20)
        {
            HWStockEntity[] list;
            HWStockEntity[] newList;
            if (history)        //获取数据库中历史记录
            {
                list = hwStockApp.GetList().ToArray();
            }
            else
            {
                StockManageModel stockManageModel = new StockManageModel();
                stockManageModel = this.TempData["stockList"] as StockManageModel;
                this.TempData["stockList"] = stockManageModel;
                list = stockManageModel.factoryInventoryList.ToArray();
                
            }

            if (rows == -1)
            {
                newList = list;
            }
            else
            {
                if (list.Length > rows * page)
                {
                    newList = new HWStockEntity[rows];
                    Array.Copy(list, rows * (page - 1), newList, 0, rows);
                }
                else
                {
                    newList = new HWStockEntity[list.Length - rows * (page - 1)];
                    Array.Copy(list, rows * (page - 1), newList, 0, list.Length - rows * (page - 1));
                }
            }

            var data = new
            {
                rows = newList,
                total = Math.Ceiling(Convert.ToDouble(list.Length / (rows * 1.00))),
                page = page,
                records = list.Length
            };
            return Content(data.ToJson());
        }

        /// <summary>
        /// 获取导入数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetUpdataList(string filename)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var uploadDir = Server.MapPath("~/Resource/Huawei/HWStock/xlsx/");  //Upload 文件夹
            var filePath = Path.Combine(uploadDir, filename);                     //文件地址
            string fileType = Path.GetExtension(filePath);

            try
            {
                //连接字符串
                string connstring = string.Format("Provider=Microsoft.ACE.OLEDB.{0}.0;" +
                                "Extended Properties=\"Excel {1}.0;HDR=YES;IMEX=1;\";" +
                                "data source={2};",
                                (fileType == ".xls" ? 4 : 12), (fileType == ".xls" ? 8 : 12), filePath);
                using (OleDbConnection conn = new OleDbConnection(connstring))
                {
                    conn.Open();
                    DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); //得到所有sheet的名字
                    string firstSheetName = sheetsName.Rows[0][2].ToString(); //得到第一个sheet的名字
                    string sql = string.Format("SELECT * FROM [{0}]", firstSheetName); //查询字符串
                    //string sql = string.Format("SELECT * FROM [{0}] WHERE [工厂代码] is not null", firstSheetName); //查询字符串

                    OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);
                    DataSet set = new DataSet();
                    ada.Fill(set);

                    string dataJson = Basic.Code.Json.SetToJson(set);

                    dataJson = dataJson.Replace("Table", "factoryInventoryList").Replace("我司工厂代码", "vendorFactoryCode").Replace("我司物料编码版本", "vendorItemRevision").Replace("华为代码", "customerCode").Replace("我司物料子库", "vendorStock").Replace("我司物料货位", "vendorLocation").Replace("入库时间", "stockTime").Replace("我司物料编码", "vendorItemCode").Replace("是否协议备货", "agreementStocking").Replace("库存(pcs)", "goodQuantity").Replace("待检库存", "inspectQty").Replace("隔离品数量", "faultQty");
                    StockManageModel stockManageModel = new StockManageModel();
                    stockManageModel = js.Deserialize<StockManageModel>(dataJson);
                    this.TempData["stockList"] = stockManageModel;
                    return "1";
                }
            }
            catch (Exception e)
            {
                this.TempData["stockList"] = "";
                return e.ToString();
            }
        }

        /// <summary>
        /// 获取华为返回信息
        /// </summary>
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult HuaweiBack(string list)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            StockManageModel stockManageModel = new StockManageModel();
            HWStockEntity[] stockDetail = JsonConvert.DeserializeObject<HWStockEntity[]>(list);
            for (int i = 0; i < stockDetail.Length; i++)
            {
                stockManageModel.factoryInventoryList.Add(stockDetail[i]);
            }

            for (int i = 0; i < stockManageModel.factoryInventoryList.Count; i++)
            {
                //处理入库时间字段
                if (stockManageModel.factoryInventoryList[i].stockTime.Length > 10)
                {
                    stockManageModel.factoryInventoryList[i].stockTime = stockManageModel.factoryInventoryList[i].stockTime.Substring(0, 10);
                }
                //处理是否协议备货字段
                stockManageModel.factoryInventoryList[i].agreementStocking = stockManageModel.factoryInventoryList[i].agreementStocking.ToUpper();
                //处理库存字段
                stockManageModel.factoryInventoryList[i].goodQuantity = Math.Floor(stockManageModel.factoryInventoryList[i].goodQuantity);
                //处理待检库存字段
                if (stockManageModel.factoryInventoryList[i].inspectQty != null)
                {
                    stockManageModel.factoryInventoryList[i].inspectQty = (long)stockManageModel.factoryInventoryList[i].inspectQty;
                }
                //处理隔离品字段
                if (stockManageModel.factoryInventoryList[i].faultQty != null)
                {
                    stockManageModel.factoryInventoryList[i].faultQty = (long)stockManageModel.factoryInventoryList[i].faultQty;
                }
            }

            string accessToken = HttpMethods.GetAccessToken(url_token, key, secury);      //获取华为access_token
           
            var json = JsonConvert.SerializeObject(stockManageModel, Formatting.Indented, jSetting);
            string result = HttpMethods.HttpPost(importInventoryurl, json, true, accessToken);

            if (result == null || result == "")
            {
                this.TempData["stockList"] = "";
                return Error("提交失败");
            }

            try
            {
                StockOut stockOut = new StockOut();
                stockOut = js.Deserialize<StockOut>(result);

                for (int i = 0; i < stockManageModel.factoryInventoryList.Count; i++)
                {
                    if (stockOut.success)
                    {
                        stockManageModel.factoryInventoryList[i].success = stockOut.success;
                        stockManageModel.factoryInventoryList[i].errorMessage = "";
                    }
                    else
                    {
                        string errorMessage = stockOut.errorMessage;
                        if (errorMessage != null)
                        {
                            string[] err = errorMessage.Split(';');
                            for (int j = 0; j < err.Length - 1; j++)
                            {
                                Regex reg = new Regex(@"(?<=\{)[^\{\}]+(?=\})");
                                string str = reg.Match(err[j]).Value;
                                if ((i + 1) == Convert.ToInt16(str))
                                {
                                    stockManageModel.factoryInventoryList[i].success = false;
                                    stockManageModel.factoryInventoryList[i].errorMessage = err[j].Substring(err[j].IndexOf('>') + 1);
                                    break;
                                }
                                else
                                {
                                    stockManageModel.factoryInventoryList[i].success = true;
                                    stockManageModel.factoryInventoryList[i].errorMessage = "";
                                }
                            }
                        }
                        else
                        {
                            this.TempData["vendorItemList"] = "";
                            return Error("提交失败  " + result);
                        }
                    }
                    //提交到数据库
                    hwStockApp.SubmitForm(stockManageModel.factoryInventoryList[i], "");
                }
                this.TempData["stockList"] = stockManageModel;
                if (stockOut.success)
                {
                    return Success("提交成功");
                }
                else {
                    return Error("提交失败 " + stockOut.errorMessage);
                }
                
            }
            catch (Exception e)
            {
                this.TempData["stockList"] = "";
                return Error("提交失败  " + e);
            }
        }

        #region 文件上传
        [HttpPost]
        public ActionResult FileUpload()
        {
            string fileName = Request["name"];
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));//设置临时存放文件夹名称
            int index = Convert.ToInt32(Request["chunk"]);//当前分块序号
            var guid = Request["guid"];//前端传来的GUID号
            var dir = Server.MapPath("~/Resource/Huawei/HWStock/xlsx/");//文件上传目录
            dir = Path.Combine(dir, fileRelName);//临时保存分块的目录
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            //if (data != null)//为null可能是暂停的那一瞬间
            //{
            data.SaveAs(filePath);//报错
            //}
            return Json(new { erron = 0 });//Demo，随便返回了个值，请勿参考
        }
        public string Merge()
        {
            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("~/Resource/Huawei/HWStock/xlsx/");//Upload 文件夹
            var fileName = Request["fileName"];//文件名
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));
            var dir = Path.Combine(uploadDir, fileRelName);//临时文件夹          
            var files = Directory.GetFiles(dir);//获得下面的所有文件
            var length1 = fileName.LastIndexOf('.');
            var finalName = fileRelName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileName.Substring(length1, (fileName.Length - length1));
            var finalPath = Path.Combine(uploadDir, finalName);//最终的文件名（文件名+当前时间）
            var fs = new FileStream(finalPath, FileMode.Create);
            foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
            {
                var bytes = System.IO.File.ReadAllBytes(part);
                fs.Write(bytes, 0, bytes.Length);
                bytes = null;
                System.IO.File.Delete(part);//删除分块
            }
            fs.Flush();
            fs.Close();
            Directory.Delete(dir);//删除文件夹
            return GetUpdataList(finalName);           //调用GetUpdataList获取上传信息和华为处理信息
        }
        #endregion

        /// <summary>
        /// 下载模板
        /// </summary>
        [HttpPost]
        public void DownloadTemplate()
        {
            string filename = Server.UrlDecode("InventoryTemplate.xlsx");
            string filepath = Server.MapPath("~/Resource/Huawei/HWStock/xlsx/InventoryTemplate.xlsx");
            if (FileDownHelper.FileExists(filepath))
            {
                FileDownHelper.DownLoadold(filepath, filename);
            }
        }
    }
}
 
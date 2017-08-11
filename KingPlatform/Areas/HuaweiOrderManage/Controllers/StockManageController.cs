using Basic.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KingPlatform.Areas.HuaweiOrderManage.Controllers
{
    public class StockManageController : Controller
    {
        string url_token = "https://api-beta.huawei.com:443/oauth2/token";                                              //查询华为access_token
        string findPOListurl = "https://api-beta.huawei.com:443/service/esupplier/importInventory/1.0.0/1";             //库存明细接口
        string key = "CkAb2QO3G50NQZcrm2VYPycgEMga";                                                                    //系统键 测试平台
        string secury = "UEvjRaxRoggXXM2G1Y5izAk1b_ga";                                                                 //系统值

        /*string url_token = "https://openapi.huawei.com:443/oauth2/token";                                              //查询华为access_token
        string findPOListurl = "https://api.huawei.com:443/service/esupplier/importInventory/1.0.0/1";                 //库存明细接口
        string key = "CoQUc1M90PLv3PpMldpvwOX1HKIa";                                                                   //系统键 正式平台
        string secury = "JqkgDMDzbDlaTA9EFpkRB9veArsa"; */                                                             //系统值

        // GET: HuaweiOrderManage/StockManage
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取POList  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public void StockList(string poSubType = "")
        {
            string findPOListurlTrue = "";
            string accessToken = HttpMethods.GetAccessToken(HttpMethods.HttpPost(url_token, key, secury));      //获取华为access_token
            JavaScriptSerializer js = new JavaScriptSerializer();
            //定义接口json数据 获取新POList传入参数
                


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
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, index.ToString());//分块文件名为索引名，更严谨一些可以加上是否存在的判断，防止多线程时并发冲突
            var data = Request.Files["file"];//表单中取得分块文件
            //if (data != null)//为null可能是暂停的那一瞬间
            //{
            data.SaveAs(filePath);//报错
            //}
            return Json(new { erron = 0 });//Demo，随便返回了个值，请勿参考
        }
        public ActionResult Merge()
        {
            var guid = Request["guid"];//GUID
            var uploadDir = Server.MapPath("~/Resource/Huawei/HWStock/xlsx/");//Upload 文件夹
            var fileName = Request["fileName"];//文件名
            string fileRelName = fileName.Substring(0, fileName.LastIndexOf('.'));
            var dir = Path.Combine(uploadDir, fileRelName);//临时文件夹          
            var files = System.IO.Directory.GetFiles(dir);//获得下面的所有文件
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
            System.IO.Directory.Delete(dir);//删除文件夹
            return Json(new { finalName = finalName });//返回新的文件名
        }
        #endregion
    }
}
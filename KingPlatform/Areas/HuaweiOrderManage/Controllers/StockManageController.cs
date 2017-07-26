using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KingPlatform.Areas.HuaweiOrderManage.Controllers
{
    public class StockManageController : Controller
    {

        string findPOListurl = "https://api-beta.huawei.com:443/service/esupplier/importInventory/1.0.0/1";             //库存明细接口
        string key = "CkAb2QO3G50NQZcrm2VYPycgEMga";                                                                    //系统键 测试平台
        string secury = "UEvjRaxRoggXXM2G1Y5izAk1b_ga";                                                                 //系统值

        /*string findPOListurl = "https://api.huawei.com:443/service/esupplier/importInventory/1.0.0/1";               //库存明细接口
        string key = "CoQUc1M90PLv3PpMldpvwOX1HKIa";                                                                   //系统键 正式平台
        string secury = "JqkgDMDzbDlaTA9EFpkRB9veArsa"; */                                                             //系统值

        // GET: HuaweiOrderManage/StockManage
        public ActionResult Index()
        {
            return View();
        }
    }
}
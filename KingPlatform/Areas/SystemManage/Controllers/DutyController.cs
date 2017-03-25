using Basic.Code;
using King.Application.SystemManage;
using King.Domain.Entity.SystemManage;
using System.Web.Mvc;

namespace KingPlatform.Areas.SystemManage.Controllers
{
    public class DutyController : ControllerBase
    {
        private DutyApp dutyApp = new DutyApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = dutyApp.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = dutyApp.GetForm(keyValue);
            UserApp userApp = new UserApp();
            if (data.F_CreateUserId != null)
            {
                data.F_CreateUserId = userApp.GetForm(data.F_CreateUserId).F_Account;
            }
            if (data.F_LastModifyUserId != null)
            {
                data.F_LastModifyUserId = userApp.GetForm(data.F_LastModifyUserId).F_Account;
            }
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(RoleEntity roleEntity, string keyValue)
        {
            dutyApp.SubmitForm(roleEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            dutyApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}
using System.Web.Mvc;

namespace KingPlatform.Areas.SystemSecurity
{
    public class SystemSecurityAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SystemSecurity";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                this.AreaName + "_default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "KingPlatform.Areas." + this.AreaName + ".Controllers"}
            );
        }
    }
}
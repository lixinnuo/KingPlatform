using System;
using System.Collections.Generic;

namespace King.Domain.Entity.HuaweiOrderManage
{
    /// <summary>
    /// 物料基础信息    非空，每次最大5000
    /// </summary>
    public class HWVendorItemEntity : IEntity<HWVendorItemEntity>, ICreationAudited
    {
        public string F_Id { get; set; }
        public string vendorItemCode { get; set; }                   //我司物料编码   Y
        public string vendorProductModel { get; set; }                      //我司产品型号   N
        public string vendorItemDesc { get; set; }                        //我司物料描述  Y
        public string itemCategory { get; set; }                         //物料小类 Y
        public string customerVendorCode { get; set; }                      //客户代码 Y  157，对应客户为'华技'；0971，对应客户为'终端'；其它华为SRM系统真实存在的供应商编码
        public string customerItemCode { get; set; }                           //客户物流编码  N      
        public string customerProductModel { get; set; }                  //客户产品型号 N
        public string unitOfMeasure { get; set; }                   //单位 Y
        public string inventoryType { get; set; }                 //供应商Item类别    Y   FG(成品)、SEMI-FG(半成品)、RM(原材料)
        public double? goodPercent { get; set; }                         //良率  N       可以空，若为空，则系统默认设置为100；限制为0-100(包含)的数字，小数位不能超过2位
        public double leadTime { get; set; }                           //货期(天） Y       非空，小数位不能超过1位
        public string lifeCycleStatus { get; set; }                        //生命周期状态   NPI(量产前)、MP(量产)、EOL(停产)
        public bool? success { get; set; }                              //是否成功
        public string errorMessage { get; set; }                        //错误信息
        public string F_CreateUserId { get; set; }
        public DateTime? F_CreateTime { get; set; }
    }

    /// <summary>
    /// 物料基础信息 物料明细入参 
    /// </summary>
    public class VendorItemModel
    {
        public List<HWVendorItemEntity> vendorItemList { get; set; }
        public VendorItemModel()
        {
            vendorItemList = new List<HWVendorItemEntity>();
        }
    }

    /// <summary>
    /// 物料基础信息出参
    /// </summary>
    public class VendorItemOut
    {
        public bool success { get; set; }
        public string errorMessage { get; set; }
        public int errorCode { get; set; }
    }
}

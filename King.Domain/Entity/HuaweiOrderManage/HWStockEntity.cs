using System;
using System.Collections.Generic;

namespace King.Domain.Entity.HuaweiOrderManage
{
    /// <summary>
    /// 库存信息    非空，每次最大5000
    /// </summary>
    public class HWStockEntity : IEntity<HWStockEntity>, ICreationAudited
    {
        public string F_Id { get; set; }
        public string vendorFactoryCode { get; set; }                   //工厂代码   Y
        public string vendorItemCode { get; set; }                      //供应商物料编码   Y
        public string customerCode { get; set; }                        //客户代码  Y
        public string vendorStock { get; set; }                         //供应商子库 Y
        public string vendorLocation { get; set; }                      //供应商货位 Y
        public string stockTime { get; set; }                           //入库时间  Y       yyyy-MM-dd格式,如2017-05-16
        public string vendorItemRevision { get; set; }                  //供应商物料编码版本 N
        public double goodQuantity { get; set; }                        //库存    Y       非空, 正整数
        public double? inspectQty { get; set; }                         //待检库存  N       可为空, 若传入, 则必须为正整数
        public double? faultQty { get; set; }                           //隔离品数量 N       可为空, 若传入, 则必须为正整数
        public bool? success { get; set; }                              //是否成功
        public string errorMessage { get; set; }                        //错误信息
        public string F_CreateUserId { get; set; }
        public DateTime? F_CreateTime { get; set; }
    }

    /// <summary>
    /// 备货管理 库存明细入参 
    /// </summary>
    public class StockManageModel
    {
        public List<HWStockEntity> factoryInventoryList { get; set; }
        public StockManageModel()
        {
            factoryInventoryList = new List<HWStockEntity>();
        }
    }

    /// <summary>
    /// 备货管理出参
    /// </summary>
    public class StockOut
    {
        public bool success { get; set; }
        public string errorMessage { get; set; }
        public int errorCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KingPlatform.Areas.HuaweiOrderManage.Models
{
    #region 备货管理入参
    /// <summary>
    /// 备货管理 库存明细入参 
    /// </summary>
    public class StockManageModel
    {
        public List<StockDetails> factoryInventoryList { get; set; }    
        public StockManageModel()
        {
            factoryInventoryList = new List<StockDetails>();
        }
    }

    /// <summary>
    /// 库存信息    非空，每次最大5000
    /// </summary>
    public class StockDetails
    {
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
    }
    #endregion



}
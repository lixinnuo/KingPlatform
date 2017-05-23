using System;

namespace Basic.Code
{
    public class PODatails
    {
        public string vendorCode { get; set; }              //供应商编码
        public string vendorName { get; set; }              //供应商名称
        public string poNumber { get; set; }                //PO号
        public string poLineNum { get; set; }               //PO行号
        public string itemCode { get; set; }                //Item编码
        public string itemDescription { get; set; }         //Item描述
        public int? quantity { get; set; }                  //需求数量
        public int? quantityReceived { get; set; }          //已收货数量
        public string unitOfMeasure { get; set; }           //单位
        public float? priceOverride { get; set; }           //单价
        public string currencyCode { get; set; }            //币种
        public float? taxRate { get; set; }                 //税率
        public string publishDate { get; set; }             //订单下发日期
        public string shipToLocation { get; set; }          //收货地点
        public string termsMode { get; set; }               //付款方式
        public string issuOffice { get; set; }              //开票单位
        public string orgName { get; set; }                 //华为子公司
        public string itemRevision { get; set; }            //物料/服务编码版本
        public string promiseDate { get; set; }             //承诺日期
        public string expireDate { get; set; }              //订单有效日期
        public string dueQty { get; set; }                  //未交付数量
        public string remark { get; set; }                  //备注
        public string prNumber { get; set; }                //PR号
        public string billToLocation { get; set; }          //开票地址
        public string taskNum { get; set; }                 //任务令
        public string agentName { get; set; }               //采购员
        public string agentEmployeeNumber { get; set; }     //采购员
        public string carrierName { get; set; }             //发运方式
        public string shipmentNum { get; set; }             //发运行号
        public string unSendQuantity { get; set; }          //未交付数量
        public string paymentTerms { get; set; }            //支付条款
        public string revisionNum { get; set; }             //版本号
        public string shipmentType { get; set; }            //PO Type
        public string sendVendorTelNum { get; set; }        //Phone
        public string sendVendorFax { get; set; }           //Fax
        public string businessMode { get; set; }            //businessMode
        public int instanceId { get; set; }              //华为系统内部标识用
        public long poHeaderId { get; set; }              //华为系统内部标识用
        public long poReleaseId { get; set; }             //华为系统内部标识用
        public long lineLocationId { get; set; }          //华为系统内部标识用
    }
}

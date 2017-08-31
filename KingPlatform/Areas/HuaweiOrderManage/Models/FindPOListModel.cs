using System;
using System.Collections.Generic;

namespace KingPlatform.Areas.HuaweiOrderManage.Models
{
    #region GetPOBoardParam 查询PO看板入参
    public class GetPOBoardParam
    {
        public string orgId { get; set; }               //华为子公司
        public string poNumber { get; set; }         //PO号
        public string itemCode { get; set; }     //Item编码
        public string promiseDateStart { get; set; }                //承诺日期（日期区间起始值）
        public string promiseDateEnd { get; set; }  //承诺日期（日期区间截止值）
        public string publishDateStart { get; set; }             //订单下达日期（日期区间起始值
        public string publishDateEnd { get; set; }             //订单下达日期（日期区间截止值
        public string partNumber { get; set; }               //厂家型号
        public string businessMode { get; set; }  //采购模式
        public string shipmentStatus { get; set; }                   //订单状态
        public string agentName { get; set; }   //采购员
        public string businessType { get; set; }           //待办类型
        public int? moreXDaysUndeal { get; set; }                //超过X天未处理
        public string manufactureSiteInfo { get; set; }               //生产厂家
    }
    #endregion

    #region GetPOBoard 查询PO看板出参
    public class GetPOBoard
    {
        public int expiredOrderQuantity { get; set; }               //新订单超三天未处理
        public int arrivalOnWeekOrderQuantity { get; set; }         //预计两周内到货
        public int vendorApplyCancelOrderQuantity { get; set; }     //待华为确认取消
        public int closedOrderQuantity { get; set; }                //订单已关闭
        public int vendorApplyQtyChangeOrderQuantity { get; set; }  //待华为确认数量变更
        public int intransitOrderQuantity { get; set; }             //在途订单
        public int cancelledOrderQuantity { get; set; }             //订单已取消
        public int changedOrderQuantity { get; set; }               //内容变更通知
        public int cancelledToConfirmedOrderQuantity { get; set; }  //订单取消确认
        public int newOrderQuantity { get; set; }                   //新订单
        public int deliveryTimeChangedOrderQuantity { get; set; }   //订单交期更改
        public int nonDeliveryOrderQuantity { get; set; }           //下单六个月未交货
        public int cancelOrderQuantity { get; set; }                //订单取消通知
        public int overdueOrderQuantity { get; set; }               //过期PO
        public int rmaBarterOrderQuantity { get; set; }             //RMA换货订单
        public int deliveryTimeChangedPendingOrderQuantity { get; set; }        //待华为确认交期更改 
        public int closed4Receving { get; set; }                    //已交货未关闭
    }
    #endregion

    #region GetPOListParam 查询PO列表的入参
    /// <summary>
    /// 定义接口json数据
    /// </summary>
    public class GetPOListParam
    {
        public GetPOListParam()
        { }
        public GetPOListParam(string poSubType, string shipmentStatus, string businessType, string poStatus, string colTaskOrPoStatus, string statusType)
        {
            this.poSubType = poSubType;
            this.shipmentStatus = shipmentStatus;
            this.businessType = businessType;
            this.poStatus = poStatus;
            this.colTaskOrPoStatus = colTaskOrPoStatus;
            this.statusType = statusType;
        }

        public string poSubType { get; set; }               //PO业务领域
        public string shipmentStatus { get; set; }
        public string businessType { get; set; }
        public string poStatus { get; set; }
        public string colTaskOrPoStatus { get; set; }       //任务单状态
        public string statusType { get; set; }              //状态类别
        public string itemCode { get; set; }              //Item编码
        public int? orgId { get; set; }                      //华为子公司ID
        public string poNumber { get; set; }              //PO号
        public string promiseDateStart { get; set; }              //承诺开始日期
        public string promiseDateEnd { get; set; }              //承诺结束日期
        public string publishDateStart { get; set; }              //发布开始日期
        public string publishDateEnd { get; set; }              //发布结束日期
        public string businessMode { get; set; }              //采购模式
    }
    #endregion

    #region GetKeyPOListParam 获取特定po列表信息
    public class GetKeyPOListParam
    {
        public int instanceId { get; set; }              //华为系统内部标识用
        public long poHeaderId { get; set; }              //华为系统内部标识用
        public long poReleaseId { get; set; }             //华为系统内部标识用
        public bool calculateOrderAmount { get; set; }
    }
    #endregion

    #region GetPOListParamBack 查询PO列表的出参
    /// <summary>
    /// POList
    /// </summary>
    public class GetPOListParamBack
    {
        public List<POListResult> result { get; set; }
        public GetPOListParamBack()
        {
            result = new List<POListResult>();
        }
        public PageVO pageVO { get; set; }
    }

    /// <summary>
    /// POList具体数据
    /// </summary>
    public class POListResult
    {
        public string vendorCode { get; set; }              //供应商编码
        public string vendorName { get; set; }              //供应商名称
        public string poNumber { get; set; }                //PO号
        public string objectChangeContext { get; set; }      //修改内容
        public DateTime? pllaAttribute2 { get; set; }          //修改后的承诺日期
        public string poLineNum { get; set; }               //PO行号
        public string shipmentStatus { get; set; }          //订单状态
        public string itemCode { get; set; }                //Item编码
        public string itemDescription { get; set; }         //Item描述
        public int? quantity { get; set; }                  //需求数量
        public int? quantityReceived { get; set; }          //已收货数量
        public int? quantityCancelled { get; set; }          //取消数量
        public int? precision { get; set; }                 //保留小数位数
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
        public string sendConnecter { get; set; }           //公司领导
        public string lastUpdateDate { get; set; }       //最后更新时间
        public string sendVendorAddr { get; set; }          //ship to address
        public string needByDate { get; set; }              //需求时间
        public string fobLookupCode { get; set; }           //打印中的Term
        public string partNumber { get; set; }              //打印中的description
        public int taskQuantity { get; set; }            //总任务单
        public int openTaskQuantity { get; set; }        //待签返任务单
        public int instanceId { get; set; }              //华为系统内部标识用
        public long poHeaderId { get; set; }              //华为系统内部标识用
        public long poReleaseId { get; set; }             //华为系统内部标识用
        public long lineLocationId { get; set; }          //华为系统内部标识用
    }

    /// <summary>
    /// POList页码数据
    /// </summary>
    public class PageVO
    {
        public int startIndex { get; set; }
        public int curPage { get; set; }
        public int mysqlStartIndex { get; set; }
        public int endIndex { get; set; }
        public int resultMode { get; set; }
        public int totalPages { get; set; }
        public string tempProp { get; set; }
        public string orderBy { get; set; }
        public int pageSize { get; set; }
        public int totalRows { get; set; }
        public int mysqlEndIndex { get; set; }
        public string filterStr { get; set; }
    }
    #endregion

    #region ConfirmPO PO签返确认的入参
    public class ConfirmPO
    {
        public string operateType { get; set; }                     //接受还是驳回 "accept"是接受；"reject"是驳回
        public List<ColTaskQueries> colTaskQueries { get; set; }
        public ConfirmPO()
        {
            colTaskQueries = new List<ColTaskQueries>();
        }
    }

    public class ColTaskQueries
    {
        public string poNum { get; set; }                       //PO号
        public string poLineNum { get; set; }                   //PO行号
        public string agentName { get; set; }                   //采购员名称
        public string businessType { get; set; }                //PO待办类型 固定值：all 8.31修改为对应类型
        public int instanceId { get; set; }                  //系统帐套标识
        public long poHeaderId { get; set; }                  //PO头ID
        public long poReleaseId { get; set; }                 //PO释放ID
        public long lineLocationId { get; set; }              //PO发运行ID
    }
    #endregion

    #region ConfirmPOBack PO签返确认的出参
    /// <summary>
    /// 新PO签返确认参数
    /// </summary>
    public class ConfirmPOBack
    {
        public string result { get; set; }
        public string code { get; set; }
        public bool success { get; set; }
        public bool failed { get; set; }
        public List<POBackData> data { get; set; }
        public ConfirmPOBack()
        {
            data = new List<POBackData>();
        }
    }

    public class POBackData
    {
        public string poNum { get; set; }               //PO号
        public string poLineNum { get; set; }           //PO行号
        public string code { get; set; }                //返回码 “00000”是成功，其他都是失败
        public string msg { get; set; }                 //返回信息描述
        public string taskIndex { get; set; }           //
        public bool success { get; set; }
        public string data { get; set; }
    }
    #endregion

    #region PoLinesAllVO 供应商变更PO入参
    public class PoLinesAllVO
    {
        public int instanceId { get; set; }                     //帐套标识
        public string poNumber { get; set; }                    //PO号
        public long lineLocationId { get; set; }                //PO行ID
        public string promiseDateChangeReason { get; set; }     //修改原因
        public string typeLookupCode { get; set; }              //修改类型  “2”是修改交期 “1”是修改数量 “3”是既修改交期也修改数量
        public string promiseDateStr { get; set; }              //新的交期时间
        public int? needQuantity { get; set; }               //新的修改数量
    }
    #endregion

    #region SupplierChangePOOut 供应商变更PO出参
    public class SupplierChangePOOut
    {
        public string msg { get; set; }
        public string code { get; set; }
        public bool success { get; set; }
        public string poNum { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public long lineLocationId { get; set; }
        public int instanceId { get; set; }
        public string taskId { get; set; }
        public int? rowId { get; set; }
    }
    #endregion

    #region GetPODetails 查询PO行变更明细入参
    /// <summary>
    /// 查询PO行变更明细参数
    /// </summary>
    public class GetPODetails
    {
        public string poNumber { get; set; }                    //PO号
        public string poLineNum { get; set; }                   //PO行号
        public string agentName { get; set; }                   //采购员名称
        public string agentEmployeeNumber { get; set; }         //采购员工号
        public long lineLocationId { get; set; }                //PO发运行ID
        public int instanceId { get; set; }                     //帐套标识
        public string shipmentNum { get; set; }                   //发运行号
        public long poReleaseId { get; set; }                   //PO释放ID
        public long poHeaderId { get; set; }                    //PO头ID
        public string selectType { get; set; }                  //选择类型  固定值：0
    }
    #endregion

    #region GetPODetailsBack 查询PO行变更明细出参
    /// <summary>
    /// 查询PO行变更明细参数
    /// </summary>
    public class GetPODetailsBack
    {
        public List<PODetailsResult> result { get; set; }
        public GetPODetailsBack()
        {
            result = new List<PODetailsResult>();
        }
        public PageVO pageVO { get; set; }
        public POConfig config { get; set; }
    }
    public class POConfig
    {
        public string config { get; set; }
    }
    public class PODetailsResult
    {
        public string poNumber { get; set; }                //PO号
        public string poLineNum { get; set; }               //PO行号
        public string changeColumnName { get; set; }        //变更字段
        public string changePreContent { get; set; }        //变更前内容
        public string changeAfterContent { get; set; }      //变更后内容
        public string openRemark { get; set; }              //变更理由
        public string lineLocationId { get; set; }            //PO发运行ID
        public long poHeaderId { get; set; }                //PO头ID
        public string businessType { get; set; }            //任务单类型
        public string currentHandler { get; set; }          //当前处理人
        public string lastHandler { get; set; }             //最后处理人
        public DateTime? creationDate { get; set; }         //任务发布日期
        public DateTime? lastUpdateDate { get; set; }       //任务结束日期
        public string status { get; set; }                  //任务单状态
        public string closeRemark { get; set; }             //审批意见
    }
    #endregion

    #region GetHWCor 查询华为子公司列表入参
    public class GetHWCor
    {
        public GetHWCor()
        { }
        public GetHWCor(string lang)
        {
            this.lang = lang;
        }
        public string lang { get; set; }
    }
    #endregion

    #region GetHWCorBack 查询华为子公司列表出参
    public class GetHWCorBack
    {
        public List<HWCorBack> result { get; set; }
        public GetHWCorBack()
        {
            result = new List<HWCorBack>();
        }
        public string pageVO { get; set; }
    }

    public class HWCorBack
    {
        public long organizationId { get; set; }        //子公司Id
        public string companyCode { get; set; }         //子公司编号
        public string i18 { get; set; }                 //i18未知
        public string instanceId { get; set; }          //未知
        public string orgName { get; set; }             //子公司名字
        public string orgCode { get; set; }             //未知
        public string name { get; set; }                //未知
        public string orgNameZh { get; set; }           //子公司中文名字
        public string orgId { get; set; }               //子公司id
        public string orgNameEn { get; set; }           //子公司英文名字
    }
    #endregion


}
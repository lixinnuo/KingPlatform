using System.Collections.Generic;

namespace King.Domain.Entity.HuaweiOrderManage
{
    #region 查询采购预测能力入参
    public class HWForecastParam
    {
        public string suppItemCode { get; set; }            //N  供方物料编码  以逗号分隔，不超过30个编码
        public string itemCode { get; set; }                //N  华为方物料编码  以逗号分隔，不超过30个编码
        public int orgId { get; set; }                   //Y  组织名称，固定值  华技固定218
        public string startTime { get; set; }               //N  开始时间   格式 yy-mm-dd
        public string endTime { get; set; }                 //N  结束时间   格式 yy-mm-dd
        public string purchaseMode { get; set; }            //N  采购模式   不填则为查询全部采购模式下数据
        public string buyerName { get; set; }               //N  采购员
    }
    #endregion

    #region  查询采购预测能力出参
    public class HWForecastEntity 
    {
        public string code { get; set; }                    //状态码
        public bool success { get; set; }                   //是否成功标志
        public string result { get; set; }                  //错误描述
        public ForecastData data { get; set; }                      //返回的数据结果
    }

    public class ForecastData
    {
        public pageVO pageVO { get; set; }                  //分页参数
        public List<ForecastResult> result { get; set; }            //结果集
        public ForecastData()
        {
            result = new List<ForecastResult>();
        }
    }

    public class pageVO
    {
        public int totalRows { get; set; }                  //总条数
        public int totalPages { get; set; }                 //总页数
        public int pageSize { get; set; }                   //一页的数据条数
        public int curPage { get; set; }                    //当前第几页
    }

    public class ForecastResult
    {
        public int lineNo { get; set; }                     //行号
        public string orgId { get; set; }                   //组织编码
        public string purchaseMode { get; set; }            //采购模式
        public string publishDate { get; set; }             //预测发布日期
        public string aslId { get; set; }                   //ASL关系ID
        public string openPOQty { get; set; }               //在途订单数量
        public string currentInventory { get; set; }        //供应商当前库存
        public string vmiQty { get; set; }                  //VMI库存
        public string dataMeasure { get; set; }             //数据类型
        public string itemCode { get; set; }                //华为物料编码
        public string suppItemCode { get; set; }            //供方物料编码
        public string buyerName { get; set; }               //采购员名称
        public string version { get; set; }                 //物料版本号
        public string partSort { get; set; }                //器件分类
        public List<string> lableList { get; set; }         //返回的数据日期列表
        public ResultData data { get; set; }                //预测、回复、gap数据，该list为有序集合，每3个为一组，分别为每一个item的预测数据、回复数据和gap数据
    }

    public class ResultData
    {
        public int Q1 { get; set; }                         //第1周数量
        public int Q2 { get; set; }                         //第2周数量
        public int Q3 { get; set; }                         //第3周数量
        public int Q4 { get; set; }
        public int Q5 { get; set; }
        public int Q6 { get; set; }
        public int Q7 { get; set; }
        public int Q8 { get; set; }
        public int Q9 { get; set; }
        public int Q10 { get; set; }
        public int Q11 { get; set; }
        public int Q12 { get; set; }
        public int Q13 { get; set; }
        public int Q14 { get; set; }
        public int Q15 { get; set; }
        public int Q16 { get; set; }
        public int Q17 { get; set; }
        public int Q18 { get; set; }
        public int Q19 { get; set; }
        public int Q20 { get; set; }
        public int Q21 { get; set; }
        public int Q22 { get; set; }
        public int Q23 { get; set; }
        public int Q24 { get; set; }
        public int Q25 { get; set; }
        public int Q26 { get; set; }
        public int Q27 { get; set; }
        public int Q28 { get; set; }
        public int Q29 { get; set; }
        public int Q30 { get; set; }
        public int Q31 { get; set; }
        public int Q32 { get; set; }
        public int Q33 { get; set; }
        public int Q34 { get; set; }
        public int Q35 { get; set; }
        public int Q36 { get; set; }
        public int Q37 { get; set; }
        public int Q38 { get; set; }
        public int Q39 { get; set; }
        public int Q40 { get; set; }
        public int Q41 { get; set; }
        public int Q42 { get; set; }
        public int Q43 { get; set; }
        public int Q44 { get; set; }
        public int Q45 { get; set; }
        public int Q46 { get; set; }
        public int Q47 { get; set; }
        public int Q48 { get; set; }
        public int Q49 { get; set; }
        public int Q50 { get; set; }
        public int Q51 { get; set; }
        public int Q52 { get; set; }                        //第52周数量
        public int total { get; set; }                      //数量总和
    }
    #endregion
}

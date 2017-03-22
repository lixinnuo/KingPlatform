using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Basic.Code
{
    /// <summary>
    /// 常用公共类
    /// </summary>
    public class Common
    {
        #region Stopwatch 计时器
        /// <summary>
        /// 计时器开始
        /// </summary>
        /// <returns></returns>
        public static Stopwatch TimerStart()
        {
            Stopwatch watch = new Stopwatch();
            watch.Reset();
            watch.Start();
            return watch;
        }

        /// <summary>
        /// 计时器结束
        /// </summary>
        /// <param name="watch"></param>
        /// <returns></returns>
        public static string TimerEnd(Stopwatch watch)
        {
            watch.Stop();
            double costTime = watch.ElapsedMilliseconds;
            return costTime.ToString();
        }
        #endregion

        #region RemoveDup 删除数组中的重复项
        /// <summary>
        /// 删除数组中的重复项
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string[] RemoveDup(string[] values)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                if (!list.Contains(values[i]))
                {
                    list.Add(values[i]);
                }
            }
            return list.ToArray();
        }
        #endregion

        #region CreateNo 自动生成编号
        /// <summary>
        /// 表示全局唯一标识符
        /// </summary>
        /// <returns></returns>
        public static string GuId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 自动生成编号
        /// </summary>
        /// <returns></returns>
        public static string CreateNo()
        {
            Random random = new Random();
            string strRandom = random.Next(1000, 10000).ToString();
            string code = DateTime.Now.ToString("yyyyMMddHHmmss") + strRandom;
            return code;
        }
        #endregion

        #region RndNum 生成0-9随机数
        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="codeNum">生成长度</param>
        /// <returns></returns>
        public static string RndNum(int codeNum)
        {
            StringBuilder sb = new StringBuilder(codeNum);
            Random random = new Random();
            for (int i = 0; i < codeNum + 1; i++)
            {
                int t = random.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();
        }
        #endregion

        #region 删除指定字符串
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strchar"></param>
        /// <returns></returns>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        /// <summary>
        /// 删除最后结尾指定的长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string DelLastLength(string str, int Length)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            return str.Substring(0, str.Length - Length);
        }
        #endregion
    }
}

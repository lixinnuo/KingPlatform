using System.Web.Security;

namespace Basic.Code
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class Md5
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16、32</param>
        /// <returns></returns>
        public static string md5(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                strEncrypt = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
            }
            if (code == 32)
            {
                strEncrypt = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }
            return strEncrypt;
        }


    }
}

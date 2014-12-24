using System.Collections.Generic;
using System.Linq;
using Beauty.Model;
using Beauty.Tool;

namespace Beauty.DataAccess
{
    /// <summary>
    /// 注册码操作
    /// </summary>
    public class RegistCodeDAL
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registCode">注册码</param>
        /// <returns></returns>
        public bool Register(string registCode)
        {
            bool flag;
            using (var con = new Connection().GetConnection)
            {
                con.Execute("Update RegistCode Set Code=@Code;" , new { Code = registCode });
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 取注册码
        /// </summary>
        /// <returns></returns>
        public RegistCode GetRegistCode()
        {
            RegistCode result = null;
            using (var con = new Connection().GetConnection)
            {
                var query = con.Query<RegistCode>("select * from RegistCode");
                if (query != null)
                {
                    result = query.First();
                    result = Encrypt.TDecryptDES(result);
                }
            }
            return result;
        }

        public bool UploadTryDays()
        {
            bool flag;
            using (var con = new Connection().GetConnection)
            {
                con.Execute("update RegistCode set SurplusDays=@SurplusDays ", new { SurplusDays = Encrypt.EncryptDES("0")});
                flag = true;
            }
            return flag;
        }
    }
}

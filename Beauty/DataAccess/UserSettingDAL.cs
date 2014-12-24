using System.Collections.Generic;
using System.Linq;
using Beauty.Model;
using Beauty.Tool;

namespace Beauty.DataAccess
{
    /// <summary>
    /// 用户设置类
    /// </summary>
    public class UserSettingDAL
    {
        /// <summary>
        /// 得到所有的默认值数据
        /// </summary>
        /// <returns></returns>
        public List<UserSettingMd> GetUserSetting()
        {
            List<UserSettingMd> result = null;
            using (var con = new Connection().GetConnection)
            {

                var query = con.Query<UserSettingMd>("select * from UserSetting ");
               var userSettingMds = query as IList<UserSettingMd> ?? query.ToList();
                if (userSettingMds.Any())
                {
                    result = userSettingMds.ToList();
                    result = Encrypt.TDecryptDES(result);
                }
            }
            return result;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="u">UserSettingMd 对象</param>
        /// <returns></returns>
        public bool Update(UserSettingMd u)
        {
            bool flag;
            using (var con = new Connection().GetConnection)
            {
                u = Encrypt.TEncryptDES(u);
                con.Execute(@"Update UserSetting set DefaultValueName=@DefaultValueName,
                        UpperValueOrDefaultValue=@UpperValueOrDefaultValue,LowerValue=@LowerValue,
                        Reserved=@Reserved where DefaultValueNo=@DefaultValueNo",
                        new { 
                            DefaultValueName=u.DefaultValueName,
                            UpperValueOrDefaultValue=u.UpperValueOrDefaultValue,
                            LowerValue= u.LowerValue,
                            Reserved = u.Reserved,
                            DefaultValueNo = u.DefaultValueNo
                        });
                flag = true;
            }
            return flag;
        }
    }
}

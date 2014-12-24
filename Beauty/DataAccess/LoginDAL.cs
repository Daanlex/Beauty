using System.Linq;
using Beauty.Model;
using Beauty.Tool;

namespace Beauty.DataAccess
{
    public class LoginDAL
    {
        /// <summary>
        /// 验证用户是否存在
        /// </summary>
        /// <param name="user">User对象</param>
        /// <returns></returns>
        public bool VerificationUser(User user)
        {
            bool flag = false;
            using (var con = new Connection().GetConnection)
            {
                string a = user.UserName;
                string b = Encrypt.EncryptDES(user.PassWord);
                var query = con.Query<User>("select * from User where UserName = @UserName and PassWord =@PassWord", new { UserName = a, PassWord =b});
                if (query != null)
                {
                    if (query.Count() == 1)
                        flag = true;
                }
            }
            return flag;
        }

        
    }
}

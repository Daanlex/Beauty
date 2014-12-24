using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Beauty.DataAccess;
using Beauty.Model;

namespace Beauty.Tool
{
    /// <summary>
    /// Pricsca Connect manager
    /// </summary>
    public class PriscaConnect
    {
        /// <summary>
        /// 自动开启Prisca Connect 监视进程，如果关闭了，自动开启。
        /// </summary>
        public  void AutoOpenPriscaConnect()
        {
            //获取Prisca Connect软件地址
            string path = new UserSettingDAL().GetUserSetting().First(o => o.DefaultValueNo == 9).UpperValueOrDefaultValue;

            //如果路径不为空，就开始操作，如果为空就不操作
            if (!string.IsNullOrEmpty(path))
            {
                StartMonitoring(path);
            }
        }


        private void StartMonitoring(string path)
        {
            Process current = Process.Start(path);
            if (current != null)
            {
                //current.StartInfo.CreateNoWindow = false;
                current.Exited += (s, e) => StartMonitoring(path);
                current.EnableRaisingEvents = true;
            }
        }

    }
}

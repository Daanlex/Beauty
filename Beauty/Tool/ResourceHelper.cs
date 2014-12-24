using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;

namespace Beauty.Tool
{
    public class ResourceHelper
    {
        /// <summary>
        /// 取资源文件的键值对
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string GetStaticResource(string Key)
        {
            return new ResourceManager("Beauty.Properties.Resources", Assembly.GetExecutingAssembly()).GetString(Key);
            
            //return Application.Current(Key).ToString();
        }
    }
}

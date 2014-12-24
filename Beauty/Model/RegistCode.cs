using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Model
{
    public class RegistCode
    {
        /// <summary>
        /// 注册码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 试用时间
        /// </summary>
        public string SurplusDays { get; set; }

        /// <summary>
        /// 首次生成数据库的事件
        /// </summary>
        public string FirstTime { get; set; }
    }
}

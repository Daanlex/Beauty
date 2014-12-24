using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Model
{
    /// <summary>
    /// 用户设置
    /// </summary>
    [Serializable]
    public class UserSettingMd
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// 默认值编号
        /// </summary>
        public Int64 DefaultValueNo { get; set; }
        /// <summary>
        /// 默认值名称
        /// </summary>
        public string DefaultValueName { get; set; }

        /// <summary>
        /// 上限值或者默认值
        /// </summary>
        public string UpperValueOrDefaultValue { get; set; }

        /// <summary>
        /// 下限值
        /// </summary>
        public string LowerValue { get; set; }

        /// <summary>
        /// 预留值
        /// </summary>
        public string Reserved { get; set; }

        
    }
}

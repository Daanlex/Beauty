using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Model
{
    /// <summary>
    /// 从prisca返回的数据
    /// </summary>
    public class MomRisk
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// 样本编号
        /// </summary>
        public string SampleNo { get; set; }

        /// <summary>
        /// AFP修正值1
        /// </summary>
        public double AFPCorrMom { get; set; }

        /// <summary>
        /// AFP修正值2
        /// </summary>
        public double AFPMom { get; set; }

        /// <summary>
        /// HCG修正值1
        /// </summary>
        public double HCGCorrMom { get; set; }

        /// <summary>
        /// HCG修正值2
        /// </summary>
        public double HCGMom { get; set; }

        /// <summary>
        /// UE3修正值1
        /// </summary>
        public double UE3CorrMom { get; set; }

        /// <summary>
        /// UE3修正值2
        /// </summary>
        public double UE3Mom { get; set; }

        /// <summary>
        /// 年龄风险
        /// </summary>
        public double AgeRisk { get; set; }

        /// <summary>
        /// 年龄风险2
        /// </summary>
        public double AgeRisk2 { get; set; }

        /// <summary>
        /// 18三体综合证风险
        /// </summary>
        public double AR18 { get; set; }

        /// <summary>
        /// 21三体综合证风险
        /// </summary>
        public double AR21 { get; set; }

        /// <summary>
        /// 神经管缺陷风险
        /// </summary>
        public double NTDRisk { get; set; }
        /// <summary>
        /// 分娩年龄
        /// </summary>
        public double AgeDelivery { get; set; }

        /// <summary>
        /// 自动计算的孕周
        /// </summary>
        public double GAWD { get; set; }

        #region 孕早期需要的字段

        /// <summary>
        /// 游离HCG的mom值
        /// </summary>
        public double FBCorrMoM { get; set; }

        /// <summary>
        /// PPAP-A的mom值
        /// </summary>
        public double PAPPCorrMoM { get; set; }

        /// <summary>
        /// NT的风险值
        /// </summary>
        public double NTCorrMoM { get; set; }

        /// <summary>
        /// 孕早期生化标志物风险
        /// </summary>
        public double EsBiochemicalMarkers { get; set; }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beauty.Model
{
    [Serializable]
    public class Patient
    {
        public Int64 Id { get; set; }

        /// <summary>
        /// 样本编号
        /// </summary>
        public string SampleNo { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string PatientName { get; set; }
        /// <summary>
        ///  患者生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Int64 Gender { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public double Age { get; set; }

        /// <summary>
        /// 住院号
        /// </summary>
        public string HospitalizedNo { get; set; }

        /// <summary>
        /// 床号
        /// </summary>
        public string BadNo { get; set; }

        /// <summary>
        /// 患者电话
        /// </summary>
        public string PatientTel { get; set; }

        /// <summary>
        /// 患者家庭住址
        /// </summary>
        public string PatientAddress { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospName { get; set; }
        /// <summary>
        /// 送检医生
        /// </summary>
        public string CensorshipDoctor { get; set; }
        /// <summary>
        /// 送检科室
        /// </summary>
        public string CensorshipDepartments { get; set; }
        /// <summary>
        /// 标本号
        /// </summary>
        public string SpecimenNo { get; set; }
        /// <summary>
        /// 组合项目名称
        /// </summary>
        public string TestName { get; set; }
        /// <summary>
        /// 组合项目名称缩写
        /// </summary>
        public string TestNameAbb { get; set; }
        /// <summary>
        /// 采样日期
        /// </summary>
        public DateTime CollectionDate { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 胎儿数量
        /// </summary>
        public Int64 FETU { get; set; }
        /// <summary>
        /// 是否吸烟
        /// </summary>
        public Int64 SMOK { get; set; }
        /// <summary>
        /// 是否体外受孕
        /// </summary>
        public Int64 IVF { get; set; }
        /// <summary>
        /// 是否有糖尿病史
        /// </summary>
        public Int64 DIAB { get; set; }

        /// <summary>
        /// 是否有不良孕产史
        /// </summary>
        public Int64 APH { get; set; }
        /// <summary>
        /// 人种 3为亚洲人种
        /// </summary>
        public Int64 RAID { get; set; }

        /// <summary>
        /// AFP值
        /// </summary>
        public double AFP { get; set; }
        /// <summary>
        /// HCG值
        /// </summary>
        public double HCG { get; set; }
        /// <summary>
        /// UE3值
        /// </summary>
        public double UE3 { get; set; }

        /// <summary>
        /// 游离β-HCG
        /// </summary>
        public double PHCG { get; set; }

        /// <summary>
        /// PPAP
        /// </summary>
        public double PPAP { get; set; }

        /// <summary>
        /// 颈部透明厚度
        /// </summary>
        public double NT { get; set; }

        /// <summary>
        /// 采样时孕周
        /// </summary>
        public string GestationalWeek { get; set; }
        /// <summary>
        /// B超孕周
        /// </summary>
        public string GestationalWeekByBC { get; set; }

        /// <summary>
        /// B超时间
        /// </summary>
        public DateTime GestationalWeekByBCDate { get; set; }

        /// <summary>
        /// 末次月经日期
        /// </summary>
        public DateTime TestLMPDate { get; set; }

        /// <summary>
        /// 月经周期
        /// </summary>
        public string TestCYCL { get; set; }

        /// <summary>
        /// 头臀长测量日期
        /// </summary>
        public DateTime TestCRLDate { get; set; }

        /// <summary>
        /// 头臀长长度
        /// </summary>
        public string TestCRLLength { get; set; }

        /// <summary>
        /// 双顶径测量日期 
        /// </summary>
        public DateTime TestBPDDate { get; set; }

        /// <summary>
        /// 双顶径厚度
        /// </summary>
        public string TestBPDLength { get; set; }

        /// <summary>
        /// 确定方法
        /// </summary>
        public string Determination { get; set; }
        /// <summary>
        /// 筛查类型 1 末次月经 2 头臀长 3 双顶径 4 B超孕周
        /// </summary>
        public Int64 TestType { get; set; }
        /// <summary>
        /// 末次月经或者是测试日期
        /// </summary>
        public DateTime TestDate { get; set; }
        /// <summary>
        /// 测试的值，可以是月经周期，长度 ，厚度
        /// </summary>
        public string TestValue { get; set; }
        /// <summary>
        /// 数据创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        
        /// <summary>
        /// 导入Prisca的次数
        /// </summary>
        public Int64 IsImportPrisca { get; set; }

        /// <summary>
        /// 表示选中和不选中
        /// </summary>
        public Int64 CurrentSelected { get; set; }

        /// <summary>
        /// 打印次数
        /// </summary>
        public string PrintCount { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        public string OtherInformation { get; set; }

        /// <summary>
        /// 是否有鼻骨
        /// </summary>
        public Int64 IsHaveNasalBone { get; set; }

        /// <summary>
        /// 当前样本的风险
        /// </summary>
        public MomRisk momrisk { get; set; }

        /// <summary>
        /// 检测者
        /// </summary>
        public string Examinee { get; set; }
        /// <summary>
        /// 审核者
        /// </summary>
        public string Audit { get; set; }

    }
}

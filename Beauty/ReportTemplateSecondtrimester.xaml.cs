using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Beauty.Tool;
using Beauty.Model;

namespace Beauty
{
    /// <summary>
    /// ReportTemplate2.xaml 的交互逻辑
    /// </summary>
    public partial class ReportTemplateSecondtrimester
    {
        public ReportTemplateSecondtrimester(Patient p, MomRisk m, List<UserSettingMd> defaultValue)
        {
            InitializeComponent();
            
            //ChartTest.Background = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
            SetValueToReport(p, m, defaultValue);
        }

        /// <summary>
        /// 为报告赋值
        /// </summary>
        /// <param name="p">病人信息</param>
        /// <param name="m">修正值和风险</param>
        /// <param name="defaultValue"></param>
        private void SetValueToReport(Patient p, MomRisk m, List<UserSettingMd> defaultValue)
        {
            tbPatientName.Text = p.PatientName;
            dtpBirthday.Text = p.Birthday == new DateTime() ? "" : p.Birthday.ToShortDateString();
            tbAge.Text = p.Age.ToString();
            //tbCensorshipDoctor.Text = p.CensorshipDoctor;
            tbSpecimenNo.Text = p.SpecimenNo;
            tbSampleNo.Text = p.SampleNo;
            dtpCollectionDate.Text = p.CollectionDate == new DateTime() ? "" : p.CollectionDate.ToShortDateString();
            tbWGHT.Text = p.Weight.ToString();
            tbFETU.Text = p.FETU.ToString();
            tbOtherInfomation.Text = p.OtherInformation;
            cbSMOK.Text = DataHandle.GetEnum(sMOK: p.SMOK);
            cbIVF.Text = DataHandle.GetEnum(iVF: p.IVF);
            cbDIAB.Text = DataHandle.GetEnum(dIAB: p.DIAB);
            cbDoctor.Text = p.CensorshipDoctor;
            //cbAPH.Text = DataHandle.GetEnum(aPH: p.APH);
            cbRAID.Text = DataHandle.GetEnum(rAID: p.RAID);

            tbAFP.Text = p.AFP.ToString();
            tbHCG.Text = p.HCG.ToString();
            tbUE3.Text = p.UE3.ToString();
            tbGestationalWeek.Text = p.GestationalWeek;
            tbDetermination.Text = p.Determination;

            //B超信息
            tbbultrasound.Text = p.GestationalWeekByBCDate.ToShortDateString();
            tbCrl.Text = p.TestCRLLength;
            tbBpd.Text = p.TestBPDLength;
            tbNt.Text = p.NT.ToString();
            tbNasalBone.Text = p.IsHaveNasalBone==0 ? "无" : "有";

            reportDate.Text = DateTime.Now.ToShortDateString();

            //第一个参数是No，第二个是上限还是下限
            Func<int, string, string> func = (a, b) =>
                {
                    if (b == "Up")
                        return defaultValue.Where(o => o.DefaultValueNo == a).ToList()[0].UpperValueOrDefaultValue;
                    return defaultValue.Where(o => o.DefaultValueNo == a).ToList()[0].LowerValue;
                };

            if (defaultValue.Count > 0)
            {
                tbAFPRef.Text = func(1, "Lower") + "-" + func(1, "Up") + "MOM";
                tbHCGRef.Text = func(2, "Lower") + "-" + func(2, "Up") + "MOM";
                tbUE3Ref.Text = func(3, "Lower") + "-" + func(3, "Up") + "MOM";
            }


            

            if (m != null)
            {
                tbAgeDelivery.Text = m.AgeDelivery.ToString("0.0");
                //修正值和风险
                tbAFPMom.Text = m.AFPCorrMom.ToString();
                tbUE3Mom.Text = m.UE3CorrMom.ToString();
                tbHCGMom.Text = m.HCGCorrMom.ToString();
                tbGestationalWeek.Text = !string.IsNullOrWhiteSpace(m.GAWD.ToString()) & m.GAWD != 0 ? (m.GAWD.ToString().IndexOf('.') < 0 ? m.GAWD.ToString() + "周" : m.GAWD.ToString().Replace(".", "周") + "天") : p.GestationalWeek;

                tbAR21Risk.Text = m.AR21<=50?">1:50": "1:" + m.AR21;
                tbAR18Risk.Text = m.AR18<=50?">1:50": "1:" + m.AR18;
                tbNtRisk.Text = m.AFPCorrMom.ToString();
                tbAgeRisk.Text = m.AgeDelivery.ToString("0.0");
                tbAR21RiskCu.Text = m.AR21 <= 270 ? "高风险" : "低风险";
                tbAR18RiskCu.Text = m.AR18 <= 350 ? "高风险" : "低风险";
                tbNtRiskCu.Text = m.AFPCorrMom > Convert.ToDouble(func(1, "Up")) ? "高风险" : "低风险";
                tbAgeRiskCu.Text = m.AgeDelivery > 35 ? "高风险" : "低风险";
                tbAr21RiskDesc.Text = string.Format(tbAr21RiskDesc.Text,
                    m.AR21 <= 270 ? "高风险，建议您立即做产前诊断及遗传咨询。" : "低风险，建议动态观察。");
                tbAr18RiskDesc.Text = string.Format(tbAr18RiskDesc.Text,
                    m.AR18 <= 350 ? "高风险，建议您立即做产前诊断及遗传咨询。" : "低风险，建议动态观察。");
                tbSjRiskDesc.Text = string.Format(tbSjRiskDesc.Text,
                    m.AFPCorrMom > Convert.ToDouble(func(1, "Up")) ? "高风险，建议您立即做产前诊断及遗传咨询。" : "低风险，建议动态观察。");
                tbAgeRiskDesc.Text = string.Format(tbAgeRiskDesc.Text,
                    m.AgeDelivery >35 ? "高风险，建议您立即做产前诊断及遗传咨询。" : "低风险，建议动态观察。");


                //生成柱状图片
                Ar21ChartDataPoint.YValue = CalculatRisk(RiskType.Ar21, m.AR21<=50?50:m.AR21);
                Ar18ChartDataPoint.YValue = CalculatRisk(RiskType.Ar18, m.AR18<=50?50:m.AR18);
                NtChartDataPoint.YValue = m.AFPCorrMom;
                AgeChartDataPoint.YValue = CalculatRisk(RiskType.Age, m.AgeDelivery); 
            }
        }

        private double CalculatRisk(RiskType riskType, double riskVal)
        {
            if (riskVal > 10000) riskVal = 9500;
            double resultVal = 0;
            double upProportion, downProportion;
            switch (riskType)
            {
                case RiskType.Ar21:
                    upProportion = 30.0 / 270;
                    downProportion = 30.0 / (11000 - 270);
                    if (riskVal > 270)
                        resultVal = 270 - (riskVal * downProportion);
                    else
                        resultVal = 300 - riskVal * upProportion;
                    break;
                case RiskType.Ar18:
                    upProportion = 30.0 / 350;
                    downProportion = 30.0 / (11000 - 350);
                    if (riskVal > 350)
                        resultVal = 350 - (riskVal * downProportion);
                    else
                        resultVal = 380 - riskVal * upProportion;
                    break;
                case RiskType.Age:
                    upProportion = 31.0 / 35;
                    downProportion = 18 / 14.0;
                    if (riskVal <= 35)
                        resultVal = upProportion * riskVal;
                    else
                        resultVal = 31 + downProportion * (riskVal - 35);

                    break;
            }
            return resultVal;
        }
    }

    public enum RiskType
    {
        Ar21,
        Ar18,
        Nt,
        Age,
        EsBio
    }
}

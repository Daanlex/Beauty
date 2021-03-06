﻿using System;
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
    public partial class ReportTemplate2
    {
        public ReportTemplate2(Patient p, MomRisk m, List<UserSettingMd> defaultValue)
        {
            InitializeComponent();
            ChartTest.Background = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
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
            tbCensorshipDoctor.Text = p.TestLMPDate == new DateTime() ? "" : p.TestLMPDate.ToShortDateString();
            tbSampleNo.Text = p.SampleNo;
            dtpCollectionDate.Text = p.CollectionDate == new DateTime() ? "" : p.CollectionDate.ToShortDateString();
            tbWGHT.Text = p.Weight.ToString();
            tbFETU.Text = p.FETU.ToString();
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
                tbUE3Ref.Text = func(1, "Lower") + "-" + func(3, "Up") + "MOM";
            }


            if (m != null)
            {
                //修正值和风险
                tbAFPMom.Text = m.AFPCorrMom.ToString();
                tbUE3Mom.Text = m.UE3CorrMom.ToString();
                tbHCGMom.Text =  m.HCGCorrMom.ToString();
                tbGestationalWeek.Text = !string.IsNullOrWhiteSpace(m.GAWD.ToString()) & m.GAWD!=0 ? (m.GAWD.ToString().IndexOf('.')<0?m.GAWD.ToString()+"周": m.GAWD.ToString().Replace(".", "周") + "天")  : p.GestationalWeek;

                tbAR21Risk.Text = "1:" + m.AR21;
                tbAgeRisk.Text = "1:" + m.AgeRisk2;
                tbAR21RiskDesc.Text = string.Format(tbAR21RiskDesc.Text, m.AR21 <= 250 ? "高于" : "低于", m.AR21 <= 250 ? "高" : "低");
                tbAR21SampleCount.Text = string.Format(tbAR21SampleCount.Text, m.AR21, m.AR21 - 1);
                tbNTDRisk.Text = string.Format(tbNTDRisk.Text, m.AFPCorrMom, m.AFPCorrMom > Convert.ToDouble(func(1, "Up")) ? "高" : "低");
                tbAFPRef2.Text = string.Format(tbAFPRef2.Text, func(1, "Up"));
                tbAR18RiskDesc.Text = string.Format(tbAR18RiskDesc.Text, m.AR18, m.AR18 <= 100 ? "高" : "低");


                //生成柱状图片
                RiskChartValue.XValue = m.AgeDelivery;
                RiskChartValue.YValue = GetAfterCalculatingRisk21(m.AR21);
            }
        }

        /// <summary>
        /// 因为要根据坐标重新计算21三体的风险，所以...
        /// </summary>
        /// <param name="ar21">真实的21三体的风险</param>
        /// <returns></returns>
        private double GetAfterCalculatingRisk21(double ar21)
        {
            double currentRisk=0;
            if (ar21 <= 10000 & ar21 > 1000)
                currentRisk = (10000-ar21)*(1150.0/9000.0);
            if (ar21 <= 1000 & ar21 > 250)
                currentRisk =2000+ (1000- ar21)*(1950.0/750.0);
            if (ar21 <= 250 & ar21 > 100)
                currentRisk = 4500 + (250- ar21)*(1450.0/150.0);
            if (ar21 <= 100 & ar21 > 10)
                currentRisk = 6000+ (100- ar21)*(3150.0/90.0);
            return currentRisk;
        }
    }
}

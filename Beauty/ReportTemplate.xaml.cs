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
    public partial class ReportTemplate
    {
        public ReportTemplate(Patient p, MomRisk m, List<UserSettingMd> defaultValue)
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
            dtpBirthday.Text = p.Birthday==new DateTime()?"":p.Birthday.ToShortDateString();
            tbAge.Text = p.Age.ToString();
            //tbCensorshipDoctor.Text = p.CensorshipDoctor;
            tbCRL.Text = p.TestCRLLength.ToString();
            tbSampleNo.Text = p.SampleNo;
            dtpCollectionDate.Text = p.CollectionDate == new DateTime() ? "" : p.CollectionDate.ToShortDateString();
            tbWGHT.Text = p.Weight.ToString();
            tbFETU.Text = p.FETU.ToString();
            cbSMOK.Text = DataHandle.GetEnum(sMOK: p.SMOK);
            cbIVF.Text = DataHandle.GetEnum(iVF: p.IVF);
            cbDIAB.Text = DataHandle.GetEnum(dIAB: p.DIAB);
            cbDoctor.Text = p.CensorshipDoctor;
            cbRAID.Text = DataHandle.GetEnum(rAID: p.RAID);
            tbPPAP.Text = p.PPAP.ToString();
            tbFBHCG.Text = p.PHCG.ToString();
            tbBCGestationalWeek.Text = p.GestationalWeekByBC;
            tbDetermination.Text = "头臀长度";
            tbNTResult.Text = p.NT.ToString();

            reportDate.Text = DateTime.Now.ToShortDateString();




            if (m != null)
            {
                //修正值和风险
                tbPPAPMom.Text = m.PAPPCorrMoM.ToString();
                tbFBHCGMom.Text =m.FBCorrMoM.ToString();
                tbNTMom.Text = m.NTCorrMoM.ToString();
                tbBCGestationalWeek.Text = m.GAWD.ToString().IndexOf('.') < 0 ? m.GAWD.ToString() + "周" : m.GAWD.ToString().Replace(".", "周") + "天"; //m.GAWD.ToString().Replace(".", "周") + "天";

                tbAR21Risk.Text = "1:" + m.AR21;
                tbAgeRisk.Text = "1:" + m.AgeRisk2;
                tbAR21RiskDesc.Text = string.Format(tbAR21RiskDesc.Text, m.AR21 <= 250 ? "高于" : "低于", m.AR21 <= 250 ? "高" : "低");
                tbAR21SampleCount.Text = string.Format(tbAR21SampleCount.Text, m.AR21, m.AR21 - 1);
                tbAR18RiskDesc.Text = string.Format(tbAR18RiskDesc.Text, m.AR18, m.AR18 <= 100 ? "高" : "低");
                EsBioRisk.Text = string.Format(EsBioRisk.Text, m.EsBiochemicalMarkers, m.EsBiochemicalMarkers <= 250 ? "高" : "低");


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

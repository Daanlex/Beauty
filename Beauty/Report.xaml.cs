using System;
using System.Collections.Generic;
using System.Printing;
using System.Windows.Controls;
using Beauty.Model;
using Beauty.DataAccess;
using Beauty.Tool;


namespace Beauty
{
    /// <summary>
    /// Report.xaml 的交互逻辑
    /// </summary>
    public partial class Report
    {
        public Report(Patient patient)
        {
            InitializeComponent();
            //先取出从Prisca中返回的数据
            MomRisk momRisk = new MomRiskDAL().GetMomRiskBySampleNo(patient.SampleNo);

            //在取出默认值
            List<UserSettingMd> userSetting = new UserSettingDAL().GetUserSetting();

            SecondTrimester.Click += (s, e) =>
            {
                PRINTAREA.Children.Clear();
                PRINTAREA.Children.Add(new ReportTemplateSecondtrimester(patient, momRisk, userSetting));
            };
            FirstTrimester.Click += (s, e) =>
            {
                PRINTAREA.Children.Clear();
                PRINTAREA.Children.Add(new ReportTemplateEarlypregnancy(patient, momRisk, userSetting));
            };
            btnPrint.MouseLeftButtonDown += (s, e) => Common.PrintReport(PRINTAREA, patient.PatientName,patient.Id);
            //测试取所有打印机
            //EnumeratedPrintQueueTypes[] enumerationFlags = { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Shared };
            //LocalPrintServer printServer =new LocalPrintServer();
            //PrintQueueCollection printQueuesOnLocalServer = printServer.GetPrintQueues(enumerationFlags);
            //foreach (PrintQueue printQueue in printQueuesOnLocalServer)
            //{
                
            //}

            //隐藏不要的报告单
            //SecondTrimester.Visibility = System.Windows.Visibility.Collapsed;
            //FirstTrimester.Visibility = System.Windows.Visibility.Collapsed;

            //默认加载孕中期报告
            PRINTAREA.Children.Clear();
            if(patient.TestType == 5)
                 PRINTAREA.Children.Add( new ReportTemplateEarlypregnancy(patient, momRisk, userSetting));
            else 
                PRINTAREA.Children.Add( new ReportTemplateSecondtrimester(patient, momRisk, userSetting));

            //var secondTrimesterReport1 = new ReportTemplate2(patient, momRisk, userSetting);
            //AllCanvas.Height = 1070*2;
            //PRINTAREA.Children.Add(secondTrimesterReport1);
        }
    }
}

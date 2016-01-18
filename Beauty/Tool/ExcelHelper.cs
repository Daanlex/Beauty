using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using Beauty.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Beauty.Tool
{
    /// <summary>
    /// Excel文件帮助类，导入和导出
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 加载Excel 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataSet LoadDataFromExcel(string filePath)
        {
            try
            {
                var app = new Microsoft.Office.Interop.Excel.Application();
                Workbook wb = app.Workbooks.Open(filePath);// app.Workbooks.Add(Type.Missing);

                //app.ActiveWorkbook.SaveAs(filePath, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);  
                string strConn = "", tableName = "";
                string fileSuffix = filePath.Substring(filePath.LastIndexOf(".", StringComparison.Ordinal) + 1);

                if (fileSuffix == "xls")
                {
                    //设置禁止弹出保存和覆盖的询问提示框 
                    app.DisplayAlerts = false;
                    app.AlertBeforeOverwriting = false;

                    wb.SaveAs(filePath, XlFileFormat.xlWorkbookNormal, null, null, false, false, XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);
                    tableName = ((_Worksheet)wb.Worksheets[1]).Name;
                    wb.Close();

                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
                }
                else if (fileSuffix == "xlsx")
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";

                var oleConn = new OleDbConnection(strConn);
                oleConn.Open();

                String sql = "SELECT * FROM  [" + tableName + "$]";//可是更改Sheet名称，比如sheet2，等等 

                var oleDaExcel = new OleDbDataAdapter(sql, oleConn);
                var oleDsExcle = new DataSet();
                oleDaExcel.Fill(oleDsExcle, "Sheet1");
                oleConn.Close();
                return oleDsExcle;
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message);
                return null;
            }
        }

        public static bool SaveDataListToExcel(IEnumerable<Patient> lst, string filePath)
        {
            var list = lst as List<Patient>;
            var hssfworkbook = new HSSFWorkbook();
            // 新建一个Excel页签
            var sheet1 = hssfworkbook.CreateSheet("Sheet1");
            var header =sheet1.CreateRow(0);
            header.CreateCell(0).SetCellValue("门诊ID");
            header.CreateCell(1).SetCellValue("姓名");
            header.CreateCell(2).SetCellValue("出生日期");
            header.CreateCell(3).SetCellValue("性别");
            header.CreateCell(4).SetCellValue("年龄");
            header.CreateCell(5).SetCellValue("住院号");
            header.CreateCell(6).SetCellValue("床号");
            header.CreateCell(7).SetCellValue("病人电话");
            header.CreateCell(8).SetCellValue("病人地址");
            header.CreateCell(9).SetCellValue("送检医生");

            header.CreateCell(10).SetCellValue("送检科室");
            header.CreateCell(11).SetCellValue("样本编号");
            header.CreateCell(12).SetCellValue("采样日期");
            header.CreateCell(13).SetCellValue("体重");
            header.CreateCell(14).SetCellValue("胎儿数量");
            header.CreateCell(15).SetCellValue("是否吸烟");
            header.CreateCell(16).SetCellValue("是否体外受孕");
            header.CreateCell(17).SetCellValue("是否有糖尿病史");
            header.CreateCell(18).SetCellValue("是否有不良孕产史");
            header.CreateCell(19).SetCellValue("人种");
            header.CreateCell(20).SetCellValue("AFP");
            header.CreateCell(21).SetCellValue("HCG");
            header.CreateCell(22).SetCellValue("UE3");
            header.CreateCell(23).SetCellValue("PAPPA");
            header.CreateCell(24).SetCellValue("β-HCG");
            header.CreateCell(25).SetCellValue("采样时孕周");
            header.CreateCell(26).SetCellValue("B超孕周");
            header.CreateCell(27).SetCellValue("B超时间");
            header.CreateCell(28).SetCellValue("确定方法");
            header.CreateCell(29).SetCellValue("末次月经");
            header.CreateCell(30).SetCellValue("月经周期");
            header.CreateCell(31).SetCellValue("头臀长测量日期");
            header.CreateCell(32).SetCellValue("头臀长");
            header.CreateCell(33).SetCellValue("双顶径测量日期");
            header.CreateCell(34).SetCellValue("双顶径"); 
            header.CreateCell(35).SetCellValue("NT");
            header.CreateCell(36).SetCellValue("登记日期");
            header.CreateCell(37).SetCellValue("导入Prisca次数");
            header.CreateCell(38).SetCellValue("AFP修正值");
            header.CreateCell(39).SetCellValue("HCG修正值");
            header.CreateCell(40).SetCellValue("UE3修正值");
            header.CreateCell(41).SetCellValue("AR18风险");
            header.CreateCell(42).SetCellValue("AR21风险");
            header.CreateCell(43).SetCellValue("年龄风险");
            header.CreateCell(44).SetCellValue("PAPP-A修正值");
            header.CreateCell(45).SetCellValue("β-HCG修正值");
            header.CreateCell(46).SetCellValue("NT修正值");
            header.CreateCell(47).SetCellValue("是否非农");
            // 创建新增行
            for (var i = 1; i <= list.Count; i++)
            {
                var p = list[i-1];
                IRow row = sheet1.CreateRow(i);
                    //新建单元格
                row.CreateCell(0).SetCellValue(p.SampleNo);
                row.CreateCell(1).SetCellValue(p.PatientName);
                row.CreateCell(2).SetCellValue(p.Birthday);
                row.CreateCell(3).SetCellValue(DataHandle.GetEnum(p.Gender));
                row.CreateCell(4).SetCellValue(p.Age);
                row.CreateCell(5).SetCellValue(p.HospitalizedNo);
                row.CreateCell(6).SetCellValue(p.BadNo);
                row.CreateCell(7).SetCellValue(p.PatientTel);
                row.CreateCell(8).SetCellValue(p.PatientAddress);
                row.CreateCell(9).SetCellValue(p.CensorshipDoctor);
                row.CreateCell(10).SetCellValue(p.CensorshipDepartments);
                row.CreateCell(11).SetCellValue(p.SpecimenNo);
                row.CreateCell(12).SetCellValue(p.CollectionDate);
                row.CreateCell(13).SetCellValue(p.Weight);
                row.CreateCell(14).SetCellValue(p.FETU);
                row.CreateCell(15).SetCellValue(DataHandle.GetEnum(sMOK: p.SMOK));
                row.CreateCell(16).SetCellValue(DataHandle.GetEnum(iVF: p.IVF));
                row.CreateCell(17).SetCellValue(DataHandle.GetEnum(dIAB: p.DIAB));
                row.CreateCell(18).SetCellValue(DataHandle.GetEnum(aPH: p.APH));
                row.CreateCell(19).SetCellValue(DataHandle.GetEnum(rAID: p.RAID));
                row.CreateCell(20).SetCellValue(p.AFP);
                row.CreateCell(21).SetCellValue(p.HCG);
                row.CreateCell(22).SetCellValue(p.UE3);
                row.CreateCell(23).SetCellValue(p.PPAP);
                row.CreateCell(24).SetCellValue(p.PHCG);
                row.CreateCell(25).SetCellValue(p.GestationalWeek);
                row.CreateCell(26).SetCellValue(p.GestationalWeekByBC);
                row.CreateCell(27).SetCellValue(p.GestationalWeekByBCDate);
                row.CreateCell(28).SetCellValue(p.Determination);

                row.CreateCell(29).SetCellValue(p.TestLMPDate.ToString("yyyy/MM/dd"));
                row.CreateCell(30).SetCellValue(p.TestCYCL);
                row.CreateCell(31).SetCellValue(p.TestCRLDate.ToString("yyyy/MM/dd"));
                row.CreateCell(32).SetCellValue(p.TestCRLLength);
                row.CreateCell(33).SetCellValue(p.TestBPDDate.ToString("yyyy/MM/dd"));
                row.CreateCell(34).SetCellValue(p.TestBPDLength);
                row.CreateCell(35).SetCellValue(p.NT);
                row.CreateCell(36).SetCellValue(p.CreateDate);
                row.CreateCell(37).SetCellValue(p.IsImportPrisca);
                if (p.momrisk != null)
                {
                    row.CreateCell(38).SetCellValue(p.momrisk.AFPCorrMom);
                    row.CreateCell(39).SetCellValue(p.momrisk.HCGCorrMom);
                    row.CreateCell(40).SetCellValue( p.momrisk.UE3CorrMom);
                    row.CreateCell(41).SetCellValue(p.momrisk.AR18);
                    row.CreateCell(42).SetCellValue(p.momrisk.AR21);
                    row.CreateCell(43).SetCellValue(p.momrisk.AgeDelivery);
                    row.CreateCell(44).SetCellValue(p.momrisk.PAPPCorrMoM);
                    row.CreateCell(45).SetCellValue(p.momrisk.FBCorrMoM);
                    row.CreateCell(46).SetCellValue(p.momrisk.NTCorrMoM);
                }
                row.CreateCell(47).SetCellValue(p.IsFn);

            }
            var ms = new MemoryStream();
            hssfworkbook.Write(ms);

            var fs = new FileStream(filePath, FileMode.OpenOrCreate);
            var w = new BinaryWriter(fs);
            w.Write(ms.ToArray());
            fs.Close();
            ms.Close();

            return true;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool SaveDataTableToExcel(IEnumerable<Patient> lst, string filePath)
        {
            var app = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                app.Visible = false;
                Workbook wBook = app.Workbooks.Add(true);
                var wSheet = wBook.Worksheets[1] as Worksheet;

                #region 增加表头

                if (wSheet != null)
                {
                    string[] tableHeader =
                        {
                            "样本编号", "姓名", "生日", "性别", "年龄", "住院号",
                            "床号", "电话", "地址", "医院名称", "送检医生", "送检科室",
                            "标本号", "检测项目", "检测项目简称", "采集时间", "体重",
                            "胎儿数量", "是否吸烟", "是否体外受孕", "是否有糖尿病史", "是否有不良孕产史",
                            "人种", "AFP", "UCG", "UE3", "孕周", "B超孕周","B超时间", "确定方法", "测试类型",
                            "末次月经时间", "月经周期", "头臀长检测时间", "头臀长长度", "双顶径检测时间",
                            "双顶径厚度", "数据创建时间", "导入Prisca次数", "AFP修正值", "HCG修正值", "UE3修正值",
                            "18三体", "21三体", "分娩年龄"
                        };
                    for (int i = 0; i < tableHeader.Length; i++)
                    {
                        wSheet.Cells[1, i + 1] = tableHeader[i];
                    }

                #endregion

                    var list = lst as List<Patient>;

                    for (int i = 2; i < list.Count + 2; i++)
                    {
                        Patient p = list[i - 2];
                        wSheet.Cells[i, 1] = p.SampleNo;
                        wSheet.Cells[i, 2] = p.PatientName;
                        wSheet.Cells[i, 3] = p.Birthday;
                        wSheet.Cells[i, 4] = DataHandle.GetEnum(p.Gender);
                        wSheet.Cells[i, 5] = p.Age;
                        wSheet.Cells[i, 6] = p.HospitalizedNo;
                        wSheet.Cells[i, 7] = p.BadNo;
                        wSheet.Cells[i, 8] = p.PatientTel;
                        wSheet.Cells[i, 9] = p.PatientAddress;
                        wSheet.Cells[i, 10] = p.HospName;
                        wSheet.Cells[i, 11] = p.CensorshipDoctor;
                        wSheet.Cells[i, 12] = p.CensorshipDepartments;
                        wSheet.Cells[i, 13] = p.SpecimenNo;
                        wSheet.Cells[i, 14] = p.TestName;
                        wSheet.Cells[i, 15] = p.TestNameAbb;
                        wSheet.Cells[i, 16] = p.CollectionDate;
                        wSheet.Cells[i, 17] = p.Weight;
                        wSheet.Cells[i, 18] = p.FETU;
                        wSheet.Cells[i, 19] = DataHandle.GetEnum(sMOK: p.SMOK);
                        wSheet.Cells[i, 20] = DataHandle.GetEnum(iVF: p.IVF);
                        wSheet.Cells[i, 21] = DataHandle.GetEnum(dIAB: p.DIAB);
                        wSheet.Cells[i, 22] = DataHandle.GetEnum(aPH: p.APH);
                        wSheet.Cells[i, 23] = DataHandle.GetEnum(rAID: p.RAID);
                        wSheet.Cells[i, 24] = p.AFP;
                        wSheet.Cells[i, 25] = p.HCG;
                        wSheet.Cells[i, 26] = p.UE3;
                        wSheet.Cells[i, 27] = p.GestationalWeek;
                        wSheet.Cells[i, 28] = p.GestationalWeekByBC;
                        wSheet.Cells[i, 29] = p.GestationalWeekByBCDate;
                        wSheet.Cells[i, 30] = p.Determination;
                        string[] test = GetTestValue(p.TestDate, p.TestType, p.TestValue);

                        wSheet.Cells[i, 31] = test[0]; // p.TestType;
                        wSheet.Cells[i, 32] = p.TestLMPDate.ToString("yyyy/MM/dd"); //test[1]; //p.TestDate;
                        wSheet.Cells[i, 33] = p.TestCYCL; //test[2]; //p.TestValue;
                        wSheet.Cells[i, 34] = p.TestCRLDate.ToString("yyyy/MM/dd"); //test[3]; //p.CreateDate;
                        wSheet.Cells[i, 35] = p.TestCRLLength;//test[4]; //p.IsImportPrisca;
                        wSheet.Cells[i, 36] = p.TestBPDDate.ToString("yyyy/MM/dd");//test[5];
                        wSheet.Cells[i, 37] = p.TestBPDLength;//test[6];
                        wSheet.Cells[i, 38] = p.CreateDate;
                        wSheet.Cells[i, 39] = p.IsImportPrisca;
                        if (p.momrisk != null)
                        {
                            wSheet.Cells[i, 40] = p.momrisk.AFPCorrMom;
                            wSheet.Cells[i, 41] = p.momrisk.HCGCorrMom;
                            wSheet.Cells[i, 42] = p.momrisk.UE3CorrMom;
                            wSheet.Cells[i, 43] = p.momrisk.AR18;
                            wSheet.Cells[i, 44] = p.momrisk.AR21;
                            wSheet.Cells[i, 45] = p.momrisk.AgeDelivery;
                        }
                    }

                }

                //设置禁止弹出保存和覆盖的询问提示框 
                app.DisplayAlerts = false;
                app.AlertBeforeOverwriting = false;
                //保存工作簿 
                wBook.Save();
                //保存excel文件 
                app.Save(filePath);
                app.SaveWorkspace(filePath);
                app.Quit();
                //app = null;
                return true;
            }
            catch (Exception err)
            {
                MessageBox.Show("导出Excel出错！错误原因：" + err.Message, "提示信息");
                return false;
            }

        }



        /// <summary>
        /// 处理检测方法的数据
        /// </summary>
        /// <param name="createDate">时间</param>
        /// <param name="testType">检测类型1 末次月经 2 头臀长 3 双顶径</param>
        /// <param name="testValue">检测值</param>
        /// <returns></returns>
        private static string[] GetTestValue(DateTime? createDate, long testType = 1, string testValue = "")
        {

            string[] test = null;
            switch (testType)
            {
                case 1:
                    test = new[] { "末次月经", Convert.ToDateTime(createDate).ToString("yyyy/MM/dd"), testValue, "", "", "", "" }; break;
                case 2:
                    test = new[] { "头臀长", "", "", Convert.ToDateTime(createDate).ToString("yyyy/MM/dd"), testValue, "", "" }; break;
                case 3:
                    test = new[] { "双顶径", "", "", "", "", Convert.ToDateTime(createDate).ToString("yyyy/MM/dd"), testValue }; break;
                case 4:
                    test = new[] { "B超孕周", "", "", "", "", "", "" }; break;
            }
            return test;
        }
    }
}

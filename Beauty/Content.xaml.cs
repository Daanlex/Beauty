using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Beauty.DataAccess;
using Beauty.Model;
using Beauty.Tool;
using System.IO;
using System.Threading;

namespace Beauty
{
    /// <summary>
    /// Content.xaml 的交互逻辑
    /// </summary>
    public partial class Content : UserControl
    {
        StringBuilder errorLog = new StringBuilder();

        /// <summary>
        /// 构造函数
        /// </summary>
        public Content()
        {
            InitializeComponent();
            //InitTestData();
            InitData();
            InitEvent();
        }

        ///// <summary>
        ///// 添加测试数据
        ///// </summary>
        //private void InitTestData()
        //{
        //    tbSampleNo.Text = "34234";
        //    tbPatientName.Text = "张三";
        //    dtpBirthday.SelectedDate = DateTime.Now.AddYears(-25);
        //    tbHospName.Text = "河南中心医院";
        //    tbSampleNo.Text = "Y34343";
        //    tbTestName.Text = "唐氏筛查";
        //    tbTestNameAbb.Text = "skd";
        //    dtpCollectionDate.SelectedDate = DateTime.Now.AddDays(-1);
        //    tbWGHT.Text = "50";
        //    tbAFP.Text = "55.00";
        //    tbHCG.Text = "45.9";
        //    tbUE3.Text = "12.9";
        //    dtpLMPDate.SelectedDate = DateTime.Now.AddDays(-60);
        //    tbCYCL.Text = "29";
        //}

        /// <summary>
        /// 加载初始化数据
        /// </summary>
        private void InitData()
        {
            //隐藏不用的功能
            btnImportExcel.Visibility = System.Windows.Visibility.Collapsed;
            btnExportExcel.Visibility = System.Windows.Visibility.Collapsed;


            InitLeftData();
            InitRightData();

        }

        /// <summary>
        /// 初始化左侧数据
        /// </summary>
        private void InitLeftData()
        {
            //左侧数据
            dpkStartTime.SelectedDate = DateTime.Now.AddDays(-7.0);
            dpkEndTime.SelectedDate = DateTime.Now;
            cbIsImportPrisca.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化右侧数据
        /// </summary>
        private void InitRightData()
        {
            //右侧数据
            tbSampleNo.Text = "";
            tbPatientName.Text = "";
            tbAge.Text = "";
            tbHospitalizedNo.Text = "";
            tbBadNo.Text = "";
            tbPatientTel.Text = "";
            tbPatientAddress.Text = "";
            tbHospName.Text = "";
            cbDoctor.Text = "";
            tbCensorshipDepartments.Text = "";
            tbSpecimenNo.Text = "";
            tbTestName.Text = "";
            tbTestNameAbb.Text = "";
            tbWGHT.Text = "";
            tbAFP.Text = "";
            tbHCG.Text = "";
            tbUE3.Text = "";
            tbPHCG.Text = "";
            tbPPAP.Text = "";
            tbNT.Text = "";
            dtpGestationalWeekByBCDate.Text = "";
            tbCYCL.Text = "";
            tbCRLLength.Text = "";
            tbBPDLength.Text = "";
            tbKey.Text = "";

            List<UserSettingMd> defaultValue = GetDefaultValue();
            tbDetermination.Text = defaultValue.Where(o => o.DefaultValueNo == 4).ToList()[0].UpperValueOrDefaultValue; //"扫描";
            //cbDoctor.Text = defaultValue.Where(o => o.DefaultValueNo == 5).ToList()[0].UpperValueOrDefaultValue; // "张月红";
            tbCensorshipDepartments.Text = defaultValue.Where(o => o.DefaultValueNo == 6).ToList()[0].UpperValueOrDefaultValue;  //"妇产科";
            tbTestName.Text = defaultValue.Where(o => o.DefaultValueNo == 7).ToList()[0].UpperValueOrDefaultValue;  //"孕中期产前筛查";
            tbTestNameAbb.Text = defaultValue.Where(o => o.DefaultValueNo == 8).ToList()[0].UpperValueOrDefaultValue;  //"YZQCQSC"
            tbGestationalWeek.Text = "周天";
            tbGestationalWeekByBC.Text = "周天";
            cbGender.SelectedIndex = 0; //默认性别女
            tbFETU.Text = "1"; //胎儿数量
            cbSMOK.SelectedIndex = 0; //是否吸烟默认否
            cbIVF.SelectedIndex = 0; //是否体外受孕默认否
            cbDIAB.SelectedIndex = 0; //是否有糖尿病史默认否
            cbAPH.SelectedIndex = 0;//是否有不良孕产史默认否
            cbRAID.SelectedIndex = 3; //人种默认亚洲人
            chbLMP.IsChecked = true; //默认使用末次月经计算风险

        }

        /// <summary>
        /// 获取所有的默认值
        /// </summary>
        /// <returns></returns>
        private List<UserSettingMd> GetDefaultValue()
        {
            return new UserSettingDAL().GetUserSetting();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            FileMonitoring.UploadSearchAreaHandle += UploadSearchArea;
            Common.UploadSearchAreaHandle += UploadSearchArea;
            #region 左边事件

            #region "查询"
            btnSearch.Click += (s, e) => SearchData();
            #endregion

            #region 导入Excel
            //btnImportExcel.Click += (s, e) =>
            //{
            //    var fileDialog = new OpenFileDialog
            //        {
            //            Title = "选择Excel文件",
            //            Filter = "文件（.xls）|*.xls|文件（.xlsx）|*.xlsx|所有文件|*.*",
            //            Multiselect = false
            //        };
            //    var open = (bool)fileDialog.ShowDialog();
            //    if (open)
            //    {
            //        string fileName = fileDialog.FileName;
            //        DataSet ds = ExcelHelper.LoadDataFromExcel(fileName);

            //        if (ds != null && ds.Tables[0].Rows.Count > 0)
            //        {
            //            //把数据处理成对象
            //            List<Patient> list = DataHandle.SerializationData(ds);
            //            //把数据插入数据库
            //            bool flag = new PatientDAL().InsertPatientInfo(list);
            //            MessageBox.Show(flag ? "导入数据库成功" : "导入数据库失败");
            //            SearchData();
            //        }
            //    }
            //};
            #endregion

            #region 导出Excel
            //btnExportExcel.Click += (s, e) =>
            //{
            //    List<Patient> list1 = SearchDataExtend();
            //    new Thread(a =>
            //    {
            //        if (ExcelHelper.SaveDataTableToExcel(list1, ConfigString.importExcelPath))
            //            MessageBox.Show(@"保存成功,存放在" + ConfigString.importExcelPath);
            //        else
            //            MessageBox.Show("保存失败");
            //    }).Start();

            //};
            #endregion

            #region 全选

            selectAll_checkBox.Click += (s, e) =>
            {

                if (dgPatient.ItemsSource != null)
                {
                    var list = dgPatient.ItemsSource as List<Patient>;
                    var allChecd = selectAll_checkBox.IsChecked != null && (bool)selectAll_checkBox.IsChecked;
                    if (list != null)
                        Parallel.ForEach(list, obj =>
                        {
                            obj.CurrentSelected = allChecd ? 1 : 0;
                        });

                    dgPatient.ItemsSource = null;
                    dgPatient.ItemsSource = list;
                }
                //if (_isAllCheck == 0)
                //    _isAllCheck = 1;
                //else
                //    _isAllCheck = 0;
                //int dgPatientCount = (dgPatient.ItemsSource as List<Patient>).Count;
                //for (int i = 0; i < dgPatientCount; i++)
                //{
                //    DataGridRow rowContainer = (DataGridRow)dgPatient.ItemContainerGenerator.ContainerFromIndex(i);
                //    DataGridCellsPresenter presenter = Common.GetVisualChild<DataGridCellsPresenter>(rowContainer);

                //    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(0);
                //    CheckBox chb = Common.GetVisualChild<CheckBox>(cell);
                //    if (_isAllCheck==1)
                //        chb.IsChecked = true;
                //    else
                //        chb.IsChecked = false;

                //}
            };
            #endregion

            #region  导入Prisca
            btnImportPrisca.Click += (s, e) =>
                {
                    var keys = new List<long>();  //记录选中的主键
                    var list2 = new List<Patient>();
                    if (dgPatient.ItemsSource != null)
                    {
                        var list = dgPatient.ItemsSource as List<Patient>;

                        if (list != null)
                            foreach (Patient obj in from obj in list let allChecd = obj.CurrentSelected != 0 where allChecd select obj)
                            {
                                keys.Add(obj.Id);
                                list2.Add(obj);
                            }
                    }

                    int successCount = 0;
                    //查询出所有的满足条件的记录
                    if (keys.Count == 0)
                        MessageBox.Show("请先选择要导入Prisca的数据");
                    else
                    {
                        if (Check(list2))
                        {
                            bool flag = new PatientDAL().UpdateIsImportPrisca(keys);
                            if (flag)
                            {
                                if (list2.Count > 0)
                                {
                                    string successMsg = "";
                                    Parallel.ForEach(list2, patient =>
                                        {
                                            CalculateDate(patient);
                                            successCount += 1;
                                            successMsg = "成功导入Prisca" + successCount + "条数据";
                                        });
                                    
                                    MessageBox.Show(successMsg);
                                }
                            }
                            else
                                MessageBox.Show("导入失败");
                        }
                    }
                    SearchData();
                };
            #endregion

            #region 批量删除
            btnBatchDelete.Click += (s, e) =>
            {
                if (dgPatient.ItemsSource != null)
                {
                    var list = dgPatient.ItemsSource as List<Patient>;

                    var keys =
                        (from obj in list let allChecd = obj.CurrentSelected != 0 where allChecd select obj).Select(
                            obj => obj.Id).ToList(); //记录选中的主键

                    if (keys.Count == 0)
                        MessageBox.Show("请先选择要批量删除的数据");
                    else
                    {
                        MessageBoxResult confirmToDel = MessageBox.Show("确认要批量删除吗？", "提示", MessageBoxButton.YesNo,
                                                                        MessageBoxImage.Question);
                        if (confirmToDel == MessageBoxResult.Yes)
                        {
                            bool flag = new PatientDAL().DeleteDataByKey(keys);
                            if (flag)
                            {
                                MessageBox.Show("批量删除成功");
                                SearchData();
                            }
                            else
                                MessageBox.Show("批量删除失败");
                        }
                    }
                }
            };
            #endregion

            #region 选择每一行
            dgPatient.SelectionChanged += (s, e) =>
            {
                if (dgPatient.SelectedValue != null)
                {
                    var patient = dgPatient.SelectedValue as Patient;
                    DataPacking(patient);

                    if (patient != null)
                    {
                        MomRisk risk = patient.momrisk; //GetRisk(patient.SampleNo);
                        DataPackingRisk(risk);
                    }
                }
            };
            #endregion

            #endregion

            #region 右边事件
            //三种测试方式类型排斥
            //chbBPD, chbCRL, chbLMP, chbTrimester,chbGAWD
            chbGAWD.Checked += (s, e) => CheckBoxMutex(false, chbBPD, chbCRL, chbLMP, chbTrimester);
            chbBPD.Checked += (s, e) => CheckBoxMutex(false, chbGAWD,chbCRL, chbLMP, chbTrimester);
            chbCRL.Checked += (s, e) => CheckBoxMutex(false, chbGAWD,chbBPD, chbLMP, chbTrimester);
            chbLMP.Checked += (s, e) => CheckBoxMutex(false, chbGAWD, chbBPD,chbCRL, chbTrimester);
            chbTrimester.Checked += (s, e) => CheckBoxMutex(false, chbBPD, chbCRL, chbLMP, chbGAWD);
            
            //添加按钮事件
            btnInsertData.Click += (s, e) => InsertData();
            //修改数据
            btnUpdateData.Click += (s, e) => UpdateData();
            //删除数据
            btnDeleteData.Click += (s, e) => DeleteData();
            //打印
            btnPrint.Click += (s, e) => PrintData();
            //判断是否打开
            cbDoctor.MouseLeftButtonDown += (s, e) => cbDoctor.IsDropDownOpen = true;
            cbDoctor.GotKeyboardFocus += (s, e) => cbDoctor.IsDropDownOpen = true;
            cbDoctor.LostKeyboardFocus += (s, e) => cbDoctor.IsDropDownOpen = false;
            cbDoctor.GotFocus += (s, e) => cbDoctor.IsDropDownOpen = true;
            cbDoctor.LostFocus += (s, e) => cbDoctor.IsDropDownOpen = false;
            cbDoctor.ItemsSource = Common.GetDefaultValue(5, "Up").Split(',');
            //通过B超孕周，B超时间，和采样时间，计算出采样时的孕周
            btnCalculation.Click += (s, e) => CalculationSamplingGestationalAge();
            tbGestationalWeekByBC.LostFocus += (s, e) => CalculationSamplingGestationalAge();

            //通过出生日期，计算年龄
            dtpBirthday.LostFocus += (s, e) => CalculationAge();
            //通过末次月经和采样时间，计算采样时孕周
            dtpLMPDate.LostFocus += (s, e) => CalculationGestationalAge();

            #endregion

        }

        /// <summary>
        /// 置换CheckBox的选中状态
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="ch"></param>
        private void CheckBoxMutex(bool flag, params CheckBox[] ch)
        {
            foreach (CheckBox checkBox in ch)
                checkBox.IsChecked = flag;
        }

        /// <summary>
        /// 通过末次月经和采样时间，计算采样时孕周
        /// </summary>
        private void CalculationGestationalAge()
        {
            if (dtpLMPDate.SelectedDate != null & dtpCollectionDate.SelectedDate != null)
            {
                var dt = (DateTime)dtpLMPDate.SelectedDate;
                TimeSpan ts =((DateTime)dtpCollectionDate.SelectedDate).Subtract(dt);
                tbGestationalWeek.Text = ts.Days/7 + "周" + ts.Days%7 + "天";

            }
        }

        /// <summary>
        /// 通过出生日期，计算年龄，保留一位小数
        /// </summary>
        private void CalculationAge()
        {
            if (dtpBirthday.SelectedDate != null)
            {
                var dt = (DateTime)dtpBirthday.SelectedDate;
                TimeSpan ts = DateTime.Now.Subtract(dt);
                tbAge.Text=((ts.Days / 365)  + ((ts.Days % 365) / 365.0)).ToString("0.0");
            }
        }

        /// <summary>
        /// 通过B超孕周，B超时间，和采样时间，计算出采样时的孕周
        /// </summary>
        /// <returns></returns>
        private void CalculationSamplingGestationalAge()
        {
            if (!Common.IsWeekDay(tbGestationalWeekByBC.Text.Trim())
                | dtpCollectionDate.SelectedDate == null | dtpGestationalWeekByBCDate.SelectedDate == null)
                return;

            var intervalTime = dtpCollectionDate.SelectedDate - dtpGestationalWeekByBCDate.SelectedDate;
            int intervalDays = intervalTime.Value.Days;

            //截取出B超孕周的时间
            int bWeek = Convert.ToInt32(tbGestationalWeekByBC.Text.Trim().Substring(0, 2));
            int bdays = Convert.ToInt32(tbGestationalWeekByBC.Text.Trim().Substring(3, 1));

            //根据B超孕周算出B超孕周的总天数
            int bTotalDays = bWeek * 7 + bdays;

            //得出当前采样日期的天数
            int currentDays = bTotalDays + intervalDays;

            //根据当前采样日期的天数，算出当前采样日期的孕周,并赋值
            tbGestationalWeek.Text = currentDays / 7 + "周" + currentDays % 7 + "天";


        }

        /// <summary>
        /// 为风险数据赋值
        /// </summary>
        /// <param name="risk"></param>
        private void DataPackingRisk(MomRisk risk)
        {
            if (risk != null)
            {
                AFPCorrMom.Text = risk.AFPCorrMom.ToString();
                HCGCorrMom.Text = risk.HCGCorrMom.ToString("0.00");
                UE3CorrMom.Text = risk.UE3CorrMom.ToString();
                AgeRisk.Text = risk.AgeRisk.ToString("0.0");
                AgeDelivery.Text = risk.AgeDelivery.ToString("0.0");
                AR18.Text = risk.AR18.ToString();
                AR21.Text = risk.AR21.ToString();
                NDTRisk.Text = risk.NTDRisk.ToString();
                AFPMOM.Text = risk.AFPMom.ToString("0.00");
                PPAPRisk.Text = risk.PAPPCorrMoM.ToString("0.00");
                FBHCGRisk.Text = risk.FBCorrMoM.ToString("0.00");
                NTMOM.Text = risk.NTCorrMoM.ToString("0.00");
                EsBiochemicalMarkersRisk.Text = risk.EsBiochemicalMarkers.ToString("0.00");
                GAWD.Text =  risk.GAWD.ToString().IndexOf('.')<0 ?risk.GAWD.ToString()+"周": risk.GAWD.ToString().Replace(".", "周") + "天";
            }
            else
            {
                AFPCorrMom.Text = "";
                HCGCorrMom.Text = "";
                UE3CorrMom.Text = "";
                AgeRisk.Text = "";
                AgeDelivery.Text = "";
                AR18.Text = "";
                AR21.Text = "";
                NDTRisk.Text = "";
                AFPMOM.Text = "";
                PPAPRisk.Text = "";
                FBHCGRisk.Text = "";
            }


        }

        /// <summary>
        /// 公开调用的查询方法
        /// </summary>
        private void SearchData()
        {
            dgPatient.ItemsSource = SearchDataExtend();
            selectAll_checkBox.IsChecked = false;
        }

        /// <summary>
        /// 查询取数据
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Patient> SearchDataExtend()
        {
            string[] t = { 
                              Convert.ToDateTime(dpkStartTime.SelectedDate).ToString("yyyy-MM-dd"), 
                              Convert.ToDateTime(dpkEndTime.SelectedDate).ToString("yyyy-MM-dd")
                          };
            return new PatientDAL().GetPatientInfoAndMomRisk(t, searchPatientName.Text.Trim(), cbIsImportPrisca.SelectedIndex);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void InsertData()
        {
            Patient patient = DataPacking();
            var list = new List<Patient> { patient };
            if (Check(list))
            {
                bool flag = new PatientDAL().InsertPatientInfo(new List<Patient> { patient });
                if (flag)
                {
                    MessageBox.Show("添加成功");
                    SearchData();
                    InitRightData();
                }
                else
                    MessageBox.Show("添加失败");
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        private void UpdateData()
        {
            if (tbKey.Text.Trim() == "")
            {
                MessageBox.Show("请先选择在保存");
            }
            else
            {
                Patient patient = DataPacking();
                var list = new List<Patient> { patient };
                if (Check(list))
                {

                    bool flag = new PatientDAL().UpdatePatient(patient);
                    if (flag)
                    {
                        MessageBox.Show("修改成功");
                        SearchData();
                    }
                    else
                        MessageBox.Show("修改失败");
                }
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        private void DeleteData()
        {
            if (tbKey.Text.Trim() == "")
            {
                MessageBox.Show("请先选择在删除");
            }
            else
            {
                bool flag = new PatientDAL().DeleteDataByKey(new List<long> { Convert.ToInt64(tbKey.Text.Trim()) });
                if (flag)
                {
                    MessageBox.Show("删除成功");
                    SearchData();
                    InitRightData();
                }
                else
                    MessageBox.Show("删除失败");

            }
        }

        /// <summary>
        /// 打印报告
        /// </summary>
        private void PrintData()
        {
            if (tbKey.Text.Trim() == "")
            {
                MessageBox.Show("请先选择在打印");
            }
            else
            {
                if (tbSampleNo.Text.Trim() == "")
                    MessageBox.Show("没有样本编号");
                else
                {
                    Patient p = new PatientDAL().GetPatientInfoByKey(tbKey.Text.Trim())[0];
                    new Report(p).Show();
                }
                //PrintDialog dialog = new PrintDialog();
                //if (dialog.ShowDialog() == true)
                //{
                //    dialog.PrintVisual(PRINTAREA, "Print Test");
                //}

                //预览

                ////实例化打印预览 
                //System.Windows.Forms.PrintPreviewDialog ppd = new System.Windows.Forms.PrintPreviewDialog();

                ////打印文档
                //System.Drawing.Printing.PrintDocument docToPrint =
                //                 new System.Drawing.Printing.PrintDocument();//创建一个PrintDocument的实例 
                //docToPrint.PrintPage += (s, e) => { };

                ////打印预览的打印文档设置为被打印文档
                //ppd.Document = docToPrint;
                //ppd.ShowDialog();

            }
        }

        /// <summary>
        /// 数据装箱
        /// </summary>
        /// <returns></returns>
        private Patient DataPacking()
        {
            var p = new Patient();
            p.SampleNo = tbSampleNo.Text.Trim();
            p.PatientName = tbPatientName.Text.Trim();
            p.Birthday = dtpBirthday.SelectedDate != null ? (DateTime)dtpBirthday.SelectedDate : DateTime.Now;
            p.Gender = Convert.ToInt32(cbGender.SelectedIndex);
            p.Age = Common.IsNumber(tbAge.Text.Trim()) ? Convert.ToDouble(tbAge.Text.Trim()) : 0;
            p.HospitalizedNo = tbHospitalizedNo.Text.Trim();
            p.BadNo = tbBadNo.Text.Trim();
            p.PatientTel = tbPatientTel.Text.Trim();
            p.PatientAddress = tbPatientAddress.Text.Trim();
            p.HospName = tbHospName.Text.Trim();
            p.CensorshipDoctor = cbDoctor.Text.Trim();
            p.CensorshipDepartments = tbCensorshipDepartments.Text.Trim();
            p.SpecimenNo = tbSpecimenNo.Text.Trim();
            p.TestName = tbTestName.Text.Trim();
            p.TestNameAbb = tbTestNameAbb.Text.Trim();
            p.CollectionDate = dtpCollectionDate.SelectedDate != null ? (DateTime)dtpCollectionDate.SelectedDate : DateTime.Now;
            p.Weight = Common.IsNumber(tbWGHT.Text.Trim()) ? Convert.ToDouble(tbWGHT.Text.Trim()) : 0;
            p.FETU = Common.IsNumber(tbFETU.Text.Trim()) ? Convert.ToInt32(tbFETU.Text.Trim()) : 0;
            p.SMOK = Convert.ToInt32(cbSMOK.SelectedIndex);
            p.IVF = Convert.ToInt32(cbIVF.SelectedIndex);
            p.DIAB = Convert.ToInt32(cbDIAB.SelectedIndex);
            p.APH = Convert.ToInt32(cbAPH.SelectedIndex);
            p.RAID = Convert.ToInt32(cbRAID.SelectedIndex);
            p.AFP = Common.IsNumber(tbAFP.Text.Trim()) ? Convert.ToDouble(tbAFP.Text.Trim()) : 0;
            p.HCG = Common.IsNumber(tbHCG.Text.Trim()) ? Convert.ToDouble(tbHCG.Text.Trim()) : 0;
            p.UE3 = Common.IsNumber(tbUE3.Text.Trim()) ? Convert.ToDouble(tbUE3.Text.Trim()) : 0;
            p.PHCG = Common.IsNumber(tbPHCG.Text.Trim()) ? Convert.ToDouble(tbPHCG.Text.Trim()) : 0;
            p.PPAP = Common.IsNumber(tbPPAP.Text.Trim()) ? Convert.ToDouble(tbPPAP.Text.Trim()) : 0;
            p.NT = Common.IsNumber(tbNT.Text.Trim()) ? Convert.ToDouble(tbNT.Text.Trim()) : 0;
            p.GestationalWeek = tbGestationalWeek.Text.Trim();
            p.GestationalWeekByBC = tbGestationalWeekByBC.Text.Trim();
            if (dtpGestationalWeekByBCDate.SelectedDate != null) p.GestationalWeekByBCDate = (DateTime)dtpGestationalWeekByBCDate.SelectedDate;
            if (dtpLMPDate.SelectedDate != null) p.TestLMPDate = (DateTime)dtpLMPDate.SelectedDate;
            p.TestCYCL = tbCYCL.Text.Trim();
            if (dtpCRLDate.SelectedDate != null) p.TestCRLDate = (DateTime)dtpCRLDate.SelectedDate;
            p.TestCRLLength = tbCRLLength.Text.Trim();
            if (dtpBPDDate.SelectedDate != null) p.TestBPDDate = (DateTime)dtpBPDDate.SelectedDate;
            p.TestBPDLength = tbBPDLength.Text.Trim();
            p.Determination = tbDetermination.Text.Trim();
            p.Id = tbKey.Text.Trim() != "" ? Convert.ToInt32(tbKey.Text.Trim()) : 0;

            if (chbGAWD.IsChecked != null && (bool)chbGAWD.IsChecked)
                p.TestType = 4; //B超孕周
            else if(chbLMP.IsChecked != null && (bool)chbLMP.IsChecked)
                p.TestType = 1;
            else if (chbCRL.IsChecked != null && (bool)chbCRL.IsChecked)
                p.TestType = 2;
            else if (chbBPD.IsChecked != null && (bool)chbBPD.IsChecked)
                p.TestType = 3;
            else if (chbTrimester.IsChecked != null && (bool) chbTrimester.IsChecked)
                p.TestType = 5;
            p.CreateDate = DateTime.Now;

            return p;
        }

        /// <summary>
        /// 数据拆箱
        /// </summary>
        /// <returns></returns>
        private void DataPacking(Patient p)
        {
            tbSampleNo.Text = p.SampleNo;
            tbPatientName.Text = p.PatientName;
            if (p.Birthday != new DateTime()) dtpBirthday.SelectedDate = p.Birthday;
            else dtpBirthday.SelectedDate = null;
            cbGender.SelectedIndex = Convert.ToInt32(p.Gender);
            tbAge.Text = p.Age.ToString();
            tbHospitalizedNo.Text = p.HospitalizedNo;
            tbBadNo.Text = p.BadNo;
            tbPatientTel.Text = p.PatientTel;
            tbPatientAddress.Text = p.PatientAddress;
            tbHospName.Text = p.HospName;
            cbDoctor.Text = p.CensorshipDoctor;
            tbCensorshipDepartments.Text = p.CensorshipDepartments;
            tbSpecimenNo.Text = p.SpecimenNo;
            tbTestName.Text = p.TestName;
            tbTestNameAbb.Text = p.TestNameAbb;
            if (p.CollectionDate != new DateTime()) dtpCollectionDate.SelectedDate = p.CollectionDate;
            else dtpCollectionDate.SelectedDate = null;
            tbWGHT.Text = p.Weight.ToString();

            tbFETU.Text = p.FETU.ToString();
            cbSMOK.SelectedIndex = Convert.ToInt32(p.SMOK);
            cbIVF.SelectedIndex = Convert.ToInt32(p.IVF);
            cbDIAB.SelectedIndex = Convert.ToInt32(p.DIAB);
            cbAPH.SelectedIndex = Convert.ToInt32(p.APH);
            cbRAID.SelectedIndex = Convert.ToInt32(p.RAID);
            tbAFP.Text = p.AFP.ToString();
            tbHCG.Text = p.HCG.ToString();
            tbUE3.Text = p.UE3.ToString();
            tbPHCG.Text = p.PHCG.ToString();
            tbPPAP.Text = p.PPAP.ToString();
            tbNT.Text = p.NT.ToString();
            tbGestationalWeek.Text = p.GestationalWeek;
            tbGestationalWeekByBC.Text = p.GestationalWeekByBC;
            if (p.GestationalWeekByBCDate != new DateTime())
                dtpGestationalWeekByBCDate.SelectedDate = p.GestationalWeekByBCDate;
            else dtpGestationalWeekByBCDate.SelectedDate = null;
            tbDetermination.Text = p.Determination;
            tbKey.Text = p.Id.ToString();

            if (p.TestLMPDate != new DateTime()) dtpLMPDate.SelectedDate = p.TestLMPDate;
            else dtpLMPDate.SelectedDate = null;
            tbCYCL.Text = p.TestCYCL;
            if (p.TestCRLDate != new DateTime()) dtpCRLDate.SelectedDate = p.TestCRLDate;
            else dtpCRLDate.SelectedDate = null;
            tbCRLLength.Text = p.TestCRLLength;
            if (p.TestBPDDate != new DateTime()) dtpBPDDate.SelectedDate = p.TestBPDDate;
            else dtpBPDDate.SelectedDate = null;
            tbBPDLength.Text = p.TestBPDLength;

            if (p.TestType == 1)
                chbLMP.IsChecked = true;
            else if (p.TestType == 2)
                chbCRL.IsChecked = true;
            else if (p.TestType == 3)
                chbBPD.IsChecked = true;
            else if (p.TestType == 4)
                chbGAWD.IsChecked = true;
            else if (p.TestType == 5)
                chbTrimester.IsChecked = true;

        }

        /// <summary>
        /// 验证数据，保存到errorLog
        /// </summary>
        private bool Check(List<Patient> list)
        {
            /*
             * 验证条件没做
             * 体重 30-50
             * 胎儿数量 1-2
             * 月经周期 15-45天
             * 双顶径 26-52
             * 头臀长 38-84
             */
            bool flag = false;
            string patientName = "";
            if (list != null && list.Count > 0)
            {
                Parallel.ForEach(list, (patient, state) =>
                    {
                        if (string.IsNullOrWhiteSpace(errorLog.ToString()))
                            state.Break();

                        string weight = patient.Weight.ToString();
                        string fetu = patient.FETU.ToString();
                        string testValue = patient.TestValue;
                        string sampleNo = patient.SampleNo;
                        patientName = patient.PatientName;
                        #region 具体判断
                        //判断体重
                        if (sampleNo == "")
                            errorLog.AppendLine("请输入患者编号");
                        if (patient.PatientName == "")
                            errorLog.AppendLine("请输入患者姓名");


                        //判断体重
                        if (weight != "")
                        {
                            if (!Common.IsNumber(weight))
                                errorLog.AppendLine("您输入的体重不是数字，只能在30-150kg");
                            else
                            {
                                if (Convert.ToDouble(weight) < 30 || Convert.ToDouble(weight) > 150)
                                    errorLog.AppendLine("您输入的体重超过了范围，只能在30-150kg");
                            }
                        }
                        //判断胎儿数量
                        if (fetu != "")
                        {
                            if (!Common.IsNumber(fetu))
                                errorLog.AppendLine("您输入的胎儿数量不是数字，只能在1-2个");
                            else
                            {
                                if (Convert.ToInt32(fetu) < 1 || Convert.ToInt32(fetu) > 2)
                                    errorLog.AppendLine("您输入的胎儿超过了范围，只能在1-2个");
                            }
                        }

                        //判断月经周期
                        if (patient.TestType == 1 && patient.TestCYCL != "")
                        {
                            if (!Common.IsNumber(patient.TestCYCL))
                                errorLog.AppendLine("您输入的月经周期不是数字，只能在15-45天");
                            else
                            {
                                if (Convert.ToInt32(patient.TestCYCL) < 15 || Convert.ToInt32(patient.TestCYCL) > 45)
                                    errorLog.AppendLine("您输入的月经周期超过了范围，只能在15-45天");
                            }
                        }

                        //判断双顶径
                        if (patient.TestType == 2 && patient.TestCRLLength != "")
                        {
                            if (!Common.IsNumber(patient.TestCRLLength))
                                errorLog.AppendLine("您输入的头臀长不是数字，只能在38-84cm");
                            else
                            {
                                if (Convert.ToInt32(patient.TestCRLLength) < 38 || Convert.ToInt32(patient.TestCRLLength) > 84)
                                    errorLog.AppendLine("您输入的头臀长超过了范围，只能在38-84cm");
                            }
                        }

                        //判断头臀长
                        if (patient.TestType == 3 && patient.TestBPDLength != "")
                        {
                            if (!Common.IsNumber(patient.TestBPDLength))
                                errorLog.AppendLine("您输入的双顶径不是数字，只能在26-52cm");
                            else
                            {
                                if (Convert.ToInt32(patient.TestBPDLength) < 26 || Convert.ToInt32(patient.TestBPDLength) > 52)
                                    errorLog.AppendLine("您输入的双顶径超过了范围，只能在26-52cm");
                            }
                        }
                        if (patient.TestType == 4)
                        {
                            if (!Common.IsWeekDay(patient.GestationalWeek))
                                errorLog.AppendLine("您输入的采样时孕周，不符合规则，如  15周3天  ");

                            if (!Common.IsWeekDay(patient.GestationalWeekByBC))
                                errorLog.AppendLine("您输入的B超孕周，不符合规则，如  15周3天  ");
                        }
                        if (patient.TestType == 5)
                        {
                            if (patient.PPAP == 0 || patient.PHCG == 0 || patient.NT == 0)
                                errorLog.AppendLine("您要计算孕早期风险，请输入PPAP,游离HCG，NT");
                            if (patient.TestCRLDate == new DateTime() || patient.TestCRLLength == "")
                                errorLog.AppendLine("请输入头臀长测量日期和测量的长度");
                        }

                        if (patient.PPAP != 0 || patient.PHCG != 0 || patient.NT != 0)
                        { 
                            if(patient.TestType!=5)
                                errorLog.AppendLine("您输入的数据是计算孕早期风险的，请勾选计算孕早期风险选项");
                        }

                        #endregion
                    });


            }

            if (errorLog.Length > 0)
            {
                errorLog.Insert(0, patientName + " 的数据有问题" + Environment.NewLine);
                MessageBox.Show(errorLog.ToString());
                errorLog.Clear();
            }
            else
                flag = true;
            return flag;
        }

        /// <summary>
        /// 计算并且创建AST文档
        /// </summary>
        private void CalculateDate(Patient patient)
        {

            var sb = new StringBuilder();
            if (patient.TestType != 5)
            {
                #region 写入孕中期接口文件
                sb.AppendLine(@"H|^\&|||PRISCA||ORU|||DUBLIN TRAINING 1||P|A2.2|20120711154235|");
                sb.AppendLine("P|1|" + patient.SampleNo + "|||" + patient.HospName + "^" + patient.PatientName + "||" +
                              patient.Birthday.ToString("yyyyMMdd") + "|F");
                sb.AppendLine("OBR|1|" + patient.SpecimenNo + "||" + patient.TestNameAbb + "^" + patient.TestName +
                              "^L^|||20090901080808||");
                sb.AppendLine("OBX|1|DT|SDAT||" + patient.CollectionDate.ToString("yyyyMMdd") + "||||||F");
                sb.AppendLine("OBX|2|NM|AFP||" + patient.AFP + "||||||F");
                sb.AppendLine("OBX|3|NM|HCG||" + patient.HCG + "||||||F");
                sb.AppendLine("OBX|4|NM|UE3||" + patient.UE3 + "||||||F");
                sb.AppendLine("OBX|5|NM|WGHT||" + patient.Weight + "||||||F");
                sb.AppendLine("OBX|6|NM|FETU||" + patient.FETU + "||||||F");
                sb.AppendLine("OBX|7|ST|SMOK||" + patient.SMOK + "||||||F");
                sb.AppendLine("OBX|8|ST|IVF||" + patient.IVF + "||||||F");
                sb.AppendLine("OBX|9|ST|DIAB||" + patient.DIAB + "||||||F");
                sb.AppendLine("OBX|10|ST|RAID||" + patient.RAID + "||||||F");

                if (patient.TestType == 1) //如果选中末次月经
                {
                    sb.AppendLine("OBX|11|DT|LMP^Datum laatste menses^L^||" + patient.TestLMPDate.ToString("yyyyMMdd") +
                                  "||||||F");
                    if (patient.TestCYCL != "")
                        sb.AppendLine("OBX|12|NM|CYCL||" + patient.TestCYCL + "||||||F");
                }
                if (patient.TestType == 2) //如果选中头臀长
                {
                    sb.AppendLine("OBX|11|NM|CRL1^crl^L^||" + patient.TestCRLLength + "||||||F");
                    sb.AppendLine("OBX|12|DT|CRLD^CRLDate^L^||" + patient.TestCRLDate.ToString("yyyyMMdd") + "||||||F");
                }
                if (patient.TestType == 3) //如果选择双顶径
                {
                    sb.AppendLine("OBX|11|NM|B1IN ^crl^L^||" + patient.TestBPDLength + "||||||F");
                    sb.AppendLine("OBX|12|DT|BPDD^BPD Date^L^||" + patient.TestBPDDate.ToString("yyyyMMdd") + "||||||F");
                }
                if (patient.TestType == 4) //如果选择B超孕周，用采样时孕周计算
                {
                    //因为输入的是15周3天,所以需要处理成15.3
                    sb.AppendLine("OBX|15|NM|GAWD^Gestetional Age^L^||" + patient.GestationalWeekByBC.Substring(0, 2) +
                                  "." + patient.GestationalWeekByBC.Substring(3, 1) + "||||||F");
                    sb.AppendLine("OBX|07|DT|SCAN^Scandate^L^||" + patient.GestationalWeekByBCDate.ToString("yyyyMMdd") +
                                  "||||||F");
                }
                //如果选择的是B超孕周时间(15周2天)
                //OBX|15|NM|GAWD^Gestetional Age^L^||15.3||||||F
                //B超扫描时间
                //OBX|07|DT|SCAN^Scandate^L^||20050112||||||F
                sb.AppendLine("L|||1|15||");
                #endregion
            }
            else
            {
                #region 写入孕早期接口文件
                sb.AppendLine(@"H|^~\&|||38-1^||ORM|||0010^HCP000||A2.2|200501260950|");
                sb.AppendLine("P|1|" + patient.SampleNo + "|||" + patient.HospName + "^" + patient.PatientName + "||" +
                              patient.Birthday.ToString("yyyyMMdd") + "|F");
                sb.AppendLine("OBR|1|" + patient.SpecimenNo + "||" + patient.TestNameAbb + "^" + patient.TestName +
                              "^L^|||" + patient.CollectionDate.ToString("yyyyMMdd") + "||||R|| |200501260950|||||||||||||R|");

                sb.AppendLine("OBX|01|NM|PAPP^PAPP^L^||" + patient.PPAP + "||||||F");
                sb.AppendLine("OBX|02|NM|FB^FB^L^||"+patient.PHCG+"||||||F");
                sb.AppendLine("OBX|05|TX|RAID^Race_ID^L^||"+patient.RAID+"||||||F");
                sb.AppendLine("OBX|06|NM|WGHT^Gewicht^L^||" + patient.Weight + "||||||F");
                //sb.AppendLine("OBX|07|DT|SCAN^Scandate^L^||" + patient.GestationalWeekByBCDate + "||||||F");
                sb.AppendLine("OBX|08|NM|FETU^Aantal foetussen^L^||"+patient.FETU+"||||||F");
                sb.AppendLine("OBX|09|NM|NTM1^nt meting^L^||"+patient.NT+"||||||F");
                sb.AppendLine("OBX|10|NM|CRL1^crl^L^||" + patient.TestCRLLength + "||||||F");
                sb.AppendLine("OBX|11|DT|CRLD^CRLDate^L^||" + patient.TestCRLDate.ToString("yyyyMMdd") + "||||||F");
                sb.AppendLine("OBX|12|TX|DIAB^Diabetes^L^||" + patient.DIAB + "||||||F");
                sb.AppendLine("OBX|13|TX|SMOK^Smoking status^L^||"+patient.SMOK+"||||||F");
                sb.AppendLine("OBX|14|CE|ULTR^Sonographer^L^||djx||||||F");
                sb.AppendLine("OBX|18|DT|SDAT^Sample Date^L^||" + patient.CollectionDate.ToString("yyyyMMdd") + "||||||F");
                sb.AppendLine("OBX|19|NM|ULID^onbekend^L^||"+patient.SampleNo+"||||||F");
                sb.AppendLine("L|||1|15||");
                #endregion
            }

            try
            {
                WriterPrisca(sb.ToString(), patient.SampleNo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 写入Prisca接口文件
        /// </summary>
        /// <param name="content">需要写入的内容</param>
        /// <param name="sampleNo">样本号</param>
        private void WriterPrisca(string content,string sampleNo)
        {
            string path = ConfigString.priscaGo; //@"c:\LIS_DOWNLOAD";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string allfileName = path + @"\PRCIN" + sampleNo + ".AST";

            FileStream fs = File.Open(allfileName, FileMode.Create);

            var sw = new StreamWriter(fs, Encoding.Default);

            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 点击datagrid每一列的checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void select_checkBox_Click(object sender, RoutedEventArgs e)
        {
            var chb = (CheckBox)sender;
            if ((bool)chb.IsChecked)
                ((Patient)dgPatient.SelectedValue).CurrentSelected = 1;
            else
                ((Patient)dgPatient.SelectedValue).CurrentSelected = 0;
        }

        /// <summary>
        /// 事件触发更新具体执行方法
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void UploadSearchArea(object s, EventArgs e)
        {
            new Thread(() => Dispatcher.Invoke(new Action(SearchData))).Start();
        }

    }
}

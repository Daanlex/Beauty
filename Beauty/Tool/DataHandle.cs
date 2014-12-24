using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Beauty.Model;
using Beauty.DataAccess;

namespace Beauty.Tool
{
    /// <summary>
    /// 数据处理
    /// </summary>
    public class DataHandle
    {
        /// <summary>
        /// 把dataset数据转成对象
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static List<Patient> SerializationData(DataSet ds)
        {
            List<Patient> list = null;
            DataTable dt = ds.Tables[0];
            try
            {
                if (dt != null)
                {
                    int rowsCount = dt.Rows.Count;
                    if (rowsCount > 0)
                    {
                        list = new List<Patient>();
                        for (int i = 0; i < rowsCount; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            var p = new Patient();

                            if (dr[0] != null)
                                p.CollectionDate = Convert.ToDateTime(dr[0]); //采集时间
                            p.SpecimenNo = dr[1] == null ? "" : dr[1].ToString();
                            p.SampleNo = dr[2] == null ? "" : dr[2].ToString();
                            p.HospitalizedNo = dr[2] == null ? "" : dr[2].ToString();
                            p.PatientName = dr[3] == null ? "" : dr[3].ToString();
                            if (dr[4] != null && dr[4].ToString() != "")
                                p.Gender = GetGender(dr[4].ToString());
                            p.BadNo = dr[5] == null ? "" : dr[5].ToString();

                            if (dr[6] != null && dr[6].ToString() != "")
                                p.Age = Convert.ToInt64(dr[6]);
                            p.CensorshipDepartments = dr[8] == null ? "" : dr[8].ToString();

                            GetDoubleData(p, dr); //给AFP、HCG、UE3赋值
                            p.CensorshipDoctor = dr[24] == null ? "" : dr[24].ToString();


                            //下面是赋值给字段赋值空的字符串
                            p.HospName = "";
                            p.PatientAddress = "";
                            p.PatientTel = "";
                            
                            p.TestValue = "";
                            p.TestType = 4;
                            p.FETU = 1;
                            p.RAID = 3;
                            List<UserSettingMd> defaultValue = new UserSettingDAL().GetUserSetting() ;
                            p.Determination = defaultValue.Where(o => o.DefaultValueNo == 4).ToList()[0].UpperValueOrDefaultValue; //"扫描";
                            p.CensorshipDoctor = defaultValue.Where(o => o.DefaultValueNo == 5).ToList()[0].UpperValueOrDefaultValue; // "张月红";
                            p.CensorshipDepartments = defaultValue.Where(o => o.DefaultValueNo == 6).ToList()[0].UpperValueOrDefaultValue;  //"妇产科";
                            p.TestName = defaultValue.Where(o => o.DefaultValueNo == 7).ToList()[0].UpperValueOrDefaultValue;  //"孕中期产前筛查";
                            p.TestNameAbb = defaultValue.Where(o => o.DefaultValueNo == 8).ToList()[0].UpperValueOrDefaultValue;  //"YZQCQSC"
                            
                            p.GestationalWeek = "周天";
                            p.GestationalWeekByBC = "周天";
                            list.Add(p);

                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// 处理性别
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        private static int GetGender(string gender)
        {
            int result = -1;
            if (gender == "")
                result = 2;
            else if (gender == "女")
                result = 0;
            else
                result = 1;
            return result;
        }

        /// <summary>
        /// 给AFP、HCG、UE3赋值
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dr"></param>
        private static void GetDoubleData(Patient p, DataRow dr)
        {
            if (Common.IsNumber(FormatValue(dr[9].ToString())))
                p.AFP = Convert.ToDouble(FormatValue(dr[9].ToString()));
            if (Common.IsNumber(FormatValue(dr[10].ToString())))
                p.HCG = Convert.ToDouble(FormatValue(dr[10].ToString()));
            if (Common.IsNumber(FormatValue(dr[11].ToString())))
                p.UE3 = Convert.ToDouble(FormatValue(dr[11].ToString()));

        }

        /// <summary>
        /// 格式化大于小于号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string FormatValue(string str)
        {
            str = str.Replace(">", "");
            str = str.Replace("<", "");
            return str;
        }

        /// <summary>
        /// 取下拉框的字符串值
        /// </summary>
        /// <param name="gender">性别</param>
        /// <param name="sMOK">是否吸烟</param>
        /// <param name="iVF">是否体外受孕</param>
        /// <param name="dIAB">是否糖尿病史</param>
        /// <param name="aPH">是否不良孕产史</param>
        /// <param name="rAID">人种</param>
        /// <returns></returns>
        public static string GetEnum(long gender = -1, long sMOK = -1, long iVF = -1, long dIAB = -1, long aPH = -1, long rAID = -1)
        {
            string result = "";
            if (gender != -1)
            {
                switch (gender)
                {
                    case 0: result = "女"; break;
                    case 1: result = "男"; break;
                    case 2: result = "未知"; break;
                }
            }
            else if (sMOK != -1)
            {
                switch (sMOK)
                {
                    case 0: result = "否"; break;
                    case 1: result = "是"; break;
                    case 2: result = "未知"; break;
                }
            }
            else if (iVF != -1)
            {
                switch (iVF)
                {
                    case 0: result = "否"; break;
                    case 1: result = "是"; break;
                    case 2: result = "未知"; break;
                }
            }
            else if (dIAB != -1)
            {
                switch (dIAB)
                {
                    case 0: result = "否"; break;
                    case 1: result = "是"; break;
                    case 2: result = "未知"; break;
                }
            }
            else if (aPH != -1)
            {
                switch (aPH)
                {
                    case 0: result = "否"; break;
                    case 1: result = "是"; break;
                    case 2: result = "未知"; break;
                }
            }
            else if (rAID != -1)
            {
                switch (rAID)
                {
                    case 0: result = "非洲人"; break;
                    case 1: result = "高加索人"; break;
                    case 2: result = "未知"; break;
                    case 3: result = "亚洲人"; break;
                }
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Printing;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Configuration;
using Beauty.DataAccess;
using Beauty.Model;

namespace Beauty.Tool
{
    public class Common
    {
        /// <summary>
        /// 取datagrid单元格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < numVisuals; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;

                if (child == null)
                    child = GetVisualChild<T>(v);
                else
                    break;
            }
            return child;
        }
        /// <summary>        
        ///  判断输入的字符串只包含数字,可以匹配整数和浮点数        
        /// </summary>        
        public static bool IsNumber(string input)
        {
            const string pattern = "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$";
            var regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
        /// <summary>
        /// 孕周是否是12周3天格式
        /// </summary>
        public static bool IsWeekDay(string input)
        {
            const string pattern = @"^\d{1,2}周\d{1,1}天$";
            var regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 取机器CPU序号
        /// </summary>
        /// <returns></returns>
        public static string GetCpu()
        {
            var myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            return (from ManagementObject myObject in myCpuConnection select myObject.Properties["Processorid"].Value.ToString()).FirstOrDefault();
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        public static void CreateFile(string filePath)
        {
            if (!File.Exists(filePath))
                File.Create(filePath);
        }
        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="msg">内容</param>
        /// <param name="filePath">文件路径</param>
        public static void WriterErrorLog(string msg, string filePath)
        {
            var i = new FileInfo(filePath);
            long l = i.Length;
            if (l / 1024 > 1024)
                i.Create().Close();

            var f = i.AppendText();
            f.WriteLine("在" + DateTime.Now + msg);
            f.WriteLine();
            f.Flush();
            f.Close();

        }

        /// <summary>
        /// 传入字符串，根据特殊字符split，取出索引所在的值
        /// </summary>
        /// <param name="currentStr">传入字符串</param>
        /// <param name="peculiarStr">特殊字符</param>
        /// <param name="index">索引,如在第一个数组索引中</param>
        /// <returns></returns>
        public static double GetValueByString(string currentStr, string peculiarStr, int index)
        {
            try
            {
                string value = currentStr.Split(new[] { peculiarStr }, StringSplitOptions.RemoveEmptyEntries)[index];
                return (IsNumber(value) ? Convert.ToDouble(value) : 0);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取config里面的键值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetAppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }


        public static event EventHandler UploadSearchAreaHandle;
        /// <summary>
        /// 打印方法
        /// </summary>
        /// <param name="ui">打印对象</param>
        /// <param name="fileName">文件名</param>
        /// <param name="key">主键</param>
        public static void PrintReport(Visual ui, string fileName, Int64 key)
        {
            var dialog = new PrintDialog();
            try
            {
                if (GetAppSettings("IsMemberPrinter") == "0")
                {
                    if (dialog.ShowDialog() == true)
                    {
                        dialog.PrintVisual(ui, fileName);
                        new PatientDAL().AddPrintCount(key);//增加打印次数
                        UploadSearchAreaHandle(null, null);
                    }
                    
                }
                else if (GetAppSettings("IsMemberPrinter") == "1")
                {
                    if (dialog.PrintQueue == null)
                    {
                        if (dialog.ShowDialog() == true)
                        {
                            PrintTicket pt = dialog.PrintTicket;
                            PrintQueue pq = dialog.PrintQueue;
                        }
                    }
                    dialog.PrintVisual(ui, fileName);
                    new PatientDAL().AddPrintCount(key);//增加打印次数
                    UploadSearchAreaHandle(null, null);
                }
            }
            catch (Exception ex)
            {
                WriterErrorLog(ex.Message, ConfigString.dberrorlog);
                throw;
            }
        }

        /// <summary>
        /// 取默认值
        /// </summary>
        /// <param name="no">序号</param>
        /// <param name="condition">条件是UP，还是?</param>
        /// <returns></returns>
        public static string GetDefaultValue(int no, string condition)
        {
            List<UserSettingMd> defaultValue = new UserSettingDAL().GetUserSetting();
            if (condition == "Up")
                return defaultValue.Where(o => o.DefaultValueNo == no).ToList()[0].UpperValueOrDefaultValue;
            return defaultValue.Where(o => o.DefaultValueNo == no).ToList()[0].LowerValue;

        }
        /// <summary>
        /// 获取电子签名的所有文件
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSignature()
        {
            FileInfo[] files = new DirectoryInfo(ConfigString.signatureUrl).GetFiles();
            return files.Select(file => file.Name).ToList();
        }

    }


}

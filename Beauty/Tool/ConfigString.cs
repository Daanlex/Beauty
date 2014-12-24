using System;

namespace Beauty.Tool
{
    public class ConfigString
    {
        /// <summary>
        /// 数据库文件名
        /// </summary>
        public static string dbFileName = "Simpledb.db";
        /// <summary>
        /// 数据库完整路径，包含文件名
        /// </summary>
        public static string dbpath = AppDomain.CurrentDomain.BaseDirectory + dbFileName;

        /// <summary>
        /// 数据库备份的路径
        /// </summary>
        public static string backupdbpath = AppDomain.CurrentDomain.BaseDirectory + "backupdb";

        /// <summary>
        /// 错误日志路径
        /// </summary>
        public static string dberrorlog = AppDomain.CurrentDomain.BaseDirectory + "log.txt";

        /// <summary>
        ///  prisca返回的文件路径
        /// </summary>
        public static string priscaBack = @"C:\LIS_Upload";

        /// <summary>
        /// 写入prisca文件的路径
        /// </summary>
        public static string priscaGo = @"c:\LIS_DOWNLOAD";

        /// <summary>
        /// prisca返回的文件，在处理后备份路径
        /// </summary>
        public static string priscaBackFileBak = AppDomain.CurrentDomain.BaseDirectory + "LIS_Upload_bak";

        /// <summary>
        /// 软件试用到期时间
        /// </summary>
        public static DateTime softwareExpireDate = DateTime.Parse("2015-06-30");

        /// <summary>
        /// 导出文件路径
        /// </summary>
        public static string importExcelPath = AppDomain.CurrentDomain.BaseDirectory + "MyExcel.xls";

    }
}

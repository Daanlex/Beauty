using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Beauty.Model;
using Beauty.DataAccess;

namespace Beauty.Tool
{
    /// <summary>
    /// 文件监控
    /// </summary>
    public class FileMonitoring
    {
        public static event EventHandler UploadSearchAreaHandle;

        public FileMonitoring()
        {
            //文件监控，需要在构造函数中起线程来单独监视这个目录
            if (!Directory.Exists(ConfigString.priscaBack))
                Directory.CreateDirectory(ConfigString.priscaBack);

            new Thread(a =>
            {
                //如果已经存在了很多文件，需要先处理了这些文件
                string[] alreadyExistFiles = Directory.GetFileSystemEntries(ConfigString.priscaBack, "*AST");
                FileLogic(alreadyExistFiles);

                var fileWatcher = new FileSystemWatcher(ConfigString.priscaBack, "*AST")
                    {
                        NotifyFilter = NotifyFilters.FileName,
                        EnableRaisingEvents = true
                    };
                fileWatcher.Created += (s, e) =>
                {
                    string fileName = e.FullPath;
                    while (true)
                    {
                        try
                        {
                            var sr = new StreamReader(fileName);
                            sr.Close();
                            break;
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(100);
                        }
                    }
                    FileLogic(new[] { fileName });

                };

            }).Start();

        }

        /// <summary>
        /// 处理得到的文件数组
        /// 1.先循环解析出所有的的数据
        /// 2.把数据保存到数据库
        /// 3.把文件备份到其他地方
        /// </summary>
        /// <param name="alreadyExistFiles"></param>
        private void FileLogic(string[] alreadyExistFiles)
        {
            int count = alreadyExistFiles.Length;
            List<MomRisk> list = null;
            for (int i = 0; i < count; i++)
            {
                if (list == null)
                    list = Jx(alreadyExistFiles[i]);
                else
                    list.AddRange(Jx(alreadyExistFiles[i]));
            }

            if (list != null)
            {
                //得到所有要处理的数据后，就需要保存到数据库里去
                if (new MomRiskDAL().InsertUpdateMomRisk(list))
                {
                    if (!Directory.Exists(ConfigString.priscaBackFileBak))
                        Directory.CreateDirectory(ConfigString.priscaBackFileBak);
                    //同时文件处理完毕后，要转移到其他地方
                    for (int i = 0; i < count; i++)
                    {
                        string cr = alreadyExistFiles[i];
                        string newFile = ConfigString.priscaBackFileBak + cr.Substring(cr.LastIndexOf('\\'), cr.Length - cr.LastIndexOf('\\'));
                        if (File.Exists(newFile))
                            File.Delete(newFile);
                        File.Move(cr, newFile);
                    }


                    //保存数据成功后，触发列表更新事件
                    UploadSearchAreaHandle(null, null);

                }
            }
        }


        /// <summary>
        /// 根据文件路径解析从Prisca返回的数据
        /// </summary>
        /// <param name="fileName">文件路径</param>
        private List<MomRisk> Jx(string fileName)
        {
            var list = new List<MomRisk>();
            using (var r = new StreamReader(fileName))
            {
                MomRisk momRisk = null;
                string currentStr;
                while ((currentStr = r.ReadLine()) != null & currentStr != "")
                {
                    if (currentStr.StartsWith("OBR"))
                    {
                        if (momRisk != null)
                            list.Add(momRisk);
                        momRisk = new MomRisk();
                    }
                    else if (currentStr.IndexOf("AFPC^AFPCorrMoM", StringComparison.Ordinal) != -1)
                        momRisk.AFPCorrMom = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("AFPM^AFPMoM", StringComparison.Ordinal) != -1)
                        momRisk.AFPMom = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("AR18^AgeRiskT18", StringComparison.Ordinal) != -1)
                        momRisk.AgeRisk = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("AR21^AgeRisk", StringComparison.Ordinal) != -1)
                        momRisk.AgeRisk2 = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("BR18^BioChemRiskT18", StringComparison.Ordinal) != -1)
                        momRisk.AR18 = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("BR21^BioChemRisk", StringComparison.Ordinal) != -1)
                        momRisk.AR21 = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("HCCM^HCGCorrMoM", StringComparison.Ordinal) != -1)
                        momRisk.HCGCorrMom = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("HCMO^HCGMoM", StringComparison.Ordinal) != -1)
                        momRisk.HCGMom = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("NTDR^NTDRisk", StringComparison.Ordinal) != -1)
                        momRisk.NTDRisk = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("UE3C^UE3CorrMoM", StringComparison.Ordinal) != -1)
                        momRisk.UE3CorrMom = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("UEMO^UE3MoM", StringComparison.Ordinal) != -1)
                        momRisk.UE3Mom = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("ADLV^AgeDelivery", StringComparison.Ordinal) != -1)
                        momRisk.AgeDelivery = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("PCOD^PatientCode", StringComparison.Ordinal) != -1)
                        momRisk.SampleNo = currentStr.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    else if (currentStr.IndexOf("FBCO^FBCorrMoM", StringComparison.Ordinal) != -1)
                        momRisk.FBCorrMoM = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("PACM^PAPPCorrMoM", StringComparison.Ordinal) != -1)
                        momRisk.PAPPCorrMoM = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("NTCO^NTCorrMoM", StringComparison.Ordinal) != -1)
                        momRisk.NTCorrMoM = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("CR21^CombinedRisk", StringComparison.Ordinal) != -1)
                        momRisk.EsBiochemicalMarkers = Common.GetValueByString(currentStr, "||", 1);
                    else if (currentStr.IndexOf("GAWD^ChartCode", StringComparison.Ordinal) != -1)
                        momRisk.GAWD = Common.GetValueByString(currentStr, "||", 1);
                }
                if (currentStr == null)
                    list.Add(momRisk);
            }
            return list;
        }

    }
}

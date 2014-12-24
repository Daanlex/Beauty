using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Beauty.Model;
using Beauty.Tool;
using System.Data;

namespace Beauty.DataAccess
{
    public class Initializationdb
    {
        public void IsInit()
        {

            using (var con = new Connection().GetConnection)
            {
                try
                {
                    var query = con.Query("select * from InitState");

                    if (!query.Any())
                        Init();

                }
                catch (Exception)
                {
                    Init();
                }

            }
        }

        public bool Init()
        {
            bool flag;
            try
            {
                Common.CreateFile(ConfigString.dbpath);
                CreateTable();
                CreateDate();
                flag = true;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }




        /// <summary>
        /// 初始化表
        /// </summary>
        public void CreateTable()
        {
            using (var con = new Connection().GetConnection)
            {
                const string createUser = @"
                                    CREATE TABLE User (
                                    Id  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    UserName  TEXT(50) NOT NULL,
                                    PassWord  TEXT(64) NOT NULL
                                    );  ";
                const string createPatient = @"
                                    CREATE TABLE Patient (
                                    Id  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    SampleNo  TEXT(50)  NULL,
                                    PatientName  TEXT(64)  NULL,
                                    Birthday  timestamp(64)  NULL,
                                    Gender  INTEGER NULL,
                                    Age  REAL NULL,
                                    HospitalizedNo  TEXT(64)  NULL,
                                    BadNo  TEXT(64)  NULL,
                                    PatientTel  TEXT(64) NULL,
                                    PatientAddress  TEXT(200) NULL,
                                    HospName  TEXT(200)  NULL,
                                    CensorshipDoctor  TEXT(64) NULL,
                                    CensorshipDepartments  TEXT(64)  NULL,
                                    SpecimenNo  TEXT(50)  NULL,
                                    TestName  TEXT(100)  NULL,
                                    TestNameAbb  TEXT(50)  NULL,
                                    CollectionDate  timestamp(64)  NULL,
                                    Weight  REAL  NULL,
                                    FETU  INTEGER  NULL,
                                    SMOK  INTEGER  NULL,
                                    IVF  INTEGER  NULL,
                                    DIAB  INTEGER  NULL,
                                    APH  INTEGER  NULL,
                                    RAID  INTEGER  NULL,
                                    AFP  REAL  NULL,
                                    HCG  REAL  NULL,
                                    UE3  REAL  NULL,
                                    PHCG REAL NULL,
                                    PPAP REAL NULL,
                                    NT REAL NULL,
                                    GestationalWeek  TEXT(64)  NULL,
                                    GestationalWeekByBC  TEXT(64)  NULL,
                                    GestationalWeekByBCDate timestamp(64)  NULL,
                                    TestLMPDate timestamp(64)  NULL,
                                    TestCYCL TEXT(32)  NULL ,
                                    TestCRLDate timestamp(64)  NULL,
                                    TestCRLLength TEXT(32)  NULL ,
                                    TestBPDDate timestamp(64)  NULL,
                                    TestBPDLength TEXT(32)  NULL ,
                                    Determination  TEXT(200)  NULL,
                                    TestType  INTEGER  NULL,
                                    TestDate  timestamp(64)  NULL,
                                    TestValue  TEXT(100)  NULL,
                                    CreateDate timestamp(100) NOT NULL,
                                    IsImportPrisca INTEGER  NULL
                                    ); ";
                const string createInitState = @"CREATE TABLE InitState (
                                    InitState  INTEGER NOT NULL
                                    );  ";
                const string createRegistCode = @"CREATE TABLE RegistCode (
                                    Code TEXT(200) NULL,
                                    SurplusDays TEXT(200) NULL,
                                    FirstTime TEXT(200) NULL
                                    );  ";
                const string createMomRisk = @"CREATE TABLE MomRisk (
                                    Id  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    SampleNo  TEXT(50) NOT NULL,
                                    AFPCorrMom  REAL  NULL,
                                    AFPMom  REAL  NULL,
                                    HCGCorrMom  REAL  NULL,
                                    HCGMom  REAL  NULL,
                                    UE3CorrMom  REAL  NULL,
                                    UE3Mom  REAL  NULL,
                                    AgeRisk  REAL  NULL,
                                    AgeRisk2  REAL  NULL,
                                    AR18  REAL  NULL,
                                    AR21  REAL  NULL,
                                    NTDRisk  REAL  NULL,
                                    AgeDelivery REAL NULL,
                                    FBCorrMoM REAL NULL,
                                    PAPPCorrMoM REAL NULL,
                                    NTCorrMoM REAL NULL,
                                    EsBiochemicalMarkers REAL NULL,
                                    GAWD REAL NULL
                                    );  ";
                const string createUserSeetting = @"CREATE TABLE UserSetting (
                                    Id  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    DefaultValueNo INTEGER NOT NULL,
                                    DefaultValueName  TEXT(100) NOT NULL,
                                    UpperValueOrDefaultValue TEXT(1000) NULL,
                                    LowerValue TEXT(1000) NULL,
                                    Reserved TEXT(1000) NULL
                                    );  ";

                try
                {
                    con.Execute(createUser + createPatient + createInitState + createRegistCode + createMomRisk + createUserSeetting);
                }
                catch (Exception e)
                {
                    throw e;
                }

            }

        }

        /// <summary>
        /// 创建数据
        /// </summary>
        public void CreateDate()
        {
            var list = new List<UserSettingMd>
                { 
                new UserSettingMd {DefaultValueName = "AFP上下限",UpperValueOrDefaultValue="2.5",LowerValue="0.4",Reserved="",DefaultValueNo=1 },
                new UserSettingMd {DefaultValueName = "HCG上下限",UpperValueOrDefaultValue="2.5",LowerValue="0.4",Reserved="",DefaultValueNo=2 },
                new UserSettingMd {DefaultValueName = "UE3上下限",UpperValueOrDefaultValue="2.5",LowerValue="0.4",Reserved="",DefaultValueNo=3 },
                new UserSettingMd {DefaultValueName = "确认方法",UpperValueOrDefaultValue="扫描",LowerValue="",Reserved="",DefaultValueNo=4 },
                new UserSettingMd {DefaultValueName = "送检医生",UpperValueOrDefaultValue="张",LowerValue="",Reserved="",DefaultValueNo=5 },
                new UserSettingMd {DefaultValueName = "送检科室",UpperValueOrDefaultValue="妇产科",LowerValue="",Reserved="",DefaultValueNo=6 },
                new UserSettingMd {DefaultValueName = "组合项目名称",UpperValueOrDefaultValue="唐氏筛查",LowerValue="",Reserved="",DefaultValueNo=7 },
                new UserSettingMd {DefaultValueName = "组合项目名称缩写",UpperValueOrDefaultValue="TSSC",LowerValue="",Reserved="",DefaultValueNo=8 },
                //new UserSettingMd {DefaultValueName = "软件启动时自动开启Prisca Connect",UpperValueOrDefaultValue="",LowerValue="",Reserved="",DefaultValueNo=9 },
            };

            CreateUserSetting(list);
            CreateUser(new User { UserName = "superuser", PassWord = "superuser" });

            CreateInitState();
            CreateTryDays();

        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(User user)
        {

            using (var con = new Connection().GetConnection)
            {
                string a = user.UserName;
                string b = Encrypt.EncryptDES(user.PassWord);
                try
                {
                    con.Execute("insert into User (UserName,PassWord) values (@UserName,@PassWord);", new { UserName = a, PassWord = b });
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
        }

        /// <summary>
        /// 创建用户默认值数据
        /// </summary>
        /// <param name="list"></param>
        public void CreateUserSetting(List<UserSettingMd> list)
        {

            using (var con = new Connection().GetConnection)
            {
                list = Encrypt.TEncryptDES(list);
                IDbTransaction transaction = con.BeginTransaction();
                foreach (var item in list)
                {
                    con.Execute(@"insert into UserSetting (DefaultValueNo,DefaultValueName,UpperValueOrDefaultValue,LowerValue,Reserved) 
                                VALUES(@DefaultValueNo,@DefaultValueName,@UpperValueOrDefaultValue,@LowerValue,@Reserved)",
                                new
                                {
                                    DefaultValueNo = item.DefaultValueNo,
                                    DefaultValueName = item.DefaultValueName,
                                    UpperValueOrDefaultValue = item.UpperValueOrDefaultValue,
                                    LowerValue = item.LowerValue,
                                    Reserved = item.Reserved
                                }, transaction);
                }
                transaction.Commit();
            }
        }

        /// <summary>
        /// 创建初始化状态
        /// </summary>
        public void CreateInitState()
        {

            using (var con = new Connection().GetConnection)
            {
                try
                {
                    con.Execute("insert into InitState values (1);");
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
        }

        public void CreateTryDays()
        {
            using (var con = new Connection().GetConnection)
            {
                try
                {
                    string a = Encrypt.EncryptDES("30");
                    string b = Encrypt.EncryptDES(DateTime.Now.ToShortDateString());
                    con.Execute("insert into RegistCode  (SurplusDays,FirstTime) values (@SurplusDays,@FirstTime);", new { SurplusDays = a, FirstTime=b });
                }
                catch (Exception e)
                {
                    throw e;
                }
                
            }
            
            
        }


        public void Backdb()
        {
            try
            {
                if (File.Exists(ConfigString.dbpath))
                {
                    if (!Directory.Exists(ConfigString.backupdbpath))
                    {
                        Directory.CreateDirectory(ConfigString.backupdbpath);
                    }
                    //判断是否有同名的备份文件，如果没有就copy过去
                    string backName = ConfigString.dbFileName.Replace(".", DateTime.Now.ToString("yyyyMMdd") + ".");

                    if (!File.Exists(ConfigString.backupdbpath + @"\" + backName))
                        File.Copy(ConfigString.dbpath, ConfigString.backupdbpath + @"\" + backName);
                }

                //清理一个月以前的备份数据库
                DirectoryInfo directoryInfo = new DirectoryInfo(ConfigString.backupdbpath);
                FileInfo[] fileInfo = directoryInfo.GetFiles();
                DateTime crt = DateTime.Now.AddDays(-30);

                Parallel.ForEach(fileInfo, info =>
                    {
                        if (info.CreationTime.CompareTo(crt) < 0)
                            info.Delete();
                    });
                
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

    }
}

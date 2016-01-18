using System;
using System.Collections.Generic;
using System.Linq;
using Beauty.Model;
using Beauty.Tool;
using System.Data;

namespace Beauty.DataAccess
{
    /// <summary>
    /// 病人的操作类
    /// </summary>
    public class PatientDAL
    {
        /// <summary>
        /// 取一段时间内的病人数据
        /// </summary>
        /// <param name="date">时间数组</param>
        /// <param name="patientName">病人姓名</param>
        /// <param name="isImportPrisca"></param>
        /// <returns></returns>
        public List<Patient> GetPatientInfo(string[] date, string patientName, int isImportPrisca)
        {
            List<Patient> result;
            using (var con = new Connection().GetConnection)
            {
                string sql = @"select * from Patient 
                                            where CreateDate between @StartDate and @EndData ";
                if (patientName != "")
                    sql += "and PatientName=@PatientName ";
                switch (isImportPrisca)
                {
                    case 1:
                        sql += " and IsImportPrisca=0 ";
                        break;
                    case 2:
                        sql += " and IsImportPrisca>0 ";
                        break;
                }

                var query = con.Query<Patient>(sql, new
                {
                    StartDate = date[0] + " 00:00:00 ",
                    EndData = date[1] + " 23:59:59",
                    PatientName = Encrypt.EncryptDES(patientName)
                });

                result = query.ToList();

                result = Encrypt.TDecryptDES(result);
            }
            return result;
        }

        /// <summary>
        /// 取一段时间内的病人数据包括风险值
        /// </summary>
        /// <param name="date">时间数组</param>
        /// <param name="patientName">病人姓名</param>
        /// <param name="isImportPrisca"></param>
        /// <returns></returns>
        public List<Patient> GetPatientInfoAndMomRisk(string[] date, string patientName, int isImportPrisca)
        {
            List<Patient> result;
            using (var con = new Connection().GetConnection)
            {
                string sql = @"select * from Patient left join MomRisk
                                            on Patient.SampleNo = MomRisk.SampleNo
                                            where CreateDate between @StartDate and @EndData ";
                if (patientName != "")
                    sql += "and PatientName=@PatientName ";
                switch (isImportPrisca)
                {
                    case 1:
                        sql += " and IsImportPrisca=0 ";
                        break;
                    case 2:
                        sql += " and IsImportPrisca>0 ";
                        break;
                }
                sql += isImportPrisca == 0 ? "" : "and IsImportPrisca>=0";


                var query = con.Query<Patient, MomRisk, Patient>(sql, (a, b) => { a.momrisk = b; return a; }, new
                                                                                    {
                                                                                        StartDate = date[0] + " 00:00:00 ",
                                                                                        EndData = date[1] + " 23:59:59",
                                                                                        PatientName = Encrypt.EncryptDES(patientName)
                                                                                    });

                result = query.ToList();
                result = Encrypt.TDecryptDES(result);
            }
            return result;
        }

        /// <summary>
        /// 通过Key取数据
        /// </summary>
        /// <param name="keyStr"></param>
        /// <returns></returns>
        public List<Patient> GetPatientInfoByKey(string keyStr)
        {
            List<Patient> result;
            using (var con = new Connection().GetConnection)
            {
                string sql = @"select * from Patient  where Id =@key";


                var query = con.Query<Patient>(sql, new { key=keyStr});

                result = query.ToList();

                result = Encrypt.TDecryptDES(result);
            }
            return result;
        }
        /// <summary>
        /// 保存病人信息到数据库
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool InsertPatientInfo(List<Patient> list)
        {
            bool flag = false;
            if (list != null && list.Count > 0)
            {
                //list = Encrypt.TEncryptDES(list);
                using (var con = new Connection().GetConnection)
                {
                    IDbTransaction transaction = con.BeginTransaction();
                    foreach (var pa in list)
                    {
                        //先加密判断sampleno有没有存在
                        string encySampleNo =  Encrypt.EncryptDES(pa.SampleNo);
                        var query = con.Query<Patient>(@"select * from Patient where SampleNo = @SampleNo",
                            new {SampleNo = encySampleNo}, transaction).ToList();
                        if (query.Count > 0)
                            pa.SampleNo = pa.SampleNo + "X" + query.Count;
                        var patient = Encrypt.TEncryptDES(pa);
                        #region 插入数据库
                        con.Execute(@"insert into Patient (
                                        SampleNo,
                                        PatientName,
                                        Birthday,
                                        Gender,
                                        Age,
                                        HospitalizedNo,
                                        BadNo,
                                        PatientTel,
                                        PatientAddress,
                                        HospName,
                                        CensorshipDoctor,
                                        CensorshipDepartments,
                                        SpecimenNo,
                                        TestName,
                                        TestNameAbb,
                                        CollectionDate,
                                        Weight,
                                        FETU,
                                        SMOK,
                                        IVF,
                                        DIAB,
                                        APH,
                                        RAID,
                                        AFP,
                                        HCG,
                                        UE3,
                                        PHCG,
                                        PPAP,
                                        NT,
                                        GestationalWeek,
                                        GestationalWeekByBC,
                                        GestationalWeekByBCDate,
                                        TestLMPDate,
                                        TestCYCL,
                                        TestCRLDate,
                                        TestCRLLength,
                                        TestBPDDate,
                                        TestBPDLength,
                                        Determination,
                                        TestType,
                                        TestDate,
                                        TestValue,
                                        CreateDate,
                                        IsImportPrisca,
                                        OtherInformation,
                                        IsHaveNasalBone,
                                        Examinee,
                                        Audit,IsFn) 
                                        values (
                                        @SampleNo,
                                        @PatientName,
                                        @Birthday,
                                        @Gender,
                                        @Age,
                                        @HospitalizedNo,
                                        @BadNo,
                                        @PatientTel,
                                        @PatientAddress,
                                        @HospName,
                                        @CensorshipDoctor,
                                        @CensorshipDepartments,
                                        @SpecimenNo,
                                        @TestName,
                                        @TestNameAbb,
                                        @CollectionDate,
                                        @Weight,
                                        @FETU,
                                        @SMOK,
                                        @IVF,
                                        @DIAB,
                                        @APH,
                                        @RAID,
                                        @AFP,
                                        @HCG,
                                        @UE3,
                                        @PHCG,
                                        @PPAP,
                                        @NT,
                                        @GestationalWeek,
                                        @GestationalWeekByBC,
                                        @GestationalWeekByBCDate,
                                        @TestLMPDate,
                                        @TestCYCL,
                                        @TestCRLDate,
                                        @TestCRLLength,
                                        @TestBPDDate,
                                        @TestBPDLength,
                                        @Determination,
                                        @TestType,
                                        @TestDate,
                                        @TestValue,
                                        @CreateDate,
                                        @IsImportPrisca, 
                                        @OtherInformation,
                                        @IsHaveNasalBone,
                                        @Examinee,
                                        @Audit,@IsFn)", new
                            {
                                SampleNo = patient.SampleNo,
                                PatientName = patient.PatientName,
                                Birthday = patient.Birthday,
                                Gender = patient.Gender,
                                Age = patient.Age,
                                HospitalizedNo = patient.HospitalizedNo,
                                BadNo = patient.BadNo,
                                PatientTel = patient.PatientTel,
                                PatientAddress = patient.PatientAddress,
                                HospName = patient.HospName,
                                CensorshipDoctor = patient.CensorshipDoctor,
                                CensorshipDepartments = patient.CensorshipDepartments,
                                SpecimenNo = patient.SpecimenNo,
                                TestName = patient.TestName,
                                TestNameAbb = patient.TestNameAbb,
                                CollectionDate = patient.CollectionDate,
                                Weight = patient.Weight,
                                FETU = patient.FETU,
                                SMOK = patient.SMOK,
                                IVF = patient.IVF,
                                DIAB = patient.DIAB,
                                APH = patient.APH,
                                RAID = patient.RAID,
                                AFP = patient.AFP,
                                HCG = patient.HCG,
                                UE3 = patient.UE3,
                                PHCG = patient.PHCG,
                                PPAP = patient.PPAP,
                                NT = patient.NT,
                                GestationalWeek = patient.GestationalWeek,
                                GestationalWeekByBC = patient.GestationalWeekByBC,
                                GestationalWeekByBCDate = patient.GestationalWeekByBCDate,
                                TestLMPDate = patient.TestLMPDate,
                                TestCYCL = patient.TestCYCL,
                                TestCRLDate = patient.TestCRLDate,
                                TestCRLLength = patient.TestCRLLength,
                                TestBPDDate = patient.TestBPDDate,
                                TestBPDLength = patient.TestBPDLength,
                                Determination = patient.Determination,
                                TestType = patient.TestType,
                                TestDate = patient.TestDate,
                                TestValue = patient.TestValue,
                                CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                IsImportPrisca = 0,
                                OtherInformation = patient.OtherInformation,
                                IsHaveNasalBone  = patient.IsHaveNasalBone,
                                Examinee=patient.Examinee,
                                Audit=patient.Audit,
                                IsFn = patient.IsFn
                            }, transaction);

                        //判断是是否添加送检医生
                        AddDoctor(con, transaction, patient.CensorshipDoctor);

                        //判断是否添加送检单位
                        AddSendUnit(con, transaction, patient.CensorshipDepartments);
                        #endregion
                    }

                    
                    transaction.Commit();
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// 更改已经导入的状态
        /// </summary>
        /// <param name="keys">需要更改状态的key的集合</param>
        /// <returns></returns>
        public bool UpdateIsImportPrisca(List<long> keys)
        {
            bool flag;

            using (var con = new Connection().GetConnection)
            {
                IDbTransaction transaction = con.BeginTransaction();
                string sql = @"update Patient set IsImportPrisca =IsImportPrisca+1
                                            where Id in @key ";
                con.Execute(sql, new {key= keys.ToArray() }, transaction);
                transaction.Commit();
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 更改数据
        /// </summary>
        /// <param name="patient">Patient对象</param>
        /// <returns></returns>
        public bool UpdatePatient(Patient patient)
        {
            bool flag;
            patient = Encrypt.TEncryptDES(patient);
            using (var con = new Connection().GetConnection)
            {
                IDbTransaction transaction = con.BeginTransaction();

                #region 更新数据
                const string sql = @"update Patient set 
                                        SampleNo=@SampleNo,
                                        PatientName=@PatientName,
                                        Birthday=@Birthday,
                                        Gender=@Gender,
                                        Age=@Age,
                                        HospitalizedNo=@HospitalizedNo,
                                        BadNo=@BadNo,
                                        PatientTel=@PatientTel,
                                        PatientAddress=@PatientAddress,
                                        HospName=@HospName,
                                        CensorshipDoctor=@CensorshipDoctor,
                                        CensorshipDepartments = @CensorshipDepartments,
                                        SpecimenNo=@SpecimenNo,
                                        TestName=@TestName,
                                        TestNameAbb=@TestNameAbb,
                                        CollectionDate=@CollectionDate,
                                        Weight=@Weight,
                                        FETU=@FETU,
                                        SMOK=@SMOK,
                                        IVF=@IVF,
                                        DIAB=@DIAB,
                                        APH=@APH,
                                        RAID=@RAID,
                                        AFP=@AFP,
                                        HCG=@HCG,
                                        UE3=@UE3,
                                        PHCG=@PHCG,
                                        PPAP=@PPAP,
                                        NT=@NT,
                                        GestationalWeek=@GestationalWeek,
                                        GestationalWeekByBC=@GestationalWeekByBC,
                                        GestationalWeekByBCDate=@GestationalWeekByBCDate,
                                        TestLMPDate=@TestLMPDate,
                                        TestCYCL=@TestCYCL,
                                        TestCRLDate=@TestCRLDate,
                                        TestCRLLength=@TestCRLLength,
                                        TestBPDDate=@TestBPDDate,
                                        TestBPDLength=@TestBPDLength,
                                        Determination=@Determination,
                                        TestType=@TestType,
                                        TestDate=@TestDate,
                                        TestValue=@TestValue,
                                        Examinee=@Examinee,
                                        Audit=@Audit,
                                        IsFn  = @IsFn
                                        where Id = @Id";
                con.Execute(sql, new
                {
                    SampleNo = patient.SampleNo,
                    PatientName = patient.PatientName,
                    Birthday = patient.Birthday,
                    Gender = patient.Gender,
                    Age = patient.Age,
                    HospitalizedNo = patient.HospitalizedNo,
                    BadNo = patient.BadNo,
                    PatientTel = patient.PatientTel,
                    PatientAddress = patient.PatientAddress,
                    HospName = patient.HospName,
                    CensorshipDoctor = patient.CensorshipDoctor,
                    CensorshipDepartments = patient.CensorshipDepartments,
                    SpecimenNo = patient.SpecimenNo,
                    TestName = patient.TestName,
                    TestNameAbb = patient.TestNameAbb,
                    CollectionDate = patient.CollectionDate,
                    Weight = patient.Weight,
                    FETU = patient.FETU,
                    SMOK = patient.SMOK,
                    IVF = patient.IVF,
                    DIAB = patient.DIAB,
                    APH = patient.APH,
                    RAID = patient.RAID,
                    AFP = patient.AFP,
                    HCG = patient.HCG,
                    UE3 = patient.UE3,
                    PHCG = patient.PHCG,
                    PPAP = patient.PPAP,
                    NT = patient.NT,
                    GestationalWeek = patient.GestationalWeek,
                    GestationalWeekByBC = patient.GestationalWeekByBC,
                    GestationalWeekByBCDate = patient.GestationalWeekByBCDate,
                    TestLMPDate = patient.TestLMPDate,
                    TestCYCL = patient.TestCYCL,
                    TestCRLDate = patient.TestCRLDate,
                    TestCRLLength = patient.TestCRLLength,
                    TestBPDDate = patient.TestBPDDate,
                    TestBPDLength = patient.TestBPDLength,
                    Determination = patient.Determination,
                    TestType = patient.TestType,
                    TestDate = patient.TestDate,
                    TestValue = patient.TestValue,
                    Examinee = patient.Examinee,
                    Audit = patient.Audit,
                    IsFn = patient.IsFn,
                    Id = patient.Id
                }, transaction);
                #endregion
                //判断是是否添加送检医生
                AddDoctor(con, transaction, patient.CensorshipDoctor);
                //判断是否添加送检单位
                AddSendUnit(con, transaction, patient.CensorshipDepartments);
                transaction.Commit();
                flag = true;
            }
            return flag;
        }

        public void AddDoctor(IDbConnection con, IDbTransaction transaction, string doctorName)
        {
            //先读取
            string oldDoctorName = con.Query<string>(@"select UpperValueOrDefaultValue from UserSetting where DefaultValueNo=5",  transaction).First();
            oldDoctorName = Encrypt.DecryptDES(oldDoctorName);
            doctorName = Encrypt.DecryptDES(doctorName);
            if (oldDoctorName.IndexOf(doctorName, System.StringComparison.Ordinal) == -1)
            {
                doctorName = Encrypt.EncryptDES(oldDoctorName + "," + doctorName);
                con.Execute(
                    "update UserSetting set UpperValueOrDefaultValue=@doctorName where DefaultValueNo=5",
                    new { doctorName = doctorName }, transaction);
            }

        }

        public void AddSendUnit(IDbConnection con, IDbTransaction transaction, string sendUnit)
        {
            //先读取
            string oldSendUnit = con.Query<string>(@"select UpperValueOrDefaultValue from UserSetting where DefaultValueNo=6", transaction).First();
            oldSendUnit = Encrypt.DecryptDES(oldSendUnit);
            sendUnit = Encrypt.DecryptDES(sendUnit);
            if (oldSendUnit.IndexOf(sendUnit, System.StringComparison.Ordinal) == -1)
            {
                sendUnit = Encrypt.EncryptDES(oldSendUnit + "," + sendUnit);
                con.Execute(
                    "update UserSetting set UpperValueOrDefaultValue=@doctorName where DefaultValueNo=6",
                    new { doctorName = sendUnit }, transaction);
            }

        }

        /// <summary>
        /// 根据KEY删除数据
        /// </summary>
        /// <param name="keys">需要删除的主键的集合</param>
        /// <returns></returns>
        public bool DeleteDataByKey(List<long> keys)
        {
            bool flag;

            using (var con = new Connection().GetConnection)
            {
                IDbTransaction transaction = con.BeginTransaction();
                string sql = @"delete from Patient where Id in @key ";
                
                con.Execute(sql, new { key= keys.ToArray()}, transaction);
                transaction.Commit();
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 增加打印次数
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public bool AddPrintCount(Int64 key)
        {
            bool flag;

            using (var con = new Connection().GetConnection)
            {
                IDbTransaction transaction = con.BeginTransaction();
                string sql = @"select * from Patient where Id = @key ";
                var query = con.Query<Patient>(sql, new {key = key});
                if (query.First().PrintCount!=null)
                    sql = @"update Patient set PrintCount=PrintCount+1 where id=@key";
                else
                    sql = @"update Patient set PrintCount=1 where id=@key";

                con.Execute(sql, new { key = key }, transaction);
                transaction.Commit();
                flag = true;
            }
            return flag;
        }
    }
}

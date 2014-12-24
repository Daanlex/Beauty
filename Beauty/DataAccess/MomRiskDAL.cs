using System.Collections.Generic;
using System.Linq;
using Beauty.Model;
using Beauty.Tool;
using System.Data;

namespace Beauty.DataAccess
{
    /// <summary>
    /// 从prisca取回数据的处理
    /// </summary>
    public class MomRiskDAL
    {
        /// <summary>
        /// 更新或者增加从Prisca返回的数据
        /// </summary>
        /// <param name="list">更新或者增加的数据集</param>
        /// <returns></returns>
        public bool InsertUpdateMomRisk(List<MomRisk> list)
        {
            bool flag = false;
            if (list != null && list.Count > 0)
            {
                list = Encrypt.TEncryptDES(list);
                using (var con = new Connection().GetConnection)
                {
                    IDbTransaction transaction = con.BeginTransaction();
                    foreach (var mr in list)
                    {
                        if (IsExistSampleNoNo(Encrypt.DecryptDES(mr.SampleNo)))
                        {
                            //更新
                            con.Execute(@"update MomRisk set AFPCorrMom=@AFPCorrMom,AFPMom=@AFPMom,
                                        HCGCorrMom=@HCGCorrMom,HCGMom=@HCGMom,UE3CorrMom=@UE3CorrMom,
                                        UE3Mom=@UE3Mom,AgeRisk=@AgeRisk,AgeRisk2=@AgeRisk2,
                                        AR18=@AR18,AR21=@AR21,NTDRisk=@NTDRisk,AgeDelivery = @AgeDelivery,
                                        FBCorrMoM=@FBCorrMoM,PAPPCorrMoM=@PAPPCorrMoM,NTCorrMoM=@NTCorrMoM,
                                        EsBiochemicalMarkers = @EsBiochemicalMarkers,GAWD=@GAWD
                                        where SampleNo=@SampleNo",
                                        new
                                        {
                                            SampleNo = mr.SampleNo,
                                            AFPCorrMom = mr.AFPCorrMom,
                                            AFPMom = mr.AFPMom,
                                            HCGCorrMom = mr.HCGCorrMom,
                                            HCGMom = mr.HCGMom,
                                            UE3CorrMom = mr.UE3CorrMom,
                                            UE3Mom = mr.UE3Mom,
                                            AgeRisk = mr.AgeRisk,
                                            AgeRisk2 = mr.AgeRisk2,
                                            AR18 = mr.AR18,
                                            AR21 = mr.AR21,
                                            NTDRisk = mr.NTDRisk,
                                            AgeDelivery = mr.AgeDelivery,
                                            FBCorrMoM = mr.FBCorrMoM,
                                            PAPPCorrMoM = mr.PAPPCorrMoM,
                                            NTCorrMoM= mr.NTCorrMoM,
                                            EsBiochemicalMarkers = mr.EsBiochemicalMarkers,
                                            GAWD = mr.GAWD
                                        });
                        }
                        else
                        {
                            //新增
                            con.Execute(@"insert into MomRisk (SampleNo,AFPCorrMom,AFPMom,HCGCorrMom,HCGMom,UE3CorrMom,UE3Mom,
                                        AgeRisk,AgeRisk2,AR18,AR21,NTDRisk,AgeDelivery,FBCorrMoM,PAPPCorrMoM,NTCorrMoM,EsBiochemicalMarkers,GAWD) 
                                        values (@SampleNo,@AFPCorrMom,@AFPMom,@HCGCorrMom,@HCGMom,@UE3CorrMom,@UE3Mom,
                                        @AgeRisk,@AgeRisk2,@AR18,@AR21,@NTDRisk,@AgeDelivery,@FBCorrMoM,@PAPPCorrMoM,@NTCorrMoM,@EsBiochemicalMarkers,@GAWD)",
                                        new
                                        {
                                            SampleNo = mr.SampleNo,
                                            AFPCorrMom = mr.AFPCorrMom,
                                            AFPMom = mr.AFPMom,
                                            HCGCorrMom = mr.HCGCorrMom,
                                            HCGMom = mr.HCGMom,
                                            UE3CorrMom = mr.UE3CorrMom,
                                            UE3Mom = mr.UE3Mom,
                                            AgeRisk = mr.AgeRisk,
                                            AgeRisk2 = mr.AgeRisk2,
                                            AR18 = mr.AR18,
                                            AR21 = mr.AR21,
                                            NTDRisk = mr.NTDRisk,
                                            AgeDelivery = mr.AgeDelivery,
                                            FBCorrMoM = mr.FBCorrMoM,
                                            PAPPCorrMoM = mr.PAPPCorrMoM,
                                            NTCorrMoM= mr.NTCorrMoM,
                                            EsBiochemicalMarkers = mr.EsBiochemicalMarkers,
                                            GAWD = mr.GAWD
                                        }, transaction);

                        }
                    }
                    transaction.Commit();
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// 根据患者编号来取出从prisca返回的数据
        /// </summary>
        /// <param name="sampleNo">样本编号</param>
        /// <returns></returns>
        public MomRisk GetMomRiskBySampleNo(string sampleNo)
        {
            MomRisk result = null;
            using (var con = new Connection().GetConnection)
            {
                sampleNo = Encrypt.EncryptDES(sampleNo);
                var query = con.Query<MomRisk>("select * from MomRisk where SampleNo=@SampleNo", new { SampleNo = sampleNo });
                var momRisks = query as IList<MomRisk> ?? query.ToList();
                if (momRisks.Any())
                {
                    result = momRisks[0];
                    result = Encrypt.TDecryptDES(result);
                }
            }
            return result;
        }

        /// <summary>
        /// 是否存在患者编号
        /// </summary>
        /// <param name="sampleNo">样本编号</param>
        /// <returns>Bool</returns>
        public bool IsExistSampleNoNo(string sampleNo)
        {
            bool flag = false;
            using (var con = new Connection().GetConnection)
            {

                sampleNo = Encrypt.EncryptDES(sampleNo);
                var query = con.Query<MomRisk>("select * from MomRisk where SampleNo = @SampleNo", new { SampleNo = sampleNo });

                if (query != null)
                {
                    if(query.Count()==1)
                        flag = true;
                }
            }
            return flag;
        }
    }
}

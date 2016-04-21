using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using COC.Resource.Data;
using System.Configuration;
using COC.Resource;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace COC.Dal.Tables
{
    public class KKCocTrRankingDal
    {
        private SLMDBEntities slmdb = null;
        //private string SLMDBName = SLMConstant.SLMDBName;

        public KKCocTrRankingDal()
        {
            slmdb = new SLMDBEntities();
        }

        //Reference : SlmScr003Biz,RoleBiz
        public List<SearchRankingResult> SearchRankingData()
        {
            string sql = "";

            sql = @" SELECT [coc_RankingId],[coc_Name],[coc_Seq],[coc_SkipDate],[coc_IsAllDealer],[Is_Delete],isnull(sc.[slm_PositionName] + '-','') + isnull (sc.[slm_StaffNameTH],'')
            [coc_CreatedBy],[coc_CreatedDate],isnull (su.[slm_PositionName] + '-','') + isnull (su.[slm_StaffNameTH],'')  [coc_UpdatedBy],[coc_UpdatedDate] 
            from kkcoc_tr_ranking r
            left join kkslm_ms_staff sc on r.coc_CreatedBy=sc.slm_UserName
            left join kkslm_ms_staff su on r.coc_UpdatedBy=su.slm_UserName";
            sql += " where Is_Delete is null or Is_Delete = 0 ";
            sql += " ORDER BY coc_seq";

            return slmdb.ExecuteStoreQuery<SearchRankingResult>(sql).ToList();
        }

        public RankingData SearchRankingData(string rankingId)
        {

            string sql = @"select * from kkcoc_tr_ranking";
            string whr = @"coc_rankingId = '" + rankingId + "'";
            sql += (whr == "" ? "" : " WHERE " + whr);
            //sql += " ORDER BY seq";

            return slmdb.ExecuteStoreQuery<RankingData>(sql).FirstOrDefault();
        }

        public bool isLastRankingData(int? seq)
        {

            string sql = @"select max(coc_Seq) from kkcoc_tr_ranking";
            //sql += " ORDER BY seq";

            int? rankingdata = slmdb.ExecuteStoreQuery<int?>(sql).FirstOrDefault();

            if (rankingdata != null && rankingdata == seq)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int? maxSeqRankingData()
        {

            string sql = @"select max(coc_Seq) from kkcoc_tr_ranking";
            //sql += " ORDER BY seq";

            return slmdb.ExecuteStoreQuery<int?>(sql).FirstOrDefault();
        }


        public bool CheckNameRanking(string Name, int? rankingId)
        {
            try
            {
                if (slmdb.kkcoc_tr_ranking.Where(r => r.coc_Name == Name && r.coc_RankingId != rankingId).Count() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateSeq(List<RankingData> rankingdatas, string username)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (RankingData rankingdata in rankingdatas)
                    {
                        kkcoc_tr_ranking rd = slmdb.kkcoc_tr_ranking.Where(r => r.coc_RankingId == rankingdata.coc_RankingId).FirstOrDefault();
                        if (rd.coc_Seq != rankingdata.coc_Seq)
                        {
                            rd.coc_Seq = rankingdata.coc_Seq;
                            rd.coc_UpdatedBy = username;
                            rd.coc_UpdatedDate = DateTime.Now;
                            slmdb.SaveChanges();
                        }
                    }
                    //Save and discard changes

                    //if we get here things are looking good.
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddRanking(RankingData ranking, List<RankingCampaignData> rankingcampaigns, List<RankingDealerData> rankingdealers, string username)
        {
            try
            {
                int key = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    int? maxSeq = maxSeqRankingData();

                    if (maxSeq == null)
                    {
                        ranking.coc_Seq = 0;

                        kkcoc_tr_ranking rk = new kkcoc_tr_ranking();

                        rk.coc_Seq = 1;

                        rk.coc_Name = ranking.coc_Name;
                        rk.coc_SkipDate = ranking.coc_SkipDate;

                        rk.coc_UpdatedBy = username;
                        rk.coc_UpdatedDate = DateTime.Now;
                        rk.coc_CreatedBy = username;
                        rk.coc_CreatedDate = DateTime.Now;



                        slmdb.kkcoc_tr_ranking.AddObject(rk);
                        slmdb.SaveChanges();

                        key = rk.coc_RankingId;
                    }
                    else
                    {
                        kkcoc_tr_ranking maxRanking = slmdb.kkcoc_tr_ranking.Where(r => r.coc_Seq == maxSeq).FirstOrDefault();

                        maxRanking.coc_Seq = maxSeq + 1;
                        slmdb.SaveChanges();

                        //ranking.Seq = maxSeq;
                        kkcoc_tr_ranking rk = new kkcoc_tr_ranking();

                        rk.coc_Seq = maxSeq;

                        rk.coc_Name = ranking.coc_Name;
                        rk.coc_SkipDate = ranking.coc_SkipDate;

                        rk.coc_UpdatedBy = username;
                        rk.coc_UpdatedDate = DateTime.Now;
                        rk.coc_CreatedBy = username;
                        rk.coc_CreatedDate = DateTime.Now;

                        slmdb.kkcoc_tr_ranking.AddObject(rk);
                        slmdb.SaveChanges();
                        key = rk.coc_RankingId;

                        if (rankingcampaigns != null)
                        {
                            foreach (RankingCampaignData rcd in rankingcampaigns)
                            {
                                if (slmdb.kkcoc_tr_ranking_campaign.Where(r => r.coc_CampaignCode == rcd.coc_CampaignCode && r.coc_CampaignCode == rcd.coc_CampaignName).Count() == 0)
                                {
                                    kkcoc_tr_ranking_campaign rc = new kkcoc_tr_ranking_campaign();

                                    rc.coc_RankingId = rk.coc_RankingId;
                                    rc.coc_CampaignCode = rcd.coc_CampaignCode;
                                    rc.coc_CampaignName = rcd.coc_CampaignName;

                                    slmdb.kkcoc_tr_ranking_campaign.AddObject(rc);
                                    slmdb.SaveChanges();
                                }
                            }
                        }

                        //add rankingdealers
                        if (rankingdealers != null)
                        {
                            foreach (RankingDealerData rdd in rankingdealers)
                            {
                                if (slmdb.kkcoc_tr_ranking_dealer.Where(r => r.coc_DealerCode == rdd.coc_DealerCode && r.coc_DealerName == rdd.coc_DealerName).Count() == 0)
                                {
                                    kkcoc_tr_ranking_dealer rd = new kkcoc_tr_ranking_dealer();

                                    rd.coc_RankingId = rk.coc_RankingId;
                                    rd.coc_DealerCode = rdd.coc_DealerCode;
                                    rd.coc_DealerName = rdd.coc_DealerName;

                                    slmdb.kkcoc_tr_ranking_dealer.AddObject(rd);
                                    slmdb.SaveChanges();
                                }
                            }
                        }
                    }

                    //add rankingcampaigns


                    scope.Complete();
                    return key;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void EditRanking(RankingData ranking, List<RankingCampaignData> rankingcampaigns, List<RankingDealerData> rankingdealers, string username)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    kkcoc_tr_ranking rk = slmdb.kkcoc_tr_ranking.Where(r => r.coc_RankingId == ranking.coc_RankingId).FirstOrDefault();

                    rk.coc_Name = ranking.coc_Name;
                    rk.coc_SkipDate = ranking.coc_SkipDate;

                    rk.coc_UpdatedBy = username;
                    rk.coc_UpdatedDate = DateTime.Now;

                    //slmdb.Ranking.AddObject(rk);
                    slmdb.SaveChanges();


                    //delete old Ranking_Campaign
                    List<kkcoc_tr_ranking_campaign> oldRanking_Campaign = slmdb.kkcoc_tr_ranking_campaign.Where(r => r.coc_RankingId == ranking.coc_RankingId).ToList();

                    if (oldRanking_Campaign != null)
                    {
                        foreach (kkcoc_tr_ranking_campaign orc in oldRanking_Campaign)
                        {
                            slmdb.kkcoc_tr_ranking_campaign.DeleteObject(orc);
                            slmdb.SaveChanges();
                        }

                    }

                    //delete old Ranking_Dealer
                    List<kkcoc_tr_ranking_dealer> oldRanking_Dealer = slmdb.kkcoc_tr_ranking_dealer.Where(r => r.coc_RankingId == ranking.coc_RankingId).ToList();

                    if (oldRanking_Dealer != null)
                    {
                        foreach (kkcoc_tr_ranking_dealer ord in oldRanking_Dealer)
                        {
                            slmdb.kkcoc_tr_ranking_dealer.DeleteObject(ord);
                            slmdb.SaveChanges();
                        }

                    }


                    //add rankingcampaigns
                    if (rankingcampaigns != null)
                    {
                        foreach (RankingCampaignData rcd in rankingcampaigns)
                        {
                            if (slmdb.kkcoc_tr_ranking_campaign.Where(r => r.coc_CampaignCode == rcd.coc_CampaignCode && r.coc_CampaignCode == rcd.coc_CampaignName).Count() == 0)
                            {
                                kkcoc_tr_ranking_campaign rc = new kkcoc_tr_ranking_campaign();

                                rc.coc_RankingId = rk.coc_RankingId;
                                rc.coc_CampaignCode = rcd.coc_CampaignCode;
                                rc.coc_CampaignName = rcd.coc_CampaignName;

                                slmdb.kkcoc_tr_ranking_campaign.AddObject(rc);
                                slmdb.SaveChanges();
                            }
                        }
                    }

                    //add rankingdealers
                    if (rankingdealers != null)
                    {
                        foreach (RankingDealerData rdd in rankingdealers)
                        {
                            if (slmdb.kkcoc_tr_ranking_dealer.Where(r => r.coc_DealerCode == rdd.coc_DealerCode && r.coc_DealerName == rdd.coc_DealerName).Count() == 0)
                            {
                                kkcoc_tr_ranking_dealer rd = new kkcoc_tr_ranking_dealer();

                                rd.coc_RankingId = rk.coc_RankingId;
                                rd.coc_DealerCode = rdd.coc_DealerCode;
                                rd.coc_DealerName = rdd.coc_DealerName;

                                slmdb.kkcoc_tr_ranking_dealer.AddObject(rd);
                                slmdb.SaveChanges();
                            }
                        }
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteRanking(int? rankingId,string username)
        {

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    DeleteRankingCampaignData(rankingId);
                    DeleteRankingDealerData(rankingId);


                    kkcoc_tr_ranking rd = slmdb.kkcoc_tr_ranking.Where(r => r.coc_RankingId == rankingId).FirstOrDefault();
                    int? seq = rd.coc_Seq;

                    List<kkcoc_tr_ranking> rds = slmdb.kkcoc_tr_ranking.Where(r => r.coc_Seq > seq).ToList();

                    if (rds != null)
                    {
                        foreach (kkcoc_tr_ranking r in rds)
                        {
                            r.coc_Seq--;
                            slmdb.SaveChanges();
                        }
                    }

                    rd.Is_Delete = true;
                    rd.coc_UpdatedBy = username;
                    rd.coc_UpdatedDate = DateTime.Now;
                    //slmdb.kkcoc_tr_ranking.DeleteObject(rd);
                    slmdb.SaveChanges();

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void DeleteRankingCampaignData(int? rankingId)
        {
            try
            {
                var sortConList = slmdb.kkcoc_tr_ranking_campaign.Where(p => p.coc_RankingId == rankingId).ToList();
                foreach (kkcoc_tr_ranking_campaign obj in sortConList)
                {
                    slmdb.kkcoc_tr_ranking_campaign.DeleteObject(obj);
                }

                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteRankingDealerData(int? rankingId)
        {
            try
            {
                var sortConList = slmdb.kkcoc_tr_ranking_dealer.Where(p => p.coc_RankingId == rankingId).ToList();
                foreach (kkcoc_tr_ranking_dealer obj in sortConList)
                {
                    slmdb.kkcoc_tr_ranking_dealer.DeleteObject(obj);
                }

                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class KKCocTrRankingCampaignDal
    {
        private SLMDBEntities slmdb = null;
        private string _errorMessage = "";

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public KKCocTrRankingCampaignDal()
        {
            slmdb = new SLMDBEntities();
        }

        public bool ValidateData(int? rankingId, string CampaignCode)
        {
            try
            {
                //validate existing ในเบส
                var conn = slmdb.kkcoc_tr_ranking_campaign.Where(p => p.coc_RankingId == rankingId && p.coc_CampaignCode == CampaignCode).FirstOrDefault();

                if (conn != null)
                {
                    _errorMessage = "ไม่สามารถบันทึกข้อมูลได้ เนื่องจากข้อมูลซ้ำกับในระบบ";
                    return false;
                }
                else { return true; }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertData(int? RankingId, string campaignCode, string campaignName, string createByUsername)
        {
            try
            {
                kkcoc_tr_ranking_campaign rc = new kkcoc_tr_ranking_campaign();
                rc.coc_RankingId = RankingId;
                rc.coc_CampaignCode = campaignCode;
                rc.coc_CampaignName = campaignName;
                //rc.is_Deleted = false;

                DateTime createDate = DateTime.Now;
                //rc.CreatedBy = createByUsername;
                //rc.CreatedDate = createDate;
                //rc.UpdatedBy = createByUsername;
                //rc.UpdatedDate = createDate;

                slmdb.kkcoc_tr_ranking_campaign.AddObject(rc);
                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RankingCampaignData> GetRankingCampaignList(string rankingId)
        {
            try
            {
                string sql = @"SELECT *
                                FROM  kkcoc_tr_ranking_campaign rc 
                                WHERE rc.coc_RankingId = '" + rankingId + @"' 
                                ORDER BY rc.coc_CampaignCode, rc.coc_CampaignName ";

                return slmdb.ExecuteStoreQuery<RankingCampaignData>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CampaignMasterData> GetCampaignMasterList()
        {
            try
            {
                string sql = @"select * from kkslm_ms_campaign_master";
                string whr = @"Is_Deleted = 0";
                sql += (whr == "" ? "" : " WHERE " + whr);
                //sql += " ORDER BY seq";

                return slmdb.ExecuteStoreQuery<CampaignMasterData>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RankingCampaignData> SearchRankingCampaign(string rankingId)
        {
            try
            {
                //string sql = @"SELECT rc.RankingCampaignId AS RankingCampaignId, rc.RankingId AS RankingId, rc.CampaignCode AS CampaignCode, rc.CampaignName AS CampaignName
                //                FROM " + SLMConstant.SLMDBName + @".dbo.Ranking_Campaign rc
                //                WHERE rc.is_Deleted = 0 {0} 
                //                ORDER BY MP.sub_product_name, grade.slm_Customer_Grade_Name, ass.slm_Assign_Type_Name ";

                string sql = @"SELECT rc.coc_RankingCampaignId AS RankingCampaignId, rc.coc_RankingId AS RankingId, rc.coc_CampaignCode AS CampaignCode, rc.coc_CampaignName AS CampaignName
                                FROM kkcoc_tr_ranking_campaign rc
                                WHERE {0}
                                ORDER BY rc.coc_CampaignCode, rc.coc_CampaignName";

                string whr = "";
                whr += (rankingId == "" ? "" : (whr == "" ? "" : " AND ") + " rc.coc_RankingCampaignId = '" + rankingId + "' ");

                //whr = whr == "" ? "" : " AND " + whr;
                sql = string.Format(sql, whr);

                return slmdb.ExecuteStoreQuery<RankingCampaignData>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteData(decimal rankingCampaignId)
        {
            try
            {
                var sortConList = slmdb.kkcoc_tr_ranking_campaign.Where(p => p.coc_RankingCampaignId == rankingCampaignId).ToList();
                foreach (kkcoc_tr_ranking_campaign obj in sortConList)
                {
                    slmdb.kkcoc_tr_ranking_campaign.DeleteObject(obj);
                }

                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class KKCocTrRankingDealerDal
    {
        private SLMDBEntities slmdb = null;
        private string _errorMessage = "";

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public KKCocTrRankingDealerDal()
        {
            slmdb = new SLMDBEntities();
        }

        public bool ValidateData(int? rankingId, string DealerCode)
        {
            try
            {
                //validate existing ในเบส
                var conn = slmdb.kkcoc_tr_ranking_dealer.Where(p => p.coc_RankingId == rankingId && p.coc_DealerCode == DealerCode).FirstOrDefault();

                if (conn != null)
                {
                    _errorMessage = "ไม่สามารถบันทึกข้อมูลได้ เนื่องจากข้อมูลซ้ำกับในระบบ";
                    return false;
                }
                else { return true; }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertData(int? RankingId, string DealerCode, string DealerName, string createByUsername)
        {
            try
            {
                kkcoc_tr_ranking_dealer rc = new kkcoc_tr_ranking_dealer();
                rc.coc_RankingId = RankingId;
                rc.coc_DealerCode = DealerCode;
                rc.coc_DealerName = DealerName;
                //rc.is_Deleted = false;

                DateTime createDate = DateTime.Now;
                //rc.CreatedBy = createByUsername;
                //rc.CreatedDate = createDate;
                //rc.UpdatedBy = createByUsername;
                //rc.UpdatedDate = createDate;

                slmdb.kkcoc_tr_ranking_dealer.AddObject(rc);
                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RankingDealerData> GetRankingDealerList(string rankingId)
        {
            try
            {
                string sql = @"SELECT *
                                FROM kkcoc_tr_ranking_dealer rc 
                                WHERE rc.coc_RankingId = '" + rankingId + @"' 
                                ORDER BY rc.coc_DealerCode, rc.coc_DealerName ";

                return slmdb.ExecuteStoreQuery<RankingDealerData>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RankingDealerData> SearchRankingDealer(string rankingDealerId)
        {
            try
            {
                //string sql = @"SELECT rc.RankingDealerId AS RankingDealerId, rc.RankingId AS RankingId, rc.DealerCode AS DealerCode, rc.DealerName AS DealerName
                //                FROM " + SLMConstant.SLMDBName + @".dbo.Ranking_Dealer rc
                //                WHERE rc.is_Deleted = 0 {0} 
                //                ORDER BY MP.sub_product_name, grade.slm_Customer_Grade_Name, ass.slm_Assign_Type_Name ";

                string sql = @"SELECT rc.coc_RankingDealerId AS RankingDealerId, rc.coc_RankingId AS RankingId, rc.coc_DealerCode AS DealerCode, rc.coc_DealerName AS DealerName
                                FROM kkcoc_tr_ranking_dealer rc
                                WHERE {0}
                                ORDER BY rc.coc_DealerCode AS DealerCode, rc.coc_DealerName AS DealerName ";

                string whr = "";
                whr += (rankingDealerId == "" ? "" : (whr == "" ? "" : " AND ") + " rc.coc_RankingDealerId = '" + rankingDealerId + "' ");

                //whr = whr == "" ? "" : " AND " + whr;
                sql = string.Format(sql, whr);

                return slmdb.ExecuteStoreQuery<RankingDealerData>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteData(decimal rankingDealerId)
        {
            try
            {
                var sortConList = slmdb.kkcoc_tr_ranking_dealer.Where(p => p.coc_RankingDealerId == rankingDealerId).ToList();
                foreach (kkcoc_tr_ranking_dealer obj in sortConList)
                {
                    slmdb.kkcoc_tr_ranking_dealer.DeleteObject(obj);
                }

                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

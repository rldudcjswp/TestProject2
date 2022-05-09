using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// TODO: Use Enterprise Library Data Block
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using KBOLib.Util;

namespace KBOLib.Util
{
    public class WebCodeUtil
    {
        private static Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("kbo_ims");

        private static DataTable dtCode = null;         // 코드
        private static DataTable dtTeam = null;         // 팀
        private static DataTable dtStadium = null;      // 구장
        private static DataTable dtPlayer = null;       // 선수
        private static DataTable dtItem = null;         // 비고 아이템
        private static DataTable dtApprovalItemSection = null;   // 승인항목구분
        private static DataTable dtApprovalSection = null;   // 승인구분

        private const string KBO_IMS_CODE_LIST_KEY = "KBO_IMS_CODE_LIST_KEY";       // 코드
        private const string KBO_IMS_TEAM_LIST_KEY = "KBO_IMS_TEAM_LIST_KEY";       // 팀
        private const string KBO_IMS_STADIUM_LIST_KEY = "KBO_IMS_STADIUM_LIST_KEY"; // 구장
        private const string KBO_IMS_PLAYER_LIST_KEY = "KBO_IMS_PLAYER_LIST_KEY";   // 선수
        private const string APPROVAL_ITEM_NOTE_LIST_KEY = "APPROVAL_ITEM_NOTE_LIST_KEY";   // 비고
        private const string APPROVAL_ITEM_SECTION_LIST_KEY = "APPROVAL_ITEM_SECTION_LIST_KEY"; // 승인항목구분
        private const string APPROVAL_SECTION_LIST_KEY = "APPROVAL_SECTION_LIST_KEY";  // 승인구분 리스트

        #region DB or Cache -> Datset -> DataTable 초기화
        /// <summary>
        /// 코드(CODE) DataTable 초기화
        /// </summary>
        public static void SetCode()
        {
            DataSet dsCode = null;

            dsCode = KBOLib.WebUtil.CacheUtil.GetDataSet(KBO_IMS_CODE_LIST_KEY);

            if (dsCode == null)
            {
                dsCode = GetCodeBind();
                KBOLib.WebUtil.CacheUtil.SetDataSet(dsCode, KBO_IMS_CODE_LIST_KEY, 0, 1, 0, 0);
            }

            dtCode = dsCode.Tables[0];
        }

        /// <summary>
        /// 팀(TEAM) DataTable 초기화
        /// </summary>
        public static void SetTeam()
        {
            DataSet dsTeam = null;

            dsTeam = KBOLib.WebUtil.CacheUtil.GetDataSet(KBO_IMS_TEAM_LIST_KEY);

            if (dsTeam == null)
            {
                dsTeam = GetTeamBind();
                KBOLib.WebUtil.CacheUtil.SetDataSet(dsTeam, KBO_IMS_TEAM_LIST_KEY, 0, 1, 0, 0);
            }

            dtTeam = dsTeam.Tables[0];
        }

        /// <summary>
        /// 구장(STADIUM) DataTable 초기화
        /// </summary>
        public static void SetStadium()
        {
            DataSet dsStadium = null;

            dsStadium = KBOLib.WebUtil.CacheUtil.GetDataSet(KBO_IMS_STADIUM_LIST_KEY);

            if (dsStadium == null)
            {
                dsStadium = GetStadiumBind();
                KBOLib.WebUtil.CacheUtil.SetDataSet(dsStadium, KBO_IMS_STADIUM_LIST_KEY, 0, 1, 0, 0);
            }

            dtStadium = dsStadium.Tables[0];
        }

        /// <summary>
        /// 선수(PLAYER) DataTable 초기화
        /// </summary>
        public static void SetPlayer()
        {
            DataSet dsPlayer = null;

            dsPlayer = KBOLib.WebUtil.CacheUtil.GetDataSet(KBO_IMS_PLAYER_LIST_KEY);

            if (dsPlayer == null)
            {
                dsPlayer = GetPlayerBind();
                KBOLib.WebUtil.CacheUtil.SetDataSet(dsPlayer, KBO_IMS_PLAYER_LIST_KEY, 0, 1, 0, 0);
            }

            dtPlayer = dsPlayer.Tables[0];
        }

        /// <summary>
        /// 승인∙공시업무 - 비고 리스트 DataTable 초기화
        /// </summary>
        public static void SetApprovalItemNote()
        {
            DataSet dsItem = null;

            dsItem = KBOLib.WebUtil.CacheUtil.GetDataSet(APPROVAL_ITEM_NOTE_LIST_KEY);

            if (dsItem == null)
            {
                dsItem = GetApprovalItemBind();
                KBOLib.WebUtil.CacheUtil.SetDataSet(dsItem, APPROVAL_ITEM_NOTE_LIST_KEY, 0, 1, 0, 0);
            }

            dtItem = dsItem.Tables[0];
        }

        /// <summary>
        /// 승인∙공시업무 / 일반업무 - 비고 리스트 DataTable 초기화
        /// </summary>
        public static void SetApprovalItemSection()
        {
            DataSet dsApprovalItemSection = null;

            dsApprovalItemSection = KBOLib.WebUtil.CacheUtil.GetDataSet(APPROVAL_ITEM_SECTION_LIST_KEY);

            if (dsApprovalItemSection == null)
            {
                dsApprovalItemSection = GetApprovalItemSectionBind();
                KBOLib.WebUtil.CacheUtil.SetDataSet(dsApprovalItemSection, APPROVAL_ITEM_SECTION_LIST_KEY, 0, 1, 0, 0);
            }

            dtApprovalItemSection = dsApprovalItemSection.Tables[0];
        }

        /// <summary>
        /// 승인∙공시업무 - 승인구분 리스트 DataTable 초기화
        /// </summary>
        public static void SetApprovalSection()
        {
            DataSet dsApprovalSection = null;

            dsApprovalSection = KBOLib.WebUtil.CacheUtil.GetDataSet(APPROVAL_SECTION_LIST_KEY);

            if (dsApprovalSection == null)
            {
                dsApprovalSection = GetApprovalSectionBind();
                KBOLib.WebUtil.CacheUtil.SetDataSet(dsApprovalSection, APPROVAL_SECTION_LIST_KEY, 0, 1, 0, 0);
            }

            dtApprovalSection = dsApprovalSection.Tables[0];
        }

        #endregion

        #region Cache Data 삭제
        /// <summary>
        /// 선수(PLAYER) Cache 삭제
        /// </summary>
        public static void RemovePlayer()
        {
            KBOLib.WebUtil.CacheUtil.Remove(KBO_IMS_PLAYER_LIST_KEY);
        }
        #endregion

        #region 프로시져 호출 후 DataSet 저장
        /// <summary>
        /// Code DB -> DataSet
        /// </summary>
        /// <returns></returns>
        private static DataSet GetCodeBind()
        {
            DataSet dsCode = null;
            DbCommand cmd = db.GetStoredProcCommand("PROC_KBO_IMS_COMMON_CODE_S");

            try
            {
                dsCode = db.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
            }

            return dsCode;
        }

        /// <summary>
        /// TEAM DB -> DataSet
        /// </summary>
        /// <returns></returns>
        private static DataSet GetTeamBind()
        {
            DataSet dsTeam = null;
            DbCommand cmd = db.GetStoredProcCommand("PROC_KBO_IMS_COMMON_TEAM_S");

            try
            {
                dsTeam = db.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
            }

            return dsTeam;
        }

        /// <summary>
        /// STADIUM DB -> DataSet
        /// </summary>
        /// <returns></returns>
        private static DataSet GetStadiumBind()
        {
            DataSet dsStadium = null;
            DbCommand cmd = db.GetStoredProcCommand("PROC_KBO_IMS_COMMON_STADIUM_LIST_S");

            try
            {
                dsStadium = db.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
            }

            return dsStadium;
        }

        /// <summary>
        /// PLAYER DB -> DataSet
        /// </summary>
        /// <returns></returns>
        private static DataSet GetPlayerBind()
        {
            DataSet dsPlayer = null;
            DbCommand cmd = db.GetStoredProcCommand("PROC_KBO_IMS_COMMON_PLAYER_S");

            try
            {
                dsPlayer = db.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
            }

            return dsPlayer;
        }

        /// <summary>
        /// 승인∙공시업무 - 비고 리스트 -> DataSet
        /// </summary>
        /// <returns></returns>
        private static DataSet GetApprovalItemBind()
        {
            DataSet dsItem = null;
            DbCommand cmd = db.GetStoredProcCommand("PROC_KBO_IMS_COMMON_APPROVAL_ITEM_NOTE_LIST_S");

            try
            {
                dsItem = db.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
            }

            return dsItem;
        }

        /// <summary>
        /// 승인∙공시업무 / 일반업무 - 세부항목 리스트 -> DataSet
        /// </summary>
        /// <returns></returns>
        private static DataSet GetApprovalItemSectionBind()
        {
            DataSet dsApprovalItemSection = null;
            DbCommand cmd = db.GetStoredProcCommand("PROC_KBO_IMS_COMMON_APPROVAL_ITEM_SECTION_S");

            try
            {
                dsApprovalItemSection = db.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
            }

            return dsApprovalItemSection;
        }

        /// <summary>
        /// 승인∙공시업무 / 일반업무 - 승인항목 리스트 -> DataSet
        /// </summary>
        /// <returns></returns>
        private static DataSet GetApprovalSectionBind()
        {
            DataSet dsApprovalSection = null;
            DbCommand cmd = db.GetStoredProcCommand("PROC_KBO_IMS_COMMON_APPROVAL_SECTION_S");

            try
            {
                dsApprovalSection = db.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
            }

            return dsApprovalSection;
        }
        #endregion

        #region 조건에 맞는 DataRow 리턴
        /// <summary>
        /// Code List
        /// </summary>
        /// <param name="columnName">코드구분값</param>
        /// <returns></returns>
        public static DataRow[] GetCodeList(object columnName)
        {
            SetCode();

            return dtCode.Select(string.Format("COLUMN_NM='{0}'", columnName.ToString()), "ORDER_NO ASC, CODE_ID ASC");
        }

        /// <summary>
        /// Code List - 예외처리
        /// </summary>
        /// <param name="columnName">코드구분값</param>
        /// <returns></returns>
        public static DataRow[] GetCodeSectionList(object columnName, object section)
        {
            SetCode();

            string filter = string.Format("COLUMN_NM='{0}'", columnName.ToString());

            if (columnName.ToString() == "ALARM_CD")
            {
                switch (section.ToString())
                {
                    case "1": // NOTICE(공지사항), revenge 20180831 in 값 변경
                        filter += " AND CODE_ID IN (7)";
                        break;
                }
            }
            else if (columnName.ToString() == "ACTIVE_CD")
            {
                switch (section.ToString())
                {
                    case "1":   // 소속선수
                        filter += " AND CODE_ID IN (6,7,11,13,14,15,17,23,24,25)";
                        break;
                    case "2":   // 육성선수
                        filter += " AND CODE_ID IN (7,10,16,26)";
                        break;
                    case "3":   // 계약승인 등록
                        filter += " AND CODE_ID IN (18,19,6,10,13,16)";
                        break;
                }
            }
            else if (columnName.ToString() == "POS_CD")
            {
                switch (section.ToString())
                {
                    case "1":   // 선수
                        filter += " AND CODE_ID IN (3,4,5,6)";
                        break;
                    case "2":   // 코치
                        filter += " AND CODE_ID IN (1,2)";
                        break;
                }
                
            }

            DataRow[] drCode = GetDataRowChange(dtCode.Select(filter, "ORDER_NO ASC, CODE_ID ASC"), columnName, section);
            return drCode;
            //return dtCode.Select(filter, "ORDER_NO ASC, CODE_ID ASC");
        }

        /// <summary>
        /// code항목 변경
        /// </summary>
        /// <param name="drCode"></param>
        /// <param name="columnName"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        private static DataRow[] GetDataRowChange(DataRow[] drCode, object columnName, object section)
        {
            DataRow[] drData = drCode;

            if (columnName.ToString() == "ACTIVE_CD" && section.ToString() == "3")
            {
                int index = 0;

                foreach (DataRow item in drData)
                {
                    if (item["CODE_NM"].ToString() == "현역")
                    {
                        drData[index]["CODE_NM"] = "등록선수";
                    }
                    index++;
                }
            }

            return drData;
        }


        /// <summary>
        /// Team List
        /// </summary>
        /// <param name="leId">리그ID</param>
        /// <param name="seasonId">시즌ID</param>
        /// <returns></returns>
        public static DataRow[] GetTeamList(object leId, object seasonId)
        {
            SetTeam();

            DataRow[] drData = null;

            if (leId.ToString() == "3") // 1군+2군
            {
                if (seasonId.ToString() == "0") // 마지막 시즌
                {
                    seasonId = MathUtil.Max(dtTeam, "SEASON_ID", "");
                }

                string filter = string.Format("(LE_ID=1 AND SEASON_ID={0}) OR (LE_ID=2 AND SEASON_ID={0} AND T_ID IN ('PL', 'SM'))", seasonId);

                drData = dtTeam.Select(filter, "LE_ID, RANK_NO ASC");
            }
            else if(leId.ToString() == "4") // 팀명으로 그룹
            {
                string filter = string.Format("LE_ID = {0}", 1);

                DataView dvData = new DataView(dtTeam);
                dvData.RowFilter = filter;
                dvData.Sort = "T_ID, T_FULL_NM ASC";

                DataTable distinctTable = dvData.ToTable(true, "T_ID", "T_NM", "T_FULL_NM", "LE_ID");
                drData = distinctTable.Select();
            }
            else
            {
                string filter = string.Format("LE_ID={0} AND SEASON_ID={1}", leId, seasonId);

                drData = dtTeam.Select(filter, "RANK_NO ASC");
            }

            return drData;
        }

        /// <summary>
        /// stadium List
        /// </summary>
        /// <param name="seasonId">시즌ID</param>
        /// <returns></returns>
        public static DataRow[] GetStadiumList(object seasonId)
        {
            SetStadium();

            DataRow[] drData = null;

            string filter = string.Format("SEASON_ID={0}", seasonId);

            drData = dtStadium.Select(filter, "S_NM ASC");

            return drData;
        }

        /// <summary>
        /// player List
        /// </summary>
        /// <param name="leId">리그ID</param>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="teamId">팀ID</param>
        /// <param name="activeCd">현역</param>
        /// <param name="posCd">포지션</param>
        /// <returns></returns>
        public static DataRow[] GetPlayerList(object leId, object seasonId, object teamId, object activeCd, string posCd)
        {
            SetPlayer();

            DataRow[] drData = null;

            string addFilter = string.Format("AND ACTIVE_CD={0}", activeCd);

            string filter = string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}' AND POS_CD IN ({3}) {4}", leId, seasonId, teamId, posCd, (activeCd == "" ? "" : addFilter));

            drData = dtPlayer.Select(filter, "P_NM ASC");

            return drData;
        }

        /// <summary>
        /// 승인∙공시업무 - 비고 리스트
        /// </summary>
        /// <param name="scId">section</param>
        /// <returns></returns>
        public static DataRow[] GetApprovalItemNoteList(object scId)
        {
            SetApprovalItemNote();

            string filter = string.Empty;

            filter = string.Format("ITEM_SC={0}", scId);

            return dtItem.Select(filter, "NOTE_ID ASC");
        }

        /// <summary>
        /// 승인∙공시업무 / 일반업무 - 세무항목 리스트
        /// </summary>
        /// <param name="scId">section</param>
        /// <returns></returns>
        public static DataRow[] GetApprovalItemSectionList(object approvalSc)
        {
            SetApprovalItemSection();

            string filter = string.Format("APPROVAL_SC={0} AND DEL_CK = 0", approvalSc);

            return dtApprovalItemSection.Select(filter, "SC_ID ASC");
        }

        /// <summary>
        /// 승인∙공시업무 / 일반업무 - 구분항목 리스트
        /// </summary>
        /// <returns></returns>
        public static DataRow[] GetApprovalSectionList(object section)
        {
            SetApprovalSection();

            string filter = string.Empty;

            if (section.ToString() == "1")
            {
                filter = "SC_ID IN (1,2,3,4,5,7)";
            }
            else
            {
            }

            return dtApprovalSection.Select(filter, "SC_ID ASC");

            //return dtApprovalSection.Select();
        }
        #endregion

        #region source 항목 -> target 항목
        /// <summary>
        /// Code > 코드명칭
        /// </summary>
        /// <param name="id">코드구분값</param>
        /// <returns>코드명칭</returns>
        public static string GetCodeName(object columnName, object id)
        {
            SetCode();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtCode.Select(string.Format("COLUMN_NM='{0}' AND CODE_ID='{1}'", columnName, id));

                if (drData.Length > 0)
                {
                    result = drData[0]["CODE_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// Team 코드 > 팀명
        /// </summary>
        /// <param name="leId">리그ID</param>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="teamId">팀ID</param>
        /// <returns></returns>
        public static string GetTeamName(object leId, object seasonId, object teamId)
        {
            SetTeam();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}'", leId, seasonId, teamId));

                if (drData.Length > 0)
                {
                    result = drData[0]["T_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// Team 코드 > 마지막 시즌 팀명
        /// </summary>
        /// <param name="teamId">팀ID</param>
        /// <returns></returns>
        public static string GetTeamName(object teamId)
        {
            SetTeam();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtTeam.Select(string.Format("T_ID='{0}'", teamId), "SEASON_ID DESC, LE_ID ASC");

                if (drData.Length > 0)
                {
                    result = drData[0]["T_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// Team 코드 > 팀명
        /// </summary>
        /// <param name="leId">리그ID</param>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="teamId">팀ID</param>
        /// <returns></returns>
        public static string GetTeamFullName(object leId, object seasonId, object teamId)
        {
            SetTeam();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}'", leId, seasonId, teamId));

                if (drData.Length > 0)
                {
                    result = drData[0]["T_FULL_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// Team 코드 > 팀명(승인·공시전용)
        /// </summary>
        /// <param name="leId">리그ID</param>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="teamId">팀ID</param>
        /// <returns></returns>
        public static string GetTeamNameApproval(object leId, object seasonId, object teamId)
        {
            SetTeam();

            string result = string.Empty;

            // 승인·공시 팀명은 1군기준
            if (teamId.ToString() == "PL" || teamId.ToString() == "SM")
            {
                leId = "2";
            }
            else
            {
                leId = "1";
            }

            try
            {
                DataRow[] drData = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}'", leId, seasonId, teamId));

                if (drData.Length > 0)
                {
                    result = drData[0]["T_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// Team 코드 > 팀명(승인·공시 전용)
        /// </summary>
        /// <param name="leId">리그ID</param>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="teamId">팀ID</param>
        /// <returns></returns>
        public static string GetTeamFullNameApproval(object leId, object seasonId, object teamId)
        {
            SetTeam();

            string result = string.Empty;

            // 승인·공시 팀명은 1군기준
            if (teamId.ToString() == "PL" || teamId.ToString() == "SM")
            {
                leId = "2";
            }
            else
            {
                leId = "1";
            }

            try
            {
                DataRow[] drData = dtTeam.Select(string.Format("LE_ID={0} AND SEASON_ID={1} AND T_ID='{2}'", leId, seasonId, teamId));

                if (drData.Length > 0)
                {
                    result = drData[0]["T_FULL_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// stadium 코드 > stadium
        /// </summary>
        /// <param name="seasonId"></param>
        /// <param name="sId">팀ID</param>
        /// <returns></returns>
        public static string GetStadiumName(object seasonId, object sId)
        {
            SetStadium();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtStadium.Select(string.Format("SEASON_ID='{0}' AND S_ID='{1}'", seasonId, sId), "SEASON_ID DESC, S_NM ASC");

                if (drData.Length > 0)
                {
                    result = drData[0]["S_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// PLAYER 코드 > 선수명
        /// </summary>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="playerId">선수ID</param>
        /// <returns></returns>
        public static string GetPlayerName(object seasonId, object playerId)
        {
            SetPlayer();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtPlayer.Select(string.Format("SEASON_ID={0} AND P_ID={1}", seasonId, playerId));

                if (drData.Length > 0)
                {
                    result = drData[0]["P_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// PLAYER 코드 > 선수 1군 등번호
        /// </summary>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="playerId">선수ID</param>
        /// <returns></returns>
        public static string GetPlayerBackNo(object seasonId, object playerId)
        {
            SetPlayer();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtPlayer.Select(string.Format("SEASON_ID={0} AND P_ID={1}", seasonId, playerId));

                if (drData.Length > 0)
                {
                    result = drData[0]["BACK_NO"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// PLAYER 코드 > 선수 퓨처스 등번호
        /// </summary>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="playerId">선수ID</param>
        /// <returns></returns>
        public static string GetPlayerFuturesBackNo(object seasonId, object playerId)
        {
            SetPlayer();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtPlayer.Select(string.Format("SEASON_ID={0} AND P_ID={1}", seasonId, playerId));

                if (drData.Length > 0)
                {
                    result = drData[0]["MINOR_BACK_NO"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// 엔트리 등록/말소 구분
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>구분</returns>
        public static string GetEntryGubunName(object code)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("1", "제출");
            nvcTb.Add("2", "수정");
            nvcTb.Add("3", "제출");
            nvcTb.Add("4", "수정");
            nvcTb.Add("5", "승인");
            nvcTb.Add("8", "미제출");

            return nvcTb.Get(code.ToString());
        }

        /// <summary>
        /// 리그 코드 -> 리그명
        /// </summary>
        /// <param name="leId">코드값</param>
        /// <returns>구분</returns>
        public static string GetLeagueName(object leId)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("1", "KBO 리그");
            nvcTb.Add("2", "퓨처스리그");

            return nvcTb.Get(leId.ToString());
        }

        /// <summary>
        /// 로그 구분 코드 -> 코드명
        /// </summary>
        /// <param name="scId">코드값</param>
        /// <returns>구분</returns>
        public static string GetLogSectionName(object scId)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("1", "웹");
            nvcTb.Add("2", "모바일");

            return nvcTb.Get(scId.ToString());
        }

        /// <summary>
        /// 로그 구분 코드 -> 코드명
        /// </summary>
        /// <param name="scId">코드값</param>
        /// <returns>구분</returns>
        public static string GetGeneralStateName(object stateSc)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("0", "제출");
            nvcTb.Add("1", "반송");
            nvcTb.Add("2", "승인완료");

            return nvcTb.Get(stateSc.ToString());
        }

        /// <summary>
        /// 투수 투구방향 한글명
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetPitDirec(string code)
        {
            NameValueCollection nvcPitDirec = new NameValueCollection();

            nvcPitDirec.Add("3", "우");
            nvcPitDirec.Add("4", "좌");
            nvcPitDirec.Add("5", "양");

            return nvcPitDirec.Get(code);
        }

        /// <summary>
        /// 투구폼 한글명
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetPitForm(string code)
        {
            NameValueCollection nvcPitForm = new NameValueCollection();

            nvcPitForm.Add("6", "투");   // 오버
            nvcPitForm.Add("7", "언");   // 언더
            nvcPitForm.Add("8", "사");   // 사이드

            return nvcPitForm.Get(code);
        }

        /// <summary>
        /// 타격방향 한글명
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>한글명</returns>
        public static string GetHitType(string code)
        {
            NameValueCollection nvcHitType = new NameValueCollection();

            nvcHitType.Add("9", "우타");
            nvcHitType.Add("10", "좌타");
            nvcHitType.Add("11", "양타");

            return nvcHitType.Get(code);
        }

        /// <summary>
        /// 승인∙공시업무 - 비고 리스트
        /// </summary>
        /// <param name="scId">section</param>
        /// <param name="noteId">NoteID</param>
        /// <returns></returns>
        public static string GetApprovalItemNoteName(object scId, object noteId)
        {
            SetApprovalItemNote();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtItem.Select(string.Format("ITEM_SC={0} AND NOTE_ID={1}", scId, noteId));

                if (drData.Length > 0)
                {
                    result = drData[0]["NOTE_IF"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// 승인∙공시업무 / 일반업무 - 세무항목 리스트
        /// </summary>
        /// <param name="scId">section</param>
        /// <param name="noteId">NoteID</param>
        /// <returns></returns>
        public static string GetApprovalItemSectionName(object scId)
        {
            SetApprovalItemSection();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtApprovalItemSection.Select(string.Format("SC_ID={0}", scId));

                if (drData.Length > 0)
                {
                    result = drData[0]["SC_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// 승인∙공시업무 / 일반업무 - 승인구분 리스트
        /// </summary>
        /// <param name="scId">section</param>
        /// <returns></returns>
        public static string GetApprovalSectionName(object scId)
        {
            SetApprovalSection();

            string result = string.Empty;

            try
            {
                //DataRow[] drData = dtApprovalSection.Select();
                DataRow[] drData = dtApprovalSection.Select(string.Format("SC_ID={0}", scId));
                if (drData.Length > 0)
                {
                    result = drData[0]["SC_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }
        #endregion

        #region 알람코드
        /// <summary>
        /// 알람코드
        /// </summary>
        /// <param name="code">코드값</param>
        /// <returns>구분</returns>
        public static string GetAlaramCode(object code)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("1", "100");  // 경기개시시간
            nvcTb.Add("2", "101");  // 경기장소
            nvcTb.Add("3", "102");  // 선수 개명
            nvcTb.Add("4", "103");  // 배번변경
            nvcTb.Add("5", "104");  // 선수 추가(육성 말소)
            nvcTb.Add("6", "105");  // 선수 추가(군보류해제)
            nvcTb.Add("7", "106");  // 선수 추가(외국인)
            nvcTb.Add("8", "107");  // 선수 추가(신규)
            nvcTb.Add("9", "108");  // 군보류
            nvcTb.Add("10", "109");  // 임의탈퇴
            nvcTb.Add("11", "110");  // 임의탈퇴 복귀
            nvcTb.Add("12", "111");  // 임의탈퇴 공시 말소
            nvcTb.Add("13", "112");  // 자유계약
            nvcTb.Add("14", "113");  // 자유계약(웨이버)
            nvcTb.Add("15", "114");  // 웨이버
            nvcTb.Add("16", "115");  // 선수 계약 양도(웨이버)
            nvcTb.Add("17", "116");  // 선수 계약 양도
            nvcTb.Add("18", "117");  // 선수 개명
            nvcTb.Add("19", "118");  // 배번 변경
            nvcTb.Add("20", "119");  // 선수 추가
            nvcTb.Add("21", "120");  // 군보류
            nvcTb.Add("22", "121");  // 선수말소
            nvcTb.Add("23", "122");  // 개명
            nvcTb.Add("24", "123");  // 배번 변경
            nvcTb.Add("25", "124");  // 추가 계약
            nvcTb.Add("26", "125");  // 감독 대행 선임
            nvcTb.Add("27", "126");  // 감독 복귀
            nvcTb.Add("28", "127");  // 신규승인신청

            nvcTb.Add("29", "200");  // 메리트 운영 계획
            nvcTb.Add("30", "201");  // 신분조회
            nvcTb.Add("31", "202");  // 등록확인서
            nvcTb.Add("32", "203");  // 경찰야구단 지원 선수
            nvcTb.Add("33", "204");  // 상무야구단 지원 선수
            nvcTb.Add("34", "205");  // 외국인선수 및 코치 사증 발급 추천
            nvcTb.Add("35", "206");  // 스페셜 유니폼

            nvcTb.Add("36", "128");  // 개막전 등록명단
            nvcTb.Add("37", "129");  // 자격 선수 공시
            nvcTb.Add("38", "130");  // 권리 행사
            nvcTb.Add("39", "131");  // 계약 승인 신청(국내)
            nvcTb.Add("40", "132");  // 계약 승인 신청(해외복귀)
            nvcTb.Add("41", "133");  // FA 계약 보상
            nvcTb.Add("42", "134");  // 신규승인신청-계약승인

            return nvcTb.Get(code.ToString());
        }
        #endregion

        #region 승인공시업무 - 진행상태
        /// <summary>
        /// 상태 구분 코드 -> 상태명
        /// </summary>
        /// <param name="scId">코드값</param>
        /// <returns>구분</returns>
        public static string GetStateName(object stateSc)
        {
            NameValueCollection nvcTb = new NameValueCollection();

            nvcTb.Add("1", "접수중");
            nvcTb.Add("2", "접수완료");
            nvcTb.Add("3", "승인•공시");
            nvcTb.Add("4", "반송");

            return nvcTb.Get(stateSc.ToString());
        }

        #endregion
    }
}

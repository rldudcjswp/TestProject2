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

namespace KBOLib.WebUtil
{
    public class CodeUtil
    {
        private static Database db = EnterpriseLibraryContainer.Current.GetInstance<Database>("kbo_ims");

        private static DataTable dtCode = null;     // 코드
        private static DataTable dtTeam = null;     // 팀
        private static DataTable dtPlayer = null;   // 선수
        private static DataTable dtItem = null;     // 비고 아이템
        
        private const string KBO_IMS_CODE_LIST_KEY = "KBO_IMS_CODE_LIST_KEY";
        private const string KBO_IMS_TEAM_LIST_KEY = "KBO_IMS_TEAM_LIST_KEY";
        private const string KBO_IMS_PLAYER_LIST_KEY = "KBO_IMS_PLAYER_LIST_KEY";
        private const string APPROVAL_ITEM_NOTE_LIST_KEY = "APPROVAL_ITEM_NOTE_LIST_KEY";

        #region DB or Cache -> Datset -> DataTable 초기화
        /// <summary>
        /// 코드(CODE) DataTable 초기화
        /// </summary>
        public static void SetCode()
        {
            DataSet dsCode = null;

            dsCode = CacheUtil.GetDataSet(KBO_IMS_CODE_LIST_KEY);

            if (dsCode == null)
            {
                dsCode = GetCodeBind();
                CacheUtil.SetDataSet(dsCode, KBO_IMS_CODE_LIST_KEY, 0, 1, 0, 0);
            }

            dtCode = dsCode.Tables[0];
        }

        /// <summary>
        /// 팀(TEAM) DataTable 초기화
        /// </summary>
        public static void SetTeam()
        {
            DataSet dsTeam = null;

            dsTeam = CacheUtil.GetDataSet(KBO_IMS_TEAM_LIST_KEY);

            if (dsTeam == null)
            {
                dsTeam = GetTeamBind();
                CacheUtil.SetDataSet(dsTeam, KBO_IMS_TEAM_LIST_KEY, 0, 1, 0, 0);
            }

            dtTeam = dsTeam.Tables[0];
        }

        /// <summary>
        /// 선수(PLAYER) DataTable 초기화
        /// </summary>
        public static void SetPlayer()
        {
            DataSet dsPlayer = null;

            dsPlayer = CacheUtil.GetDataSet(KBO_IMS_PLAYER_LIST_KEY);

            if (dsPlayer == null)
            {
                dsPlayer = GetPlayerBind();
                CacheUtil.SetDataSet(dsPlayer, KBO_IMS_PLAYER_LIST_KEY, 0, 1, 0, 0);
            }

            dtPlayer = dsPlayer.Tables[0];
        }

        /// <summary>
        /// 승인∙공시업무 - 비고 리스트 DataTable 초기화
        /// </summary>
        public static void SetApprovalItemNote()
        {
            DataSet dsItem = null;

            dsItem = CacheUtil.GetDataSet(APPROVAL_ITEM_NOTE_LIST_KEY);

            if (dsItem == null)
            {
                dsItem = GetApprovalItemBind();
                CacheUtil.SetDataSet(dsItem, APPROVAL_ITEM_NOTE_LIST_KEY, 0, 1, 0, 0);
            }

            dtItem = dsItem.Tables[0];
        }
        #endregion

        #region Cache Data 삭제
        /// <summary>
        /// 선수(PLAYER) Cache 삭제
        /// </summary>
        public static void RemovePlayer()
        {
            CacheUtil.Remove(KBO_IMS_PLAYER_LIST_KEY);
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
            DbCommand cmd = db.GetStoredProcCommand("PROC_KBO_IMS_COMMON_APPROVAL_ITEM_NOTE_LIST");

            try
            {
                dsItem = db.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
            }

            return dsItem;
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

            return dtCode.Select(string.Format("COLUMN_NM='{0}'", columnName.ToString()), "ORDER_NO ASC, ID ASC");
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

            string filter = string.Empty;

            if (leId.ToString() == CommonValue.KBO_LE_ID.ToString()) // 1군
            {
                filter = string.Format("SEASON_ID={0} AND T_NM <> ''", seasonId);
            }
            else // 퓨처스
            {
                filter = string.Format("SEASON_ID={0}", seasonId);
            }

            return dtTeam.Select(filter, "T_ID ASC");
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
        #endregion

        #region source 항목 -> target 항목
        /// <summary>
        /// Code > 코드명칭
        /// </summary>
        /// <param name="id">코드구분값</param>
        /// <returns>코드명칭</returns>
        public static string GetCodeName(object id)
        {
            SetCode();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtCode.Select(string.Format("ID='{0}'", id.ToString()));

                if (drData.Length > 0)
                {
                    result = drData[0]["CODE_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// Team 코드 > 1군 팀명
        /// </summary>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="teamId">팀ID</param>
        /// <returns></returns>
        public static string GetTeamName(object seasonId, object teamId)
        {
            SetTeam();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtTeam.Select(string.Format("SEASON_ID={0} AND T_ID='{0}'", seasonId, teamId));

                if (drData.Length > 0)
                {
                    result = drData[0]["T_NM"].ToString();
                }
            }
            catch (Exception ex) { }

            return result;
        }

        /// <summary>
        /// Team 코드 > 퓨처스 팀명
        /// </summary>
        /// <param name="seasonId">시즌ID</param>
        /// <param name="teamId">팀ID</param>
        /// <returns></returns>
        public static string GetTeamFuturesName(object seasonId, object teamId)
        {
            SetTeam();

            string result = string.Empty;

            try
            {
                DataRow[] drData = dtTeam.Select(string.Format("SEASON_ID={0} AND T_ID='{0}'", seasonId, teamId));

                if (drData.Length > 0)
                {
                    result = drData[0]["MINOR_T_NM"].ToString();
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
            nvcTb.Add("11", "승인");

            return nvcTb.Get(code.ToString());
        }

        /// <summary>
        /// 승인∙공시업무 - 비고 리스트
        /// </summary>
        /// <param name="scId">section</param>
        /// <param name="noteId">NoteID</param>
        /// <returns></returns>
        public static string GetApprovalItemNote(object scId, object noteId)
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

            return nvcTb.Get(code.ToString());
        }
        #endregion
        
    }
}

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace KBOLib.Util
{
    public class CodeUtil
    {
        private static Database imsDB = EnterpriseLibraryContainer.Current.GetInstance<Database>("kbo_ims");
        private DataTable dtCode;
        private DataTable dtTeamCode;
        private Hashtable htCode;

        private static CodeUtil _intance;
        private static readonly object clslock = new object();
        public static CodeUtil GetInstance()
        {
            lock (clslock)
            {
                if (_intance == null)
                {
                    _intance = new CodeUtil();
                }
            }

            return _intance;
        }

        public CodeUtil()
        {
            try
            {
                dtCode = DBHelper.ExecuteDataSet(imsDB, imsDB.GetStoredProcCommand("PROC_KBO_M_IMS_CODE_LIST_S")).Tables[0];
                dtTeamCode = DBHelper.ExecuteDataSet(imsDB, imsDB.GetStoredProcCommand("PROC_KBO_IMS_COMMON_TEAM_S")).Tables[0];
                SetHashTableTeamName();
            }
            catch
            {
                _intance = null;
            }
        }

        public string GetPosName(object code)
        {
            return GetCodeName("POS_CD", code);
        }

        public string GetAlarmName(object code)
        {
            return GetCodeName("ALARM_CD", code);
        }

        public string GetTeamName(object code, object le_id) {
            if (code == null || code.ToString().Length == 0)
                return string.Empty;

            try
            {
                return dtTeamCode.Select(String.Format("LE_ID={0} AND T_ID='{1}'", le_id, code), "SEASON_ID DESC")[0]["T_NM"].ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string GetTeamFullName(object code, object le_id)
        {
            return htCode[string.Format("T_F_NM_{0}_{1}", code, le_id)].ToString();
        }

        public DataRow[] GetTeamList(object season_id, object le_id = null)
        {
            if (le_id == null)
                return dtTeamCode.Select(String.Format("SEASON_ID={0}", season_id), "RANK_NO ASC");
            else
                return dtTeamCode.Select(String.Format("SEASON_ID={0} AND LE_ID={1}", season_id, le_id), "RANK_NO ASC");
        }


        private string GetCodeName(string columnName, object id)
        {
            string ret = string.Empty;
            try
            {
                ret = dtCode.Select(string.Format("COLUMN_NM='{0}' AND CODE_ID={1}", columnName, id.ToString()))[0]["CODE_NM"].ToString();
            } catch {}
            return ret;
        }

        private void SetHashTableTeamName()
        {
            htCode = new Hashtable();

            // KBO 리그 팀
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "SS", "1"), "삼성 라이온즈");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "SK", "1"), "SK 와이번스");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "LT", "1"), "롯데 자이언츠");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "HT", "1"), "KIA 타이거즈");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "OB", "1"), "두산 베어스");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "LG", "1"), "LG 트윈스");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "HH", "1"), "한화 이글스");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "WO", "1"), "넥센 히어로즈");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "NC", "1"), "NC 다이노스");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "KT", "1"), "kt 위즈");

            // 퓨처스 리그 팀
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "HT", "2"), "KIA 타이거즈");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "OB", "2"), "두산 베어스");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "LT", "2"), "롯데 자이언츠");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "SS", "2"), "삼성 라이온즈");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "SM", "2"), "상무");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "SK", "2"), "SK 와이번스");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "LG", "2"), "LG 트윈스");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "HH", "2"), "한화 이글스");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "WO", "2"), "화성 히어로즈");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "PL", "2"), "경찰");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "NC", "2"), "고양 다이노스");
            htCode.Add(string.Format("T_F_NM_{0}_{1}", "KT", "2"), "kt 위즈");
        }


    }
}

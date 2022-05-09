using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace KBOLib.Util
{
    [Serializable]
    public class UserInfo
    {
        public string uId;
        public string uPw;
        public string uNm;
        public string auCd;
        public string companyCd;
        public string tId;
        public string mainUrl;
        public string entryUrl;
        public string generalUrl;
        public string menuSc;
        public string uDt;
    }

    // GetUserInfo 메서드에서 사용할 열거형 값
    public enum UserInfoType
    {
        uId,
        uPw,
        uNm,
        auCd,
        companyCd,
        tId,
        uDt,
        mainUrl,
        entryUrl,
        generalUrl,
        menuSc
    }

    public class UserInfoUtil
    {
        private static String SessionAuthKey = "SessionManagerID";

        /// <summary>
        /// 인증 처리를 위한 메서드
        /// 인증처리 후 초기 액세스 페이지 또는 기본 페이지로 이동시킴
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="strRoles"></param>
        public static void LoginProcess(string uId, string uPw, string uNm, string auCd, string companyCd, string tId, string mainUrl, string entryUrl, string generalUrl)
        {
            UserInfo user = new UserInfo();
            user.uId = uId;
            user.uPw = uPw;
            user.uNm = uNm;
            user.auCd = auCd;
            user.companyCd = companyCd;
            user.tId = tId;
            user.mainUrl = mainUrl;
            user.entryUrl = entryUrl;
            user.generalUrl = generalUrl;
            user.menuSc = GetCompanyMenu(companyCd);

            user.uDt = DateUtil.GetFormatDate(DateTime.Now.ToString(), "yyyy-MM-dd HH:mm:ss");

            HttpContext.Current.Session.Timeout = 20;
            HttpContext.Current.Session[SessionAuthKey] = user;
        }

        /// <summary>
        /// 소속별 메뉴
        /// </summary>
        /// <param name="companyCd"></param>
        /// <returns></returns>
        private static string GetCompanyMenu(object companyCd)
        {
            string result = string.Empty;

            switch (companyCd.ToString())
            {
                case "1": // KBO
                    result = "01,03,07,09,10";
                    break;
                case "2": // 구단
                    result = "02";
                    break;
                case "3": // 일반
                    result = "02";
                    break;
                case "4": // 사용자 관리(=시스템관리)
                    result = "01,03,07,08,09,11";
                    break;
                case "5": // 구단일반(신규계정)
                    result = "02,03,19";
                    break;
            }

            return result;
        }

        /// <summary>
        ///로그아웃 처리
        ///로그아웃된 후 페이지 자기자신을 한번더 호출함
        /// </summary>
        public static void LogoutProcess()
        {
            HttpContext.Current.Session.Abandon();
        }


        /// <summary>
        /// 타입별 사용자 정보 리턴
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public static string GetUserInfo(UserInfoType userType)
        {
            if (HttpContext.Current.Session == null || HttpContext.Current.Session[SessionAuthKey] == null)
            {
                return "";          //인증되지 않았음을 의미
            }
            else
            {
                UserInfo user = (UserInfo)HttpContext.Current.Session[SessionAuthKey];
                if (user == null)
                {
                    return "";
                }
                switch (userType)
                {
                    case UserInfoType.uId:
                        return user.uId;
                    case UserInfoType.uPw:
                        return user.uPw;
                    case UserInfoType.uNm:
                        return user.uNm;
                    case UserInfoType.auCd:
                        return user.auCd;
                    case UserInfoType.companyCd:
                        return user.companyCd;
                    case UserInfoType.tId:
                        return user.tId;
                    case UserInfoType.mainUrl:
                        return user.mainUrl;
                    case UserInfoType.entryUrl:
                        return user.entryUrl;
                    case UserInfoType.generalUrl:
                        return user.generalUrl;
                    case UserInfoType.menuSc:
                        return user.menuSc;
                    case UserInfoType.uDt:
                        return user.uDt;
                    default:
                        return "";
                }
            }
        }
    }
}

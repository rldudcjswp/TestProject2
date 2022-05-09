using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KBOLib.Util
{
    public class CommonUtil
    {
        /// <summary>
        /// html 태그 제거 후 문자열 길이 넘는 부분 "..."으로 리턴
        /// </summary>
        /// <param name="text">본문</param>
        /// <param name="size">사이즈</param>
        /// <returns>축약문</returns>
        public static string GetTxtLen(string text, int size)
        {
            string ret = Regex.Replace(text, "<.*?>", "", RegexOptions.Singleline); // html 태그 삭제 구문 추가

            ret = ret.Replace("\"", "").Replace("&nbsp;", "");

            if (ret.Length > size)
            {
                ret = ret.Substring(0, size);
            }
            return ret;
        }

        /// <summary>
        /// html 태그 제거 후 문자열 길이 넘는 부분 "..."으로 리턴
        /// </summary>
        /// <param name="text">본문</param>
        /// <param name="size">사이즈</param>
        /// <returns>축약문</returns>
        public static string GetTextLen(string text, int size)
        {
            string ret = Regex.Replace(text, "<.*?>", "", RegexOptions.Singleline); // html 태그 삭제 구문 추가

            ret = ret.Replace("\"", "").Replace("&nbsp;", "");

            if (ret.Length > size)
            {
                ret = ret.Substring(0, size) + "...";
            }
            return ret;
        }

        /// <summary>
        /// 소스 데이터를 분리 후 "'" 싱글 쿼트를 추가 제 조립
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string SplitAddSingleQuart(string source, string separator)
        {
            string result = "";
            string[] itemArray = source.Split(',');

            for (int i = 0; i < itemArray.Length; i++)
            {
                result = result + "'" + itemArray[i] + "'";

                if (i < itemArray.Length - 1)
                {
                    result = result + ",";
                }
            }

            return result;
        }

        /// <summary>
        /// null 또는 empty가 아닌 경우 empty 반환
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>변환 문자열</returns>
        public static string ConvertNull(object data)
        {
            string result = data.ToString();

            if (string.IsNullOrEmpty(data.ToString()))
            {
                result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// 입력 문자열이 null이면 대치 문자열로 변환하고, null이 아닐경우 변환하지 않는다.
        /// </summary>
        /// <param name="data">문자열</param>
        /// <param name="target">null을 대치할 문자열</param>
        /// <returns>변환 문자열</returns>
        public static string ConvertNullToTarget(object data, string target)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(data.ToString()))
            {
                result = target;
            }
            else
            {
                result = data.ToString();
            }

            return result;
        }

        /// <summary>
        /// 입력 문자열이 null이면 0으로 변환
        /// </summary>
        /// <param name="data">문자열</param>
        /// <returns>변환 문자열</returns>
        public static string ConvertNullToZero(object data)
        {
            string result = data.ToString();

            if (string.IsNullOrEmpty(data.ToString()))
            {
                result = "0";
            }

            return result;
        }

        /// <summary>
        /// null 또는 empty 인 경우 DBNull.Value 반환
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>변환 데이터</returns>
        public static object ConvertDBNull(string data)
        {
            object result = data;

            if (string.IsNullOrEmpty(data) || data == "0")
            {
                result = DBNull.Value;
            }

            return result;
        }

        /// <summary>
        /// NULL 체크 NULL 또는 ""이면 true, 아니면 false
        /// </summary>
        /// <param name="data">문자열</param>
        /// <returns>NULL 또는 ""이면 true, 아니면 false를 리턴</returns>
        public static bool IsNull(string data)
        {
            bool flag = false;

            if (data == null || data == string.Empty)
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// object -> bool 변경
        /// </summary>
        /// <param name="data">object</param>
        /// <returns>object 형 -> bool 형 변환</returns>
        public static bool ConvertBool(object data)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(data.ToString()))
            {
                result = Convert.ToBoolean(data.ToString());
            }

            return result;
        }

        /// <summary>
        /// 입력 문자열이 제외하고자하는 특정값이라면 다른값으로 변경
        /// </summary>
        /// <param name="data">문자열</param>
        /// <param name="dislike">제외하고자하는 특정 문자열</param>
        /// <param name="prefer">dislike 문자열을 대치할 문자열</param>
        /// <returns>변환 문자열</returns>
        public static string ConvertPreferString(string data, string dislike, string prefer)
        {
            return (data != null && data.Equals(dislike)) ? prefer : data;
        }

        /// <summary>
        /// 부울 값을 YN 형태로 변환
        /// </summary>
        /// <param name="data">object</param>
        /// <returns></returns>
        public static string ConvertBoolToYN(object data)
        {
            return ((bool)data == true) ? "Y" : "N";
        }

        /// <summary>
        /// 부울 값을 0/1 형태로 변환
        /// </summary>
        /// <param name="data">object</param>
        /// <returns></returns>
        public static string ConvertBoolToNumber(object data)
        {
            return ((bool)data == true) ? "1" : "0";
        }

        /// <summary>
        /// 메시지 박스 처리
        /// </summary>
        /// <param name="msg">출력 문자열</param>
        /// <returns>메시지 박스</returns>
        public static string GetMsg(string msg)
        {
            msg = msg.Replace("'", "\\'");  // '는 \로 변환
            msg = msg.Replace("\"", "\\\"");    // "는 \로 변환

            return string.Format("<script type=\"text/javascript\">alert('{0}');</script>", msg);
        }

        /// <summary>
        /// 메시지 박스 처리후 페이지 이동
        /// </summary>
        /// <param name="msg">출력 문자열</param>
        /// <returns>메시지 박스</returns>
        public static string GetMsgRedirectUrl(string msg, string url)
        {
            msg = msg.Replace("'", "\\'");  // '는 \로 변환
            msg = msg.Replace("\"", "\\\"");    // "는 \로 변환

            return string.Format("<script type=\"text/javascript\">alert('{0}');location.href='{1}';</script>", msg, url);
        }

        /// <summary>
        /// 스트리밍 서버 URL 반환
        /// </summary>    
        /// <returns>스트리밍 서버 URL</returns>
        public static string GetStreamingServerUrl()
        {
            string ret = ConfigurationManager.AppSettings["StreamingServer"].ToString();

            return ret;
        }

        /// <summary>
        /// DateTime 형식의 문자열을 비디오 재생시간 기준 형식으로 변경
        /// </summary>
        public static string TimeCalculation(string sTime, string eTime)
        {
            string result = "";

            if (!string.IsNullOrEmpty(sTime) && !string.IsNullOrEmpty(eTime))
            {
                DateTime startDt = DateTime.Parse(sTime);
                DateTime endDt = DateTime.Parse(eTime);

                result = (endDt - startDt).TotalSeconds.ToString();
            }

            return result;
        }

        /// <summary>
        /// 한자리 정수를 두자리 정수형태로 변환한다.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetTwoInt(int num)
        {
            return num < 10 ? "0" + num : "" + num;
        }

        /// <summary>
        /// 한자리 숫자 문자를 두 자리형식 숫자 문자로 변환한다.
        /// </summary>
        /// <param name="num">숫자 데이터</param>
        /// <returns>규격에 맞추어진 데이터</returns>
        public static string GetTwoInt(string num)
        {
            string ret = num.Length == 1 ? "0" + num : "" + num;

            if (num.CompareTo("--") == 0)
            {
                ret = "00";
            }

            return ret;
        }

        /// <summary>
        /// 0 -> "" 숫자 0이면 공백으로 변경
        /// </summary>
        /// <param name="data">데이터</param>
        /// <returns>변경된 문자열</returns>
        public static string ConvertZeroToBlank(string data)
        {
            string result = data;

            if (data.Equals("0"))
            {
                result = "";
            }

            return result;
        }

        /// <summary>
        /// http -> https 보안 접속이 필요한 페이지에 적용
        /// </summary>
        public static void RedirectHttps()
        {
            string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri;

            if (!HttpContext.Current.Request.IsSecureConnection && currentUrl.ToUpper().IndexOf("LOCALHOST") == -1 && currentUrl.ToUpper().IndexOf("TEST") == -1)
            {
                string redirectUrl = currentUrl.Replace("http:", "https:");
                HttpContext.Current.Response.Redirect(redirectUrl);
            }
        }

        /// <summary>
        /// https -> http 일반 접속이 필요한 페이지에 적용
        /// </summary>
        public static void RedirectHttp()
        {
            string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri;

            if (HttpContext.Current.Request.IsSecureConnection && currentUrl.ToUpper().IndexOf("LOCALHOST") == -1 && currentUrl.ToUpper().IndexOf("TEST") == -1)
            {
                string redirectUrl = currentUrl.Replace("https:", "http:");
                HttpContext.Current.Response.Redirect(redirectUrl);
            }
        }

        /// <summary>
        /// 모바일 브라우저 판별
        /// </summary>
        /// <returns></returns>
        public static bool isMobileBrowser()
        {
            HttpContext context = HttpContext.Current;

            if (context.Request.Browser.IsMobileDevice)
            {
                return true;
            }

            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }

            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }

            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                string[] mobiles =
            {
                "midp", "j2me", "avant", "docomo", 
                "novarra", "palmos", "palmsource", 
                "240x320", "opwv", "chtml",
                "pda", "windows ce", "mmp/", 
                "blackberry", "mib/", "symbian", 
                "wireless", "nokia", "hand", "mobi",
                "phone", "cdm", "up.b", "audio", 
                "SIE-", "SEC-", "samsung", "HTC", 
                "mot-", "mitsu", "sagem", "sony"
                , "alcatel", "lg", "eric", "vx", 
                "NEC", "philips", "mmm", "xx", 
                "panasonic", "sharp", "wap", "sch",
                "rover", "pocket", "benq", "java", 
                "pt", "pg", "vox", "amoi", 
                "bird", "compal", "kg", "voda",
                "sany", "kdd", "dbt", "sendo", 
                "sgh", "gradi", "jb", "dddi", 
                "moto", "iphone"
            };

                foreach (string s in mobiles)
                {
                    if (context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 프로시저 실행 구문 리턴
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <returns>프로시저 실행 구문</returns>
        public static string ToProcParam(DbCommand cmd)
        {
            StringBuilder sbProc = new StringBuilder();

            sbProc.Append("EXEC ").Append(cmd.CommandText).Append(" ");

            for (int i = 0; i < cmd.Parameters.Count; i++)
            {
                if (cmd.Parameters[i].DbType == DbType.String)
                {
                    sbProc.Append("'");
                }

                sbProc.Append(cmd.Parameters[i].Value);

                if (cmd.Parameters[i].DbType == DbType.String)
                {
                    sbProc.Append("'");
                }

                sbProc.Append(",");
            }

            return sbProc.ToString().Substring(0, sbProc.Length - 1);
        }

        /// <summary>
        /// 소수점 데이터 포맷 변경
        /// </summary>
        /// <param name="data">Decimal</param>
        /// <param name="digits">소수 자릿수</param>
        /// <returns>digits와 일치 하는 포맷</returns>
        public static string ConvertFormat(Decimal data, int digits)
        {
            string result = "-";

            switch (digits)
            {
                case 0:
                    result = string.Format("{0:0}", data);
                    break;
                case 1:
                    result = string.Format("{0:0.0}", data);
                    break;
                case 2:
                    result = string.Format("{0:0.00}", data);
                    break;
                case 3:
                    result = string.Format("{0:0.000}", data);
                    break;
            }

            return result;
        }

        /// <summary>
        /// 소숫점 자릿수 셋팅
        /// </summary>
        /// <param name="cho">표현 소수점 자리수</param>
        /// <param name="data">대상 데이터</param>
        /// <returns></returns>
        public static string ConvertComma(object data)
        {
            string ret = "";

            if (data.ToString() == null || data.ToString().Equals("") || data.ToString() == "NaN")
                return ret;

            if (data.ToString().Length > 10)
            {
                data = data.ToString().Substring(0, 10);
            }

            ret = string.Format("{0:#,0}", Convert.ToDecimal(data));

            return ret;
        }

        // <summary>
        /// 비밀번호 유효성체크
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Boolean GetPasswordRegex(string password)
        {
            Boolean result = false;

            Regex hasNumber = new Regex(@"[0-9]+");
            Regex hasUpperChar = new Regex(@"[A-Z]+");
            Regex hasMiniMaxChars = new Regex(@".{10,14}");
            Regex hasLowerChar = new Regex(@"[a-z]+");
            Regex hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            // 소문자, 대문자, 숫자, 특수문자
            if (hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && password.Length >= 10 && password.Length <= 14 && hasLowerChar.IsMatch(password) && hasSymbols.IsMatch(password))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 파일 정보에서 파일 전체 경로 추출
        /// </summary>
        /// <param name="jsonData">파일정보</param>
        /// <returns>파일 전체 경로</returns>
        public static string GetFilePath(string jsonData)
        {
            string result = "";

            if (jsonData.Contains("No files received"))
            {
                return result;
            }

            if (!string.IsNullOrEmpty(jsonData))
            {
                JObject obj = JObject.Parse(JsonConvert.DeserializeObject(jsonData).ToString());
                JArray list = (JArray)obj["info"];

                for (int i = 0; i < list.Count; i++)
                {
                    JObject item = (JObject)list[i];

                    string path = item["path"].ToString();
                    path = path.Replace("/file/", "");
                    result = string.Format("{0}{1}", path, item["name"]);
                    //result = string.Format("{0}{1}", item["path"], item["name"]);
                    break;
                }
            }


            return result;
        }

        /// <summary>
        /// 파일 정보에서 파일 전체 경로 추출
        /// </summary>
        /// <param name="jsonData">파일정보</param>
        /// <param name="filePath">파일경로</param>
        /// <returns>파일 전체 경로</returns>
        public static string GetFilePath(string jsonData, string filePath)
        {
            string result = filePath;

            if (jsonData == "No files received.")
            {
                return result;
            }

            if (!string.IsNullOrEmpty(jsonData))
            {
                result = GetFilePath(jsonData);
            }

            return result;
        }

        /// <summary>
        /// 파라미터를 받아 허용된 값(null, empty)이 아니면 empty를 반환
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ParamRequest(string param)
        {
            string retValue = string.Empty;
            if (!string.IsNullOrEmpty(param))
            {
                //retValue = param;
                retValue = InJectionUtil.replaceSqlInjaction(param);
            }
            return retValue;
        }

        /// <summary>
        /// 파라미터를 받아 허용된 값(null, empty)이 아니면 empty를 반환
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ParamRequestInjactionBoard(string param)
        {
            string retValue = string.Empty;
            if (!string.IsNullOrEmpty(param))
            {
                //retValue = param;
                retValue = InJectionUtil.replaceSqlInjactionBoard(param);
            }
            return retValue;
        }

        /// <summary>
        /// inn2 이닝을 실제 이닝으로 (3 1/3) 계산
        /// </summary>
        /// <param name="inn2">이닝*3</param>
        /// <returns>실제이닝</returns>
        public static string ConvertInn(object inn2)
        {
            string result = "";

            if (!string.IsNullOrEmpty(inn2.ToString()))
            {
                int inn = int.Parse(inn2.ToString());
                int quotient = inn / 3;   // 몫
                int rest = inn % 3;   // 나머지

                if (inn == 0)
                {
                    result = "0";
                }
                else if (quotient == 0)
                {
                    result = string.Format("{0}/3", rest.ToString());
                }
                else
                {
                    if (rest == 0)
                    {
                        result = quotient.ToString();
                    }
                    else
                    {
                        result = string.Format("{0} {1}/3", quotient.ToString(), rest.ToString());
                    }
                }
            }

            return result;
        }

        /// 글번호 셋팅
        /// </summary>
        /// <returns></returns>
        public static int GetPageNumber(int totalCount, int currentPage, int pageSize, int nowIndex)
        {
            int ret = 0;

            ret = totalCount - nowIndex - ((currentPage - 1) * pageSize);

            return ret;
        }

    }
}

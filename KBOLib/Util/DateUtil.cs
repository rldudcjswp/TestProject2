using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace KBOLib.Util
{
    public class DateUtil
    {
        /// <summary>
        /// YYYYMMDD 형식 오늘 날짜 구하기    
        /// </summary>
        /// <returns>YYYYMMDD</returns>
        public static string GetNowDate()
        {
            string date = DateTime.Now.ToString();

            date = GetFormatDate(date, "yyyyMMdd");

            return date;
        }

        /// <summary>
        /// 날짜 형식 반환 [지정한 형식] 예) "2011-02-08", "yyyy 년 MM 월 dd 일"
        /// </summary>
        /// <param name="data">날짜</param>
        /// <param name="format">날짜 포맷</param>
        /// <returns></returns>
        public static string GetFormatDate(object data, string format)
        {
            string date = data.ToString();

            if (date.Length == 8)
            {
                date = ConvertDateType(date);
            }

            try
            {
                date = (String)DateTime.Parse(date).ToString(format);
            }
            catch (Exception) { }

            return date;
        }

        /// <summary>
        /// 현재 년도 ex, yyyy
        /// </summary>
        /// <returns>yyyy 년도</returns>
        public static string GetNowYear()
        {
            string date = DateTime.Now.ToString();

            return GetFormatDate(date, "yyyy");
        }

        /// <summary>
        /// 현재 월 ex, MM 형태로 변환 후 return
        /// </summary>
        /// <returns>MM 월</returns>
        public static string GetNowMonth()
        {
            string date = DateTime.Now.ToString();

            return GetFormatDate(date, "MM");
        }

        /// <summary>
        /// 한국 현재 일을 dd 형태로 변환 후 return
        /// </summary>
        /// <returns>dd 일</returns>
        public static string GetNowDay()
        {
            string date = DateTime.Now.ToString();

            return GetFormatDate(date, "dd");
        }

        /// <summary>
        /// 해당 날짜의 요일
        /// </summary>
        /// <param name="date">날짜</param>
        /// <returns>요일</returns>
        public static string GetDayOfWeek(DateTime date)
        {
            return date.ToString("ddd", new CultureInfo("ko-KR"));
        }

        /// <summary>
        /// 해당 날짜의 요일
        /// </summary>
        /// <param name="date">날짜</param>
        /// <returns>요일</returns>
        public static string GetDayOfWeekStr(object date)
        {
            return GetDayOfWeek(DateTime.Parse(date.ToString()));
        }

        /// <summary>
        /// 두 날짜 사이의 기간을 돌려 준다.
        /// </summary>
        /// <param name="sDate">시작날짜</param>
        /// <param name="eDate">종료날짜</param>
        /// <returns>두 날짜 사이의 기간</returns>
        public static int GetCompareDate(String sDate, String eDate)
        {
            DateTime startDate = Convert.ToDateTime(ConvertDateType(sDate));
            DateTime endDate = Convert.ToDateTime(ConvertDateType(eDate));

            // 두 날짜 사이의 기간 찾기
            TimeSpan period = endDate - startDate;

            // 두 날짜 사이의 기간 돌려 주기
            return period.Days;
        }

        /// <summary>
        /// 날짜 비교 앞 날짜가 크면 true;
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        public static bool IsCompareDate(String data1, String data2)
        {
            bool isCompare = false;

            if (data1.Length.Equals(8)) data1 = ConvertDateType(data1);
            if (data2.Length.Equals(8)) data2 = ConvertDateType(data2);

            DateTime d1 = DateTime.Parse(data1);
            DateTime d2 = DateTime.Parse(data2);

            if (DateTime.Compare(d1, d2) > 0)
            {
                isCompare = true;
            }

            return isCompare;
        }

        /// <summary>
        ///  날짜 형식 변환 YYYYMMDD=>YYYY-MM-DD
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDateType(string date)
        {
            string ret = date;

            if (date.Length >= 8)
            {
                ret = string.Format("{0}-{1}-{2}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6, 2));
            }

            return ret;
        }

        /// <summary>
        /// 날짜 더하기
        /// </summary>
        /// <param name="date"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string AddDays(string date, int day)
        {
            DateTime inputDate = DateTime.Parse(ConvertDateType(date));
            return GetFormatDate(inputDate.AddDays(day), "yyyyMMdd");
        }
    }
}

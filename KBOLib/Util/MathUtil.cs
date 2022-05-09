using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace KBOLib.Util
{
    public class MathUtil
    {
        #region 반올림, count, sum, avg, min, max
        /// <summary>
        /// 지정된 소수 자릿수로 반올림
        /// </summary>
        /// <param name="data">데이터</param>
        /// <param name="decimals">반환값의 소수 자릿수</param>
        /// <returns>digits와 일치하는 소수 자릿수가 들어 있는 data에 가장 가까운 수입니다. </returns>
        public static string Round(object data, int digits)
        {
            string result = "0";

            if (data == DBNull.Value)
            {
                result = CommonUtil.ConvertFormat(0, digits);
            }
            else
            {
                result = CommonUtil.ConvertFormat(Math.Round(Convert.ToDecimal(data), digits, MidpointRounding.AwayFromZero), digits);
            }

            return result;
        }

        /// <summary>
        /// 데이터 갯수
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">컬럼명</param>
        /// <param name="filter">검색 조건</param>
        /// <returns>데이터 갯수</returns>
        public static string Count(DataTable dt, string colName, string filter)
        {
            string result = "0";
            object count = dt.Compute(string.Format("Count({0})", colName), filter);

            if (count != DBNull.Value && decimal.Parse(count.ToString()) > 0)
            {
                result = count.ToString();
            }

            return result;
        }

        /// <summary>
        /// 데이터 합계
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">컬럼명</param>
        /// <param name="filter">검색 조건</param>
        /// <returns>합계</returns>
        public static string Sum(DataTable dt, string colName, string filter)
        {
            string result = "0";
            object sum = dt.Compute(string.Format("Sum({0})", colName), filter);

            if (sum != DBNull.Value) // && decimal.Parse(sum.ToString()) > 0)
            {
                result = sum.ToString();
            }

            return result;
        }

        /// <summary>
        /// 데이터 평균
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">컬럼명</param>
        /// <param name="filter">검색 조건</param>
        /// <returns>평균</returns>
        public static string Avg(DataTable dt, string colName, string filter)
        {
            string result = "0";
            object avg = dt.Compute(string.Format("Avg({0})", colName), filter);

            if (avg != DBNull.Value) // && decimal.Parse(avg.ToString()) > 0)
            {
                result = avg.ToString();
            }

            return result;
        }

        /// <summary>
        /// 데이터 평균
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">컬럼명</param>
        /// <param name="filter">검색 조건</param>
        /// <returns>평균</returns>
        public static string Avg2(DataTable dt, string colName, string filter)
        {
            string result = "0";
            object sum = dt.Compute(string.Format("SUM({0})", colName), filter);
            object count = dt.Compute(string.Format("COUNT({0})", colName), filter);

            if (count != DBNull.Value && decimal.Parse(count.ToString()) > 0)
            {
                result = (decimal.Parse(sum.ToString()) / decimal.Parse(count.ToString())).ToString();
            }

            return result;
        }

        /// <summary>
        /// 데이터 최소값
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">컬럼명</param>
        /// <param name="filter">검색 조건</param>
        /// <returns>최소값</returns>
        public static string Min(DataTable dt, string colName, string filter)
        {
            string result = "0";
            object min = dt.Compute(string.Format("Min({0})", colName), filter);

            if (colName == "ENTRY_START_DT")
            {
                return min.ToString();
            }

            if (min != DBNull.Value && decimal.Parse(min.ToString()) > 0)
            {
                result = min.ToString();
            }

            return result;
        }

        /// <summary>
        /// 데이터 최대값
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="colName">컬럼명</param>
        /// <param name="filter">검색 조건</param>
        /// <returns>최대값</returns>
        public static string Max(DataTable dt, string colName, string filter)
        {
            object max = dt.Compute(string.Format("Max({0})", colName), filter);

            return max.ToString();
        }
        #endregion
    }
}

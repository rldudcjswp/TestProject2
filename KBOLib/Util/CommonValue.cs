using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace KBOLib.Util
{
    public class CommonValue
    {
        public const int KBO_LE_ID = 1;       // 1군 리그 ID
        public const int FUTURES_LE_ID = 2;   // 퓨처스리그 ID

        public const int KBO_END_YEAR = 2018;  // KBO 1군 최종 년도
        public const int FUTURES_END_YEAR = 2018; // 퓨처스 최종 년도

        public static string IMG_SERVER = ConfigurationManager.AppSettings["IMG_SERVER"].ToString();
        public static string CDN_URL = ConfigurationManager.AppSettings["CDN_URL"].ToString();
        public static string CSS_VERSION = ConfigurationManager.AppSettings["CSS_VERSION"].ToString();
        public static string JS_VERSION = ConfigurationManager.AppSettings["JS_VERSION"].ToString();
        public static string FILE_PATH = ConfigurationManager.AppSettings["FILE_PATH"].ToString();
        public static string FILE_URL = ConfigurationManager.AppSettings["FILE_URL"].ToString();
        public static string RESOURCES_VERSION = ConfigurationManager.AppSettings["RESOURCES_VERSION"].ToString();
        public static string EBOOK_URL = ConfigurationManager.AppSettings["EBOOK_URL"].ToString();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace KBOLib._Util
{
    public class StaticVariable
    {
        /****************************** Common Variable ******************************/

        public static string FILE_URL = ConfigurationManager.AppSettings["file_url"].ToString();
        public static string WS_URL = ConfigurationManager.AppSettings["ws_url"].ToString();
    }
}
using KBOLib.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace KBOLib.Util
{
    public class DBHelper
    {
        private static CacheUtil cache = new CacheUtil();

        public static DataSet ExecuteDataSet(Database db, DbCommand cmd, int cacheTime = 0)
        {
            DataSet ds = null;

            if (cacheTime == 0)
                ds = db.ExecuteDataSet(cmd);
            else
            {
                var sb = new System.Text.StringBuilder();
                sb.Append(cmd.CommandText).Append("|");
                foreach (DbParameter item in cmd.Parameters)
                    sb.AppendFormat("{0}={1}", item.ParameterName, item.Value);

                String cacheKey = sb.ToString();

                if (cache.IsCacheData(cacheKey) && cache.GetCacheData(cacheKey) != null)
                    ds = (DataSet)cache.GetCacheData(cacheKey);
                else
                {
                    ds = db.ExecuteDataSet(cmd);
                    cache.SetCacheData(cacheKey, ds, cacheTime);
                }
            }

            return ds;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Specialized;
using System.Configuration;
// TODO: Use Enterprise Library Data Block
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace KBOLib.WebUtil
{
    public class CacheUtil
    {
        /// <summary>
        /// Cache -> DataSet
        /// </summary>
        /// <param name="cacheKey">Key</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string cacheKey)
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            DataSet dsData = (DataSet)cache[cacheKey];

            return dsData;
        }

        /// <summary>
        /// Cache Key에 해당 하는 Cache Data Delete
        /// </summary>
        /// <param name="cacheKey"></param>
        public static void Remove(string cacheKey)
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            cache.Remove(cacheKey);
        }

        /// <summary>
        /// All Cach Data Clear
        /// </summary>
        public static void Flush()
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            cache.Flush();
        }

        /// <summary>
        /// DataSet -> Cache
        /// </summary>
        /// <param name="dsData">DataSet</param>
        /// <param name="cacheKey">Key</param>
        /// <param name="days">일</param>
        /// <param name="hours">시</param>
        /// <param name="minutes">분</param>
        /// <param name="seconds">초</param>
        public static void SetDataSet(DataSet dsData, string cacheKey, int days, int hours, int minutes, int seconds)
        {
            ICacheManager cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            AbsoluteTime expiry = new AbsoluteTime(new TimeSpan(days, hours, minutes, seconds));
            cache.Add(cacheKey, dsData, CacheItemPriority.High, null, new ICacheItemExpiration[] { expiry });
        }
    }
}

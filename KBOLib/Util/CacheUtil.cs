using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace KBOLib.Util
{
    /// <summary>
    /// 웹 서버 캐시 유틸리티 클래스 
    /// </summary>
    public class CacheUtil
    {
        static readonly object clslock = new object();
        public static class CacheTime
        {
            public const int SECOND_1 = 1;
            public const int SECOND_10 = SECOND_1 * 10;
            public const int SECOND_30 = SECOND_1 * 30;
            public const int MINUTE_1 = SECOND_1 * 60;
            public const int MINUTE_3 = MINUTE_1 * 3;
            public const int MINUTE_5 = MINUTE_1 * 5;
            public const int MINUTE_10 = MINUTE_1 * 10;
            public const int MINUTE_30 = MINUTE_1 * 30;
            public const int HOUR_1 = MINUTE_1 * 60;
            public const int HOUR_2 = HOUR_1 * 2;
            public const int HOUR_3 = HOUR_1 * 3;
            public const int HOUR_6 = HOUR_1 * 6;
            public const int HOUR_12 = HOUR_1 * 12;
            public const int DAY_1 = HOUR_1 * 24;
        }

        /// <summary>
        /// 캐싱 되어진 데이터가 있는지 검사
        /// </summary>
        /// <param name="cacheKey">키</param>
        /// <returns></returns>
        public Boolean IsCacheData(String cacheKey)
        {
            Boolean ret = false;

            if (HttpContext.Current != null)
            {
                Cache cache = HttpContext.Current.Cache;
                Object value = cache[cacheKey];

                if (value != null)
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// 캐시 삭제
        /// </summary>
        /// <param name="cacheKey"></param>
        public void ClearCacheData(String cacheKey)
        {
            Cache cache = HttpContext.Current.Cache;
            Object value = cache[cacheKey];
            if (value != null)
            {
                lock (clslock)
                {
                    // 락을 걸고 한번 더 검사
                    // 이런식으로 이중으로 검사해야 쓰레드에서 안전하다.                     
                    cache = HttpContext.Current.Cache;
                    value = cache[cacheKey];

                    if (value != null)
                    {
                        cache.Remove(cacheKey);
                    }
                }
            }
        }

        public void ClearCacheDataAll()
        {
            Cache cache = HttpContext.Current.Cache;

            foreach (DictionaryEntry dEntry in cache)
            {
                ClearCacheData(dEntry.Key.ToString());
            }
        }

        /// <summary>
        /// 캐싱 데이터 리턴
        /// </summary>
        /// <param name="cacheKey">키</param>
        /// <returns>캐시된 객체</returns>
        public Object GetCacheData(String cacheKey)
        {
            Object value = null;

            if (IsCacheData(cacheKey))
            {
                value = HttpContext.Current.Cache[cacheKey];
            }

            return value;
        }

        /// <summary>
        /// 캐시 설정 
        /// </summary>
        /// <param name="cacheKey">키</param>
        /// <param name="value">대상 객체</param>
        /// <param name="cacheTime">캐시 시간(단위: 초)</param>
        public void SetCacheData(String cacheKey, Object value, int cacheTime)
        {
            if (HttpContext.Current != null)
            {
                Cache cache = HttpContext.Current.Cache;

                Object prevValue = cache[cacheKey];

                if (prevValue == null)
                {
                    lock (clslock)
                    {
                        cache = HttpContext.Current.Cache;
                        prevValue = cache[cacheKey];

                        if (prevValue == null)
                        {
                            cache.Insert(cacheKey, value, null, DateTime.Now.AddSeconds(Convert.ToDouble(cacheTime)), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                        }
                    }
                }
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KBOLib.Util
{
    public class EntryItem
    {
        public string EntryDate { get; set; }
        public string RegYn { get; set; }
        public string ReasonCd { get; set; }
        public EntryItem(string entryDate, string regYn, string reasonCd)
        {
            this.EntryDate = entryDate;
            this.RegYn = regYn;
            this.ReasonCd = reasonCd;
        }

        public override string ToString()
        {
            return string.Format("ENTRY_DS : {0} && REG_YN : {1} && REASON_CD : {2}", this.EntryDate, this.RegYn, this.ReasonCd);
        }
    }

    public class EntryList
    {
        private List<EntryItem> list = new List<EntryItem>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return this.list.Count == 0;
        }

        public EntryItem this[string entryDs]
        {
            get
            {
                if (IsEmpty())
                {
                    return null;
                }

                foreach(EntryItem item in list)
                {
                    if (item.EntryDate.Equals(entryDs))
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        public bool Remove(EntryItem item)
        {
            return list.Remove(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entryDate"></param>
        /// <param name="regYn"></param>
        /// <returns></returns>
        public EntryItem Add(object entryDate, object regYn, object reasonCd)
        {
            bool needAdd = false;
            int insertIdx = 0;

            list.Add(new EntryItem(entryDate.ToString(), regYn.ToString(), reasonCd.ToString()));

            //if (list.Count == 0)
            //{
            //    list.Add(new EntryItem(entryDate.ToString(), regYn.ToString()));
            //}
            //else
            //{
            //    for (insertIdx = 0; insertIdx < list.Count && needAdd == false; insertIdx++)
            //    {
            //        if (Compare(entryDate, list[insertIdx].EntryDate) < 0)
            //        {
            //            list.Insert(insertIdx, new EntryItem(entryDate.ToString(), regYn.ToString()));
            //            needAdd = true;
            //            return list[insertIdx];
            //        }
            //    }

            //    if (needAdd == false)
            //    {
            //        list.Add(new EntryItem(entryDate.ToString(), regYn.ToString()));
            //    }
            //}

            return null; // 마지막 추가 혹은 추가 X
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EntryItem PopSmallest()
        {
            EntryItem first = list[0];
            list.Remove(first);
            return first;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EntryItem PopBiggest()
        {
            EntryItem last = list[list.Count - 1];
            list.Remove(last);
            return last;
        }

        /// <summary>
        /// obj1 > obj2 = 1
        /// obj1 = obj2 = 0
        /// obj1 < obj2 = -1
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public int Compare(object obj1, object obj2)
        {
            return string.Compare(obj1.ToString(), obj2.ToString());
        }

        public IEnumerator<EntryItem> GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetStudio
{
    public interface ICustomMonoBehavior
    {
        protected static T[] MakeArray<T>(object objListA, Func<object, T> insertFunc)
        {
            var objList = (List<object>)objListA;
            T[] arr = new T[objList.Count];
            for (int i = 0; i < objList.Count; i++)
            {
                arr[i] = insertFunc(objList[i]);
            }
            return arr;
        }
        protected static List<T> MakeList<T>(object objListA, Func<object, T> insertFunc)
        {
            var objList = (List<object>)objListA;
            var listOut = new List<T>();
            for (int i = 0; i < objList.Count; i++)
            {
                listOut.Add(insertFunc(objList[i]));
            }
            return listOut;
        }
        protected static void FillFields(MonoBehaviour behavior, Action<string, object> fldCheck)
        {
            OrderedDictionary bDict = behavior.ToType();
            foreach (DictionaryEntry dictEntry in bDict)
            {
                fldCheck((string)dictEntry.Key, dictEntry.Value);
            }
        }
        protected static long GetObjectPathID(object dictIntA)
        {
            OrderedDictionary dictInt = (OrderedDictionary)dictIntA;
            long val = 0;
            foreach (DictionaryEntry dictEntry in dictInt)
            {
                switch ((string)dictEntry.Key)
                {
                    case "m_PathID":
                        val = (long)dictEntry.Value;
                        break;
                }
            }
            return val;
        }
    }
}

namespace GlobalPlay.Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class Extensions
    {
        public static bool AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return false;
            }

            dictionary.Add(key, value);
            return true;
        }

        public static bool AddOrIgnore<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                return true;
            }

            return false;
        }

        public static void Shuffle<T>(this List<T> texts)
        {
            // Knuth shuffle algorithm
            for (int t = 0; t < texts.Count; t++)
            {
                T tmp = texts[t];
                int r = Random.Range(t, texts.Count);
                texts[t] = texts[r];
                texts[r] = tmp;
            }
        }

        public static void Shuffle<T>(this List<T> texts, int seed)
        {
            // Knuth shuffle algorithm with explicitly specified seed for random
            int oldSeed = Random.seed;
            Random.seed = seed;
            for (int t = 0; t < texts.Count; t++)
            {
                T tmp = texts[t];
                int r = Random.Range(t, texts.Count);
                texts[t] = texts[r];
                texts[r] = tmp;
            }
            Random.seed = oldSeed;
        }

        public static bool AddOrReplace<T>(this List<T> list, T value) where T : class
        {
            int index = list.FindIndex(0, v => { return v == value; });
            if (index > -1)
            {
                list[index] = value;
                return false;
            }

            list.Add(value);
            return true;
        }

        public static string AsString<T>(this List<T> list)
        {
            if (list == null)
            {
                return "null";
            }
            string result = "[";
            if (list.Any())
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    result += list[i] + ", ";
                }

                result += list[list.Count - 1].ToString();
            }

            return result + "]";
        }

        public static string AsString<T>(this T[] list)
        {
            string result = "[";
            if (list.Any())
            {
                for (int i = 0; i < list.Length - 1; i++)
                {
                    result += list[i] + ", ";
                }

                result += list[list.Length - 1].ToString();
            }

            return result + "]";
        }

        /// <summary>
        /// Something wrong here
        /// </summary>
        public static bool ContainsAll<T>(this List<T> list, List<T> other)
        {
            foreach (var a in list)
            {
                //bool has = false;
                foreach (var b in other)
                {
                    if (b.Equals(a))
                    {
                        // has = true;
                        break;
                    }
                }

                return false;
            }

            return true;
        }
    }
}
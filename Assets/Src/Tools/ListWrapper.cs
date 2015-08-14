#region imports

using System;
using System.Collections.Generic;

#endregion

namespace GlobalPlay.Solitaire.Tools
{
    internal class ListWrapper<T>
    {
        private List<T> list;

        public ListWrapper(List<T> list)
        {
            this.list = list;
        }

        public List<T> List
        {
            get { return list; }
        }

        public ListWrapper<T> Distinct()
        {
            var result = new List<T>();
            for (int i = 0, n = list.Count; i < n; i++)
            {
                if (!result.Contains(list[i]))
                {
                    result.Add(list[i]);
                }
            }

            list = result;
            return this;
        }

        public ListWrapper<T> Where(Func<T, bool> predicate)
        {
            return Where(predicate, false);
        }

        public ListWrapper<T> WhereNot(Func<T, bool> predicate)
        {
            return Where(predicate, true);
        }

        private ListWrapper<T> Where(Func<T, bool> predicate, bool negatePredicate)
        {
            var result = new List<T>();
            for (int i = 0, n = list.Count; i < n; i++)
            {
                if (predicate(list[i]) ^ negatePredicate)
                {
                    result.Add(list[i]);
                }
            }

            list = result;
            return this;
        }

        public List<K> Select<K>(Func<T, K> mapper)
        {
            var result = new List<K>();
            for (int i = 0, n = list.Count; i < n; i++)
            {
                result.Add(mapper(list[i]));
            }

            return result;
        }

        public List<K> SelectNotNull<K>(Func<T, K> mapper)
        {
            var result = new List<K>();
            for (int i = 0, n = list.Count; i < n; i++)
            {
                var mapped = mapper(list[i]);
                if (mapped != null)
                {
                    result.Add(mapped);
                }
            }

            return result;
        }

        public int Sum(Func<T, int> mapper)
        {
            var sum = 0;
            for (int i = 0, n = list.Count; i < n; i++)
            {
                sum += mapper(list[i]);
            }

            return sum;
        }

        public int Count()
        {
            return list.Count;
        }
    }
}
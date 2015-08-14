#region imports

using System.Collections.Generic;

#endregion

namespace GlobalPlay.Tools
{
    public class CountDictionary<T> : Dictionary<T, int>
    {
        public new void Add(T key, int value = 1)
        {
            if (ContainsKey(key))
            {
                base[key]++;
            }
            else
            {
                base.Add(key, 1);
            }
        }
    }
}
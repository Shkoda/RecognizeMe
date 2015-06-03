namespace GlobalPlay.Tools
{
    using System.Collections.Generic;

    public class CountDictionary<T> : Dictionary<T, int>
    {
        public new void Add(T key, int value = 1)
        {
            if (this.ContainsKey(key))
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
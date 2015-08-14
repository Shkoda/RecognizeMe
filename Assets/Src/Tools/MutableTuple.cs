namespace GlobalPlay.Tools
{
    public class MutableTuple<T, K>
    {
        public MutableTuple(T first, K second)
        {
            First = first;
            Second = second;
        }

        public T First { get; set; }
        public K Second { get; set; }
    }
}
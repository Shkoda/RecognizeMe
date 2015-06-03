namespace GlobalPlay.Tools
{
    public class MutableTuple<T, K>
    {
        public T First { get; set; }

        public K Second { get; set; }

        public MutableTuple(T first, K second)
        {
            this.First = first;
            this.Second = second;
        }
    }
}
namespace GlobalPlay.Solitaire.Tools
{
    public class Tuple<T, K>
    {
        public readonly T First;

        public readonly K Second;

        public Tuple(T first, K second)
        {
            this.First = first;
            this.Second = second;
        }
    }

    public class Tuple<T, K, V>
    {
        public readonly T First;

        public readonly K Second;

        public readonly V Third;

        public Tuple(T first, K second, V third)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }
    }
}
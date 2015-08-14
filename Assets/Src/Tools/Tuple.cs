namespace GlobalPlay.Solitaire.Tools
{
    public class Tuple<T, K>
    {
        public readonly T First;
        public readonly K Second;

        public Tuple(T first, K second)
        {
            First = first;
            Second = second;
        }
    }

    public class Tuple<T, K, V>
    {
        public readonly T First;
        public readonly K Second;
        public readonly V Third;

        public Tuple(T first, K second, V third)
        {
            First = first;
            Second = second;
            Third = third;
        }
    }
}
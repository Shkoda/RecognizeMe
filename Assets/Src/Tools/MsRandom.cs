namespace GlobalPlay.Solitaire.Tools
{
    using UnityEngine;

    public class MsRandom
    {
        public int Seed { get; set; }

        public MsRandom()
        {
            Seed = (int) (Random.value*32000);
        }

        public int Next()
        {
            Seed = Seed*0x343fd + 0x269EC3; // a=214013, b=2531011
            return (Seed >> 0x10) & 0x7FFF;
        }
    }
}
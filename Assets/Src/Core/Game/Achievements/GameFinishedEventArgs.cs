namespace Shkoda.RecognizeMe.Core.Game.Achievements
{
    public class GameFinishedEventArgs
    {
        public long Time { get; set; }

        public int Moves { get; set; }

        public int Points { get; set; }
    }
}
namespace Shkoda.RecognizeMe.Core.Mechanics.Actions
{
    public interface IAction
    {
        void Act(Mechanics mechanics);
        float Draw(Graphics.Graphics graphics);
    }
}
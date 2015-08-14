#region imports

using Shkoda.RecognizeMe.Core.Game.Achievements;

#endregion

namespace Shkoda.RecognizeMe.Core.Mechanics.Actions.Handlers
{
    public abstract class GameActionHandler
    {
        protected bool triggerAchievements;

        /// <summary>
        /// If seed is -1, achievements' triggers are not processed
        /// </summary>
        /// <param name="seed"></param>
        public GameActionHandler(int seed)
        {
            triggerAchievements = (seed != -1);
        }

        public virtual void HandleAction(IAction action)
        {
//           var move = action as MoveAction;
//           if (move != null && move.Dst.DeckClass == DeckClass.Foundation)
//           {
//               this.CountExperienceAndPoints(move);
//               this.ProcessMoveToFoundation(move.Cards[0].Rank);
//           }
//
//           if (action is ICountableAsMoveAction)
//           {
//               this.Moves++;
//               Graphics.Instance.UpdateMovesInHud(Moves);
//           }
        }

        public GameFinishedEventArgs CreateGameFinishedEventArgs()
        {
            return new GameFinishedEventArgs();
        }
    }
}
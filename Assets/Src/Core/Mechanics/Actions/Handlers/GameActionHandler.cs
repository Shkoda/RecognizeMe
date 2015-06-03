

using Shkoda.RecognizeMe.Core.Game.Achievements;

namespace Shkoda.RecognizeMe.Core.Mechanics.Actions.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
   public abstract class GameActionHandler
    {
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

       public  GameFinishedEventArgs CreateGameFinishedEventArgs()
       {
           return new GameFinishedEventArgs();
       }

    }
}

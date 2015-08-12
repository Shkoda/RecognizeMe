using UnityEngine;
using System.Collections;
using Shkoda.RecognizeMe.Core.Graphics;

namespace Shkoda.RecognizeMe.Core.Mechanics.Actions
{
    public interface IAction
    {
        void Act(Mechanics mechanics);

        float Draw(Graphics.Graphics graphics);
    }
}
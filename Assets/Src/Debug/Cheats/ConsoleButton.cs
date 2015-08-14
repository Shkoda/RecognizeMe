#region imports

using UnityEngine;

#endregion

namespace Assets.Src.Debug.Cheats
{
    public abstract class ConsoleButton
    {
        protected ConsoleButton(float height, float offset)
        {
            Height = height;
            Offset = offset;
        }

        public float Height { get; private set; }
        public float Offset { get; private set; }
        public string Name { get; protected set; }
        public abstract void Perform();

        public override string ToString()
        {
            return Name;
        }
    }

    public class CheatButton : ConsoleButton
    {
        public CheatButton(Performable performable, float height, float offset)
            : base(height, offset)
        {
            Name = performable.Name;
            Action = performable;
        }

        public Performable Action { get; private set; }

        public override void Perform()
        {
            Action.Perform();
        }
    }

    public class ScriptButton : ConsoleButton
    {
        public ScriptButton(MonoBehaviour sript, float height, float offset)
            : base(height, offset)
        {
            Name = sript.GetType().ToString();
            Script = sript;
        }

        public MonoBehaviour Script { get; private set; }

        public override void Perform()
        {
            Script.enabled = !Script.enabled;
        }
    }
}
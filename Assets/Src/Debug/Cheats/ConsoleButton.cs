namespace Assets.Src.Debug.Cheats
{
    using UnityEngine;

    public abstract class ConsoleButton
    {
        protected ConsoleButton(float height, float offset)
        {
            this.Height = height;
            this.Offset = offset;
        }

        public float Height { get; private set; }

        public float Offset { get; private set; }

        public string Name { get; protected set; }

        public abstract void Perform();

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class CheatButton : ConsoleButton
    {
        public CheatButton(Performable performable, float height, float offset)
            : base(height, offset)
        {
            this.Name = performable.Name;
            this.Action = performable;
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
            this.Name = sript.GetType().ToString();
            this.Script = sript;
        }

        public MonoBehaviour Script { get; private set; }

        public override void Perform()
        {
            Script.enabled = !Script.enabled;
        }
    }
}
namespace GlobalPlay.Tools
{
    using JetBrains.Annotations;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof (Text))]
    public sealed class Localize : MonoBehaviour
    {
        private Text label;

        [UsedImplicitly]
        private void Awake()
        {
            label = gameObject.GetComponent<Text>();
        }

        [UsedImplicitly]
        private void Start()
        {
            // Localizer initialized before this
            this.DoLocalize();
        }

        public void DoLocalize(params object[] args)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            try
            {
                string tmp;
                Localizer.Instance.DoLocalizeAndFormat(label.text, out tmp, args);
                label.text = tmp;
            }
            catch (System.Exception)
            {
                Debug.LogError("Localization failed for " + gameObject.name);
            }
        }
    }
}
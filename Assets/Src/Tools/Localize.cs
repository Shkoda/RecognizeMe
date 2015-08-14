#region imports

using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace GlobalPlay.Tools
{
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
            DoLocalize();
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
            catch (Exception)
            {
                Debug.LogError("Localization failed for " + gameObject.name);
            }
        }
    }
}
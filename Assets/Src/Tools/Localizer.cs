namespace GlobalPlay.Tools
{
    using JetBrains.Annotations;
    using System.Collections.Generic;
    using System.Xml;
    using UnityEngine;

    public sealed class Localizer : MonoBehaviour
    {
        private Dictionary<string, Dictionary<string, string>> languages;

        private Dictionary<string, TextAsset> filesCache;

        /// <summary>
        /// Strings collection for current language.
        /// </summary>
        public Dictionary<string, string> Strings { get; private set; }

        [EditorAssigned] public TextAsset[] xmlFiles;

        public static Localizer Instance { get; private set; }

        public bool DoLocalizeAndFormat(string key, out string result, params object[] args)
        {
            result = key;
            bool localized = false;
            string val = null;

            if (this.Strings.TryGetValue(key, out val))
            {
                result = val;

                if (args.Length > 0)
                {
                    result = string.Format(result, args);
                }
                localized = true;
            }

            return localized;
        }

        [UsedImplicitly]
        private void Awake()
        {
            Instance = this;
            this.Strings = new Dictionary<string, string>();

#if UNITY_EDITOR
            PlayerPrefs.SetString("Language", "EN");
#endif

            SetLanguageFromSettings();
        }

        private void SetLanguageFromSettings()
        {
            if (!PlayerPrefs.HasKey("Language"))
            {
                Debug.LogError("Failed to load localization preference key");
                return;
            }

            string langName = PlayerPrefs.GetString("Language").ToLower();

            if (filesCache == null)
            {
                StoreFilesCache();
            }

            TextAsset file = null;
            if (filesCache.TryGetValue(langName, out file))
            {
                Strings = ReadXML(file);
                Debug.Log("Language set to " + langName);
            }
            else
            {
                Debug.LogError("Failed to load localization for language " + langName);
            }
        }

        private void StoreFilesCache()
        {
            filesCache = new Dictionary<string, TextAsset>();

            foreach (var file in xmlFiles)
            {
                if (file == null)
                {
                    continue;
                }

                string langName = GetLocale(file.name);

                if (filesCache.ContainsKey(langName))
                {
                    continue;
                }

                filesCache.Add(langName, file);
            }
        }

        private static string GetLocale(string fileName)
        {
            int idx = fileName.IndexOf('_') + 1;

            if (idx < 0 || idx + 1 > fileName.Length)
            {
                Debug.LogError("Invalid localization file name: " + fileName);
                return null;
            }

            string langName = fileName.Substring(idx).ToLower();
            return langName;
        }

        //private void ReadAllXML()
        //{
        //    if (filesCache == null)
        //    {
        //        StoreFilesCache();
        //    }

        //    languages = new Dictionary<string, Dictionary<string, string>>();

        //    foreach (var kvp in filesCache)
        //    {
        //        string langName = kvp.Key;
        //        TextAsset file = kvp.Value;
        //        var lang = ReadXML(file);
        //        languages.Add(langName, lang);
        //    }
        //}

        private Dictionary<string, string> ReadXML(TextAsset file)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(file.text);
            XmlNodeList wordsList = xmlDoc.GetElementsByTagName("item");

            var obj = new Dictionary<string, string>();

            foreach (XmlNode word in wordsList)
            {
                string key = word.Attributes["key"].Value;
                string text = word.Attributes["text"].Value;

                if (obj.ContainsKey(key))
                {
                    Debug.LogWarning("Key duplicate found: " + key);
                    continue;
                }

                obj.Add(key, text);
            }
            return obj;
        }
    }
}
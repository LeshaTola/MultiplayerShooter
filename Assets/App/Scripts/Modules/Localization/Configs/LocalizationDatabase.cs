using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.Localization.Configs
{
    [CreateAssetMenu(fileName = "LocalizationDatabase", menuName = "Databases/Localization")]
    public class LocalizationDatabase : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "Language Key", ValueLabel = "Language Data")]
        [SerializeField]
        private Dictionary<string, LocalizationData> languages;

        public Dictionary<string, LocalizationData> Languages => languages;
    }


    public class LocalizationData
    {
        public string Name;
        public string Path;
        public Sprite Sprite;
    }
}
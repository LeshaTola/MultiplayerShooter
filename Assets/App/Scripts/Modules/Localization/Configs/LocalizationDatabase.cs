using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modules.Localization.Configs
{
    [CreateAssetMenu(fileName = "LocalizationDatabase", menuName = "Databases/Localization")]
    public class LocalizationDatabase : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "Language Key", ValueLabel = "Path To CSV File")]
        [SerializeField]
        private Dictionary<string, string> languages;

        public Dictionary<string, string> Languages => languages;
    }
}
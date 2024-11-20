using System;
using System.Collections.Generic;
using App.Scripts.Modules.FileProvider;
using App.Scripts.Modules.Localization.Configs;
using App.Scripts.Modules.Localization.Data;
using App.Scripts.Modules.Localization.Parsers;
using App.Scripts.Modules.Saves;

namespace App.Scripts.Modules.Localization
{
    public class LocalizationSystem : ILocalizationSystem
    {
        public event Action OnLanguageChanged;

        private LocalizationDatabase localizationDictionary;
        private string language;
        private IParser parser;
        private IFileProvider fileProvider;
        private IDataProvider<LocalizationData> dataProvider;

        private Dictionary<string, string> languageDictionary = new();

        public LocalizationSystem(
            LocalizationDatabase localizationDictionary,
            string language,
            IParser parser,
            IFileProvider fileProvider,
            IDataProvider<LocalizationData> dataProvider
        )
        {
            this.localizationDictionary = localizationDictionary;
            this.language = language;
            this.parser = parser;
            this.fileProvider = fileProvider;
            this.dataProvider = dataProvider;

            languageDictionary = new Dictionary<string, string>();

            LoadLocalization();
            ChangeLanguage(this.language);
        }

        public string Language => language;

        public void ChangeLanguage(string languageKey)
        {
            if (!localizationDictionary.Languages.ContainsKey(languageKey))
            {
                return;
            }

            language = languageKey;
            var localizationFile = fileProvider.GetTextAsset(
                localizationDictionary.Languages[language]
            );
            languageDictionary = parser.Parse(localizationFile.text);
            OnLanguageChanged?.Invoke();
            SaveLocalization(languageKey);
        }

        public IEnumerable<string> GetLanguages()
        {
            if (localizationDictionary == null)
            {
                return null;
            }

            return new List<string>(localizationDictionary.Languages.Keys);
        }

        private void LoadLocalization()
        {
            var loadedLanguage = dataProvider.GetData();
            if (loadedLanguage != null)
            {
                language = loadedLanguage.LanguageKey;
            }
        }

        private void SaveLocalization(string languageKey)
        {
            dataProvider.SaveData(new LocalizationData() {LanguageKey = languageKey,});
        }

        public string Translate(string key)
        {
            if (!languageDictionary.ContainsKey(key))
            {
                return key;
            }

            return languageDictionary[key];
        }
    }
}
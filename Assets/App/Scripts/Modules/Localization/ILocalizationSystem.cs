using System;
using System.Collections.Generic;
using App.Scripts.Modules.Localization.Configs;

namespace App.Scripts.Modules.Localization
{
    public interface ILocalizationSystem
    {
        event Action OnLanguageChanged;

        string Language { get; }

        void ChangeLanguage(string languageKey);
        Dictionary<string, LocalizationData> GetLanguages();
        string Translate(string key);
    }
}
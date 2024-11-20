using System;
using System.Collections.Generic;

namespace App.Scripts.Modules.Localization
{
    public interface ILocalizationSystem
    {
        event Action OnLanguageChanged;

        string Language { get; }

        void ChangeLanguage(string languageKey);
        IEnumerable<string> GetLanguages();
        string Translate(string key);
    }
}
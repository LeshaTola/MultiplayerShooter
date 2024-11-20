using System.Collections.Generic;

namespace App.Scripts.Modules.Localization.Parsers
{
    public interface IParser
    {
        public Dictionary<string, string> Parse(string localizationFile);
    }
}
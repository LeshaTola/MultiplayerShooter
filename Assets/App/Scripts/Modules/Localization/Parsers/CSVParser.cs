using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace App.Scripts.Modules.Localization.Parsers
{
    public class CSVParser : IParser
    {
        private char lineSeparator = '\n';

        public Dictionary<string, string> Parse(string localizationFile)
        {
            var lines = localizationFile.Split(lineSeparator);

            var parsedLanguage = GetDictionary(lines);
            return parsedLanguage;
        }

        private Dictionary<string, string> GetDictionary(string[] lines)
        {
            var regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            Dictionary<string, string> parsedLanguage = new();
            for (var i = 1; i < lines.Length; i++)
            {
                var line = lines[i];

                var fields = regex.Split(line);
                for (var j = 0; j < fields.Length; j++)
                {
                    fields[j] = RemoveQuotes(fields[j]);
                }

                var key = fields[0];
                if (parsedLanguage.ContainsKey(key))
                {
                    continue;
                }

                var value = fields[1];
                parsedLanguage.Add(key, value);
            }

            return parsedLanguage;
        }

        string RemoveQuotes(string field)
        {
            field = field.Replace("\r", "");
            if(field[0].Equals('\"'))
                field = field.Substring(1, field.Length - 2);
            field = field.Replace("\"\"", "\"");
            return field;
        }
    }
}
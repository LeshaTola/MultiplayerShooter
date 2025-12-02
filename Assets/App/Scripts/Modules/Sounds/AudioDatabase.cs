using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Modules.Sounds
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "Databases/Audio")]
    public class AudioDatabase : SerializedScriptableObject
    {
        [field: SerializeField]
        public Dictionary<string, List<AudioClip>> Audios { get; private set; }

        [SerializeField, FolderPath(AbsolutePath = false)]
        private string _generatedScriptFolder = "Assets/Scripts/Generated";

        [Button("Generate Key Class")]
        private void GenerateKeyClass()
        {
#if UNITY_EDITOR
            if (!ValidateGenerationPath())
                return;

            string classCode = GenerateClassCode("AudioKeys", Audios.Keys);
            SaveClassToFile("AudioKeys", classCode);
#endif
        }

        public List<string> GetKeys()
        {
            return new List<string>(Audios.Keys);
        }

#if UNITY_EDITOR
        private bool ValidateGenerationPath()
        {
            if (string.IsNullOrWhiteSpace(_generatedScriptFolder))
            {
                Debug.LogError("Generation path is not specified.");
                return false;
            }

            return true;
        }

        private string GenerateClassCode(string className, IEnumerable<string> keys)
        {
            var sb = new StringBuilder();

            sb.AppendLine("// This class is auto-generated. Do not edit manually.");
            sb.AppendLine("public static class " + className);
            sb.AppendLine("{");

            foreach (var key in keys)
            {
                if (string.IsNullOrEmpty(key)) continue;

                string constName = SanitizeKey(key);
                sb.AppendLine($"    public const string {constName} = \"{key}\";");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private void SaveClassToFile(string className, string classCode)
        {
            Directory.CreateDirectory(_generatedScriptFolder);

            string filePath = Path.Combine(_generatedScriptFolder, className + ".cs");
            File.WriteAllText(filePath, classCode);

            AssetDatabase.Refresh();
            Debug.Log($"Class '{className}' was successfully generated at: {filePath}");
        }

        private static string SanitizeKey(string input)
        {
            var sb = new StringBuilder();

            if (!char.IsLetter(input[0]) && input[0] != '_')
                sb.Append('_');

            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c) || c == '_')
                    sb.Append(c);
                else
                    sb.Append('_');
            }

            return sb.ToString();
        }
#endif
    }
}

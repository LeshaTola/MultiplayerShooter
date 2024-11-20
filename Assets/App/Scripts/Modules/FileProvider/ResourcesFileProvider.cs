using UnityEngine;

namespace App.Scripts.Modules.FileProvider
{
    public class ResourcesFileProvider : IFileProvider
    {
        public TextAsset GetTextAsset(string path)
        {
            return Resources.Load<TextAsset>(path);
        }
    }
}
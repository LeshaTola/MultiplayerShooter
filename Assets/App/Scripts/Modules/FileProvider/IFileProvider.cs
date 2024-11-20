using UnityEngine;

namespace App.Scripts.Modules.FileProvider
{
    public interface IFileProvider
    {
        TextAsset GetTextAsset(string path);
    }
}
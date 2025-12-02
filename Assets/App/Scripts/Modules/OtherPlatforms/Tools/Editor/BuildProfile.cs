using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Modules.OtherPlatforms.Tools.Editor
{
    public enum AndroidBuildFormat
    {
        APK,
        AAB
    }


    [CreateAssetMenu(menuName = "Configs/BuildProfiles/BuildProfile")]
    public class BuildProfile : ScriptableObject
    {
        public string _profileName;
        public PlatformType _platformType;
        public string _outputPath;
        public string[] _addDefineSymbols;

        [Space,ShowIf("IsWebGL")] public string _webGLTemplate;
        [ShowIf("IsWebGL")] public WebGLCompressionFormat _webGLCompression = WebGLCompressionFormat.Disabled;
        [ShowIf("IsWebGL")] public bool _isYandex;

        // --- Android ---
        [Space,ShowIf("IsAndroid")] public AndroidBuildFormat _androidBuildFormat;
        [Space,ShowIf("IsAndroid")] public string _keystorePath;
        [ShowIf("IsAndroid")] public string _keystorePassword;
        [ShowIf("IsAndroid")] public string _keyAliasName;
        [ShowIf("IsAndroid")] public string _keyAliasPassword;

        private bool IsWebGL() => _platformType == PlatformType.WebGL;
        private bool IsAndroid() => _platformType == PlatformType.Android;
    }
}
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Modules.OtherPlatforms.Tools.Editor
{
    public static class BuildProfileApplier
    {
        public static void Apply(BuildProfile profile)
        {
            if (profile == null)
            {
                return;
            }
            
            // Switch platform
            var group = BuildPipeline.GetBuildTargetGroup(ToBuildTarget(profile._platformType));
            EditorUserBuildSettings.SwitchActiveBuildTarget(group, ToBuildTarget(profile._platformType));

            // Apply Define Symbols
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';').ToList();

            foreach (var d in profile._addDefineSymbols)
                if (!defines.Contains(d)) defines.Add(d);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defines));
            
            // Apply WebGL settings
            if (profile._platformType == PlatformType.WebGL)
            {
                PlayerSettings.WebGL.template = profile._webGLTemplate;
                PlayerSettings.WebGL.compressionFormat = profile._webGLCompression;
            }

            // 4. Android — формат сборки (ААБ или АПК)
            if (profile._platformType == PlatformType.Android)
            {
                EditorUserBuildSettings.buildAppBundle =
                    profile._androidBuildFormat == AndroidBuildFormat.AAB;
                
                if (!string.IsNullOrEmpty(profile._keystorePath))
                {
                    PlayerSettings.Android.useCustomKeystore = true;
                    PlayerSettings.Android.keystoreName = profile._keystorePath;
                    PlayerSettings.Android.keystorePass = profile._keystorePassword;
                    PlayerSettings.Android.keyaliasName = profile._keyAliasName;
                    PlayerSettings.Android.keyaliasPass = profile._keyAliasPassword;
                }
            }
            
            Debug.Log($"✅ Build profile '{profile._profileName}' applied.");
        }

        public static void Remove(BuildProfile profile)
        {
            if (profile == null)
            {
                return;
            }
            var group = BuildPipeline.GetBuildTargetGroup(ToBuildTarget(profile._platformType));
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';').ToList();

            foreach (var d in profile._addDefineSymbols)
                if (defines.Contains(d)) defines.Remove(d);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defines));
            Debug.Log($"✅ Build profile '{profile._profileName}' removed.");

        }
        
        public static BuildTarget ToBuildTarget(PlatformType platform)
        {
            switch (platform)
            {
                case PlatformType.WebGL:
                    return BuildTarget.WebGL;

                case PlatformType.Android:
                    return BuildTarget.Android;

                default:
                    throw new System.Exception("Unknown platform: " + platform);
            }
        }
    }
}
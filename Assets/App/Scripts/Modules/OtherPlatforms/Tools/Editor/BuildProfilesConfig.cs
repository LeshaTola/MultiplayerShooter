using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using YG;

namespace App.Scripts.Modules.OtherPlatforms.Tools.Editor
{
    [CreateAssetMenu(menuName = "Configs/BuildProfiles/BuildProfiles")]
    public class BuildProfilesConfig : ScriptableObject
    {
        [ReadOnly] public BuildProfile _currentProfile;
        public List<BuildProfile> _profiles;
        public InfoYG InfoYg;
        
        [Header("Global Version / Bundle Settings")]
        public string _bundleVersion = "1.32";
        public int _androidVersionCode = 32;
    }
}
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace App.Scripts.Modules.OtherPlatforms
{
    public class DefineMaskComponentDisabler : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField] private string[] _selectedDefines;
        [SerializeField] private Behaviour[] _targetComponents;

        private void OnValidate()
        {
#if UNITY_EDITOR
            string defines =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            string[] allDefines = defines.Split(';');

            bool found = _selectedDefines.Any(d => allDefines.Contains(d));

            foreach (var comp in _targetComponents)
            {
                if (comp != null)
                    comp.enabled = found;
            }
#endif
        }
#endif
    }
}

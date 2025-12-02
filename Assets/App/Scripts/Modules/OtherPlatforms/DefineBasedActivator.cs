#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Linq;
using UnityEngine;

namespace App.Scripts.Modules.OtherPlatforms
{
    public class DefineMaskActivator : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField] public string[] selectedDefines;

        private void OnValidate()
        {
#if UNITY_EDITOR
            string defines =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            string[] allDefines = defines.Split(';');

            bool found = selectedDefines.Any(d => allDefines.Contains(d));
            gameObject.SetActive(found);
#endif
        }
#endif
    }
}

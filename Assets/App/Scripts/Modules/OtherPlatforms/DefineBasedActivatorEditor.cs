#if UNITY_EDITOR
using UnityEditor;

using System.Linq;
using UnityEngine;

namespace App.Scripts.Modules.OtherPlatforms
{
    [CustomEditor(typeof(DefineMaskActivator))]
    public class DefineMaskActivatorEditor : Editor
    {
#if UNITY_EDITOR
        private string[] _allDefines;
        private int _mask;

        private void OnEnable()
        {
            string defines =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            _allDefines = defines.Split(';');
        }

        public override void OnInspectorGUI()
        {
            var script = (DefineMaskActivator) target;

            // Восстанавливаем маску из выбранных
            _mask = 0;
            for (int i = 0; i < _allDefines.Length; i++)
            {
                if (script.selectedDefines != null && script.selectedDefines.Contains(_allDefines[i]))
                    _mask |= 1 << i;
            }

            // Рисуем маску
            int newMask = EditorGUILayout.MaskField("Defines", _mask, _allDefines);

            // Если изменилось — обновляем список
            if (newMask != _mask)
            {
                _mask = newMask;
                script.selectedDefines = _allDefines
                    .Where((d, i) => (_mask & (1 << i)) != 0)
                    .ToArray();

                EditorUtility.SetDirty(script);
            }

            DrawDefaultInspector(); // чтобы видеть selectedDefines (можно убрать)
        }
#endif
    }
}
#endif


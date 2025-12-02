using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Modules.OtherPlatforms.Tools.Editor
{
    [InitializeOnLoad]
    public static class BuildQueueExecutor
    {
        private const string QueueKey = "BUILD_PROFILE_QUEUE";
        private const string IsBuildingKey = "BUILD_QUEUE_ACTIVE";

        static BuildQueueExecutor()
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (!EditorPrefs.GetBool(IsBuildingKey, false))
                return;

            // Проверяем, можно ли продолжать
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
                return;

            ExecuteNext();
        }

        public static void StartQueue(List<string> profileGuids)
        {
            EditorPrefs.SetBool(IsBuildingKey, true);
            EditorPrefs.SetString(QueueKey, string.Join("|", profileGuids));
        }

        private static void ExecuteNext()
        {
            string raw = EditorPrefs.GetString(QueueKey, "");
            if (string.IsNullOrEmpty(raw))
            {
                Debug.Log("Build Queue completed!");
                EditorPrefs.SetBool(IsBuildingKey, false);
                return;
            }

            var parts = new List<string>(raw.Split('|'));

            string nextGuid = parts[0];
            parts.RemoveAt(0);

            EditorPrefs.SetString(QueueKey, string.Join("|", parts));

            var path = AssetDatabase.GUIDToAssetPath(nextGuid);
            var profile = AssetDatabase.LoadAssetAtPath<BuildProfile>(path);

            if (profile == null)
            {
                Debug.LogError($"Profile not found: {nextGuid}");
                return;
            }

            Debug.Log($"== Building profile: {profile._profileName}");

            BuildProfilesWindow.StartBuildForProfile(profile);
        }
    }
}
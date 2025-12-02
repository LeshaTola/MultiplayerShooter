using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using YG;

namespace App.Scripts.Modules.OtherPlatforms.Tools.Editor
{
    public class BuildProfilesWindow : EditorWindow
    {
        private BuildProfilesConfig _buildProfilesConfig;
        private Vector2 scroll;

        // Состояние чекбоксов для каждого профиля
        private Dictionary<BuildProfile, bool> _selectedProfiles = new Dictionary<BuildProfile, bool>();

        [MenuItem("Tools/Build Profiles")]
        public static void ShowWindow()
        {
            GetWindow<BuildProfilesWindow>("Build Profiles");
        }

        private void OnEnable()
        {
            _buildProfilesConfig = Resources.Load<BuildProfilesConfig>("_BuildProfilesConfig");

            if (_buildProfilesConfig?._profiles != null)
            {
                foreach (var profile in _buildProfilesConfig._profiles)
                {
                    if (!_selectedProfiles.ContainsKey(profile))
                        _selectedProfiles.Add(profile, false);
                }
            }
        }

        void OnGUI()
        {
            if (_buildProfilesConfig._profiles == null || _buildProfilesConfig._profiles.Count == 0)
            {
                GUILayout.Label("No Build Profiles found in Resources/");
                return;
            }

            // --- Глобальные версии и bundle --- //
            GUILayout.Label("Global Version / Bundle Settings", EditorStyles.boldLabel);
            _buildProfilesConfig._bundleVersion = EditorGUILayout.TextField("Bundle Version", _buildProfilesConfig._bundleVersion);
            _buildProfilesConfig._androidVersionCode = EditorGUILayout.IntField("Android Version Code", _buildProfilesConfig._androidVersionCode);
            GUILayout.Space(10);

            // --- Кнопки управления чекбоксами --- //
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("All", GUILayout.Width(50)))
                SetAllCheckboxes(true);
            if (GUILayout.Button("Clear", GUILayout.Width(50)))
                SetAllCheckboxes(false);
            if (GUILayout.Button("Invert", GUILayout.Width(50)))
                InvertCheckboxes();
            GUILayout.Space(20);
            if (GUILayout.Button("Build Selected", GUILayout.Width(120)))
                BuildSelectedProfiles();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            // --- Список профилей с чекбоксами --- //
            scroll = GUILayout.BeginScrollView(scroll);

            foreach (var profile in _buildProfilesConfig._profiles)
            {
                GUILayout.BeginHorizontal("box");

                _selectedProfiles[profile] = GUILayout.Toggle(_selectedProfiles[profile], "");

                GUILayout.Label(profile._profileName);

                if (GUILayout.Button("Apply", GUILayout.Width(100)))
                {
                    Apply(profile);
                }

                if (GUILayout.Button("Build", GUILayout.Width(100)))
                {
                    StartBuildForProfile(profile);
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }

        // --- Управление чекбоксами --- //
        private void SetAllCheckboxes(bool value)
        {
            var keys = _selectedProfiles.Keys.ToList();
            foreach (var key in keys)
                _selectedProfiles[key] = value;
        }

        private void InvertCheckboxes()
        {
            var keys = _selectedProfiles.Keys.ToList();
            foreach (var key in keys)
                _selectedProfiles[key] = !_selectedProfiles[key];
        }

        // --- Применить профиль --- //
        private void Apply(BuildProfile profile)
        {
            ApplyVersionCode(profile);
            BuildProfileApplier.Remove(_buildProfilesConfig._currentProfile);
            BuildProfileApplier.Apply(profile);
            _buildProfilesConfig._currentProfile = profile;
            EditorUtility.SetDirty(_buildProfilesConfig);
            AssetDatabase.SaveAssets();
        }

        private void ApplyVersionCode(BuildProfile profile)
        {
            PlayerSettings.bundleVersion = _buildProfilesConfig._bundleVersion;

            if(profile._platformType == PlatformType.Android)
            {
                PlayerSettings.Android.bundleVersionCode = _buildProfilesConfig._androidVersionCode;
            }

            if (profile._isYandex)
            {
                InfoYG.SetDefaultPlatform();
            }
            else
            {
                InfoYG.instance.Basic.platform = null;
            }
        }

        // --- Построение билдов --- //
        void BuildWithProfile(BuildProfile profile)
        {
            Apply(profile);

            BuildPlayerOptions opts = new BuildPlayerOptions()
            {
                target = BuildProfileApplier.ToBuildTarget(profile._platformType),
                targetGroup = BuildPipeline.GetBuildTargetGroup(BuildProfileApplier.ToBuildTarget(profile._platformType)),
                locationPathName = profile._outputPath,
                scenes = EditorBuildSettings.scenes
                    .Where(s => s.enabled)
                    .Select(s => s.path)
                    .ToArray()
            };

            BuildPipeline.BuildPlayer(opts);
        }

        private void BuildSelectedProfiles()
        {
            var guids = new List<string>();

            foreach (var kvp in _selectedProfiles)
            {
                if (kvp.Value)
                {
                    string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(kvp.Key));
                    guids.Add(guid);
                }
            }

            if (guids.Count == 0)
            {
                Debug.LogWarning("No profiles selected.");
                return;
            }

            Debug.Log("Starting build queue…");

            BuildQueueExecutor.StartQueue(guids);
        }

        public static void StartBuildForProfile(BuildProfile profile)
        {
            var window = GetWindow<BuildProfilesWindow>();

            window.Apply(profile);

            BuildPlayerOptions opts = new BuildPlayerOptions()
            {
                target = BuildProfileApplier.ToBuildTarget(profile._platformType),
                targetGroup = BuildPipeline.GetBuildTargetGroup(BuildProfileApplier.ToBuildTarget(profile._platformType)),
                locationPathName = profile._outputPath,
                scenes = EditorBuildSettings.scenes
                    .Where(s => s.enabled)
                    .Select(s => s.path)
                    .ToArray()
            };

            BuildPipeline.BuildPlayer(opts);
        }

        
    }
}

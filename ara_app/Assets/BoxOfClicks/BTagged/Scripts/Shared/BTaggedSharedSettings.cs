/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace BOC.BTagged.Shared
{
    public class BTaggedSharedSettings : ScriptableObject
    {
        static string bTaggedSettingsName = "BTaggedSettings.asset";
        static public string BTaggedSettingsPath
        {
            get
            {
                return Path.Combine(BTaggedSharedUtils.GetASMDEFDirectory(@"BOC.BTagged.Editor"), bTaggedSettingsName);
            }
        }

        public enum SearchRegistryOption
        {
            FullRefresh,
            IterativeRefresh,
            CachedResultsOnly
        }


        [SerializeField]
        public bool showAssetReferences = false;
        static public bool ShowAssetReferences
        {
            get
            {
                return GetOrCreateSettings().showAssetReferences;
            }
            set
            {
                GetOrCreateSettings().showAssetReferences = value;
            }
        }

        static public SearchRegistryOption SearchMode => GetOrCreateSettings().searchMode;
        static public bool DisableEditorSafetyChecks => GetOrCreateSettings().disableEditorChecks;

        [SerializeField]
        public bool showGroupNames;
        static public bool ShowGroupNames
        {
            get
            {
                return GetOrCreateSettings().showGroupNames;
            }
        }

        [SerializeField]
        public bool useOdinSO;

        [SerializeField]
        public bool showHashes;
        static public bool ShowHashes
        {
            get
            {
                return GetOrCreateSettings().showHashes;
            }
        }

        [SerializeField]
        public SearchRegistryOption searchMode;

        [SerializeField]
        public bool disableEditorChecks;

        static BTaggedSharedSettings LoadedSettings = null;
        internal static BTaggedSharedSettings GetOrCreateSettings()
        {
            if (LoadedSettings != null) return LoadedSettings;

            string fullPath = BTaggedSettingsPath;
            var settings = LoadedSettings = AssetDatabase.LoadAssetAtPath<BTaggedSharedSettings>(fullPath);
            if (settings == null)
            {
                LoadedSettings = settings = ScriptableObject.CreateInstance<BTaggedSharedSettings>();
                settings.searchMode = SearchRegistryOption.IterativeRefresh;
                settings.showGroupNames = true;
                settings.showHashes = true;
                settings.disableEditorChecks = false;
                AssetDatabase.CreateAsset(settings, fullPath);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}
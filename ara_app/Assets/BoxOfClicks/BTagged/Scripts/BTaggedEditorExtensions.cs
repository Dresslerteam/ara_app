#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BOC.BTagged
{
    public class BTaggedEditorExtensions
    {
        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            EditorApplication.playModeStateChanged += state => ClearListeners(state);
        }

        static private void ClearListeners(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode || state == PlayModeStateChange.ExitingPlayMode) return;
            ClearListeners();
        }

        static private void ClearListeners()
        {
            BTagged.AwakeComplete = false;
            BTagged.StoreTagNames = false;
            BTagged._AllTaggedGOs.Clear();
            BTagged._AllTaggedGOsByIndex.Clear();
            BTagged.AllTags.Clear();
            BTagged.OnGameObjectStartListeners.Clear();
            BTagged.OnGameObjectEnabledListeners.Clear();
            BTagged.OnGameObjectDisabledListeners.Clear();
            BTagged.OnGameObjectDestroyedListeners.Clear();
            TagGameObjectMasterUpdate.TaggedGameObjectsWithEvents.Clear();
        }
    }
}
#endif
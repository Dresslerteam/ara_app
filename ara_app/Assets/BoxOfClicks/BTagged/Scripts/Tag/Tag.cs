/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace BOC.BTagged
{
    [CreateAssetMenu(fileName = "Tag", menuName = "BTagged/Tag")]
    public class Tag : BTaggedSO
    {
        public Action<GameObject> OnGameObjectStart;
        public Action<GameObject> OnGameObjectEnabled;
        public Action<GameObject> OnGameObjectDisabled;
        public Action<GameObject> OnGameObjectDestroyed;

        private void OnEnable()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += state => ClearListeners(state);
#endif
        }

#if UNITY_EDITOR
        private void ClearListeners(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode || state == PlayModeStateChange.ExitingPlayMode) return;
            ClearListeners();
        }
#endif
        private void ClearListeners()
        {
            OnGameObjectStart = null;
            OnGameObjectEnabled = null;
            OnGameObjectDisabled = null;
            OnGameObjectDestroyed = null;
        }

        public static implicit operator Tag(string tagName) => BTagged.ByName(tagName);
    }
}

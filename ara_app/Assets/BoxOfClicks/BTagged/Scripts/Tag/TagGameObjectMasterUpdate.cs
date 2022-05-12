using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BOC.BTagged
{
    public interface ITaggedGameObject
    {
        void InvokeIfRequired();
    }

    public class TagGameObjectMasterUpdate : MonoBehaviour
    {
        static GameObject TaggedGameObjectUpdater = null;
        static internal List<ITaggedGameObject> TaggedGameObjectsWithEvents = new List<ITaggedGameObject>();
        public static void Init()
        {
            if (TaggedGameObjectUpdater == null)
            {
                TaggedGameObjectUpdater = new GameObject("TaggedGameObjectUpdater", typeof(TagGameObjectMasterUpdate));
                TaggedGameObjectUpdater.hideFlags = HideFlags.HideAndDontSave;
            }
        }
        static internal bool AddIfListening(ITaggedGameObject taggedGO, Action<GameObject> action)
        {
            if (action != null)// || !BTagged.AwakeComplete)
            {
                if (!TaggedGameObjectsWithEvents.Contains(taggedGO)) TaggedGameObjectsWithEvents.Add(taggedGO);
                return true;
            }
            return false;
        }
        static internal void RemoveIfListening(ITaggedGameObject taggedGO)
        {
            if (TaggedGameObjectsWithEvents.Contains(taggedGO)) TaggedGameObjectsWithEvents.Remove(taggedGO);
        }
        private void LateUpdate()
        {
            for (int i = 0; i < TaggedGameObjectsWithEvents.Count; ++i)
            {
                TaggedGameObjectsWithEvents[i].InvokeIfRequired();
            }
            BTagged.CheckQueuedGlobalQueriesLateUpdate();
#if UNITY_EDITOR
            BTagged.ResetWarnCount();
#endif
        }
    }

}

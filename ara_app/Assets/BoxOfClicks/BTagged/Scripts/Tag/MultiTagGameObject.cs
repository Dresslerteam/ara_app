/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
#if UNITY_ENTITIES
using Unity.Entities;
using Unity.Collections;
#endif

namespace BOC.BTagged
{
    [HelpURL("https://docs.google.com/document/d/10FXOb7qEpLLTN9G8t8vi6b_JdCpanXJwkyyHINyU9Z0/")]
    [AddComponentMenu("BTagged/Multi-Tag", 1)]
    public class MultiTagGameObject : MonoBehaviour, ITaggedGameObject
#if UNITY_ENTITIES
        , IConvertGameObjectToEntity
#endif
    {
        // The Tag - replaces Component.tag mostly to avoid confusion and
        // as I believe it is rarely used
        public Tag[] tags = new Tag[0];
        // If changing the tags at runtime please use 'SetTag()' to ensure the new tag is registered
        public void SetTags(Tag[] newTags)
        {
            UnRegisterTags();
            tags = newTags;
            RegisterTags();
        }

        bool initialized = false;
        void Awake()
        {
            if (!initialized)
            {
                BTagged.AwakeComplete = false;
                Init();
            }
        }
        public void Init()
        {
            if (!initialized) RegisterTags();
        }

        void RegisterTags()
        {
            if (tags == null) return;
            initialized = Application.isPlaying;
            for (int t = 0; t < tags.Length; ++t)
            {
                var tag = tags[t];
                // If the tag is set to default / empty then don't tag this GameObject
                if (tag == null || tag.IsDefault) continue;
                BTagged.Register(gameObject, tag);
            }
        }
        private void UnRegisterTags()
        {
            if (tags == null) return;
            for (int t = 0; t < tags.Length; ++t)
            {
                if (tags[t] == null || tags[t].IsDefault) continue;
                BTagged.Unregister(gameObject, tags[t]);
            }
        }

        void Start()
        {
            BTagged.AwakeComplete = true;
            if (tags == null) return;
            for (int t = 0; t < tags.Length; ++t)
            {
                triggerOnStart = TagGameObjectMasterUpdate.AddIfListening(this, tags[t]?.OnGameObjectStart);
                if (triggerOnStart) break;
            }
            if (BTagged.HasGlobalListeners) BTagged.CheckGlobalQueries(transform, BTagged.GOEventType.Start);
        }

        // Invoke in LateUpdate, giving user scripts an opportunity to Awake/Enable
        bool triggerOnStart = false;
        internal void TriggerOnStart()
        {
            triggerOnStart = false;
            if (tags == null) return;
            for (int t = 0; t < tags.Length; ++t)
            {
                if (tags[t] != null) tags[t].OnGameObjectStart?.Invoke(gameObject);
            }
        }

        // Invoke in LateUpdate, giving user scripts an opportunity to Awake/Enable
        bool triggerOnEnable = false;
        void OnEnable()
        {
            if (tags == null) return;
            for (int t = 0; t < tags.Length; ++t)
            {
                triggerOnEnable = TagGameObjectMasterUpdate.AddIfListening(this, tags[t]?.OnGameObjectEnabled);
                if (triggerOnEnable) break;
            }
            if (BTagged.HasGlobalListeners) BTagged.CheckGlobalQueries(transform, BTagged.GOEventType.Enabled);
        }

        internal void TriggerOnEnable()
        {
            triggerOnEnable = false;
            if (tags == null) return;
            for (int t = 0; t < tags.Length; ++t)
            {
                tags[t]?.OnGameObjectEnabled?.Invoke(gameObject);
            }
        }

        void ITaggedGameObject.InvokeIfRequired()
        {
            if (triggerOnEnable) TriggerOnEnable();
            if (triggerOnStart) TriggerOnStart();
        }

        void OnDisable()
        {
            if (tags == null) return;
            for (int t = 0; t < tags.Length; ++t)
            {
                tags[t].OnGameObjectDisabled?.Invoke(gameObject);
            }
            if (BTagged.HasGlobalListeners) BTagged.CheckGlobalQueries(transform, BTagged.GOEventType.Disabled);
        }
        void OnDestroy()
        {
            if (BTagged.HasGlobalListeners) BTagged.CheckGlobalQueries(transform, BTagged.GOEventType.Destroyed);
            if (tags != null)
            {
                for (int t = 0; t < tags.Length; ++t)
                {
                    if (tags[t] == null || tags[t].IsDefault) continue;
                    tags[t].OnGameObjectDestroyed?.Invoke(gameObject);
                    BTagged.Unregister(gameObject, tags[t]);
                }
            }
            TagGameObjectMasterUpdate.RemoveIfListening(this);
        }


#if UNITY_ENTITIES
        // If the Unity.Entities package is present, support Converting to entities
        // It's possible to tag a GameObject multiple times therefore using a buffer/BlobArray.
        // We also choose not to store the data in the chunk and use a BlobAssetReference instead.
        // This is beneficial when instantiating Prefabs and also keeps the chunk utilisation higher than it might be otherwise
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            // For each converting GameObject, we want to create and Populate a TagICD just once, 
            // using all the BTaggedTagGameObject components associated with this GameObject.
            // If the Tag already exists we assume the conversion for this GO/Entity has already occured.
            if (conversionSystem.EntityManager.HasComponent<TagICD>(entity)) return;

            BlobAssetReference<BTaggedBlobAsset> blobAssetReference = default;

            // Select all BTaggedTagGameObject 'Tags' and use the utility method to create a BlobAssetReference of them
            List<Tag> allTags = gameObject.GetComponents<TagGameObject>().Select(btgo => btgo.tag).ToList();
            List<Tag[]> multiTags = gameObject.GetComponents<MultiTagGameObject>().Select(btgo => btgo.tags).ToList();
            for(int m = 0; m < multiTags.Count; ++m) allTags.AddRange(multiTags[m]);
            var validTags = allTags.TakeWhile(x => x != null).ToArray();
            BTagged.PopulateBlobAssetRefenceForTags(validTags, ref blobAssetReference);
            dstManager.AddComponentData(entity, new TagICD() { Blob = blobAssetReference });
        }
#endif
    }
}

/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor;
#if UNITY_ENTITIES
using Unity.Entities;
using Unity.Collections;
#endif

namespace BOC.BTagged
{
    [HelpURL("https://docs.google.com/document/d/10FXOb7qEpLLTN9G8t8vi6b_JdCpanXJwkyyHINyU9Z0/")]
    [AddComponentMenu("BTagged/Tag", 1)]
    public class TagGameObject : MonoBehaviour, ITaggedGameObject
#if UNITY_ENTITIES
        , IConvertGameObjectToEntity
#endif
    {
        // The Tag - replaces Component.tag mostly to avoid confusion and
        // as I believe it is rarely used
        public new Tag tag;

        // If changing the tag at runtime please use 'SetTag()' to ensure the new tag is registered
        public void SetTag(Tag newTag)
        {
            if (newTag == tag) return;
            if (tag != null && !tag.IsDefault) BTagged.Unregister(gameObject, tag);
            tag = newTag;
            if (tag != null && !tag.IsDefault) BTagged.Register(gameObject, tag);
            initialized = Application.isPlaying;
        }
        public string TagLabel() => (tag == null ? "None" : tag.name);


        bool initialized = false;

        void Awake()
        {
            TagGameObjectMasterUpdate.Init();
            //SetTag could have been called before awake
            if (!initialized)
            {
                BTagged.AwakeComplete = false;
                Init();
            }
        }
        public void Init()
        {
            //Debug.Log("Init called on " + this + ". Already initialized? " + initialized);
            if (initialized) return;

            initialized = Application.isPlaying;
            // If the tag is set to default / empty then don't tag this GameObject
            if (tag == null || tag.IsDefault) return;
            BTagged.Register(gameObject, tag);
        }

        void Start()
        {
            BTagged.AwakeComplete = true;
            triggerOnStart = TagGameObjectMasterUpdate.AddIfListening(this, tag?.OnGameObjectStart);
            if (BTagged.HasGlobalListeners) BTagged.CheckGlobalQueries(transform, BTagged.GOEventType.Start);
        }

        // Invoke in LateUpdate, giving user scripts an opportunity to Awake/Enable
        bool triggerOnStart = false;
        internal void TriggerOnStart()
        {
            triggerOnStart = false;
            tag?.OnGameObjectStart?.Invoke(gameObject);
        }

        // Invoke in LateUpdate, giving user scripts an opportunity to Awake/Enable
        bool triggerOnEnable = false;
        void OnEnable()
        {
            triggerOnEnable = TagGameObjectMasterUpdate.AddIfListening(this, tag?.OnGameObjectEnabled);
            if (BTagged.HasGlobalListeners) BTagged.CheckGlobalQueries(transform, BTagged.GOEventType.Enabled);
        }
        internal void TriggerOnEnable()
        {
            triggerOnEnable = false;
            tag?.OnGameObjectEnabled?.Invoke(gameObject);
        }

        public void InvokeIfRequired()
        {
            if (triggerOnEnable) TriggerOnEnable();
            if (triggerOnStart) TriggerOnStart();
        }

        void OnDisable()
        {
            if (tag != null) tag.OnGameObjectDisabled?.Invoke(gameObject);
            if (BTagged.HasGlobalListeners) BTagged.CheckGlobalQueries(transform, BTagged.GOEventType.Disabled);
        }
        void OnDestroy()
        {
            if (BTagged.HasGlobalListeners) BTagged.CheckGlobalQueries(transform, BTagged.GOEventType.Destroyed);
            if (tag != null)
            {
                tag.OnGameObjectDestroyed?.Invoke(gameObject);
                // When this GameObject is destroyed, remove its reference from the lookup list
                BTagged.Unregister(gameObject, tag);
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

            // Don't tag this entity if the Tag is set to default/empty
            if (tag == null || tag.IsDefault) return;

            BlobAssetReference<BTaggedBlobAsset> blobAssetReference = default;

            // Select all BTaggedTagGameObject 'Tags' and use the utility method to create a BlobAssetReference of them
            List<Tag> allTags = gameObject.GetComponents<TagGameObject>().Select(btgo => btgo.tag).ToList();
            List<Tag[]> multiTags = gameObject.GetComponents<MultiTagGameObject>().Select(btgo => btgo.tags).ToList();
            for (int m = 0; m < multiTags.Count; ++m) allTags.AddRange(multiTags[m]);
            var validTags = allTags.TakeWhile(x => x != null).ToArray();
            BTagged.PopulateBlobAssetRefenceForTags(validTags, ref blobAssetReference);
            dstManager.AddComponentData(entity, new TagICD() { Blob = blobAssetReference });
        }
#endif
    }
}

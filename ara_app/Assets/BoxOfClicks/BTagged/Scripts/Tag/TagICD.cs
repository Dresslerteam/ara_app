/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

#if UNITY_ENTITIES
using BOC.BTagged;
using Unity.Entities;

namespace BOC.BTagged
{
    public struct TagICD : IComponentData
    {
        public BlobAssetReference<BTaggedBlobAsset> Blob;
    }
}
#endif
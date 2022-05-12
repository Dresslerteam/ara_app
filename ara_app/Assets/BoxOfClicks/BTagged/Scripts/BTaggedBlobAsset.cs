#if UNITY_ENTITIES
using Unity.Entities;

namespace BOC.BTagged
{
    // An Entity can have multiple Hashes associated with it
    // These are all stored in a BlobArray and 
    // TagICD is used to hold a reference to the blob
    public struct BTaggedBlobAsset
    {
        public BlobArray<BHash128> hashes;
    }

}
#endif
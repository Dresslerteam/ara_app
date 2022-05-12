/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using UnityEngine;

namespace BOC.BTagged
{
#if ODIN_INSPECTOR && BTAGGED_ODIN_BASE
    public class BTaggedSO : Sirenix.OdinInspector.SerializedScriptableObject
#else
    public class BTaggedSO : ScriptableObject
#endif
    {
        // A ReadOnly unique hash created once for the asset
        [SerializeField, HideInInspector]
        BHash128 _Hash = GenerateDefaultGuid();
        public BHash128 Hash => _Hash;

        public bool IsDefault => !_Hash.IsValid;
        public string ShortName
        {
            get => (this == null ? "null" : (name.Contains("/") ? name.Substring(name.LastIndexOf("/") + 1) : name));
        }


        // The following property has been added for detecting Duplication of an asset
        // through Unity Editor. When this happens, the hash will no longer be unique.
        public void EditorGenerateNewHash() =>
            _Hash = GenerateDefaultGuid();

        // Updating a runtime asset's hash is sometimes required - in general this method should not be required
        public void ManuallySetHash(BHash128 hash) => _Hash = hash;

        // Can implicitly convert to a Hash
        public static implicit operator BHash128(BTaggedSO so) { return so == null ? default : so.Hash; }
        static unsafe BHash128 GenerateDefaultGuid()
        {
            var guid = System.Guid.NewGuid();
            var hash = new BHash128();
            hash = *(BHash128*)&guid;
            return hash;
        }
    }
}

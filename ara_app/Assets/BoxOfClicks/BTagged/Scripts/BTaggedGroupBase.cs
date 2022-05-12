/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using UnityEngine;

namespace BOC.BTagged
{
    public class BTaggedGroupBase : ScriptableObject{}

    public class BTaggedGroup<SO> : BTaggedGroupBase, ISerializationCallbackReceiver
        where SO : ScriptableObject
    {
        public SO[] Assets = new SO[0];

        public void OnAfterDeserialize(){}

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            var subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(this)).Where(x => x is SO).ToArray();
            if (subAssets.Length != Assets.Length)
            {
                Assets = new SO[subAssets.Length];
                for (int i = 0; i < subAssets.Length; ++i) Assets[i] = subAssets[i] as SO;
            }
#endif
        }
    }
}

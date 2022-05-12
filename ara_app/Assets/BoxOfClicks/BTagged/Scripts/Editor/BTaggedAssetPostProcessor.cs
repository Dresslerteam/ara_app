/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BOC.BTagged.EditorTools
{
    /// <summary>
    /// These methods are concerned with ensuring all BTagged assets have unique hashes,
    /// even if they are created through a Duplicate command
    /// </summary>
    public class BTaggedDetectDuplicates : UnityEditor.AssetModificationProcessor
    {
        public static List<string> newAssets = new List<string>();

        // Seems to reliably detect ctrl+drag for duplication but not Ctrl+D
        // Luckily there's no Right-click duplicate so an asset has to be selected before using Ctrl+D
        // So we can use the asset's editor to detect that instance - it will call RegenerateHashForAsset below
        static void OnWillCreateAsset(string aMetaAssetPath)
        {
            //string assetPath = aMetaAssetPath.Substring(0, aMetaAssetPath.Length - 5);
            //newAssets.Add(assetPath);
        }
    }

    public class BTaggedAssetPostProcessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            BTaggedSettingsProvider.CheckForCollisions();
        }

    }
}

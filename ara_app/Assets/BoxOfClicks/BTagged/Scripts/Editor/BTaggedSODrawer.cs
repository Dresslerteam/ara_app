/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using UnityEditor;
using UnityEngine;

namespace BOC.BTagged.EditorTools
{
    [CustomPropertyDrawer(typeof(BTaggedSO), true)]
    public class BTaggedSODrawer : BTaggedSOPropertyDrawerBase<BTaggedGroup<BTaggedSO>, BTaggedSO>
    {
        public BTaggedSODrawer()
        {
            label = "Asset";
            defaultLabel = "- None -";
            showHelpIcon = false;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent lbl)
        {
            base.OnGUI(position, property, lbl);
        }
    }
}

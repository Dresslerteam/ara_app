/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using UnityEditor;
using UnityEngine;

namespace BOC.BTagged.EditorTools
{
    [CustomPropertyDrawer(typeof(Tag), true)]
    public class BTaggedTagPropertyDrawer : BTaggedSOPropertyDrawerBase<TagGroup, Tag>
    {
        public BTaggedTagPropertyDrawer()
        {
            label = "Tag";
            defaultLabel = "- Untagged -";
            showHelpIcon = false;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent lbl)
        {
            base.OnGUI(position, property, lbl);
        }
    }
}

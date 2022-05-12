/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BOC.BTagged.EditorTools
{
    [CustomPropertyDrawer(typeof(TagQuery), true)]
    public class TagQueryDrawer : BTaggedSOPropertyDrawerBase<BTaggedGroupBase, TagQuery>
    {
        public TagQueryDrawer()
        {
            defaultLabel = "- None -";
        }
        //public override VisualElement CreatePropertyGUI(SerializedProperty property)
        //{
        //    return base.CreatePropertyGUI(property);
        //}
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent lbl)
        {
            //bool wideDisplay = position.width > 250f;
            //float actionWidth = wideDisplay ? 100f : 60f;
            //position.width -= actionWidth;
            //EditorGUI.PropertyField(position, property.FindPropertyRelative("query"), new GUIContent("Target"));
            //position.x += position.width + 5;
            //position.width = actionWidth - 5f;

            //EditorGUI.PropertyField(position, property.FindPropertyRelative("target"), new GUIContent("Target"));
            //EditorGUI.PropertyField(position, property.FindPropertyRelative("subQueries"), new GUIContent("Additional Queries"));
            //var actionProp = property.FindPropertyRelative("action");
            //actionProp.enumValueIndex = (int)(BTweenPlayAction)EditorGUI.EnumPopup(position, (BTweenPlayAction)actionProp.enumValueIndex);
            base.OnGUI(position, property, lbl);
        }
    }
}

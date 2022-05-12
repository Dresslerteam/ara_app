/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static BOC.BTagged.BTagged;

namespace BOC.BTagged.EditorTools
{
    [CustomPropertyDrawer(typeof(TagQueryWithTarget), true)]
    public class TagQueryWithTargetDrawer : BTaggedSOPropertyDrawerBase<BTaggedGroupBase, TagQuery>
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent lbl)
        {
            EditorGUI.BeginProperty(position, lbl, property);
            //EditorGUI.BeginChangeCheck();
            bool wideDisplay = position.width > 250f;
            float actionWidth = wideDisplay ? 100f : 60f;
            position.width -= actionWidth;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("query"), new GUIContent("Query"));
            position.x += position.width + 5;
            position.width = actionWidth - 5f;

            //EditorGUI.PropertyField(position, property.FindPropertyRelative("target"), new GUIContent("Target"), true);
            var actionProp = property.FindPropertyRelative("target");
            actionProp.enumValueIndex = (int)(Search)EditorGUI.EnumPopup(position, (Search)actionProp.enumValueIndex);
            //var search = (Search)(property.FindPropertyRelative("target").enumValueIndex);
            //var result = (Search)EditorGUI.EnumPopup(position, search);
            //if (result != search)
            //{
            //    property.serializedObject.ApplyModifiedProperties();
            //}
            ////var actionProp = property.FindPropertyRelative("action");
            ////actionProp.enumValueIndex = (int)(BTweenPlayAction)EditorGUI.EnumPopup(position, (BTweenPlayAction)actionProp.enumValueIndex);
            ////base.OnGUI(position, property, lbl);

            //if (EditorGUI.EndChangeCheck())
            //{
            //}
            EditorGUI.EndProperty();
        }
    }
}

/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static BOC.BTagged.BTagged;

namespace BOC.BTagged.EditorTools
{
    [CustomPropertyDrawer(typeof(TagWithRule), true)]
    public class TagWithRuleDrawer : BTaggedSOPropertyDrawerBase
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent lbl)
        {
            EditorGUI.BeginProperty(position, lbl, property);

            var origPos = position;
            position.width = 80f;
            var ruleProp = property.FindPropertyRelative("rule");
            ruleProp.enumValueIndex = (int)(InclusionRule)EditorGUI.EnumPopup(position, (InclusionRule)ruleProp.enumValueIndex);

            position.x += position.width + 5;
            position.width = origPos.width - position.width;

            EditorGUI.PropertyField(position, property.FindPropertyRelative("tag"), GUIContent.none);
            //EditorGUI.PropertyField(position, property.FindPropertyRelative("target"), new GUIContent("Target"), true);
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

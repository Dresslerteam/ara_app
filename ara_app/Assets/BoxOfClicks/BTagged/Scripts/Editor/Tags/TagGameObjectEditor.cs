/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace BOC.BTagged.EditorTools
{
    [CustomEditor(typeof(TagGameObject), true)]
    [CanEditMultipleObjects]
    public class BTaggedTagGameObjectEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            root.Add(new PropertyField(serializedObject.FindProperty("tag")));
            return root;
        }
    }
}

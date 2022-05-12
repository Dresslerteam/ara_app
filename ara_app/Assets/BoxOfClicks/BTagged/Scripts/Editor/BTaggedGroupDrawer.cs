/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using BOC.BTagged.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace BOC.BTagged.EditorTools
{
    [CustomEditor(typeof(BTaggedGroupBase), true)]
    [CanEditMultipleObjects]
    public class BTaggedGroupEditor : Editor
    {
        private void ShowSettings()
        {
            SettingsService.OpenUserPreferences("Preferences/BTaggedSettings");
        }

        Vector2 scrollPos = Vector2.zero;
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox(target + "\nSelect any of the children (sub assets) to see where they are used.\n\nCreate a new Group via the BTagged drop-down menu, by Selecting one or more assets and grouping them (Cmd+G) or via the project window's context menu.", MessageType.Info);

            string localPath = BTaggedSharedUtils.GetASMDEFDirectory(@"BOC.BTagged.Editor");
            GUIStyle deleteBtnStyle = new GUIStyle(GUI.skin.button);
            deleteBtnStyle.padding = new RectOffset();

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            string newGroupName = EditorGUILayout.TextField(target.name);
            if (newGroupName != target.name)
            {
                BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.RenameGroup((target as BTaggedGroupBase), newGroupName);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Children:");
            Rect r = EditorGUILayout.GetControlRect(false, 0);
            r.y += EditorGUIUtility.standardVerticalSpacing;
            r.height = 200;
            EditorGUI.DrawRect(r, new Color(0, 0, 0, 0.1f));

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));
            GUIContent pickerBtnContent = new GUIContent(EditorGUIUtility.FindTexture("Record Off@2x"), "Click to select the asset associated with this " + target.name);
            GUIStyle pickerBtnStyle = new GUIStyle(GUI.skin.button);
            pickerBtnStyle.alignment = TextAnchor.MiddleLeft;
            pickerBtnStyle.padding = new RectOffset(3, 3, 3, 3);
            pickerBtnStyle.fixedHeight = EditorGUIUtility.singleLineHeight;
            var subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(target));
            int assetToBeginEditing = -1;
            for (int s = 0; s < subAssets.Length; ++s)
            {
                if (subAssets[s] != null && subAssets[s] is BTaggedSO)
                {
                    string tfControlName = "edit_tf_" + s;
                    EditorGUILayout.BeginHorizontal();
                    pickerBtnContent.text = "  " + subAssets[s].name;
                    pickerBtnContent.tooltip = "Click to select " + subAssets[s];
                    bool isEditing = BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.GetIsEdit(serializedObject, s.ToString());
                    bool shouldEdit = BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.GetShouldEdit(serializedObject, s.ToString());
                    if (shouldEdit)
                    {
                        GUI.SetNextControlName(tfControlName);

                        string newAssetName = EditorGUILayout.TextField(BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.GetEditString(serializedObject, s.ToString()));
                        BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.SetEditString(serializedObject, s.ToString(), newAssetName);
                        if (!isEditing)
                        {
                            if(Event.current.type == EventType.Layout) EditorGUI.FocusTextInControl(tfControlName);
                            BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.SetIsEdit(serializedObject, s.ToString(), true);
                            BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.SetEditString(serializedObject, s.ToString(), subAssets[s].name);
                        }
                        if ((Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter || GUI.GetNameOfFocusedControl() != tfControlName))
                        {
                            BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.SetShouldEdit(serializedObject, s.ToString(), false);
                            BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.SetIsEdit(serializedObject, s.ToString(), false);
                            newAssetName = BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.GetEditString(serializedObject, s.ToString());
                            BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.RenameSOAsset((target as BTaggedGroupBase), subAssets[s] as BTaggedSO, target.name + "/" + newAssetName);
                        }
                        else if (Event.current.keyCode == KeyCode.Escape)
                        {
                            BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.SetShouldEdit(serializedObject, s.ToString(), false);
                            BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.SetIsEdit(serializedObject, s.ToString(), false);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(pickerBtnContent, pickerBtnStyle))
                        {
                            Selection.activeObject = (subAssets[s] as BTaggedSO);
                        }
                    }
                    if (!EditorGUIUtility.isProSkin) GUI.contentColor = Color.black;
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(localPath, "BTagged_Edit.png")), deleteBtnStyle, GUILayout.Width(20), GUILayout.Height(20)))
                    {   
                        if(!isEditing) assetToBeginEditing = s;
                        EditorGUI.FocusTextInControl(null);
                    }
                    if (GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(localPath, "BTagged_Delete.png")), deleteBtnStyle, GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        if (EditorUtility.DisplayDialog("Deleting " + subAssets[s].name, "Are you sure you wish to delete " + target.name + "/" + subAssets[s].name + "?", "Ok", "Cancel"))
                        {
                            BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.TryDeleteSO(subAssets[s] as BTaggedSO);
                        }
                        GUIUtility.ExitGUI();
                    }
                    if (!EditorGUIUtility.isProSkin) GUI.contentColor = Color.white;

                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();

            if(assetToBeginEditing >= 0)
            {
                string p = assetToBeginEditing.ToString();
                EditorApplication.delayCall += () => BTaggedSOPropertyDrawerBase<BTaggedGroupBase, BTaggedSO>.SetShouldEdit(serializedObject, p, true);
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add")) BTaggedEditorUtils.ShowAddToGroupPopup(target as BTaggedGroupBase, string.Empty, null);
            GUIContent lbl = new GUIContent("BTagged Settings", EditorGUIUtility.FindTexture("_Popup"));
            EditorGUILayout.Space();
            if (GUILayout.Button(lbl)) ShowSettings();
            EditorGUILayout.EndHorizontal();
        }

    }
}

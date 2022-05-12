/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using BOC.BTagged.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using static BOC.BTagged.Shared.BTaggedSharedSettings;

namespace BOC.BTagged.EditorTools
{
    public abstract class BTaggedSOPropertyDrawerBase : PropertyDrawer
    {
        [NonSerialized]
        // Used for a visual highlight to help editor navigation of assets
        public static int SentFromInstanceId = 0;
    }

    [CanEditMultipleObjects]
    public abstract class BTaggedSOPropertyDrawerBase<GROUP, SO> : BTaggedSOPropertyDrawerBase
        where GROUP : BTaggedGroupBase
        where SO : BTaggedSO
    {
        static protected bool Initialized = false;
        static protected BTaggerDropDownMenuWindow menuPopup;
        static protected List<(BTaggedGroupBase group, SO asset)> foundAssets = new List<(BTaggedGroupBase, SO)>();
        static protected List<(string label, int index, bool editable)> menuEntries = new List<(string, int, bool editable)>();

        // This is quite unfortunate.
        // It seems creating an asset can cause the underlying serializedProperty to update and consequently cause the property drawer to be rebuilt.
        // Therefore something like a local bool will be lost during an operation such as add or duplicate.
        // Consequently we need to store and lookup whether to edit and what the edit string is through a combination of the serializedObject and property path.
        // 'serializedProperty' and 'this' cannot be used as keys as they will be unique upon rebuild. Thus: 
        // key = serializedProperty.serializedObject, serializedProperty.propertyPath
        static internal Dictionary<(SerializedObject, string), bool> propertyDrawerShouldEdit = new Dictionary<(SerializedObject, string), bool>();
        static internal Dictionary<(SerializedObject, string), bool> propertyDrawerIsEditing = new Dictionary<(SerializedObject, string), bool>();
        static internal Dictionary<(SerializedObject, string), string> propertyDrawerEditString = new Dictionary<(SerializedObject, string), string>();

        static internal void SetShouldEdit(SerializedProperty serializedProperty, bool value)
        {
            if (serializedProperty != null) SetShouldEdit(serializedProperty.serializedObject, serializedProperty.propertyPath, value);
        }
        static internal void SetShouldEdit(SerializedObject serializedObject, string propPath, bool value)
        {
            if (serializedObject != null) propertyDrawerShouldEdit[(serializedObject, propPath)] = value;
        }
        static internal bool GetShouldEdit(SerializedProperty serializedProperty)
        {
            return serializedProperty.serializedObject == null ? default : GetShouldEdit(serializedProperty.serializedObject, serializedProperty.propertyPath);
        }
        static internal bool GetShouldEdit(SerializedObject serializedObject, string propPath)
        {
            return propertyDrawerShouldEdit.TryGetValue((serializedObject, propPath), out var result) ? result : default;
        }

        static internal void SetIsEdit(SerializedProperty serializedProperty, bool value)
        {
            if (serializedProperty != null) SetIsEdit(serializedProperty.serializedObject, serializedProperty.propertyPath, value);
        }
        static internal void SetIsEdit(SerializedObject serializedObject, string propPath, bool value)
        {
            if (serializedObject != null) propertyDrawerIsEditing[(serializedObject, propPath)] = value;
        }
        static internal bool GetIsEdit(SerializedProperty serializedProperty)
        {
            return serializedProperty.serializedObject == null ? default : GetIsEdit(serializedProperty.serializedObject, serializedProperty.propertyPath);
        }
        static internal bool GetIsEdit(SerializedObject serializedObject, string propPath)
        {
            return propertyDrawerIsEditing.TryGetValue((serializedObject, propPath), out var result) ? result : default;
        }

        static internal void SetEditString(SerializedProperty serializedProperty, string value)
        {
            if (serializedProperty != null) SetEditString(serializedProperty.serializedObject, serializedProperty.propertyPath, value);
        }
        static internal void SetEditString(SerializedObject serializedObject, string propPath, string value)
        {
            if (serializedObject != null) propertyDrawerEditString[(serializedObject, propPath)] = value;
        }
        static internal string GetEditString(SerializedProperty serializedProperty)
        {
            return serializedProperty.serializedObject == null ? default : GetEditString(serializedProperty.serializedObject, serializedProperty.propertyPath);
        }
        static internal string GetEditString(SerializedObject serializedObject, string propPath)
        {
            return propertyDrawerEditString.TryGetValue((serializedObject, propPath), out var result) ? result : default;
        }

        bool shouldEdit
        {
            set => SetShouldEdit(serializedProperty, value);
            get => GetShouldEdit(serializedProperty);
        }
        bool isEditing
        {
            set => SetIsEdit(serializedProperty, value);
            get => GetIsEdit(serializedProperty);
        }
        string editString
        {
            set => SetEditString(serializedProperty, value);
            get => GetEditString(serializedProperty);
        }

        protected SerializedProperty serializedProperty;
        protected string label = "";
        protected string helpText = "";
        protected bool showHelpIcon = false;
        protected string defaultLabel = "-";

        bool editingCategory = false;
        bool renamingMainAsset = false;
        List<int> assetIndiciesToEdit = new List<int>();

        ~BTaggedSOPropertyDrawerBase()
        {
            var key = (serializedProperty.serializedObject, serializedProperty.propertyPath);
            if (propertyDrawerShouldEdit.ContainsKey(key)) propertyDrawerShouldEdit.Remove(key);
            if (propertyDrawerIsEditing.ContainsKey(key)) propertyDrawerIsEditing.Remove(key);
            if (propertyDrawerEditString.ContainsKey(key)) propertyDrawerEditString.Remove(key);
            SentFromInstanceId = 0;
        }

        protected const float OpenButtonWidth = 20f;

        protected float MaxLabelWidth = 80f;
        protected float labelWidth = 80f;
        protected Rect dropDownRect;
        float lastLabelClickTime = 0;
        static Type propertyType = typeof(BTaggedSO);
        string origSOGuid = string.Empty;
        GUIStyle lblStyle;
        Rect editRect;
        GUIContent pickerBtnContent = new GUIContent(EditorGUIUtility.FindTexture("Record Off@2x"), "Click to Open/Select this asset.");
        GUIStyle pickerBtnStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent lblIn)
        {
            lblStyle = new GUIStyle(GUI.skin.label);
            pickerBtnStyle = new GUIStyle(GUI.skin.button);
            GUIContent lbl = new GUIContent(lblIn);
            serializedProperty = property;
            //if (showHelpIcon)
            //{
            //    position.width -= 16;
            //}

            string origLabel = lbl.text;
            SO currentSO = serializedProperty.objectReferenceValue as SO;
            if (currentSO != null) AssetDatabase.TryGetGUIDAndLocalFileIdentifier(currentSO, out origSOGuid, out long newId);
            Init(property, defaultLabel);

            //Debug.Log(serializedProperty.objectReferenceValue + ", " + serializedProperty.objectReferenceInstanceIDValue + ", " + serializedProperty.FindPropertyRelative("_Hash") + ", " + serializedProperty.FindPropertyRelative("Hash"));
            if (!Application.isPlaying && (currentSO == null || (!AssetDatabase.IsSubAsset(currentSO) && !AssetDatabase.IsMainAsset(currentSO))))
            {
                if (defaultSO == null) FindAllAssets(property, defaultLabel);
                serializedProperty.objectReferenceValue = defaultSO;
                serializedProperty.serializedObject.ApplyModifiedProperties();
            }

            editRect = new Rect(position);
            if (Event.current.type == EventType.Repaint)
            {
                MaxLabelWidth = (position.width - 140f);

                float widthForLabel = GUI.skin.label.CalcSize(lbl).x + 10f;
                int labelLength = origLabel.Length;
                while (widthForLabel > MaxLabelWidth && labelLength > 3)
                {
                    lbl.text = origLabel.Substring(0, labelLength--) + "..";
                    widthForLabel = GUI.skin.label.CalcSize(lbl).x + 10f;
                }
                labelWidth = Mathf.Min(widthForLabel, MaxLabelWidth);
            }
            //if (labelWidth < 10)
            //{
            //    Debug.LogWarning("FOR " + position.width + " MAXLABELWIDTH: " + labelWidth + ":: " + Event.current);
            //}
            if (string.IsNullOrEmpty(origLabel)) labelWidth = -6f;
            float buttonLabelWidth = (labelWidth + OpenButtonWidth);
            editRect.x += buttonLabelWidth;
            editRect.y += 1;
            editRect.width -= buttonLabelWidth;

            float totalWidth = position.width;
            position.width = OpenButtonWidth;

            lblStyle.richText = true;

            pickerBtnStyle.padding = new RectOffset(0, 0, 3, 3);
            Color defaultGUIColor = GUI.color;

            if (currentSO != null && SentFromInstanceId == currentSO.GetInstanceID()) GUI.color = Color.yellow;
            if (GUI.Button(position, pickerBtnContent, pickerBtnStyle))
            {
                if (property.objectReferenceValue != null)
                {
                    AssetDatabase.OpenAsset(property.objectReferenceValue);
                }
            }
            GUI.color = defaultGUIColor;
            position.x += position.width + 6f;
            position.width = labelWidth;
            lbl.tooltip = origLabel + "\nClick to ping, double-click to select this asset.";
            if (GUI.Button(position, lbl, GUI.skin.label))
            {
                if (property.objectReferenceValue != null)
                {
                    if (Time.time - lastLabelClickTime < 0.5f)
                    {
                        BTaggedSOEditor.SentFromInstanceId = serializedProperty.serializedObject.targetObject.GetInstanceID();
                        Selection.activeObject = property.objectReferenceValue;
                    }
                    else
                    {
                        EditorGUIUtility.PingObject(property.objectReferenceValue);
                    }
                }
                lastLabelClickTime = Time.time;
            }

            position.x += position.width;
            //position.y += 1f;
            //position.width = totalWidth - labelWidth - OpenButtonWidth - HelpButtonWidth - 10f;
            position.width = totalWidth - labelWidth - OpenButtonWidth - 6f;

            string tfControlName = "edit_tf";
            if (shouldEdit)
            {
                GUI.SetNextControlName(tfControlName);
                editString = EditorGUI.TextField(editRect, editString);
                if (!isEditing)
                {
                    switch (Event.current.type)
                    {
                        case EventType.Layout:
                            EditorGUI.FocusTextInControl(tfControlName);
                            break;
                        case EventType.Repaint:
                            TextEditor tEditor = GetCurrTextEditor();
                            if (tEditor != null)
                            {
                                isEditing = true;
                                float txtW = EditorStyles.textField.CalcSize(new GUIContent(editString)).x;
                                tEditor.selectIndex = editString.LastIndexOf("/") + 1;
                                tEditor.cursorIndex = editString.Length;
                                tEditor.scrollOffset = new Vector2(txtW - tEditor.position.width + 1, 0);
                            }
                            break;
                    }
                }
                if ((Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter || GUI.GetNameOfFocusedControl() != tfControlName))
                {
                    shouldEdit = false;
                    isEditing = false;
                    TryApplyEdit();
                }
                else if (Event.current.keyCode == KeyCode.Escape)
                {
                    shouldEdit = false;
                    isEditing = false;
                }
            }
            else
            {

                GUIContent content = new GUIContent(currentSO == null || currentSO.IsDefault ? defaultLabel : currentSO.name);
                if (BTaggedSharedSettings.ShowGroupNames && currentSO != null && !currentSO.IsDefault && !AssetDatabase.IsMainAsset(currentSO))
                {
                    var soPath = AssetDatabase.GetAssetPath(currentSO);
                    if (!string.IsNullOrEmpty(soPath))
                    {
                        var mainAss = AssetDatabase.LoadMainAssetAtPath(soPath);
                        if (mainAss != null) content = new GUIContent(mainAss.name + "/" + content.text);
                    }
                }
                content.tooltip = content.text + "\nClick to choose a different " + label;
                float widthForDropDown = EditorStyles.textField.CalcSize(content).x + 10f;
                int lblLength = content.text.Length;
                if (widthForDropDown > (position.width - 15f))
                {
                    int idx = content.text.Length - Mathf.Clamp(Mathf.RoundToInt((position.width - 15f) / widthForDropDown * lblLength) - 1, 8, lblLength - 1);
                    if (idx > 5 && lblLength > 5) content.text = content.text.Substring(0, 5) + ".." + content.text.Substring(idx + 5);
                }
                bool pressed = EditorGUI.DropdownButton(position, content, FocusType.Keyboard);
                if (pressed)
                {
                    FindAllAssets(serializedProperty, defaultLabel);
                    dropDownRect = EditorGUIUtility.GUIToScreenRect(position);
                    ShowPopup(dropDownRect);
                }
            }

            if (showHelpIcon)
            {
                GUIContent helpTxt = new GUIContent("?", helpText);
                position.x += position.width + 4f;
                position.width = 10f;
                GUI.Button(position, helpTxt, GUI.skin.label);
            }
        }

        // Just WOW
        // Not only is *this* the way to change selection but we have to use reflection for EditorGUI.TextField
        // Although we could use GUI.TextField, text doesn't scroll as you type or allow copy/paste with one of those so here we are
        // Oh and it's not available on the first frame but we only want to override the selction on the frame we started editing
        // so there's that too. 
        TextEditor GetCurrTextEditor()
        {
            return typeof(EditorGUI)
                .GetField("activeEditor", BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(null) as TextEditor;
        }

        const float MinPopupWidth = 270f;
        public void ShowPopup(Rect popupButtonRect)
        {
            if (menuEntries == null || menuEntries.Count < 1) FindAllAssets(serializedProperty, defaultLabel);
            if (menuPopup == null)
            {
                menuPopup = EditorWindow.CreateInstance<BTaggerDropDownMenuWindow>();
            }
            else
            {
                menuPopup.Close();
                menuPopup = EditorWindow.CreateInstance<BTaggerDropDownMenuWindow>();
            }

            Rect popupWindowSize = new Rect(popupButtonRect);
            if (popupWindowSize.width < MinPopupWidth)
            {
                popupButtonRect.x += popupWindowSize.width - MinPopupWidth;
                popupWindowSize.width = MinPopupWidth;
            }
            SO currentSO = serializedProperty.objectReferenceValue as SO;
            if (currentSO != null)
            {
                BTaggedGroupBase grp = AssetDatabase.IsMainAsset(currentSO) ? null : AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(currentSO)) as BTaggedGroupBase;
                menuPopup.SelectedEntry = foundAssets.IndexOf((grp, currentSO));
            }
            else
            {
                menuPopup.SelectedEntry = -1;
            }

            // Arrays call the same property drawer multiple times so we need the 
            // popup window to return the property it was actually called for
            menuPopup.RelatedProperty = serializedProperty;
            menuPopup.OnAddCategory = HandleAddCategory;
            menuPopup.OnEditCategory = HandleEditCategory;
            menuPopup.OnDeleteCategory = HandleDeleteCategory;
            menuPopup.OnSelect = HandleSelect;
            menuPopup.OnEdit = HandleEdit;
            menuPopup.OnDuplicate = HandleDuplicate;
            menuPopup.OnDelete = HandleDelete;
            menuPopup.Build(menuEntries, popupWindowSize.width);
            menuPopup.ShowAsDropDown(popupButtonRect, new Vector2(popupWindowSize.width, 200f));
            menuPopup.Focus();
        }

        #region Selection
        private void HandleSelect(SerializedProperty prop, int menuIdx)
        {
            serializedProperty = prop;
            if (serializedProperty.serializedObject.targetObject == null) Debug.LogWarning("No targetobj for " + serializedProperty.serializedObject);
            Undo.RecordObject(serializedProperty.serializedObject.targetObject, "Change " + label);
            if (menuIdx < 0 || menuIdx >= foundAssets.Count)
            {
                SetToEmpty(ref serializedProperty);
                return;
            }
            if (foundAssets[menuIdx].asset != null) SetValue(foundAssets[menuIdx].asset);
        }

        protected virtual void SetValue(SO asset)
        {
            if (serializedProperty.objectReferenceValue != null && serializedProperty.serializedObject.targetObject == asset)
            {
                Debug.LogWarning("Cyclical dependency detected. Reference to " + serializedProperty.serializedObject.targetObject + " can not be set to itself.");
                return;
            }

            serializedProperty.objectReferenceValue = asset;

            // Allows updating of registered tags when changing via the inspector at runtime
            if (asset is Tag && serializedProperty.serializedObject.targetObject != null)
            {
                var mtComponent = (serializedProperty.serializedObject.targetObject as MultiTagGameObject);
                if (mtComponent != null)
                {
                    var tags = serializedProperty.serializedObject.FindProperty("tags");
                    if (tags != null && tags.isArray)
                    {
                        Tag[] newTags = new Tag[tags.arraySize];
                        for (int i = 0; i < newTags.Length; ++i)
                        {
                            newTags[i] = tags.GetArrayElementAtIndex(i).objectReferenceValue as Tag;
                        }
                        mtComponent.SetTags(newTags);
                    }
                }
                else
                {
                    var tComponent = (serializedProperty.serializedObject.targetObject as TagGameObject);
                    if (tComponent != null)
                    {
                        tComponent.SetTag(asset as Tag);
                    }
                }
            }

            serializedProperty.serializedObject.ApplyModifiedProperties();

            // Hate that this is necessary
            // It appears if e.g. a subasset becomes a main asset with a new id, the object under serializedProperty.objectReferenceValue
            // is swapped out automatically under us without marking the serialized object as dirty.
            // As we don't want to dirty the component if the asset didn't actually change we get around it by comparing
            // the guid of the selected asset with the guid it had in OnGUI. 
            // For clarity, checking serizliedProperty.objectReferenceValue before setting it to 'asset' for any kind of evaluation
            // (guid, file id, instance id) will all appear as though they are identical.
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out var newGuid, out long newId);
            if (newGuid != origSOGuid) EditorUtility.SetDirty(serializedProperty.serializedObject.targetObject);
            origSOGuid = newGuid;
            if (menuPopup != null) menuPopup.Close();
        }
        #endregion

        #region Adding
        private void HandleAddCategory(SerializedProperty prop, int menuIdx, string categoryLabel)
        {
            //Debug.LogWarning("Handle add category " + typeof(GROUP) + ", " + typeof(SO) + ", " + prop.GetType() + ", " + serializedProperty.objectReferenceValue?.GetType());
            serializedProperty = prop;
            if (menuIdx < 0 || menuIdx >= foundAssets.Count || foundAssets[menuIdx].group == null)
            {
                if (typeof(BTaggedSO).IsAssignableFrom(typeof(SO)))
                {
                    var foundType = fieldInfo.FieldType;
                    if (foundType.IsArray) foundType = foundType.GetElementType();
                    var group = BTaggedEditorUtils.CreateGroupForType(foundType, "Assets/", "New Group").group;
                    if (group == null)
                    {
                        // Can't create a Group for this type of SO - should have created a generic BTaggedGroup but we'll try and create new individual asset
                        var newAsset = BTaggedEditorUtils.CreateSOAsMainAssetForType(foundType, "Assets/", "New").asset as SO;
                        if (newAsset == null)
                        {
                            Debug.LogWarning("Unable to create asset of type " + foundType);
                            return;
                        }
                        SetValue(newAsset);
                        SetEdit(null, newAsset);
                    }
                    else
                    {
                        var newAsset = CreateAssetInGroup(group, "New", true);
                        SetValue(newAsset);
                        SetEdit(group, newAsset);
                    }
                }
                //var newTag = BTaggedSOPropertyDrawerBase<BTaggedGroup, SO>.CreateAssetInGroupAtPath("", "New", true);
                //SetValue(newTag);
                //SetEdit(AssetDatabase.LoadAssetAtPath<GROUP>(AssetDatabase.GetAssetPath(newTag)), newTag);
            }
            else
            {
                //Debug.LogWarning("Handle add category for " + foundAssets[menuIdx].group + " | " + foundAssets[menuIdx].asset + ", " + categoryLabel + " : " + typeof(SO));
                BTaggedEditorUtils.ShowAddToGroupPopup(foundAssets[menuIdx].group, string.IsNullOrEmpty(categoryLabel) ? string.Empty : categoryLabel + "/New", HandleCreatedNewSO);
            }
        }

        internal void HandleCreatedNewSO(BTaggedSO so)
        {
            SetValue(so as SO);
            FindAllAssets(serializedProperty, defaultLabel);
            SetEdit(AssetDatabase.LoadAssetAtPath<BTaggedGroupBase>(AssetDatabase.GetAssetPath(so)), so as SO);
        }

        //Group path is set when 'New' is clicked within a groups popup menu
        internal void AddNew(string groupPath = "", string existingName = "")
        {
            if (menuPopup != null) menuPopup.Close();

            SO currentSO = serializedProperty.objectReferenceValue as SO;
            BTaggedGroupBase soGroup = (currentSO != null && !AssetDatabase.IsMainAsset(currentSO)) ? AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(currentSO)) as BTaggedGroupBase : null;

            SO newSO = CreateAssetInGroup(soGroup, existingName);

            if (newSO != null)
            {
                // Select the newly created asset
                SetValue(newSO);

                // Immediately make it available to be renamed
                SetEdit(soGroup, newSO);
            }
        }

        static public (BTaggedGroupBase group, string path) CreateGroup(string groupPath, string groupName)
            => BTaggedEditorUtils.CreateGroupForType(propertyType, groupPath, groupName);

        static public SO CreateAssetInGroupAtPath(string groupPath = "", string soName = "", bool createIfAlreadyExists = false)
        {
            if (string.IsNullOrEmpty(groupPath)) groupPath = "Assets/New";
            bool addAssetExt = !Path.HasExtension(groupPath);
            BTaggedGroupBase soGroup = AssetDatabase.LoadMainAssetAtPath(groupPath + (addAssetExt ? ".asset" : string.Empty)) as BTaggedGroupBase;

            if (soGroup == null) soGroup = CreateGroup(groupPath, string.Empty).Item1;
            return (soGroup != null && !string.IsNullOrEmpty(soName) ? CreateAssetInGroup(soGroup, soName, createIfAlreadyExists) : null);
        }
        static public SO CreateAssetInGroup(BTaggedGroupBase soGroup, string soName = "", bool createIfAlreadyExists = true)
        {
            if (soGroup == null) return CreateAssetInGroupAtPath(string.Empty, soName, createIfAlreadyExists);
            if (!createIfAlreadyExists)
            {
                var existing = BTaggedEditorUtils<BTaggedGroupBase, SO>.GetSOsForGroup(soGroup).FirstOrDefault(x => x.name.ToLower() == soName.ToLower());
                if (existing != null) return existing;
            }
            string newAssetName = BTaggedEditorUtils.GetNextAvailableName(AssetDatabase.GetAssetPath(soGroup), soName);

            if (propertyType == typeof(BTaggedSO)) propertyType = typeof(SO);

            // Create the new asset & reimport it to force update
            SO newSO = ScriptableObject.CreateInstance(propertyType) as SO;
            //Debug.LogWarning("Creating new asset " + propertyType + ", " + typeof(SO) + " : " + newAssetName);
            if (newSO == null)
            {
                Debug.LogWarning("Unable to create an asset of type " + typeof(SO) + ", " + propertyType);
                return default;
            }
            newSO.name = newAssetName;
            AddAssetToGroup(newSO, soGroup);
            //Debug.LogWarning("Creating new instance of " + propertyType + " and adding it to " + soGroup);
            return newSO;
        }

        static public SO AddAssetToGroup(SO newSO, BTaggedGroupBase soGroup)
        {
            AssetDatabase.AddObjectToAsset(newSO, soGroup);
            BTaggedEditorUtils.ForceRefresh(soGroup);
            return newSO;
        }
        #endregion

        #region Duplication
        private void HandleDuplicate(SerializedProperty prop, int menuIdx)
        {
            serializedProperty = prop;
            if (menuIdx < 0 || menuIdx >= foundAssets.Count) return;

            if (menuPopup != null) menuPopup.Close();

            SO currentSO = foundAssets[menuIdx].asset;
            BTaggedGroupBase soGroup = (currentSO != null && !AssetDatabase.IsMainAsset(currentSO)) ? AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(currentSO)) as BTaggedGroupBase : null;

            SO newSO = ScriptableObject.Instantiate(currentSO);
            if (newSO != null)
            {
                string newAssetName = BTaggedEditorUtils.GetNextAvailableName(AssetDatabase.GetAssetPath(soGroup), currentSO.name);
                newSO.EditorGenerateNewHash();
                newSO.name = newAssetName;
                AddAssetToGroup(newSO, soGroup);

                // Select the newly created asset
                SetValue(newSO);

                // Immediately make it available to be renamed
                SetEdit(soGroup, newSO);
            }
        }
        #endregion

        #region Editing
        private void HandleEdit(SerializedProperty prop, int menuIdx)
        {
            serializedProperty = prop;
            if (menuIdx < 0 || menuIdx >= foundAssets.Count) return;
            SetValue(foundAssets[menuIdx].asset);
            SetEdit(foundAssets[menuIdx].group, foundAssets[menuIdx].asset);
            FindAllAssets(serializedProperty, defaultLabel);
        }

        BTaggedGroupBase workingGroup;
        private void HandleEditCategory(SerializedProperty prop, int menuIdx, int depth, string categoryLabel)
        {
            serializedProperty = prop;
            if (menuPopup != null) menuPopup.Close();

            editingCategory = true;

            assetIndiciesToEdit.Clear();
            FindmatchingAssets(menuIdx, depth, ref assetIndiciesToEdit);
            renamingMainAsset = false;
            if (assetIndiciesToEdit.Count > 0)
            {
                BTaggedGroupBase group = foundAssets[menuIdx].group;
                workingGroup = group;
                string assetName = foundAssets[menuIdx].asset.name;
                string[] splitLabels = assetName.Split('/');
                for (int i = 1; i < splitLabels.Length; ++i) splitLabels[i] = splitLabels[i - 1] + "/" + splitLabels[i];
                string subDirName = string.Empty;
                if (depth > 0 && splitLabels.Length > 1 && depth <= splitLabels.Length) subDirName = splitLabels[depth - 1];

                if (subDirName == string.Empty)
                {
                    editString = group != null ? group.name : string.Empty;
                    renamingMainAsset = true;
                }
                else
                {
                    editString = (group != null ? group.name + "/" : string.Empty) + subDirName;
                }
                //editString = partToEdit;
                shouldEdit = true;
            }
        }

        private void SetEdit(BTaggedGroupBase soGroup, SO currentSO)
        {
            if (currentSO != null)
            {
                shouldEdit = true;
                editString = (soGroup != null ? soGroup.name + "/" : string.Empty) + currentSO.name;
            }
        }

        private void TryApplyEdit()
        {
            Undo.SetCurrentGroupName("Renaming " + label + "s");
            SO currentSO = serializedProperty.objectReferenceValue as SO;
            if (editingCategory)
            {
                string newName = editString;
                if (renamingMainAsset)
                {
                    RenameGroup(workingGroup, newName);
                }
                else
                {
                    for (int i = 0; i < assetIndiciesToEdit.Count; ++i)
                    {
                        SO asset = foundAssets[assetIndiciesToEdit[i]].asset;
                        int lastDirIdx = Mathf.Max(0, asset.name.LastIndexOf("/"));

                        currentSO = RenameSOAsset(workingGroup, asset, newName + asset.name.Substring(lastDirIdx));

                    }
                }
                editingCategory = false;
            }
            else
            {
                if (currentSO == null) return;
                string newAssetName = editString;
                workingGroup = AssetDatabase.LoadAssetAtPath<BTaggedGroupBase>(AssetDatabase.GetAssetPath(currentSO));
                currentSO = RenameSOAsset(workingGroup, currentSO, newAssetName);
            }

            isEditing = false;
            BTaggedEditorUtils.ForceRefresh(workingGroup);
            FindAllAssets(serializedProperty, defaultLabel);
            SetValue(currentSO);
        }
        #endregion

        #region Deleting
        void HandleDelete(SerializedProperty prop, int menuIdx)
        {
            serializedProperty = prop;
            if (menuIdx < 0 || menuIdx >= foundAssets.Count) return;
            var delRslt = TryDeleteSO(foundAssets[menuIdx].asset, serializedProperty);
            FindAllAssets(serializedProperty, defaultLabel);
            if (delRslt) ShowPopup(dropDownRect);
        }

        void HandleDeleteCategory(SerializedProperty prop, int menuIdx, int depth, string categoryLabel)
        {
            int numToDelete = 0;
            int numDeleted = 0;
            serializedProperty = prop;
            List<int> assetIndiciesToDelete = new List<int>();
            FindmatchingAssets(menuIdx, depth, ref assetIndiciesToDelete);

            for (int i = 0; i < assetIndiciesToDelete.Count; ++i)
            {
                if (assetIndiciesToDelete[i] < 0 && depth == 0 && foundAssets[menuIdx].group != null)
                {
                    for (int a = 0; a < foundAssets.Count; ++a)
                    {
                        if (foundAssets[a].group == foundAssets[menuIdx].group)
                        {
                            numToDelete++;
                            if (TryDeleteSO(foundAssets[a].asset, serializedProperty)) numDeleted++;
                        }
                    }
                }
                else
                {
                    numToDelete++;
                    if (TryDeleteSO(foundAssets[assetIndiciesToDelete[i]].asset, serializedProperty)) numDeleted++;
                }
            }

            if (numDeleted == numToDelete)
            {
                ShowNotification("Deleted " + categoryLabel);
            }
            else if (numDeleted > 0)
            {
                ShowNotification("Deleted " + numDeleted + "/" + numToDelete + " items in " + categoryLabel);
            }
            if (numDeleted == numToDelete && depth == 0)
            {
                if (foundAssets[menuIdx].group != null) DeleteAsset(foundAssets[menuIdx].group, serializedProperty);
            }
            if (menuPopup != null) menuPopup.Close();
            FindAllAssets(serializedProperty, defaultLabel);
            ShowPopup(dropDownRect);
        }

        static public bool TryDeleteSO(SO asset, SerializedProperty serializedProperty = null)
        {
            bool success = true;
            if (asset != null)
            {
                success = CanDelete(asset, serializedProperty);
                if (success) DeleteAsset(asset, serializedProperty);
            }
            return success;
        }

        static private bool CanDelete(SO asset, SerializedProperty serializedProperty = null)
        {
            if (asset == null) return false;
            List<SceneObjectIDBundle> assetReferences = new List<SceneObjectIDBundle>();
            BTaggedSORegistry.References(asset, ref assetReferences, SearchRegistryOption.FullRefresh);

            if (assetReferences.Count > 1 ||
                (serializedProperty != null
                && assetReferences.Count == 1
                && serializedProperty.serializedObject.targetObject != null
                && assetReferences[0].id != serializedProperty.serializedObject.targetObject.GetInstanceID()))
            {
                var showReferences = EditorUtility.DisplayDialog("Warning", asset.name + " has " + assetReferences.Count + " references in this project. Would you like to see them?", "No", "Yes");
                if (!showReferences)
                {
                    if (menuPopup != null) menuPopup.Close();
                    Selection.activeObject = asset;
                    return false;
                }
                string msg = "This can't be undone. Any remaining references to " + asset.name + " in your project will become invalid.\nAre you really sure you wish to delete it?";
                bool shouldDelete = !EditorUtility.DisplayDialog(asset.name + " uses", msg, "No", "Yes");

                if (!shouldDelete)
                {
                    BTaggedGroupBase group = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(asset)) as BTaggedGroupBase;
                    msg = "<b>" + (group != null && group != asset ? group.name + "/" : string.Empty) + asset.name + "</b> is referenced in the following locations:\n";
                    msg += "<color=yellow>Warning:</color>\n\n";
                    for (int a = 0; a < Math.Min(100, assetReferences.Count); ++a)
                    {
                        msg += "<b>" + assetReferences[a].scenePath + ": " + assetReferences[a].objectName + "</b>\n";
                    }
                    msg += "\n";
                    Debug.LogWarning(msg);
                }
                return shouldDelete;
            }

            return true;
        }

        static private void DeleteAsset(ScriptableObject asset, SerializedProperty serializedProperty = null)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            if (AssetDatabase.IsMainAsset(asset))
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
            }
            else
            {
                AssetDatabase.RemoveObjectFromAsset(asset);
                BTaggedEditorUtils.ForceRefresh(AssetDatabase.LoadAssetAtPath<BTaggedGroupBase>(assetPath));
            }
            if (serializedProperty != null)
            {
                bool deletedAssetWasSelected = asset == serializedProperty.objectReferenceValue;
                if (deletedAssetWasSelected) SetToEmpty(ref serializedProperty);
            }
        }
        #endregion

        #region Utils
        static private void SetToEmpty(ref SerializedProperty prop)
        {
            prop.objectReferenceValue = defaultSO;
            prop.serializedObject.ApplyModifiedProperties();
            if (menuPopup != null) menuPopup.Close();
        }

        private void ShowNotification(string msg)
        {
            foreach (SceneView scene in SceneView.sceneViews)
            {
                scene.ShowNotification(new GUIContent(msg));
            }
        }

        private void FindmatchingAssets(int menuIdx, int depth, ref List<int> results)
        {
            BTaggedGroupBase group = foundAssets[menuIdx].group;

            if (group != null && depth == 0)
            {
                // If we're affecting an entire group - i.e. a main asset,
                // simply return an entry out of range and the main asset will be edited
                results.Add(-1);
                return;
            }
            string assetName = foundAssets[menuIdx].asset.name;
            string[] splitLabels = assetName.Split('/');
            for (int i = 1; i < splitLabels.Length; ++i) splitLabels[i] = splitLabels[i - 1] + "/" + splitLabels[i];
            string subDirName = string.Empty;
            if (depth > 0 && splitLabels.Length > 1 && depth <= splitLabels.Length) subDirName = splitLabels[depth - 1];
            for (int i = 0; i < foundAssets.Count; ++i)
            {
                if (foundAssets[i].asset == null) continue;
                string curAssetName = foundAssets[i].asset.name;
                bool matches = group != null && foundAssets[i].group == group && !assetName.Contains('/');
                if (curAssetName.StartsWith(subDirName)) matches = foundAssets[i].group == group;
                if (matches) results.Add(i);
            }
        }

        static internal void RenameGroup(BTaggedGroupBase group, string newName)
        {
            if (string.IsNullOrEmpty(newName)) return;

            Undo.RegisterCompleteObjectUndo(group, "Rename " + group.name + " to " + newName);

            newName = ReplaceInvalidChars(newName);
            string newMainAsset = newName.Split('/')[0];
            string remainingName = newName.Length > newMainAsset.Length ? newName.Substring(newMainAsset.Length + 1) : string.Empty;
            BTaggedGroupBase existingGroup = null;
            for (int a = 0; a < foundAssets.Count; ++a)
            {
                if (foundAssets[a].group != null && foundAssets[a].group != group && foundAssets[a].group.name.ToLower() == newMainAsset.ToLower())
                {
                    existingGroup = foundAssets[a].group;
                    break;
                }
            }

            bool moveSubAssets = false;

            // If moving all assets in a group to a different existing group
            if (existingGroup != null)
            {
                // This will potentially change guids for serialized assets 
                if (!CheckOKIfInvalidateReference(group, "Asset")) return;
                moveSubAssets = true;
            }
            else
            {
                // If renaming the main name of the group, do that here
                AssetDatabase.SetMainObject(group, AssetDatabase.GetAssetPath(group));
                //Debug.LogWarning("Renaming " + group + " to " + newMainAsset);
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(group), newMainAsset);
            }

            // It's possible that an entire group could be renamed in the following way:
            // GroupA -> Group/A
            // In such a case, as well as renaming the main asset we also want to change the names
            // of all the subassets of GroupA
            if (remainingName.Length > 0 || moveSubAssets)
            {
                for (int a = 0; a < foundAssets.Count; ++a)
                {
                    if (foundAssets[a].group == group && foundAssets[a].asset != null)
                    {
                        //Debug.LogWarning("Renaming " + foundAssets[a].asset.name + " to " + remainingName + "/" + foundAssets[a].asset.name);
                        foundAssets[a].asset.name = remainingName + "/" + foundAssets[a].asset.name;

                        // Subassets are moving to a different main asset
                        // Doing so will change GUIDs for any serialized asset references
                        if (moveSubAssets)
                        {
                            var existingHash = foundAssets[a].asset.Hash;
                            AssetDatabase.RemoveObjectFromAsset(foundAssets[a].asset);
                            AssetDatabase.SetMainObject(existingGroup, AssetDatabase.GetAssetPath(existingGroup));
                            AssetDatabase.AddObjectToAsset(foundAssets[a].asset, AssetDatabase.GetAssetPath(existingGroup));
                            var prop = typeof(BTaggedSO).GetField("_Hash", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            prop.SetValue(foundAssets[a].asset, existingHash);
                        }
                    }
                }
            }

            BTaggedEditorUtils.ForceRefresh(group);
        }

        static internal SO RenameSOAsset(BTaggedGroupBase existingGroup, SO soAsset, string newName)
        {
            if (string.IsNullOrEmpty(newName)) return soAsset;

            Undo.RecordObject(soAsset, "Rename " + soAsset.name);

            newName = ReplaceInvalidChars(newName);
            bool nameHasDirectory = newName.Contains("/");
            // If soAsset has no related group, assumed to also be main asset
            // If the new name contains / this means the main asset wants to be relocated to be a subasset
            if (existingGroup == null && !nameHasDirectory)
            {
                // This sub asset apparently has no group - it is then assumped it must in fact be a main asset
                if (!AssetDatabase.IsMainAsset(soAsset))
                {
                    Debug.LogAssertion("This was unexpected " + soAsset + " is not a Main Asset");
                    return soAsset;
                }

                // If it is a main asset and can be simply renamed
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(soAsset), newName);
            }
            else
            {
                var nameSlices = newName.Split('/');
                string newMainAsset = nameSlices[0];
                string remainingName = newName.Length > newMainAsset.Length ? newName.Substring(newMainAsset.Length + 1) : newName;

                if (existingGroup != null && newMainAsset == existingGroup.name)
                {
                    // If the subasset is still a member of it's original group then simply rename
                    //Debug.Log("Renaming subasset " + soAsset.name + " to " + newName + " staying in group " + existingGroup);
                    if (newName.Length > existingGroup.name.Length) soAsset.name = newName.Substring(existingGroup.name.Length + 1);
                    BTaggedEditorUtils.ForceRefresh(existingGroup);
                }
                else
                {
                    // Asset was subasset (as otherwise would have had group and caught in previous if)
                    // so if no "/" assume sub asset wants to be removed from group
                    // and exist as its own main asset
                    if (!nameHasDirectory)
                    {
                        return BTaggedEditorUtils.ChangeSubAssetToMainAsset(soAsset, existingGroup, newName);
                    }

                    //BTaggedGroupBase existingGroup = null;
                    BTaggedGroupBase destGroup = null;
                    for (int a = 0; a < foundAssets.Count; ++a)
                    {
                        if (foundAssets[a].group != null && foundAssets[a].group.name.ToLower() == newMainAsset.ToLower())
                        {
                            destGroup = foundAssets[a].group;
                            break;
                        }
                    }

                    string destGroupPath;
                    if (destGroup == null)
                    {
                        // If group doesn't exist, create it
                        if (existingGroup == null)
                        {
                            destGroupPath = AssetDatabase.GetAssetPath(soAsset).Replace(soAsset.name, newMainAsset);
                        }
                        else
                        {
                            destGroupPath = AssetDatabase.GetAssetPath(existingGroup).Replace(existingGroup.name, newMainAsset);
                        }
                        var newGrpResult = CreateGroup(destGroupPath, newMainAsset);
                        destGroup = newGrpResult.group;
                        destGroupPath = newGrpResult.path;
                    }
                    else
                    {
                        destGroupPath = AssetDatabase.GetAssetPath(destGroup);
                    }

                    // Merging sub asset into existing group
                    soAsset.name = remainingName;

                    bool alreadyExists = false;
                    if (destGroup == null)
                    {
                        Debug.LogWarning("Failed to find or create group " + destGroupPath + "," + newMainAsset);
                        return soAsset;
                    }

                    // If a subasset within an existing group has the same name, check whether user
                    // wants to merge them or keep two with identical names
                    var existingSubAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(destGroupPath);
                    for (int i = 0; i < existingSubAssets.Length; ++i)
                    {
                        if (existingSubAssets[i] is SO && existingSubAssets[i].name.ToLower() == soAsset.name.ToLower())
                        {
                            alreadyExists = true;
                            break;
                        }
                    }
                    if (alreadyExists)
                    {
                        bool shouldHaveSameName = EditorUtility.DisplayDialog("Renaming " + soAsset.name, "An asset in " + destGroup + " already has the same name: " + soAsset.name + ". Are you sure you want two with identical names?", "Yes", "No");
                        if (shouldHaveSameName) alreadyExists = false;
                    }
                    if (!alreadyExists)
                    {
                        var clone = ScriptableObject.Instantiate(soAsset);
                        AssetDatabase.AddObjectToAsset(clone, destGroupPath);
                        clone.name = soAsset.name;

                        // Otherwise this asset is moving to a different asset
                        // This will potentially change guids for serialized assets 
                        if (!CheckOKIfInvalidateReference(soAsset, "asset's")) return soAsset;
                        BTaggedSORegistry.ReplaceSOUsage(soAsset, SearchRegistryOption.FullRefresh, clone);

                        // If there is only one sub asset, delete the existing group too
                        if (existingGroup != null)
                        {
                            //Debug.Log(AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(existingGroup)) + " for " + existingGroup);
                            if (AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(existingGroup)).Length <= 1)
                            {
                                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(existingGroup));
                            }
                            else
                            {
                                AssetDatabase.RemoveObjectFromAsset(soAsset);
                                BTaggedEditorUtils.ForceRefresh(existingGroup);
                            }
                        }
                        else
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(soAsset));
                        }
                        BTaggedEditorUtils.ForceRefresh(destGroup);
                        return clone;
                    }
                }
            }
            return soAsset;
        }

        static char[] invalidChars = Path.GetInvalidFileNameChars().Where(x => !x.Equals('/')).ToArray();
        static public string ReplaceInvalidChars(string filename)
        {
            while (filename.Contains("//")) filename = filename.Replace("//", "/");
            if (filename.StartsWith("/")) filename = filename.Substring(1);
            return string.Join("_", filename.Split(invalidChars));
        }

        // BTagged serialized references to a base ScriptableObject (inheriting from BTaggedSOBase).
        // As names and categories are managed by manipulating Assets & Subassets it's important to note that:
        // If a subasset moves to another asset (i.e. it gets re-categorised) the GUID associated with the SO 
        // will change. This can lead to components referencing either un-intended or non-existent assets.
        // BTagged automatically replaces all references in Prefabs, Scenes, Subscenes & Assets so we should no longer need to warn.
        static private bool CheckOKIfInvalidateReference(UnityEngine.Object obj, string label)
        {
            return true;
            //return EditorUtility.DisplayDialog("Renaming " + obj.name, "Changing the " + label + @"'s main Group will cause it's GUID to change and all references to this asset in the project to be lost. Are you *sure* you wish to continue?", "Ok", "Cancel");
        }
        #endregion

        #region Static Find Assets
        static private void Init(SerializedProperty property, string defaultLabel)
        {
            if (!Initialized)
            {
                Initialized = true;
                FindAllAssets(property, defaultLabel);
                Undo.undoRedoPerformed -= () => FindAllAssets(property, defaultLabel);
                Undo.undoRedoPerformed += () => FindAllAssets(property, defaultLabel);
            }
        }

        static internal void SetPropertyTypeFrom(SerializedProperty property)
        {
            try
            {
                SetPropertyTypeFrom(property.type);
            }
            catch { }
        }
        static internal void SetPropertyTypeFrom(string type)
        {
            var match = Regex.Match(type, @"PPtr<\$(.*?)>");
            if (!match.Success)
            {
                Debug.LogWarning("No results for " + type);
                return;
            }
            type = match.Groups[1].Value;
            propertyType = TypeCache.GetTypesDerivedFrom<BTaggedSO>().FirstOrDefault(x => x.Name == type);
        }

        static internal SO FindDefault()
        {
            if (defaultSO != null) return defaultSO;
            FindAllAssets(null, string.Empty);
            return defaultSO;
        }

        static SO defaultSO = null;
        static internal void FindAllAssets(SerializedProperty property, string defaultLabel)
        {
            menuEntries.Clear();
            foundAssets.Clear();

            SetPropertyTypeFrom(property);

            string[] groupGuids = AssetDatabase.FindAssets("t:" + typeof(BTaggedGroupBase));
            for (int groupIndex = 0; groupIndex < groupGuids.Length; ++groupIndex)
            {
                string groupPath = AssetDatabase.GUIDToAssetPath(groupGuids[groupIndex]);
                BTaggedGroupBase group = AssetDatabase.LoadMainAssetAtPath(groupPath) as BTaggedGroupBase;
                string groupName = Path.GetFileNameWithoutExtension(groupPath) + "/";
                UnityEngine.Object[] allAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(groupPath);
                List<string> uniqueNames = new List<string>();

                for (int i = 0; i < allAssets.Length; ++i)
                {
                    if (allAssets[i] == null) continue;
                    string name = groupName + (allAssets[i].name.Length >= 1 ? allAssets[i].name : " -- ");
                    int attempts = 0;
                    while (uniqueNames.Contains(name) && attempts < 100)
                    {
                        attempts++;
                        name = groupName + allAssets[i].name + " (" + attempts + ")";
                    }
                    uniqueNames.Add(name);

                    if (propertyType.IsAssignableFrom(allAssets[i].GetType()))
                    {
                        SO asset = allAssets[i] as SO;
                        if (asset != null)
                        {
                            if (!asset.IsDefault)
                            {
                                foundAssets.Add((group, asset));
                                menuEntries.Add((name, foundAssets.Count, true));
                            }
                            else if (defaultSO == null)
                            {
                                defaultSO = asset;
                            }
                        }
                        //else
                        //{
                        //    Debug.LogAssertion(allAssets[i] + " was assignable from " + propertyType + " but could not be cast to " + typeof(SO));
                        //}
                    }
                    else if (defaultSO == null && allAssets[i] is SO)
                    {
                        defaultSO = allAssets[i] as SO;
                    }
                }
                if (allAssets.Length == 0)
                {
                    foundAssets.Add((group, null));
                    menuEntries.Add((groupName, foundAssets.Count, true));
                }
            }
            string[] individualSOGuids = AssetDatabase.FindAssets("t:" + typeof(SO));
            for (int i = 0; i < individualSOGuids.Length; ++i)
            {
                SO individualAsset = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(individualSOGuids[i])) as SO;
                if (individualAsset != null && propertyType.IsAssignableFrom(individualAsset.GetType()))
                {
                    if (individualAsset.IsDefault)
                    {
                        if (defaultSO == null) defaultSO = individualAsset;
                    }
                    else
                    {
                        foundAssets.Add((null, individualAsset));
                        menuEntries.Add((individualAsset.name, foundAssets.Count, true));
                    }
                }
            }
            if (defaultSO == null)
            {
                CreateDefaultSO(defaultLabel);
            }

            menuEntries.Sort((x, y) => x.label.CompareTo(y.label));

            // Add -None- entry
            foundAssets.Insert(0, (null, null));
            menuEntries.Insert(0, (defaultLabel, -1, false));

            // Add Create New Group entry
            foundAssets.Add((AssetDatabase.LoadAssetAtPath<BTaggedGroupBase>(AssetDatabase.GetAssetPath(defaultSO)), defaultSO));
            menuEntries.Add(("New Group", -1, false));
        }

        private static void CreateDefaultSO(string defaultLabel)
        {
            string localPath = BTaggedSharedUtils.GetASMDEFDirectory(@"BOC.BTagged");
            localPath = Path.Combine(localPath, "Defaults.asset");
            var defaultGroup = AssetDatabase.LoadAssetAtPath<BTaggedGroupBase>(localPath);
            if (defaultGroup == null)
            {
                var gp = BTaggedEditorUtils.CreateGroupForType(typeof(BTaggedSO), localPath, "Defaults");
                defaultGroup = gp.group;
                localPath = gp.path;
            }
            defaultSO = CreateAssetInGroup(defaultGroup, "-None-", true);

            if (defaultSO == null)
            {
                Debug.LogWarning("Failed to automatically create default SO for " + propertyType);
            }
            else
            {
                defaultSO.name = defaultLabel;
                defaultSO.ManuallySetHash(default);
                BTaggedEditorUtils.ForceRefresh(defaultGroup);
                //Debug.LogWarning("Created default asset for " + typeof(SO) + " at " + localPath + ". Feel free to move this file anywhere. By selecting it you can see anywhere in your project that has perhaps erroneously been set to the default.");
            }
        }

        #endregion
    }
}

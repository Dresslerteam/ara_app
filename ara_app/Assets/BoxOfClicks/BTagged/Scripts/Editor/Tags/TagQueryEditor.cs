/* Copyright(c) Tim Watts, Box of Clicks - All Rights Reserved */

using BOC.BTagged.Shared;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static BOC.BTagged.BTagged;

namespace BOC.BTagged.EditorTools
{
    //[CustomEditor(typeof(TagQuery), true)]
    public class TagQueryEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            Undo.undoRedoPerformed += ReBuild;
            return Build();
        }
        private void OnDestroy() => Undo.undoRedoPerformed -= ReBuild;

        VisualElement root = null;
        VisualTreeAsset queryUI;
        VisualTreeAsset queryTagUI;
        StyleEnum<DisplayStyle> visible = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        StyleEnum<DisplayStyle> hidden = new StyleEnum<DisplayStyle>(DisplayStyle.None);

        void OnEnable()
        {
            //menuItemUSS = AssetDatabase.LoadAssetAtPath<StyleSheet>(localPath + "/BTaggedMenuItemUI.uss");
        }

        void ReBuild() => Build();
        VisualElement Build()
        {
            serializedObject.Update();
            if (root == null)
            {
                root = new VisualElement();
                root.style.flexGrow = 1f;
            }
            else
            {
                root.Clear();
            }

            string localPath = BTaggedSharedUtils.GetASMDEFDirectory(@"BOC.BTagged.Editor");
            queryUI = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(localPath + "/Tags/BTaggedQueryUI.uxml");
            queryTagUI = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(localPath + "/Tags/BTaggedQueryTagUI.uxml");
            var uxmlInstance = queryUI.CloneTree();
            uxmlInstance.style.flexGrow = 1f;
            root.Add(uxmlInstance);
            //root.Add(new Button() { text = "BABSODAS" });
            //var tagPanel = new VisualElement();
            //var tagScrollView = new ScrollView(ScrollViewMode.Vertical);
            //tagPanel.style.marginBottom = tagPanel.style.marginLeft = tagPanel.style.marginRight = tagPanel.style.marginTop = 8f;

            var orPanel = root.Q("Or");
            var andPanel = root.Q("And");
            var notPanel = root.Q("Not");
            var orPanelContent = orPanel.Q("DropRegion");
            var andPanelContent = andPanel.Q("DropRegion");
            var notPanelContent = notPanel.Q("DropRegion");
            var queriesPanel = root.Q("AdditionalQueries");

            orPanel.RegisterCallback<DragEnterEvent>(dee => HandleDragEnter(dee, orPanelContent, BTagged.InclusionRule.Any, "tag-rule-any"));
            andPanel.RegisterCallback<DragEnterEvent>(dee => HandleDragEnter(dee, andPanelContent, BTagged.InclusionRule.MustInclude, "tag-rule-include"));
            notPanel.RegisterCallback<DragEnterEvent>(dee => HandleDragEnter(dee, notPanelContent, BTagged.InclusionRule.MustExclude, "tag-rule-exclude"));

            orPanel.RegisterCallback<DragLeaveEvent>(HandleDragLeave);
            andPanel.RegisterCallback<DragLeaveEvent>(HandleDragLeave);
            notPanel.RegisterCallback<DragLeaveEvent>(HandleDragLeave);
#if UNITY_2020_1_OR_NEWER
            andPanel.Q<Button>().RegisterCallback<ClickEvent>(ce => AddTagWithRule(BTagged.InclusionRule.MustInclude));
            orPanel.Q<Button>().RegisterCallback<ClickEvent>(ce => AddTagWithRule(BTagged.InclusionRule.Any));
            notPanel.Q<Button>().RegisterCallback<ClickEvent>(ce => AddTagWithRule(BTagged.InclusionRule.MustExclude));
#else
            andPanel.Q<Button>().clicked += () => AddTagWithRule(BTagged.InclusionRule.MustInclude);
            orPanel.Q<Button>().clicked += () => AddTagWithRule(BTagged.InclusionRule.Any);
            notPanel.Q<Button>().clicked += () => AddTagWithRule(BTagged.InclusionRule.MustExclude);
#endif
            //var orPanel = new VisualElement();
            //var andPanel = new VisualElement();
            //var notPanel = new VisualElement();
            //orPanel.style.backgroundColor = Color.blue;
            //andPanel.style.backgroundColor = Color.green;
            //notPanel.style.backgroundColor = Color.red;
            //notPanel.style.width = andPanel.style.width = orPanel.style.width = 200f;

            //tagScrollView.Add(orPanel);
            //tagScrollView.Add(andPanel);
            //tagScrollView.Add(notPanel);
            //var scrollChild = tagScrollView.Children().FirstOrDefault();
            //if (scrollChild == null) Debug.LogWarning("No children of " + tagScrollView.childCount);
            //var contents = scrollChild.Children().FirstOrDefault();
            //if (contents == null) Debug.LogWarning("No children of " + tagScrollView + ": " + tagScrollView.childCount);
            //contents.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            //contents.style.flexWrap = new StyleEnum<Wrap>(Wrap.Wrap);

            var tq = (target as TagQuery);
            uxmlInstance.RegisterCallback<MouseMoveEvent>(mde => OnMouseMove(mde.localMousePosition));
            uxmlInstance.RegisterCallback<DragUpdatedEvent>(mde => OnMouseMove(mde.localMousePosition));
            uxmlInstance.RegisterCallback<DragLeaveEvent>(mde => StopDragging());
            uxmlInstance.RegisterCallback<DragExitedEvent>(mde => StopDragging());
            uxmlInstance.RegisterCallback<MouseUpEvent>(mde => StopDragging());
            uxmlInstance.RegisterCallback<MouseDownEvent>(mde => StopEditing());
            uxmlInstance.RegisterCallback<DragPerformEvent>(mde =>
            {
                ReBuild();
            });

            //Debug.Log("BUILDING " + tq.matchingTags.Length + " elements");
            for (int i = 0; i < tq.matchingTags.Length; ++i)
            {
                //if (tq.matchingTags[i] == default) continue;
                int idx = i;
                var item = queryTagUI.CloneTree();// new VisualElement();
                item.userData = i;
                item.Q<Label>().text = tq.matchingTags[i].tag == null ? "Null" : tq.matchingTags[i].tag.name;
//#if UNITY_2020_1_OR_NEWER
//#else
                item.RegisterCallback<FocusOutEvent>(foe => StopEditing());
                item.RegisterCallback<MouseDownEvent>(mde => StartDragging(mde, item, idx));
                //item.Q<Button>("Tag").clicked += () => Debug.Log("Here");
                item.Q<Button>("DeleteBtn").clicked += () => DeleteTag(idx);
//#endif
                //item.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                var underlyingProperty = new PropertyField(serializedObject.FindProperty("matchingTags").GetArrayElementAtIndex(i).FindPropertyRelative("tag"));
                underlyingProperty.name = "UnderlyingProp";
                item.Q("TagEdit").style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                item.Q("TagEdit").Add(underlyingProperty);
                //item.Add(new Button(() => DeleteTag(idx)) { text = "X" });
                if (tq.matchingTags[i].rule == BTagged.InclusionRule.Any)
                {
                    orPanelContent.Add(item);
                    item.AddToClassList("tag-rule-any");
                }
                else if (tq.matchingTags[i].rule == BTagged.InclusionRule.MustInclude)
                {
                    andPanelContent.Add(item);
                    item.AddToClassList("tag-rule-include");
                }
                else if (tq.matchingTags[i].rule == BTagged.InclusionRule.MustExclude)
                {
                    notPanelContent.Add(item);
                    item.AddToClassList("tag-rule-exclude");
                }
            }

            //Button addTagButton = new Button() { text = "Add" };
            //addTagButton.style.marginBottom = addTagButton.style.marginLeft = addTagButton.style.marginRight = addTagButton.style.marginTop = 8f;
            //addTagButton.clicked += AddTagWithRule;
            //tagPanel.Add(tagScrollView);
            //tagPanel.Add(addTagButton);

            //root.Add(tagPanel);
            uxmlInstance.Bind(serializedObject);

            // Hack to expand parent window so drag & drop isn't arbitrarily cut-off
            root.schedule.Execute(() =>
            {
                var ve = root.parent;
                while(ve != null && !(ve is ScrollView))
                {
                    ve.style.flexGrow = 1f;
                    ve = ve.parent;
                }
                if (ve is ScrollView) ve.Q("unity-content-container").style.flexGrow = 1f;
                //var containingSV = root.GetFirstAncestorOfType<ScrollView>().Q("unity-content-container").style.flexGrow = new StyleFloat(1f);
            });
            return root;
        }

        private void HandleDragLeave(DragLeaveEvent evt)
        {
            if (draggingItemRepresentation == null) return;
            draggingItem.style.opacity = 0.2f;
            Debug.Log("Left");
            DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            draggingItemRepresentation.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            draggingItemRepresentation.style.position = new StyleEnum<Position>(Position.Absolute);
            draggingItemRepresentation.Q("Tag").RemoveFromClassList("tag-drop-valid");
            draggingItem.Q("Tag").RemoveFromClassList("tag-drop-valid");
            root.Add(draggingItemRepresentation);
        }

        private void HandleDragEnter(DragEnterEvent evt, VisualElement ve, BTagged.InclusionRule containerRule, string ruleClass)
        {
            if (draggingItemRepresentation == null) return;
            if(ve == originalContainer)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                Debug.Log("Entered original");
                draggingItemRepresentation.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
                draggingItem.Q("Tag").AddToClassList("tag-drop-valid");
                draggingItem.style.opacity = 1f;
            }
            else
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                Debug.Log("Entered new " + DragAndDrop.visualMode);
            }
            //var tq = (target as TagQuery);
            //tq.matchingTags[draggingIndex].rule = containerRule;
            dragRule = containerRule;

            var tag = draggingItemRepresentation.Q("Tag");
            draggingItemRepresentation.RemoveFromClassList("tag-rule-include");
            draggingItemRepresentation.RemoveFromClassList("tag-rule-exclude");
            draggingItemRepresentation.RemoveFromClassList("tag-rule-any");
            draggingItemRepresentation.AddToClassList(ruleClass);
            tag.AddToClassList("tag-drop-valid");
            draggingItemRepresentation.style.position = new StyleEnum<Position>(Position.Relative);
            draggingItemRepresentation.transform.position = Vector3.zero;
            ve.Add(draggingItemRepresentation);
        }

        bool canDrag = false;
        int draggingIndex = -1;
        BTagged.InclusionRule dragRule;
        TemplateContainer draggingItem = null;
        VisualElement originalContainer = null;
        VisualElement draggingItemRepresentation = null;
        TemplateContainer editingItem = null;
        private void StartDragging(MouseDownEvent mde, TemplateContainer item, int idx)
        {
            if (mde.clickCount > 1)
            {
                StopDragging();
                StartEditing(item);
            }
            else
            {
                canDrag = true;
                draggingIndex = idx;
                draggingItem = item;
                draggingItem.style.opacity = 0.2f;
                originalContainer = item.parent;
                DragAndDrop.PrepareStartDrag();
            }
            mde.StopImmediatePropagation();
        }

        private void StartEditing(TemplateContainer item)
        {
            if (editingItem != null)
            {
                var tq = (target as TagQuery);
                int idx = (int)editingItem.userData;
                editingItem.Q<Label>().text = (target as TagQuery).matchingTags[idx].tag == null ? "Null" : tq.matchingTags[idx].tag.name;
                editingItem.Q("Tag").style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
                editingItem.Q("TagEdit").style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }
            editingItem = item;
            editingItem.Q("Tag").style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            editingItem.Q("TagEdit").style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }
        private void StopEditing()
        {
            if (editingItem == null) return;
            editingItem.Q("Tag").style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            editingItem.Q("TagEdit").style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            editingItem = null;
            ReBuild();
        }


        private void OnMouseMove(Vector2 mouseLoc)
        {
            if (canDrag)
            {
                canDrag = false;
                draggingItemRepresentation = queryTagUI.CloneTree();
                draggingItemRepresentation.pickingMode = PickingMode.Ignore;
                draggingItemRepresentation.Q<Label>().text = draggingItem.Q<Label>().text;
                dragRule = (target as TagQuery).matchingTags[(int)draggingItem.userData].rule;
                //draggingItemRepresentation.style.opacity = 0.5f;
                root.Add(draggingItemRepresentation);
                Debug.Log("Started dragging");
                DragAndDrop.StartDrag("Dragging " + draggingItem);
                DragAndDrop.activeControlID = 1;
            }
            if(draggingItemRepresentation != null)
            {
                if(draggingItemRepresentation.parent == root || draggingItemRepresentation.parent == originalContainer)
                {
                    draggingItemRepresentation.style.position = new StyleEnum<Position>(Position.Absolute);
                    draggingItemRepresentation.transform.position = mouseLoc;
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                }
                else
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                }
                //draggingItemRepresentation.style.left = mouseLoc.x;
                //draggingItemRepresentation.style.top = mouseLoc.y;
            }
            //DragAndDrop.AcceptDrag();
            //DragAndDrop.objectReferences = new UnityEngine.Object[] { item };
        }

        private void StopDragging()
        {
            canDrag = false;
            if(draggingItemRepresentation != null)
            {
                if (draggingItemRepresentation.parent == root || draggingItemRepresentation.parent == originalContainer)
                {
                    //Debug.LogWarning("Releasing " + draggingItemRepresentation);
                    //DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                }
                else
                {
                    //DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                    //Undo.RegisterCompleteObjectUndo(target, "Move Tag");

                    var arr = (target as TagQuery).matchingTags;
                    if (draggingIndex >= 0 && draggingIndex < arr.Length)
                    {
                        serializedObject.Update();
                        //var entry = arr[draggingIndex];
                        serializedObject.FindProperty("matchingTags").GetArrayElementAtIndex(draggingIndex).FindPropertyRelative("rule").enumValueIndex = (int)dragRule;
                        Debug.Log("Changed rule for " + draggingIndex + "  to " + dragRule);
                        //serializedObject.FindProperty("matchingTags").GetArrayElementAtIndex(arr.Length - 1).managedReferenceValue = entry;
                        //arr[draggingIndex] = arr[arr.Length - 1];
                        //arr[arr.Length - 1] = entry;
                        //serializedObject.Update();
                        serializedObject.ApplyModifiedProperties();
                        //ReBuild();
                        //Build();
                    }
                    DragAndDrop.AcceptDrag();
                }
                draggingItemRepresentation.parent?.Remove(draggingItemRepresentation);
                draggingItemRepresentation = null;
            }
            
            if(draggingItem != null)
            {
                draggingItem.Q("Tag").RemoveFromClassList("tag-drop-valid");
                draggingItem.style.opacity = 1f;
            }
            draggingIndex = -1;
            Debug.Log("Stopped dragging");
        }


        private void DeleteTag(int idx)
        {
            serializedObject.Update();
            Undo.RegisterCompleteObjectUndo(target, "Delete Tag");
            var numEntries = serializedObject.FindProperty("matchingTags").arraySize;
            if(numEntries > 1)
            {
                var lastElement = serializedObject.FindProperty("matchingTags").GetArrayElementAtIndex(numEntries-1);
                serializedObject.FindProperty("matchingTags").GetArrayElementAtIndex(idx).FindPropertyRelative("tag").objectReferenceValue = lastElement.FindPropertyRelative("tag").objectReferenceValue;
                serializedObject.FindProperty("matchingTags").GetArrayElementAtIndex(idx).FindPropertyRelative("rule").enumValueIndex = lastElement.FindPropertyRelative("rule").enumValueIndex;
            }
            serializedObject.FindProperty("matchingTags").arraySize = numEntries - 1;
            serializedObject.ApplyModifiedProperties();
            ReBuild();
        }

        private void AddTagWithRule(BTagged.InclusionRule rule)
        {
            serializedObject.Update();
            Undo.RegisterCompleteObjectUndo(target, "Add Tag");
            var defaultTag = BTaggedSOPropertyDrawerBase<BTaggedGroupBase, Tag>.FindDefault();
            Debug.LogWarning("Default tag: " + defaultTag);

            var numEntries = serializedObject.FindProperty("matchingTags").arraySize;
            serializedObject.FindProperty("matchingTags").arraySize = numEntries+1;
            serializedObject.FindProperty("matchingTags").GetArrayElementAtIndex(numEntries).FindPropertyRelative("tag").objectReferenceValue = defaultTag;
            serializedObject.FindProperty("matchingTags").GetArrayElementAtIndex(numEntries).FindPropertyRelative("rule").enumValueIndex = (int)rule;
            serializedObject.ApplyModifiedProperties();
            ReBuild();
        }

    }
}

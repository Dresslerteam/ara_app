using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BOC.BTagged;
[CreateAssetMenu(fileName = "StepInfo", menuName = "Tutorial/StepInfo", order = 0)]
[System.Serializable]
public class StepInfoSO : BTaggedSO {
        [HorizontalGroup("Info",.5f,LabelWidth=2.5f)]
        [VerticalGroup("Info/Left")]
        [Title("Step Title",TitleAlignment = TitleAlignments.Centered)]
        public string stepTitle = "Step";
        [ShowInInspector,MultiLineProperty(6),HorizontalGroup("Info",.5f)]
        [Title("Instructions", TitleAlignment = TitleAlignments.Centered)]
        [HideLabel]
        public string instructions;
        [HorizontalGroup("Info",.5f)]
        [Title("Hint Sprite",TitleAlignment = TitleAlignments.Centered)]
        [VerticalGroup("Info/Left")]
        [PreviewField(90)]
        public Sprite _hintSprite;
        [HorizontalGroup("Color",0f)]
        public bool changeColor = false;
        [HideLabel]
        [HorizontalGroup("Color",0.5f)]
        [ShowIf("changeColor")]
        public Color uiColor;

}
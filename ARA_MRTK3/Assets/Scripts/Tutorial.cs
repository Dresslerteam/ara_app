using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
public class Tutorial : MonoBehaviour
{
    #region Classes
    [System.Serializable][GUIColor(.6f, .74f, .61f)]
    public class Step{
        public StepInfoSO stepInfo;
        [Space(50)]

        [Header("Audio")]
        [FoldoutGroup("Audio")]
        public AudioSource audioSource;
        [FoldoutGroup("Audio")]
        public AudioClip audioClip;
        [FoldoutGroup("Audio")]
        public AudioClip secondaryClip;
        [Header("Events")]
        public UnityEvent OnStart;
        [PropertySpace(0,40)]
        public UnityEvent OnComplete;

    }
    [System.Serializable]
    public class Segment{
        public string name = "Segment";
        [Tooltip("A reference to a gameobject that this segment is based on")]
        public GameObject segmentAsset = null; 
        public List<Step> steps = new List<Step>();
    }
    #endregion
    [Header("Display Components")]
    [SerializeField][Required]
    private TextMeshProUGUI _stepTitleDisplay = null;
    [SerializeField][Required]
    private TextMeshProUGUI _instructionsDisplay = null;
    [SerializeField][Required]
    private Image _hintDisplay = null;
    [SerializeField]
    private Material _highlightMaterial;
    private Material originalMaterial;
    private MeshRenderer highlightedRenderer;
    private List<MeshRenderer> renderers = new List<MeshRenderer>();
    private List<Material> originalMaterials = new List<Material>();
    [Space(25f),Title("Tutorial Segments")]

    public List<Segment> tutorialSegments;
    [Title("DEBUG"),GUIColor(1f,0,0,1f)]
    [SerializeField]
    private bool _debugging = false;
    [ShowIf("_debugging",true)]
    public Step currentStep;
    private Segment currentSegment;
    private AudioSource defaultAudioSource;
    Transform hintRoot = null;
    //Routines
    private IEnumerator delayedForSecondsRoutine;
    private IEnumerator delayForPromptRoutine;
    private IEnumerator waitForAudioRoutine;

    private void Start() {
        defaultAudioSource = GetComponent<AudioSource>();
        if(defaultAudioSource==null){
            defaultAudioSource = gameObject.AddComponent<AudioSource>();
        }
        currentSegment = tutorialSegments[0];
        currentStep = currentSegment.steps[0];
//        hintRoot = _hintDisplay.transform.parent.parent.parent.transform;
 //       hintRoot.localScale = Vector3.zero;
        currentStep.OnStart.Invoke();
        SetVisuals();
    } 
    private void Update() {
        
        //DEBUG
        if(!_debugging)
            return;
         if(Input.GetKeyDown(KeyCode.P)){
             IncrementStepIndex();
         }
    }
    public void PlayCurrentAudioClip(bool continueAfterAudio){
        if(currentStep.audioSource!=null && currentStep.audioClip!=null){
            currentStep.audioSource.clip = (currentStep.audioClip);
            if(currentStep.audioSource.isPlaying)
                currentStep.audioSource.Stop();
            currentStep.audioSource.Play();
            if(continueAfterAudio)
                WaitThenComplete(currentStep.audioClip.length);
        }else if(currentStep.audioSource==null && currentStep.audioClip!=null){
            defaultAudioSource.clip = (currentStep.audioClip);
            if(defaultAudioSource.isPlaying)
                defaultAudioSource.Stop();
            defaultAudioSource.Play();
            if(continueAfterAudio)
                WaitThenComplete(currentStep.audioClip.length);
        }
    }
    public void PlaySecondaryAudioClip(bool continueAfterAudio){
    if(currentStep.audioSource!=null && currentStep.secondaryClip!=null){
        currentStep.audioSource.clip = (currentStep.secondaryClip);
        if(currentStep.audioSource.isPlaying)
            currentStep.audioSource.Stop();
        currentStep.audioSource.Play();
        if(continueAfterAudio)
            WaitThenComplete(currentStep.secondaryClip.length);
    }else if(currentStep.audioSource==null && currentStep.secondaryClip!=null){
        defaultAudioSource.clip = (currentStep.secondaryClip);
        if(defaultAudioSource.isPlaying)
            defaultAudioSource.Stop();
        defaultAudioSource.Play();
        if(continueAfterAudio)
            WaitThenComplete(currentStep.secondaryClip.length);
    }
    }
    public void WaitThenComplete(float seconds){
        if(delayedForSecondsRoutine!=null){
            StopCoroutine(delayedForSecondsRoutine);
        }
        delayedForSecondsRoutine = Delay(seconds);
        StartCoroutine(delayedForSecondsRoutine);
    }
    public void WaitThenPrompt(float seconds){
        if(delayForPromptRoutine!=null){
            StopCoroutine(delayForPromptRoutine);
        }
        delayForPromptRoutine = PromptWaiting(seconds);
        StartCoroutine(delayForPromptRoutine);
    }
    public void WaitThenSecondary(float seconds){
        if(delayForPromptRoutine!=null){
            StopCoroutine(delayForPromptRoutine);
        }
        delayForPromptRoutine = PromptSecondary(seconds);
        StartCoroutine(delayForPromptRoutine);
    }
    private IEnumerator Delay(float seconds){
        yield return new WaitForSeconds(seconds);
        currentStep.OnComplete.Invoke();
    }
    private IEnumerator PromptWaiting(float seconds){
        yield return new WaitForSeconds(seconds);
        PlayCurrentAudioClip(false);
        //This makes it loop forever until action completed
        yield return new WaitForSeconds(currentStep.audioClip.length);
        WaitThenPrompt(seconds);    
    }
    private IEnumerator PromptSecondary(float seconds){
        yield return new WaitForSeconds(seconds);
        PlaySecondaryAudioClip(false);
        yield return new WaitForSeconds(currentStep.secondaryClip.length);
        WaitThenSecondary(seconds);
    }
    /// <summary>
    /// Advance to the next step. If there is no more steps in the segment, 
    /// continue on to the next segment's first step.
    /// </summary>
    public void NextStep()
    {
        //If we reminded the player to act, and they did before we needed to again, stop coroutine
        if (delayForPromptRoutine != null)
        {
            StopCoroutine(delayForPromptRoutine);
            delayForPromptRoutine = null;
        }
        //If there is audio playing, wait for it to finish
        if (currentStep.audioSource != null && currentStep.audioClip != null)
        {
            if (currentStep.audioSource.isPlaying)
            {
                if(waitForAudioRoutine!=null){
                    StopCoroutine(waitForAudioRoutine);
                }
                waitForAudioRoutine = WaitForAudioToFinish();
                StartCoroutine(waitForAudioRoutine);
            }else{
                IncrementStepIndex();        
            }
        }else{
            IncrementStepIndex();
        }
    }
    /// <summary>
    /// Turn the segment asset on and off. This is useful to activate 
    /// things that will progress the tutorial.
    /// </summary>
    public void SegmentAssetToggle(bool isOn){
        currentSegment.segmentAsset.SetActive(isOn);
    }

    private IEnumerator WaitForAudioToFinish(){
        while(currentStep.audioSource.isPlaying || defaultAudioSource.isPlaying){
            yield return new WaitForEndOfFrame();
        }
        IncrementStepIndex();
    }

    private void IncrementStepIndex()
    {
        if(waitForAudioRoutine!=null){
            StopCoroutine(waitForAudioRoutine);
            waitForAudioRoutine = null;
        }
        int curStepIndex = currentSegment.steps.IndexOf(currentStep);
        int curSegmentIndex = tutorialSegments.IndexOf(currentSegment);

        //If the currentStep is the last step of the segment, go to the next segment
        if (curStepIndex == currentSegment.steps.Count - 1)
        {
            //Go to the next segment
            if (tutorialSegments.Count - 1 >= curSegmentIndex + 1)
            {
                currentSegment = tutorialSegments[curSegmentIndex + 1];
                curStepIndex = -1;
            }
            else
            {
                Debug.Log("<color=yellow>End of tutorial.</color>");
                return;
            }
        }
        int nextStepIndex = curStepIndex + 1;
        currentStep = currentSegment.steps[nextStepIndex];
        currentStep.OnStart.Invoke();
        SetVisuals();
    }
    public void HighlightObject(MeshRenderer renderer){
        highlightedRenderer = renderer;
        originalMaterial = highlightedRenderer.material;
        highlightedRenderer.material = _highlightMaterial;
    }
    public void HighlightAllChildren(Transform parent){
        //Resets the list
        // renderers.Clear();

        renderers.AddRange(parent.GetComponentsInChildren<MeshRenderer>());
        foreach (var renderer in renderers)
        {
            originalMaterials.Add(renderer.material);
            renderer.material = _highlightMaterial;
        }
    }
    public void UnhighlightAllChildren(){
        if(renderers.Count<1){
            Debug.LogWarning("<color=red>There is nothing to unhighlight.</color>");
            return;
        }
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material = originalMaterials[i];
        } 
        renderers.Clear();
        originalMaterials.Clear();
    }
    public void UnhighlightObject(){
    //Reset any highlighted materials
        if(highlightedRenderer!=null){
            highlightedRenderer.material = originalMaterial;
            highlightedRenderer = null;
        }
    }
    private void SetVisuals(){
        //hintRoot.localRotation = Quaternion.Euler(Vector3.zero);
        //Set the step title text
        _stepTitleDisplay.text = currentStep.stepInfo.stepTitle;
        //Set the instructions text
        _instructionsDisplay.text = currentStep.stepInfo.instructions;
        //If there is a hint sprite, display it
        if(currentStep.stepInfo._hintSprite!=null){
            _hintDisplay.sprite = currentStep.stepInfo._hintSprite;
        }
        if(currentStep.stepInfo.changeColor){
            // _shapeGroup.Color = currentStep.stepInfo.uiColor;
            _hintDisplay.color = currentStep.stepInfo.uiColor;
        }
    }
}

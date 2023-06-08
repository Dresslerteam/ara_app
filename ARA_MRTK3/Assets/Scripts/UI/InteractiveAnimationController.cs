
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UX;
using UnityEngine;
using Slider = Microsoft.MixedReality.Toolkit.UX.Slider;

public class InteractiveAnimationController : MonoBehaviour
{
    public Animator animator;
    public AnimationClip animationClip;
    public List<AnimationStep> keyframes;
    public PressableButton nextButton;
    public PressableButton previousButton;
    public Slider slider;
    public Transform midpointTransform;
   // public float transitionDuration = 1f;
    [SerializeField]
    private Material highlightMaterial;
    [SerializeField]
    private Material HiddenMaterial;

    private Dictionary<MeshRenderer, Material> originalMaterials = new Dictionary<MeshRenderer, Material>();
    
    private int lastKeyframeIndex = 0;
    private Coroutine currentLerpCoroutine;
    private bool shouldStopLerping = false;

    private int currentTargetFrame = 0;
    private float currentFrame = 0;
    private float animationSpeed = .2f;
    private bool shouldAnimate = false;
    private void Start()
    {
        if (animator == null || animationClip == null || keyframes == null || keyframes.Count == 0)
        {
            Debug.LogError("Please ensure all necessary references have been assigned in the Inspector.");
            return;
        }

        animator.enabled = true;
        animator.speed = 0;

        if (nextButton != null)
            nextButton.OnClicked.AddListener(() => UpdateSliderValue(1f));
        if (previousButton != null)
            previousButton.OnClicked.AddListener(() => UpdateSliderValue(-1f));

    }
    
    private void UpdateSliderValue(float increment)
    {
        float segments = keyframes.Count+1;
        float segmentValue = 1f / (segments - 1);
        slider.Value = Mathf.Clamp(slider.Value + increment*segmentValue, slider.MinValue, slider.MaxValue);
    }
    public void AdvanceViaSlider(SliderEventData sliderEventData)
    {
        float t = sliderEventData.NewValue;
        int targetIndex = Mathf.Clamp(Mathf.FloorToInt(t * keyframes.Count), 0, keyframes.Count - 1);
        AdvanceToTargetKeyframe(targetIndex);
    }

    private void AdvanceToTargetKeyframe(int targetIndex)
    {
        AnimationStep step = keyframes[Mathf.Clamp(targetIndex, 0, keyframes.Count - 1)];
        AnimationStep HighlightStep = keyframes[Mathf.Clamp(   targetIndex -(targetIndex > lastKeyframeIndex? 1:0), 0, keyframes.Count - 1)];




        currentTargetFrame = step.frame;
        shouldAnimate = true;
        ResetHighlightMaterials();
        for (int i = 0; i < HighlightStep.partsToHighlight.Count; i++)
        {
            MeshRenderer meshRenderer = HighlightStep.partsToHighlight[i];
            if (meshRenderer != null)
            {
                // Cache the original material
                originalMaterials[meshRenderer] = meshRenderer.material;

                // Assign the highlightMaterial
                meshRenderer.material = highlightMaterial;
            }
        }
        for (int i = 0; i < step.partsToHide.Count; i++)
        {
            MeshRenderer meshRenderer = step.partsToHide[i];
            if (meshRenderer != null)
            {
                // Cache the original material
                originalMaterials[meshRenderer] = meshRenderer.material;

                // Assign the HiddenMaterial
                meshRenderer.material = HiddenMaterial;
            }
        }
        lastKeyframeIndex = targetIndex;
/*
        if (targetIndex < keyframes.Count)
        {
            shouldStopLerping = true;
            if (currentLerpCoroutine != null)
            {
                StopCoroutine(currentLerpCoroutine);
            }

            // Reset highlighted materials to their original state before moving to the next step
            ResetHighlightMaterials();

            shouldStopLerping = false;
            currentLerpCoroutine = StartCoroutine(LerpToNextKeyframe(lastKeyframeIndex, targetIndex));

            // Find the appropriate AnimationStep based on the targetIndex
            int lastIndex = targetIndex - 1;
            lastIndex = Mathf.Clamp(lastIndex, 0, keyframes.Count - 1);
            AnimationStep step = keyframes[lastIndex];

            // Loop through partsToHighlight and assign highlightMaterial while caching the original materials
            for (int i = 0; i < step.partsToHighlight.Count; i++)
            {
                MeshRenderer meshRenderer = step.partsToHighlight[i];
                if (meshRenderer != null)
                {
                    // Cache the original material
                    originalMaterials[meshRenderer] = meshRenderer.material;

                    // Assign the highlightMaterial
                    meshRenderer.material = highlightMaterial;
                }
            }
        }
        else
        {
            Debug.Log("Reached the end of keyframes.");
        }*/
    }
    
    private void ResetHighlightMaterials()
    {
        foreach (KeyValuePair<MeshRenderer, Material> entry in originalMaterials)
        {
            MeshRenderer meshRenderer = entry.Key;
            Material originalMaterial = entry.Value;
            meshRenderer.material = originalMaterial;
        }

        originalMaterials.Clear();
    }


    private IEnumerator LerpToNextKeyframe(int startIndex, int endIndex)
    {
        float startTime = (float)keyframes[startIndex].frame / animationClip.frameRate;
        float endTime = (float)keyframes[endIndex].frame / animationClip.frameRate;

        float duration = Mathf.Abs(endTime - startTime);
        float elapsedTime = 0f;
        
        while (elapsedTime < duration && !shouldStopLerping)
        {
            float t = elapsedTime / duration;
            float currentTime = Mathf.Lerp(startTime, endTime, t);
            animationClip.SampleAnimation(animator.gameObject, currentTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set the final keyframe
        if (!shouldStopLerping)
        {
            animationClip.SampleAnimation(animator.gameObject, endTime);
        }

        lastKeyframeIndex = endIndex; // Store the index of the current keyframe
    }


    private void Update()
    {
        if (slider != null)
        {
            // Get the midpoint between the main camera and the animating object, include a minumum distance
            Vector3 midpoint = (Camera.main.transform.position + animator.gameObject.transform.position) / 2f;
            float distance = Vector3.Distance(Camera.main.transform.position, animator.gameObject.transform.position);
            float minDistance = 0.5f;
            if (distance < minDistance)
            {
                midpoint = Vector3.Lerp(Camera.main.transform.position, animator.gameObject.transform.position, minDistance / distance);
            }
            // set position of midPoint Transform
            midpointTransform.position = midpoint;
        }
        UpdateAnimationFrame();
    }


    void UpdateAnimationFrame()
    {
        if (shouldAnimate && currentFrame != currentTargetFrame)
        {
            if (currentFrame > currentTargetFrame)
            {
                currentFrame -= (Time.fixedDeltaTime * 100f * animationSpeed);
                if (currentFrame < currentTargetFrame)
                {
                    shouldAnimate = false;
                    currentFrame = currentTargetFrame;
                }
            }
            else
            {
                currentFrame += (Time.fixedDeltaTime * 100f * animationSpeed);
                if (currentFrame > currentTargetFrame)
                {
                    shouldAnimate = false;
                    currentFrame = currentTargetFrame;
                }

            }

            animator.Play(animationClip.name, 0, (1f / (animationClip.frameRate * animationClip.length) * currentFrame));

        }
    }
}
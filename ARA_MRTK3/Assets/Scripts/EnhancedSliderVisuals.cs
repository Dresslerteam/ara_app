// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.UX
{
    [RequireComponent(typeof(Slider))]
    [ExecuteAlways]
    /// <summary>
    /// A visuals script to provide a visual layer on top of
    /// <see cref="Slider"/>.
    /// </summary>
    [AddComponentMenu("MRTK/UX/Enhanced Slider Visuals")]
    public class EnhancedSliderVisuals : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The transform of the handle.")]
        private Transform handle;

        /// <summary>
        /// The transform of the handle.
        /// </summary>
        public Transform Handle
        {
            get => handle;
            set => handle = value;
        }

        [SerializeField]
        [Tooltip("The transform of the overall slider track area.")]
        private Transform trackArea;

        /// <summary>
        /// The transform of the overall slider track area.
        /// </summary>
        /// <remarks>
        /// This is snapped between the start/end transforms defined in the <see cref="Slider"/> script.
        /// </remarks>
        public Transform TrackArea
        {
            get => trackArea;
            set => trackArea = value;
        }

        [SerializeField]
        [Tooltip("The transform of the area representing the 'progress' of the slider.")]
        private Transform fillVisual;

        /// <summary>
        /// The transform of the area representing the 'progress' of the slider.
        /// </summary>
        public Transform FillVisual
        {
            get => fillVisual;
            set => fillVisual = value;
        }

        private Slider sliderState;

        /// <summary>
        /// Reference to the <see cref="Slider"/> component.
        /// </summary>
        protected Slider SliderState
        {
            get
            {
                if (sliderState == null)
                {
                    sliderState = GetComponent<Slider>();
                }

                return sliderState;
            }
        }
        [SerializeField]
        [Tooltip("The tick mark prefab.")]
        private GameObject tickMarkPrefab;

        [SerializeField]
        [Tooltip("The number of steps for the slider.")]
        private int steps = 12;

        private void Start()
        {
            if (tickMarkPrefab != null)
            {
                //CreateTickMarks();
            }
        }

        void Update()
        {
            handle.position = SliderState.SliderStart.position + (SliderState.NormalizedValue * SliderState.SliderTrackDirection);
            trackArea.transform.position = (SliderState.SliderStart.position + SliderState.SliderEnd.position) * 0.5f;

            Vector2 localSliderTrackDirection = SliderState.transform.InverseTransformDirection(SliderState.SliderTrackDirection);
            float angle = Vector2.SignedAngle(Vector2.right, localSliderTrackDirection);
            trackArea.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            handle.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            trackArea.localScale = new Vector3(SliderState.SliderTrackDirection.magnitude, trackArea.localScale.y, trackArea.localScale.z);

            if (fillVisual != null)
            {
                fillVisual.localPosition = new Vector3(-0.5f + SliderState.NormalizedValue * 0.5f, fillVisual.localPosition.y, fillVisual.localPosition.z);
                fillVisual.localScale = new Vector3(SliderState.NormalizedValue, fillVisual.localScale.y, fillVisual.localScale.z);
            }
        }
        private void CreateTickMarks()
        {
            int steps = 12;
            float trackLength = SliderState.SliderTrackDirection.magnitude;
            float stepSize = trackLength / steps;
            Vector3 stepVector = stepSize * SliderState.SliderTrackDirection.normalized;

            for (int i = 0; i <= steps; i++)
            {
                GameObject tickMark = Instantiate(tickMarkPrefab, trackArea);
                Vector3 tickMarkPosition = SliderState.SliderStart.position + (i * stepVector);
                tickMark.transform.position = tickMarkPosition;

                // Set the tick mark's rotation to match the trackArea's rotation
                tickMark.transform.rotation = trackArea.transform.rotation;

                // Set the tick mark's Y scale to 4
                tickMark.transform.localScale = new Vector3(tickMark.transform.localScale.x, 4, tickMark.transform.localScale.z);
            }
        }

    }
}

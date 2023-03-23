using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Subsystems;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
    /// ハンド定規
    /// 参考動画：https://twitter.com/hi_rom_/status/1267100537578639363
    /// </summary>
    public class TwoHandsRuler : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI DistanceText = default;

        [SerializeField]
        private LineRenderer line = default;

        [SerializeField] private GameObject segmentPrefab;
        private HandsAggregatorSubsystem aggregator = null;
        
        private float distance = 0;
        [SerializeField] [Range(0,1)] private float pinchTolerance = .9f;
        [SerializeField] [Range(0,1)] private float openTolerance = .2f;

        private bool canPlace = false;
        void Start()
        {
            aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
            if (aggregator == null)
            {
                Debug.LogError("Can't get IMixedRealityHandJointService.");
                return;
            }

            Initialize();
        }

        public void Initialize()
        {
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
            DistanceText.text = "0 cm";
        }

        void Update()
        {
            // Get Left Tip
            var leftIndexValid = aggregator.TryGetJoint(TrackedHandJoint.IndexTip, XRNode.LeftHand, out HandJointPose leftIndexTip);
             
            if (!leftIndexValid)
            {
                //Debug.Log("leftIndexTip is null.");
                return;
            }
             
            var rightIndexValid = aggregator.TryGetJoint(TrackedHandJoint.IndexTip, XRNode.RightHand, out HandJointPose rightIndexTip);
             
            if (!rightIndexValid)
            {
                //Debug.Log("leftIndexTip is null.");
                return;
            }

            // Set Line Renderer
            line.SetPosition(0, leftIndexTip.Position);
            line.SetPosition(1, rightIndexTip.Position);
            line.startWidth = 0.001f;
            line.endWidth = 0.001f;
            // Calculate Distance
            distance = Vector3.Distance(leftIndexTip.Position, rightIndexTip.Position);
            distance = distance * 100;
            DistanceText.text = distance.ToString("0.0") + " cm";
            DistanceText.transform.parent.position = (leftIndexTip.Position + rightIndexTip.Position) / 2;
            DistanceText.transform.parent.LookAt(Camera.main.transform.position);
            
            //Check Pinch
            bool leftHandPinch = aggregator.TryGetPinchProgress(XRNode.LeftHand, out bool leftIsReadyToPinch,
                out bool leftIsPinching, out float leftPinchAmount);
            bool rightHandPinch = aggregator.TryGetPinchProgress(XRNode.RightHand, out bool rightIsReadyToPinch,
                out bool rightIsPinching, out float rightPinchAmount);
            // Check if Open
            if (leftHandPinch && rightHandPinch && !canPlace && (leftPinchAmount < openTolerance ||
                                                                 rightPinchAmount < openTolerance))
            {
                canPlace = true;
            }
                
            // Pinch to place 
            if (canPlace && leftHandPinch && rightHandPinch && leftPinchAmount > pinchTolerance &&
                rightPinchAmount > pinchTolerance)
            {
                PlaceMeasurement(leftIndexTip, rightIndexTip);
            }
        }

        public void PlaceMeasurement(HandJointPose leftIndexTip, HandJointPose rightIndexTip)
        {
            canPlace = false;
            GameObject spawnedSegment = Instantiate(segmentPrefab);
            MeasureSegment segment = spawnedSegment.GetComponent<MeasureSegment>();
            segment.Init(leftIndexTip.Position,rightIndexTip.Position,distance);
        }
    }
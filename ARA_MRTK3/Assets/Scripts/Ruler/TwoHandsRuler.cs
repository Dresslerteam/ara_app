// using Microsoft.MixedReality.Toolkit;
// using Microsoft.MixedReality.Toolkit.Input;
// using Microsoft.MixedReality.Toolkit.Utilities;
// using TMPro;
// using UnityEngine;
//
//     /// <summary>
//     /// ハンド定規
//     /// 参考動画：https://twitter.com/hi_rom_/status/1267100537578639363
//     /// </summary>
//     public class TwoHandsRuler : MonoBehaviour
//     {
//         [SerializeField]
//         private TextMeshProUGUI DistanceText = default;
//
//         [SerializeField]
//         private LineRenderer line = default;
//
//         [SerializeField] private GameObject segmentPrefab;
//         private IMixedRealityHandJointService handJointService = null;
//         private IMixedRealityDataProviderAccess dataProviderAccess = null;
//
//         private Transform leftIndexTip;
//         private Transform rightIndexTip;
//         private float distance = 0;
//         void Start()
//         {
//             handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
//             if (handJointService == null)
//             {
//                 Debug.LogError("Can't get IMixedRealityHandJointService.");
//                 return;
//             }
//
//             dataProviderAccess = CoreServices.InputSystem as IMixedRealityDataProviderAccess;
//             if (dataProviderAccess == null)
//             {
//                 Debug.LogError("Can't get IMixedRealityDataProviderAccess.");
//                 return;
//             }
//
//             // ハンドレイを非表示にする
//             //PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);
//
//             Initialize();
//         }
//
//         public void Initialize()
//         {
//             line.SetPosition(0, Vector3.zero);
//             line.SetPosition(1, Vector3.zero);
//             DistanceText.text = "0 cm";
//         }
//
//         void Update()
//         {
//             // Get Left Tip
//             leftIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left);
//             if (leftIndexTip == null)
//             {
//                 Debug.Log("leftIndexTip is null.");
//                 return;
//             }
//
//             // Get Right Tip
//             rightIndexTip = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right);
//             if (rightIndexTip == null)
//             {
//                 Debug.Log("rightIndexTip is null.");
//                 return;
//             }
//
//             // Set Line Renderer
//             line.SetPosition(0, leftIndexTip.position);
//             line.SetPosition(1, rightIndexTip.position);
//             line.startWidth = 0.001f;
//             line.endWidth = 0.001f;
//             // Calculate Distance
//             distance = Vector3.Distance(leftIndexTip.position, rightIndexTip.position);
//             distance = distance * 100;
//             DistanceText.text = distance.ToString("0.0") + " cm";
//             DistanceText.transform.parent.position = (leftIndexTip.position + rightIndexTip.position) / 2;
//             DistanceText.transform.parent.LookAt(Camera.main.transform.position);
//         }
//
//         public void PlaceMeasurement()
//         {
//             GameObject spawnedSegment = Instantiate(segmentPrefab);
//             MeasureSegment segment = spawnedSegment.GetComponent<MeasureSegment>();
//             segment.Init(leftIndexTip.position,rightIndexTip.position,distance);
//         }
//     }
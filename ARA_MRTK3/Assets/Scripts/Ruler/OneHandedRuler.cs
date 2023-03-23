 using Microsoft.MixedReality.Toolkit;
 using Microsoft.MixedReality.Toolkit.Input;
 using Microsoft.MixedReality.Toolkit.Subsystems;
 using TMPro;
 using UnityEngine;
 using UnityEngine.XR;


 public class OneHandedRuler : MonoBehaviour
     {
         [SerializeField]
         private TextMeshProUGUI leftDistanceText = default;

         [SerializeField]
         private TextMeshProUGUI rightDistanceText = default;

         [SerializeField]
         private LineRenderer leftLine = default;

         [SerializeField]
         private LineRenderer rightLine = default;

         private HandsAggregatorSubsystem aggregator = null;

         void Start()
         {
             // Get a reference to the aggregator.
             aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
             if (this.aggregator == null)
             {
                 Debug.LogError("Can't get IMixedRealityHandJointService.");
                 return;
             }
             // ハンドレイを非表示にする
             //PointerUtils.SetHandRayPointerBehavior(PointerBehavior.AlwaysOff);

             Initialize();
         }

         public void Initialize()
         {
             leftLine.SetPosition(0, Vector3.zero);
             leftLine.SetPosition(1, Vector3.zero);
             leftDistanceText.text = "0 cm";

             rightLine.SetPosition(0, Vector3.zero);
             rightLine.SetPosition(1, Vector3.zero);
             rightDistanceText.text = "0 cm";
         }

         void Update()
         {
             var leftIndexValid = aggregator.TryGetJoint(TrackedHandJoint.IndexTip, XRNode.LeftHand, out HandJointPose leftIndexTip);
             
             if (!leftIndexValid)
             {
                 Debug.Log("leftIndexTip is null.");
                 return;
             }

             // 左手 親指
             var leftThumbValid = aggregator.TryGetJoint(TrackedHandJoint.ThumbTip, XRNode.LeftHand,
                 out HandJointPose leftThumbTip);
             if (!leftThumbValid)
             {
                 Debug.Log("leftThumbTip is null.");
                 return;
             }

             // 線を描画
             leftLine.SetPosition(0, leftIndexTip.Position);
             leftLine.SetPosition(1, leftThumbTip.Position);
             leftLine.startWidth = 0.001f;
             leftLine.endWidth = 0.001f;

             // 距離を算出
             var leftDistance = Vector3.Distance(leftIndexTip.Position, leftThumbTip.Position);
             // cmに変換
             leftDistance = leftDistance * 100;
             leftDistanceText.text = leftDistance.ToString("0.0") + " cm";
             leftDistanceText.transform.position = (leftIndexTip.Position + leftThumbTip.Position) / 2;
            
             //Right 
             
             var rightIndexValid = aggregator.TryGetJoint(TrackedHandJoint.IndexTip, XRNode.RightHand, out HandJointPose rightIndexTip);
             
             if (!rightIndexValid)
             {
                 Debug.Log("leftIndexTip is null.");
                 return;
             }

             // 左手 親指
             var rightThumbValid = aggregator.TryGetJoint(TrackedHandJoint.ThumbTip, XRNode.RightHand,
                 out HandJointPose rightThumbTip);
             if (!rightThumbValid)
             {
                 Debug.Log("leftThumbTip is null.");
                 return;
             }

             // 線を描画
             rightLine.SetPosition(0, rightIndexTip.Position);
             rightLine.SetPosition(1, rightThumbTip.Position);
             rightLine.startWidth = 0.001f;
             rightLine.endWidth = 0.001f;

             // 距離を算出
             var rightDistance = Vector3.Distance(rightIndexTip.Position, rightThumbTip.Position);
             // cmに変換
             rightDistance = rightDistance * 100;
             rightDistanceText.text = rightDistance.ToString("0.0") + " cm";
             rightDistanceText.transform.position = (rightIndexTip.Position + rightThumbTip.Position) / 2;


         }
     }
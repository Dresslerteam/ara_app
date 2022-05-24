    using UnityEngine;

    [System.Serializable]
    public class KeyPart
    {
        public Transform transformToAnimate;
        [Header("Time")]
        [Range(0,5)]
        [Tooltip("How long to wait before starting the animation")]
        public float delay = .5f;
        [Header("Movement")]
        [Range(0, 4)]
        public float moveSpeedMultiplier = 1f;
        public Vector3 directionToExplode = Vector3.forward;
        public bool localDirection = true;
        public AnimationCurve movementLerp = new AnimationCurve(new Keyframe(0,0),new Keyframe(1,1));
        [Range(0,4)]
        public float distanceToMove = 1f;

        [Header("Rotation")] public Vector3 rotationToAdd = new Vector3(0, 180, 0);
        public float rotationSpeedMultiplier = 1f;
        public AnimationCurve rotationLerp = new AnimationCurve(new Keyframe(0,0),new Keyframe(1,1));
        
    }

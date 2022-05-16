using UnityEngine;
    
[System.Serializable]
    public class GlobalKeyPart : KeyPart
    {
        [Header("Values to copy")]
        public bool globalDelay;
        public bool globalMoveSpeedMultiplier;
        public bool globalDirectionToExplode;
        public bool globalLocalDirection;
        public bool globalMovementLerp;
        public bool globalDistanceToMove;
        public bool globalRotationSpeedMultiplier;
        public bool globalRotationLerp;
        public bool globalRotationToAdd;
    }
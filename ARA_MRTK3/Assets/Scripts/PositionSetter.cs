using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSetter : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;
    public void ResetToTargetWithOffset()
    {
        if(target != null)
            return;
        transform.position = target.transform.position + offset;
    }
}

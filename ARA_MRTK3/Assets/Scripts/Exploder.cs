using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UX;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Exploder : MonoBehaviour
{
    // Populate a list of all the parts you want to move
    // We need a class that holds: Delay, speed, direction of explosion, magnitude
    // Two public events can be called to explode/assemble
    [Required]
    [SerializeField] private Transform rootTransform;

    [Searchable]
    [SerializeField] private List<KeyPart> keyParts;
    [PropertyTooltip("These are settings that you can apply to every part.")]
    [SerializeField] private GlobalKeyPart globalPart;
    
    /// <summary>
    /// Look through the top-most children of the root object, make a KeyPart for them, and add to the parts list
    /// </summary>
    [Button]
    public void PopulatePartList()
    {
        if (!rootTransform)
        {
            Debug.LogError("You need to assign a rootTransform");
            return;
        }
        for (int i = 0; i < rootTransform.childCount; i++)
        {
            KeyPart childPart = new KeyPart
            {
                transformToAnimate = rootTransform.GetChild(i)
            };
            keyParts.Add(childPart);
        }
    }
    [Button]
    public void SetupColliders()
    {
        for (int i = 0; i < rootTransform.childCount; i++)
        {

            List<MeshFilter> meshFilter = new List<MeshFilter>();
            meshFilter.AddRange(rootTransform.GetChild(i).GetComponentsInChildren<MeshFilter>());
            foreach (var mesh in meshFilter)
            {
                // If we don't have a collider, let's go ahead and add one.
                if (!mesh.GetComponent<MeshCollider>())
                {
                    MeshCollider meshCollider = mesh.gameObject.AddComponent<MeshCollider>();
                    meshCollider.sharedMesh = mesh.sharedMesh;   
                }

                // Since these have coliders, we can go ahead setup the Tooltips
                //ToolTipSpawner toolTipSpawner = mesh.gameObject.AddComponent<ToolTipSpawner>();
                //toolTipSpawner.ChoosePrefab(tooltipPrefab);
                //toolTipSpawner.UpdateAnchor(rootTransform.GetChild(i));
                //toolTipSpawner.UpdateTooltipText(rootTransform.GetChild(i).name);
            }
            
        }
    }
    [ContextMenu("ExplodeEntireModel")]
    public void ExplodeEntireModel()
    {
        foreach (var part in keyParts)
        {
            StartCoroutine(MovePart(part));
            StartCoroutine(RotatePart(part));
        }
    }
    
    public void ExplodePart(KeyPart part)
    {
        StartCoroutine(MovePart(part));
        StartCoroutine(RotatePart(part));
    }
    private IEnumerator MovePart(KeyPart part)
    {
        yield return new WaitForSeconds(part.delay);
        Vector3 moveDir = part.directionToExplode;
        float timePassed = 0;
        Vector3 startPosition = part.transformToAnimate.position;
        Vector3 endPosition = startPosition;
        moveDir = ConvertDirection(part, moveDir);
        endPosition += moveDir*part.distanceToMove;
        
        while (timePassed < 1)
        {
            Vector3 translation = Vector3.Lerp(startPosition, endPosition,
                part.movementLerp.Evaluate(timePassed));
            timePassed += Time.deltaTime * part.moveSpeedMultiplier;
            part.transformToAnimate.position = translation;
            yield return null;    
        }
        
    }
    /// <summary>
    /// Rotates the part over time.
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    private IEnumerator RotatePart(KeyPart part)
    {
        yield return new WaitForSeconds(part.delay);
        Quaternion startRot = part.transformToAnimate.localRotation;
        Quaternion endRot = startRot * Quaternion.Euler(part.rotationToAdd);
        float timePassed = 0;
        while (timePassed < 1)
        {
            Quaternion rot = Quaternion.Lerp(startRot, endRot, part.rotationLerp.Evaluate(timePassed));
            timePassed += Time.deltaTime * part.rotationSpeedMultiplier;
            part.transformToAnimate.localRotation = rot;
            yield return null;
        }
        
    }
    /// <summary>
    /// Converts a direction from world to local if the KeyPart calls for Local Direction
    /// </summary>
    /// <param name="part">The part that holds the bool</param>
    /// <param name="moveDir">The direction it will move</param>
    /// <returns></returns>
    public static Vector3 ConvertDirection(KeyPart part, Vector3 moveDir)
    {
        // Convert direction to local direction
        if (part.localDirection)
        {
            moveDir = part.transformToAnimate.InverseTransformDirection(moveDir);
        }

        return moveDir;
    }

    /// <summary>
    /// Copies the values from the global part data to ALL of the other parts.
    /// </summary>
    [Button]
    public void UpdateAllPartsPropertiesToGlobalValues()
    {
        foreach (var part in keyParts)
        {
            if (globalPart.globalDelay)
                part.delay = globalPart.delay;
            if (globalPart.globalMoveSpeedMultiplier)
                part.moveSpeedMultiplier = globalPart.moveSpeedMultiplier;
            if (globalPart.globalDirectionToExplode)
                part.directionToExplode = globalPart.directionToExplode;
            if (globalPart.globalLocalDirection)
                part.localDirection = globalPart.localDirection;
            if (globalPart.globalMovementLerp)
                part.movementLerp = globalPart.movementLerp;
            if (globalPart.globalDistanceToMove)
                part.distanceToMove = globalPart.distanceToMove;
            if (globalPart.globalRotationSpeedMultiplier)
                part.rotationSpeedMultiplier = globalPart.rotationSpeedMultiplier;
            if (globalPart.globalRotationLerp)
                part.rotationLerp = globalPart.rotationLerp;
            if (globalPart.globalRotationToAdd)
            {
                part.rotationToAdd = globalPart.rotationToAdd;
            }
        }
    }

    /// <summary>
    /// Some lines and debug stuff for in editor view.
    /// </summary>
    private void OnDrawGizmosSelected()
    {

        foreach (var part in keyParts)
        {
            Gizmos.color = part.localDirection ? Color.green : Color.red;

            Gizmos.DrawRay(part.transformToAnimate.position,ConvertDirection(part, part.directionToExplode)*part.distanceToMove);
        }
        
    }
    
    
    
    // public void Click(MixedRealityPointerEventData eventData)
    // {
    //     var result = eventData.Pointer.Result;
    //     if(result.Details.Object.gameObject.layer == LayerMask.NameToLayer("UI"))
    //         return;
    //     Debug.Log("result: "+result.Details.Object.name);
    //     for (int i = 0; i < keyParts.Count; i++)
    //     {
    //         if (result.Details.Object.transform.IsChildOf(keyParts[i].transformToAnimate))
    //         {
    //             ExplodePart(keyParts[i]);
    //             Debug.Log("keypart: "+keyParts[i].transformToAnimate.gameObject.name);   
    //         }
    //     }
    // }
    
}



using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicExploder : MonoBehaviour
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
    public void Explode()
    {
        foreach (var part in keyParts)
        {
            StartCoroutine(MovePart(part));
            StartCoroutine(RotatePart(part));
        }
    }

    private IEnumerator MovePart(KeyPart part)
    {
        yield return new WaitForSeconds(part.delay);
        Vector3 moveDir = part.directionToExplode;
        float timePassed = 0;
        Vector3 startPosition = part.transformToAnimate.position;
        Vector3 endPosition = startPosition;
        moveDir = ConvertDirection(part, moveDir);
        endPosition += moveDir * part.distanceToMove;

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
    private static Vector3 ConvertDirection(KeyPart part, Vector3 moveDir)
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

    private void OnDrawGizmosSelected()
    {

        foreach (var part in keyParts)
        {
            Gizmos.color = part.localDirection ? Color.green : Color.red;

            Gizmos.DrawRay(part.transformToAnimate.position, ConvertDirection(part, part.directionToExplode) * part.distanceToMove);
        }

    }
}

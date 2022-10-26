using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEvent : MonoBehaviour
{
    public void ToggleGameObject()
    {
         this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}

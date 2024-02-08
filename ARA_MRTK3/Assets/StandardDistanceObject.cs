using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardDistanceObject : MonoBehaviour
{
    public float distance = 0;
    Transform start;
    Transform end;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(start.position, this.transform.position);
        //distance = distance * 100;
    }
}

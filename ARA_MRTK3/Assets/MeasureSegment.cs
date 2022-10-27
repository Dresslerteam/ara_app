using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeasureSegment : MonoBehaviour
{
    [SerializeField] private LineRenderer line;

    [SerializeField] private TextMeshProUGUI text;

    private bool initiated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!initiated)
            return;
        text.transform.parent.LookAt(Camera.main.transform.position);
    }

    public void Init(Vector3 leftPos, Vector3 rightPos, float distance)
    {
        line.SetPosition(0, leftPos);
        line.SetPosition(1, rightPos);
        line.startWidth = 0.001f;
        line.endWidth = 0.001f;
        text.transform.parent.position = (leftPos + rightPos) / 2;
        text.text = distance.ToString("0.0") + " cm";
        initiated = true;
    }
}

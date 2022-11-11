using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Callout : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private TextMeshPro _calloutTextmesh;
    [SerializeField] private Transform _targetTransform;

    [SerializeField] private SolverHandler _handler;

    [SerializeField] private bool doAlternateText = false;

    [ShowIf("doAlternateText")] [SerializeField]
    private string altText = "alt";
    // Start is called before the first frame update
    void Start()
    {
        
        Init(_targetTransform);
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetTransform != null)
        {
            _lineRenderer.SetPosition(0,_lineRenderer.transform.position);
            _lineRenderer.SetPosition(1,_targetTransform.transform.position);
        }
        
    }

    public void Init(Transform target)
    {
        _targetTransform = target;
        _handler.TransformOverride = target;
        _calloutTextmesh.text = !doAlternateText ? target.name : altText;
    }

}

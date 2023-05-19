using Ara.Domain.JobManagement;
using Ara.Domain.RepairManualManagement;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.UX;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepsListController : MonoBehaviour
{

    [SerializeField] private RectTransform listContainerRectTransform;
    [SerializeField][AssetsOnly] private GameObject repairManualDisplayPrefab;
    [SerializeField][AssetsOnly] private GameObject stepDisplayPrefab;
    public GameObject StepDisplayPrefab { get { return stepDisplayPrefab; } }

    private List<RepairManual> repairManuals = new List<RepairManual>();
    private Dictionary<int, RepairManualDisplay> _repairManualDisplays = new Dictionary<int, RepairManualDisplay>();
    public Dictionary<int, RepairManualDisplay> RepairManualDisplays { get { return _repairManualDisplays; } }

    private Dictionary<int, StepDisplay> _stepDisplays = new Dictionary<int, StepDisplay>();
    public Dictionary<int, StepDisplay> StepDisplays { get { return _stepDisplays; } }

    private List<Toggle> stepGroupButtonParentList = new List<Toggle>();
    [SerializeField] private CustomToggleCollection stepToggleCollection;

    [SerializeField] private ScrollRect _scrollRect;

    public ScrollRect ScrollRect { get { return _scrollRect; } }

    [SerializeField] private StepScroller _stepScroller;

    public StepScroller StepScroller { get { return _stepScroller; } }


    public WorkingHUDManager HUD;

    // Start is called before the first frame update
    void Awake()
    {
        HUD = GetComponentInParent<WorkingHUDManager>();

        HUD.StepsListController = this;

        if (_scrollRect != null) _scrollRect.onValueChanged.AddListener((Vector2 value) => { UpdateColliders(); });

        if (_stepScroller != null) _stepScroller.OnMove += UpdateColliders;

    }

    public void PopulateTaskGroups(TaskInfo task)
    {
        // Clear previous groups
        for (int i = listContainerRectTransform.childCount - 1; i >= 0; i--)
        {
            GameObject childObject = listContainerRectTransform.GetChild(i).gameObject;
            Destroy(childObject);
        }


        _stepDisplays = new Dictionary<int, StepDisplay>();
        _repairManualDisplays = new Dictionary<int, RepairManualDisplay>();

        repairManuals.Clear();

        stepGroupButtonParentList.Clear();



        HUD.CurrentTask = task;
        repairManuals.AddRange(task.RepairManuals);

        listContainerRectTransform.sizeDelta = new Vector2(listContainerRectTransform.rect.width, (repairManuals.Count * 104));

        foreach (var repairManual in repairManuals)
        {

            RepairManualDisplay repairManualDisplay = Instantiate(repairManualDisplayPrefab, listContainerRectTransform).GetComponent<RepairManualDisplay>();
            repairManualDisplay.Initialize(repairManual,this);
            repairManualDisplay.OnClicked += () => HUD.SetPdfUrl(repairManual.Document.Url);

            stepGroupButtonParentList.Add(repairManualDisplay.GetComponent<Toggle>());
            _repairManualDisplays.Add(repairManual.Id, repairManualDisplay);

        }

        StartCoroutine(DisableTheGroupsOverride());
        StartCoroutine(SetupButtonCollider());
        ScrollRect.normalizedPosition = new Vector2(0,1);
    }

    public void SetupStepToggleButton(PressableButton stepButton , ManualStep step)
    {
        stepToggleCollection.AddToggle(stepButton);
    }

  
    private IEnumerator DisableTheGroupsOverride()
    {
        yield return new WaitForEndOfFrame();
        foreach (RepairManualDisplay Toggle in _repairManualDisplays.Values)
        {
            Toggle.SetOpen(false);
        }
        
    }
    private IEnumerator SetupButtonCollider()
    {
        yield return new WaitForSeconds(.1f);
        UpdateColliders();
    }

    public void UpdateColliders()
    {
        foreach (var entry in _repairManualDisplays) entry.Value.UpdateColliders();
        foreach (var entry in _stepDisplays) entry.Value.UpdateColliders();
    }

    private IEnumerator UpdateCollidersLateRoutine()
    {
        yield return new WaitForEndOfFrame();
        UpdateColliders();
    }

    public void UpdateCollidersLate()
    {
       StartCoroutine(UpdateCollidersLateRoutine());
    }
}

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance {get; private set;}
    [SerializeField, HideInInspector] private Camera _cam;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private LayerMask _clickable;


    public List<GameObject> unitsAll = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();


    private bool _selectionIsActive;
    private RaycastHit _hit;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        _cam = Camera.main;
        GameInput.Instance.OnMouseLeftStarted += OnLeftClickStarted;
    }
    private void Update()
    {
        HandleUnitSelection(_selectionIsActive);
    }
    private void OnLeftClickStarted(object sender, EventArgs e)
    {
        _selectionIsActive = true;
    }
    private void HandleUnitSelection(bool selectionIsActive)
    {
        if(!selectionIsActive)
            return;

        Ray ray = _cam.ScreenPointToRay(GameInput.Instance.GetMousePosition());

        if(!Physics.Raycast(ray, out _hit, Mathf.Infinity, _clickable))
        {
            if(!GameInput.Instance.GetShiftIsPressed())
                DeselectAll();
            _selectionIsActive = false;
            return;
        }
        
        if(!GameInput.Instance.GetShiftIsPressed())
        {
            SelectByClicking(_hit.collider.gameObject);
        }
        else
        {
            MultipleSelection(_hit.collider.gameObject);
        }

        _selectionIsActive = false;
    }

    private void MultipleSelection(GameObject unit)
    {
        if(unitsSelected.Contains(unit))
        {
            SetSelectionIndicator(unit, false);
            unitsSelected.Remove(unit);
        }
        else
        {
            SetSelectionIndicator(unit, true);
            unitsSelected.Add(unit);
        }
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();
        unitsSelected.Add(unit);
        SetSelectionIndicator(unit, true);
    }
    private void DeselectAll()
    {
        foreach(var unit in unitsSelected)
        {
            SetSelectionIndicator(unit, false);
        }
        unitsSelected.Clear();
    }
    private void SetSelectionIndicator(GameObject unit, bool isActive)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isActive);
    }
    private void OnValidate()
    {
        if(_cam == null)
            _cam = GetComponent<Camera>();
    }
}

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


    private GameObject _unitHovered;
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
        Ray ray = _cam.ScreenPointToRay(GameInput.Instance.GetMousePosition());
        HandleUnitHovering(ray);
    }
    
    private void HandleUnitHovering(Ray ray)
    {
        if(Physics.Raycast(ray, out _hit, Mathf.Infinity, _clickable))
        {
            if(_hit.collider.gameObject == _unitHovered)
                return;
            if(_unitHovered != null)
            {
                SetHoveringIndicator(_unitHovered, false);
                _unitHovered = null;
            }
            _unitHovered = _hit.collider.gameObject;
            SetHoveringIndicator(_unitHovered, true);
        }
        else
        {
            if(_unitHovered != null)
            {
                SetHoveringIndicator(_unitHovered, false);
                _unitHovered = null;
            }
        }
    }
    private void OnLeftClickStarted(object sender, EventArgs e)
    {
        Ray ray = _cam.ScreenPointToRay(GameInput.Instance.GetMousePosition());
        if(!Physics.Raycast(ray, out _hit, Mathf.Infinity, _clickable))
        {
            if(!GameInput.Instance.LeftShift_IsPressed())
                DeselectAll();
            return;
        }
        if(GameInput.Instance.LeftShift_IsPressed())
        {
            MultipleSelection(_hit.collider.gameObject);
        }
        else
        {
            SelectByClicking(_hit.collider.gameObject);
        }
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();
        unitsSelected.Add(unit);
        SetSelectionIndicator(unit, true);
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
    public void DragSelect(GameObject unit)
    {
        if(unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            SetSelectionIndicator(unit, true);
        }
    }

    private void SetSelectionIndicator(GameObject unit, bool isSelected)
    {
        unit.GetComponent<UnitVisual>().SetSelected(isSelected);
    }
    private void SetHoveringIndicator(GameObject unit, bool isHovered)
    {
        unit.GetComponent<UnitVisual>().SetHovered(isHovered);
    }
    public void DeselectAll()
    {
        foreach(var unit in unitsSelected)
        {
            SetSelectionIndicator(unit, false);
        }
        unitsSelected.Clear();
    }
    private void OnValidate()
    {
        if(_cam == null)
            _cam = GetComponent<Camera>();
    }
}

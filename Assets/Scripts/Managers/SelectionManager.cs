using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance {get; private set;}
    [SerializeField] private LayerMask _ground;
    [SerializeField] private LayerMask _clickable;


    private Camera _cam;
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
                SwitchHover(_unitHovered, false);
                _unitHovered = null;
            }
            _unitHovered = _hit.collider.gameObject;
            SwitchHover(_unitHovered, true);
        }
        else
        {
            if(_unitHovered != null)
            {
                SwitchHover(_unitHovered, false);
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
        AddUnitToSelected(unit);
    }
    private void MultipleSelection(GameObject unit)
    {
        if(unitsSelected.Contains(unit))
        {
            RemoveUnitFromSelected(unit);
        }
        else
        {
            AddUnitToSelected(unit);
        }
    }
    public void DragSelect(GameObject unit)
    {
        if(unitsSelected.Contains(unit) == false)
        {
            AddUnitToSelected(unit);
        }
    }
    private void AddUnitToSelected(GameObject unit)
    {
        unitsSelected.Add(unit);
        SwitchSelectionIndicator(unit, true);
    }
    private void RemoveUnitFromSelected(GameObject unit)
    {
        SwitchSelectionIndicator(unit, false);
        unitsSelected.Remove(unit);
    }
    private void SwitchSelectionIndicator(GameObject unit, bool isSelected)
    {
        unit.GetComponentInChildren<UnitVisual>().ShowSelected(isSelected);
    }
    private void SwitchHover(GameObject unit, bool isHovering)
    {
        unit.GetComponentInChildren<UnitVisual>().ShowHover(isHovering);
    }
    public void DeselectAll()
    {
        foreach(var unit in unitsSelected)
        {
            SwitchSelectionIndicator(unit, false);
        }
        unitsSelected.Clear();
    }
    private void OnValidate()
    {
        if(_cam == null)
            _cam = GetComponent<Camera>();
    }
}

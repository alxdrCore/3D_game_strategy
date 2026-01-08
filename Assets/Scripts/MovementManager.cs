using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static MovementManager Instance {get; private set;}
    [SerializeField, HideInInspector] private Camera _cam;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject _groundDestinationMarker; 
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        _cam = Camera.main;
        GameInput.Instance.OnMouseRightStarted += UnitMovementManager_OnMouseRightStarted;
    } 

    private void Update()
    {
    }
    
    
    private void UnitMovementManager_OnMouseRightStarted(object sender, EventArgs e)
    {
        RaycastHit hit;
        Ray ray = _cam.ScreenPointToRay(GameInput.Instance.GetMousePosition());
        if(!Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
        {
            return;
        }
        if(SelectionManager.Instance.unitsSelected.Count > 0)
        {
            SetDestinationMarker(hit);
            SendToDestination(SelectionManager.Instance.unitsSelected, hit);
        }
    }
    
    private void SetDestinationMarker(RaycastHit destinationHit)
    {
        _groundDestinationMarker.transform.position = destinationHit.point;

        _groundDestinationMarker.SetActive(false);
        _groundDestinationMarker.SetActive(true);
    }
    private void SendToDestination(List<GameObject> selectedUnits, RaycastHit destinationHit)
    {
        foreach(var unit in selectedUnits)
        {
            unit.GetComponentInChildren<UnitLogic>().SetUnitDestination(destinationHit.point);
        }
    }
}

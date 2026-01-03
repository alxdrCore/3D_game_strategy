using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementManager : MonoBehaviour
{
    public static UnitMovementManager Instance {get; private set;}
    [SerializeField, HideInInspector] private Camera _cam;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject _groundDestinationMarker; 
    private bool _checkIsActive;
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
        CheckIfGround(_checkIsActive);
    }
    private void UnitMovementManager_OnMouseRightStarted(object sender, EventArgs e)
    {
        _checkIsActive = true;
    }
    private void CheckIfGround(bool checkIsActivated)
    {
        if(!checkIsActivated)
            return;

        RaycastHit hit;
        Ray ray = _cam.ScreenPointToRay(GameInput.Instance.GetMousePosition());

        if(!Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
        {
            _checkIsActive = false;
            return;
        }

        if(UnitSelectionManager.Instance.unitsSelected.Count > 0)
        {
            SetDestinationMarker(hit);
            SendUnitsToDestination(UnitSelectionManager.Instance.unitsSelected, hit);
        }

        _checkIsActive = false;
    }
    private void SetDestinationMarker(RaycastHit destinationHit)
    {
        _groundDestinationMarker.transform.position = destinationHit.point;

        _groundDestinationMarker.SetActive(false);
        _groundDestinationMarker.SetActive(true);
    }
    private void SendUnitsToDestination(List<GameObject> selectedUnits, RaycastHit destinationHit)
    {
        foreach(var unit in selectedUnits)
        {
            unit.GetComponent<UnitMovement>().UnitSetDestination(destinationHit);
        }
    }
}

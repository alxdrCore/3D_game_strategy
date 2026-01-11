using System;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static MovementManager Instance {get; private set;}
    [SerializeField, HideInInspector] private Camera _cam;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _objectAttackableLayer;

    [SerializeField] private GameObject _groundDestinationMarker; 

    public bool inputMovePriority;
    public bool inputAttackPriority;
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
    } 
    private void OnEnable()
    {
        GameInput.Instance.OnMouseRightStarted += UnitMovementManager_OnMouseRightStarted;
    }

    private void UnitMovementManager_OnMouseRightStarted(object sender, EventArgs e)
    {
        if(SelectionManager.Instance.unitsSelected.Count <= 0)
            return;

        RaycastHit hit;
        Ray ray = _cam.ScreenPointToRay(GameInput.Instance.GetMousePosition());
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, _objectAttackableLayer))
        {
            //Send to an enemy and set attack priority
            Debug.Log("RaycastHit on enemy object");
            SendToAttack(SelectionManager.Instance.unitsSelected, hit);
        }
        else if(Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
        {
            //Sets destination and use input priority to move
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
            unit.GetComponentInChildren<UnitLogic>().OrderToMoveTo(destinationHit);
        }
    }
    private void SendToAttack(List<GameObject> selectedUnits, RaycastHit destinationHit)
    {
        foreach(var unit in selectedUnits)
        {
            unit.GetComponentInChildren<UnitLogic>().OrderToAttack(destinationHit.transform); 
        }
    }
    private void OnDisable()
    {
        GameInput.Instance.OnMouseRightStarted -= UnitMovementManager_OnMouseRightStarted;
    }
}

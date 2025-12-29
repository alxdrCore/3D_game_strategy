using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [SerializeField, HideInInspector] private NavMeshAgent _agent;
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask _ground;
    private bool _moveUnits;
    private void Start()
    {
        _cam = Camera.main;
        GameInput.Instance.OnMouseRightStarted += Unit_OnMouseRightStarted;
    }
    private void Update()
    {
        RaycastHit hit;
        Ray ray = _cam.ScreenPointToRay(GameInput.Instance.GetMousePosition());

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, _ground) && _moveUnits)
        {
            _agent.SetDestination(hit.point);
            _moveUnits = false;
        }

    }
    private void Unit_OnMouseRightStarted(object obj, EventArgs e)
    {
        _moveUnits = true;

    }
    private void OnDestroy()
    {
        GameInput.Instance.OnMouseRightStarted -= Unit_OnMouseRightStarted;

    }
    private void OnValidate()
    {
        if(_agent == null)
            _agent = GetComponent<NavMeshAgent>();
    }
}

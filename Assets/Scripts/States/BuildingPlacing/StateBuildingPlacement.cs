using System.Collections;
using UnityEngine;

public class StateBuildingPlacement : MonoBehaviour, IInputState
{
    [SerializeField] private LayerMask _raycastLayer;
    private float _rotationSpeed = 149f;
    private void Start()
    {
        HandleBuildingPosition();

    }
    private void Update()
    {
        HandleBuildingPosition();        
    }
    private void HandleBuildingPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(GameInput.Instance.GetMousePosition());
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000f, _raycastLayer ))
            transform.position = hit.point;  
        if(GameInput.Instance.GetBuildingRotationDirection() != 0)
            transform.Rotate(Vector3.up * GameInput.Instance.GetBuildingRotationDirection() * _rotationSpeed * Time.deltaTime);          
    }
    
    public void OnLeftClickStarted()
    {
        

    }
    public void OnLeftClickCanceled()
    {
        
    }
    public void OnRightClickStarted()
    {
        
        
    }
    public void OnRightClickCanceled()
    {
        if(this != null)
            StartCoroutine(DisableNextFrame());
        StateHandler.Instance.SetState(new StateUnitControl());
    }
    private IEnumerator DisableNextFrame()
    {
        yield return null;
        enabled = false;
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class PlaceObject : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    private float _rotationSpeed = 149f;
    private void Start()
    {
        GameInput.Instance.OnBuildingPlacement += Building_OnBuildingPlaced;
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
        if(Physics.Raycast(ray, out hit, 1000f, _layer ))
            transform.position = hit.point;  
        if(GameInput.Instance.GetBuildingRotationDirection() != 0)
            transform.Rotate(Vector3.up * GameInput.Instance.GetBuildingRotationDirection() * _rotationSpeed * Time.deltaTime);          
    }
    private void Building_OnBuildingPlaced(object sender, System.EventArgs e)
    {
            Destroy(gameObject.GetComponent<PlaceObject>());
    }
    private void OnDestroy()
    {
        GameInput.Instance.OnBuildingPlacement -= Building_OnBuildingPlaced;
    }
}

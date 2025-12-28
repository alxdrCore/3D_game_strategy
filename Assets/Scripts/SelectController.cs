using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectController : MonoBehaviour
{
    [SerializeField, HideInInspector]private Camera _camera;
    [SerializeField] private LayerMask raycastLayer;
    [SerializeField] private LayerMask _objectsLayer;
    [SerializeField] private GameObject _cube;
    [SerializeField] List<GameObject> SelectedObjects;
    private GameObject _selectionCube;
    private RaycastHit _hit;

    private void Awake()
    {
    }
    private void Start()
    {
        GameInput.Instance.OnSelectionStarted += Select_OnSelectionStarted;
        GameInput.Instance.OnSelectionCanceled += Select_OnSelectionCanceled;
    }
    private void Update()
    {
        HandleSelectionCubeSize(_selectionCube);
            
    }
    
    private void Select_OnSelectionStarted(object sender, EventArgs e)
    {
        SelectedObjects.Clear();
        Ray ray = _camera.ScreenPointToRay(GameInput.Instance.GetMousePosition());

        if(Physics.Raycast(ray, out _hit, 100f, raycastLayer))
        {
            _selectionCube = Instantiate(_cube, new Vector3(_hit.point.x, 1, _hit.point.z), Quaternion.identity);       
        }
    }
    private void HandleSelectionCubeSize(GameObject _selectionCube)
    {
        if(_selectionCube)
            {
                Ray ray = _camera.ScreenPointToRay(GameInput.Instance.GetMousePosition());
                if(Physics.Raycast(ray, out RaycastHit hitDrag, 1000f, raycastLayer))
                {
                    float xScale = ((_hit.point.x - hitDrag.point.x) * -1);
                    float zScale = _hit.point.z - hitDrag.point.z;

                    if(xScale < 0.0f && zScale < 0.0f)
                        _selectionCube.transform.localRotation = Quaternion.Euler(new Vector3(0,180,0));
                    else if (xScale < 0.0f)
                        _selectionCube.transform.localRotation = Quaternion.Euler(new Vector3(0,0,180));
                    else if(zScale < 0.0f)
                        _selectionCube.transform.localRotation = Quaternion.Euler(new Vector3(180, 0, 0));
                    else 
                        _selectionCube.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
                    
                    _selectionCube.transform.localScale = new Vector3(Mathf.Abs(xScale), 1, Mathf.Abs(zScale));

                }
            }

    }
    private void Select_OnSelectionCanceled(object sender, EventArgs e)
    {
        RaycastHit[] hits = Physics.BoxCastAll(
            _selectionCube.transform.GetChild(0).position,
            _selectionCube.transform.localScale/2,
            Vector3.up,
            Quaternion.identity,
            0,
            _objectsLayer);

        foreach (var element in hits)
        {
            SelectedObjects.Add(element.transform.gameObject);
        }
        if(_selectionCube)
            Destroy(_selectionCube);
    }
    private void OnValidate()
    {
        if(_camera == null)
            _camera = GetComponent<Camera>();
    }
}

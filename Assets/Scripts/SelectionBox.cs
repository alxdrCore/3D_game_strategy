using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
 
public class SelectionBox : MonoBehaviour
{
    private Camera _cam;

    private bool _mouseLeftIsStarted;
    private bool _mouseLeftIsCanceled;
    [SerializeField] private RectTransform _boxVisual;
 
    private Rect _selectionBox;
    Vector2 startPosition;
    Vector2 endPosition;
 
    private void Start()
    {
        GameInput.Instance.OnMouseLeftStarted += SelectionBox_OnMouseLeftStarted;
        GameInput.Instance.OnMouseLeftCanceled += SelectionBox_OnMouseLeftCanceled;
        _cam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }
 
    private void Update()
    {
        HandleSelectionStarted(_mouseLeftIsStarted);
 
        HandleSelection(GameInput.Instance.MouseLeft_IsPressed());
 
        HandleSelectionIsCanceled(_mouseLeftIsCanceled);
    }
 
    private void SelectionBox_OnMouseLeftStarted(object sender, EventArgs e)
    {
        _mouseLeftIsStarted = true;
    }
    private void SelectionBox_OnMouseLeftCanceled(object sender, EventArgs e)
    {
        _mouseLeftIsCanceled = true;
    }
    private void HandleSelectionStarted(bool mouseLeftIsStarted)
    {
        if(!mouseLeftIsStarted)
            return;
        
        startPosition = GameInput.Instance.GetMousePosition();
        _selectionBox = new Rect();
        _mouseLeftIsStarted = false;
    }
    private void HandleSelection(bool mouseLeftIsPressed)
    {
        if(!mouseLeftIsPressed)
            return;

        if(_boxVisual.rect.width > 15 || _boxVisual.rect.height > 15)
        {
            if(!GameInput.Instance.LeftShift_IsPressed())
                UnitSelectionManager.Instance.DeselectAll();
            SelectUnits();
        }
        endPosition = Input.mousePosition;
        DrawVisual();
        DrawSelection();

    }
    private void HandleSelectionIsCanceled(bool mouseLeftIsCanceled)
    {
        if(!mouseLeftIsCanceled)
            return;
 
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
        DrawSelection();
        _mouseLeftIsCanceled = false;
    }
    void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;
 
        Vector2 boxCenter = (boxStart + boxEnd) / 2;
 
        _boxVisual.position = boxCenter;
 
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
 
        _boxVisual.sizeDelta = boxSize;
    }
 
    void DrawSelection()
    {
        if (Input.mousePosition.x < startPosition.x)
        {
            _selectionBox.xMin = Input.mousePosition.x;
            _selectionBox.xMax = startPosition.x;
        }
        else
        {
            _selectionBox.xMin = startPosition.x;
            _selectionBox.xMax = Input.mousePosition.x;
        }
 
 
        if (Input.mousePosition.y < startPosition.y)
        {
            _selectionBox.yMin = Input.mousePosition.y;
            _selectionBox.yMax = startPosition.y;
        }
        else
        {
            _selectionBox.yMin = startPosition.y;
            _selectionBox.yMax = Input.mousePosition.y;
        }
    }
 
    void SelectUnits()
    {
        foreach (var unit in UnitSelectionManager.Instance.unitsAll)
        {
            if (_selectionBox.Contains(_cam.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelectionManager.Instance.DragSelect(unit);
            }
        }
    }
}

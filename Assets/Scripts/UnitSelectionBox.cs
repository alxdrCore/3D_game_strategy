using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
 
public class UnitSelectionBox : MonoBehaviour
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
        // When Clicked
        HandleSelectionStarted(_mouseLeftIsStarted);
 
        // When Dragging
        HandleSelection(GameInput.Instance.MouseLeft_IsPressed());
 
        // When Releasing
        HandleSelectionIsCanceled(_mouseLeftIsCanceled);

        _mouseLeftIsStarted = false;
        _mouseLeftIsCanceled = false;
    }
 
    private void SelectionBox_OnMouseLeftStarted(object sender, EventArgs e)
    {
        _mouseLeftIsStarted = true;
    }
    private void SelectionBox_OnMouseLeftCanceled(object sender, EventArgs e)
    {
        _mouseLeftIsCanceled = true;
    }
    private void HandleSelectionStarted(bool selectionIsStarted)
    {
        if(!selectionIsStarted)
            return;
        
        startPosition = GameInput.Instance.GetMousePosition();
        _selectionBox = new Rect();
    }
    private void HandleSelection(bool mouseLeftButtonIsPressed)
    {
        if(!mouseLeftButtonIsPressed)
            return;
        
        endPosition = Input.mousePosition;
        DrawVisual();
        DrawSelection();

    }
    private void HandleSelectionIsCanceled(bool mouseLeftButtonIsCanceled)
    {
        if(!mouseLeftButtonIsCanceled)
            return;

        if(_boxVisual.rect.width > 0.01 || _boxVisual.rect.height > 0.01)
        {
            SelectUnits();
        }
 
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
    }
    void DrawVisual()
    {
        // Calculate the starting and ending positions of the selection box.
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;
 
        // Calculate the center of the selection box.
        Vector2 boxCenter = (boxStart + boxEnd) / 2;
 
        // Set the position of the visual selection box based on its center.
        _boxVisual.position = boxCenter;
 
        // Calculate the size of the selection box in both width and height.
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
 
        // Set the size of the visual selection box based on its calculated size.
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
        UnitSelectionManager.Instance.DeselectAll();

        foreach (var unit in UnitSelectionManager.Instance.unitsAll)
        {
            if (_selectionBox.Contains(_cam.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelectionManager.Instance.DragSelect(unit);
            }
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RTSCameraController : MonoBehaviour
{
    public static RTSCameraController Instance { get; private set; }

    // If we want to select an item to follow, inside the item script add:
    // public void OnMouseDown(){
    //   CameraController.instance.followTransform = transform;
    // }

    [Header("General")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _followTransform;
    Vector3 newPosition;
    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;

    [Header("Optional Functionality")]
    [SerializeField] private bool _moveWithKeyboard;
    [SerializeField] private bool _moveWithEdgeScrolling;
    [SerializeField] private bool _moveWithMouseDrag;

    [Header("Keyboard Movement")]
    [SerializeField] private float _fastSpeed = 0.05f;
    [SerializeField] private float _normalSpeed = 0.01f;
    [SerializeField] private float _movementSensitivity = 1f; // Hardcoded Sensitivity
    private float _currentMovementSpeed;

    [Header("Edge Scrolling Movement")]
    [SerializeField] private float _edgeSize = 40f;
    private bool _isCursorSet;
    [SerializeField] private Texture2D _cursorArrowUp;
    [SerializeField] private Texture2D _cursorArrowDown;
    [SerializeField] private Texture2D _cursorArrowLeft;
    [SerializeField] private Texture2D _cursorArrowRight;
    private bool _escapeButtonStarted;

    CursorArrow currentCursor = CursorArrow.DEFAULT;
    enum CursorArrow
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DEFAULT
    }

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        newPosition = transform.position;

        _currentMovementSpeed = _normalSpeed;

        GameInput.Instance.OnMouseMiddleStarted += OnMouseMiddleStarted_HandleMouseDragInput;
    }

    private void Update()
    {
        // Allow Camera to follow Target
        if (_followTransform != null)
        {
            transform.position = _followTransform.position;
        }
        // Let us control Camera
        else
        {
            HandleCameraMovement();
        }
    }

    private void HandleCameraMovement()
    {
        if (_moveWithMouseDrag)
            OnMouseMiddleIsPressed_HandleMouseDragInput();

        // Keyboard Control
        if (_moveWithKeyboard)
            KeyboardMovement();

        // Edge Scrolling
        if (_moveWithEdgeScrolling)
            EdgeScrollingMovement();

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _movementSensitivity);
        Cursor.lockState = CursorLockMode.Confined; // If we have an extra monitor we don't want to exit screen bounds
    }
    private void CameraController_OnEscapeStarted()
    {
        _followTransform = null;
    }

    private void EdgeScrollingMovement()
    {
        Vector2 mousePosition = GameInput.Instance.GetMousePosition();
        Vector3 edgeMoveDirection = Vector3.zero;

        if (mousePosition.x > Screen.width - _edgeSize)
        {
            edgeMoveDirection += transform.right;
            ChangeCursor(CursorArrow.RIGHT);
            _isCursorSet = true;
        }
        else if (mousePosition.x < _edgeSize)
        {
            edgeMoveDirection += -transform.right;
            ChangeCursor(CursorArrow.LEFT);
            _isCursorSet = true;
        }

        if (mousePosition.y > Screen.height - _edgeSize)
        {
            edgeMoveDirection += transform.forward;
            ChangeCursor(CursorArrow.UP);
            _isCursorSet = true;
        }
        else if (mousePosition.y < _edgeSize)
        {
            edgeMoveDirection += -transform.forward;
            ChangeCursor(CursorArrow.DOWN);
            _isCursorSet = true;
        }

        if (edgeMoveDirection != Vector3.zero)
        {
            newPosition = transform.position + edgeMoveDirection.normalized * _currentMovementSpeed;
        }
        else if (_isCursorSet)
        {
            ChangeCursor(CursorArrow.DEFAULT);
            _isCursorSet = false;
        }
    }
    private void ChangeCursor(CursorArrow newCursor)
    {
        // Only change cursor if its not the same cursor
        if (currentCursor != newCursor)
        {
            switch (newCursor)
            {
                case CursorArrow.UP:
                    Cursor.SetCursor(_cursorArrowUp, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.DOWN:
                    Cursor.SetCursor(_cursorArrowDown, new Vector2(_cursorArrowDown.width, _cursorArrowDown.height), CursorMode.Auto); // So the Cursor will stay inside view
                    break;
                case CursorArrow.LEFT:
                    Cursor.SetCursor(_cursorArrowLeft, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.RIGHT:
                    Cursor.SetCursor(_cursorArrowRight, new Vector2(_cursorArrowRight.width, _cursorArrowRight.height), CursorMode.Auto); // So the Cursor will stay inside view
                    break;
                case CursorArrow.DEFAULT:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;
            }

            currentCursor = newCursor;
        }
    }
    private void KeyboardMovement()
    {
        Vector2 moveInput = GameInput.Instance.GetMoveDirection();
        if (moveInput == Vector2.zero)
            return;
        Vector3 moveDirection3D = transform.right * moveInput.x + transform.forward * moveInput.y;
        moveDirection3D.y = 0f;
        if (GameInput.Instance.LeftControl_IsPressed())
        {
            _currentMovementSpeed = _fastSpeed;
        }
        else
        {
            _currentMovementSpeed = _normalSpeed;
        }

        newPosition = transform.position + moveDirection3D.normalized * _currentMovementSpeed;
    }

    private void OnMouseMiddleStarted_HandleMouseDragInput(object sender, EventArgs e)
    {
        if (!_moveWithMouseDrag)
            return;
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(GameInput.Instance.GetMousePosition());

        float entry;

        if (plane.Raycast(ray, out entry))
        {
            dragStartPosition = ray.GetPoint(entry);
        }
    }
    private void OnMouseMiddleIsPressed_HandleMouseDragInput()
    {
        if (!GameInput.Instance.MouseMiddle_IsPressed())
            return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(GameInput.Instance.GetMousePosition());

        float entry;

        if (plane.Raycast(ray, out entry))
        {
            dragCurrentPosition = ray.GetPoint(entry);
            newPosition = transform.position + dragStartPosition - dragCurrentPosition;
        }
    }
}
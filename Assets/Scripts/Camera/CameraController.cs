// using UnityEngine;
// public class CameraController : MonoBehaviour
// {
//     public static CameraController Instance {get; private set;}
//     [SerializeField] private float _rotateSpeed = 60f;
//     [SerializeField] private float _moveSpeed = 60f;
//     [SerializeField] private float _flySpeed = 50f;
//     [SerializeField] private float _minFlyHeight = 20f;
//     [SerializeField] private float _maxFlyHeight = 50f;
//     private float _moveBaseMultiplier = 1f;
//     private float _moveAcceleration = 2f;

//     private void Start()
//     {

//     }
//     private void Awake()
//     {
//         Instance = this;
//     }
//     private void Update()
//     {
//         HandleMovementAndRotation();

//     } 
//     private void FixedUpdate()
//     {
//     } 
//     private void HandleMovementAndRotation()
//     {
//         transform.Rotate(Vector3.up, GameInput.Instance.GetCameraRotationDirection() * _rotateSpeed * GetAccelerationIfActive() * Time.deltaTime );
//         transform.Translate(GameInput.Instance.GetMoveDirection() * _moveSpeed * GetAccelerationIfActive()* Time.deltaTime );
//         transform.position += transform.up * GameInput.Instance.GetFlyDirection() * _flySpeed * GetAccelerationIfActive() * Time.deltaTime;

//         Vector3 posBordering = transform.position;
//         posBordering.y = Mathf.Clamp(posBordering.y, _minFlyHeight, _maxFlyHeight);
//         transform.position = posBordering;
//     }
//     private float GetAccelerationIfActive()
//     {
//         if(GameInput.Instance.GetIsAccelerationActive())
//             return _moveAcceleration;
//         return _moveBaseMultiplier;
//     }
// }


using UnityEngine;
using UnityEngine.EventSystems;

public class RTSCameraController : MonoBehaviour
{
    public static RTSCameraController Instance {get; private set;}

    // If we want to select an item to follow, inside the item script add:
    // public void OnMouseDown(){
    //   CameraController.instance.followTransform = transform;
    // }

    [Header("General")]
    [SerializeField] private Transform _cameraTransform;
    private Transform _followTransform;
    Vector3 newPosition;
    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;

    [Header("Optional Functionality")]
    [SerializeField] private bool _moveWithKeyboad;
    [SerializeField] private bool _moveWithEdgeScrolling;
    [SerializeField] private bool _moveWithMouseDrag;

    [Header("Keyboard Movement")]
    [SerializeField] private float _fastSpeed = 0.05f;
    [SerializeField] private float _normalSpeed = 0.01f;
    [SerializeField] private float _movementSensitivity = 1f; // Hardcoded Sensitivity
    private float _currentMovementSpeed;

    [Header("Edge Scrolling Movement")]
    [SerializeField] private float _edgeSize = 50f;
    bool isCursorSet;
    private Texture2D _cursorArrowUp;
    private Texture2D _cursorArrowDown;
    private Texture2D _cursorArrowLeft;
    private Texture2D _cursorArrowRight;
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
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        newPosition = transform.position;

        _currentMovementSpeed = _normalSpeed;


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

        if (_escapeButtonStarted)
        {
            _followTransform = null;
            _escapeButtonStarted = false;
        }
    }

    void HandleCameraMovement()
    {
        // Mouse Drag
        if (_moveWithMouseDrag)
        {
            HandleMouseDragInput();
        }

        // Keyboard Control
        if (_moveWithKeyboad)
        {
            if (Input.GetKey(KeyCode.LeftCommand))
            {
                _currentMovementSpeed = _fastSpeed;
            }
            else
            {
                _currentMovementSpeed = _normalSpeed;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (transform.forward * _currentMovementSpeed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += (transform.forward * -_currentMovementSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (transform.right * _currentMovementSpeed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += (transform.right * -_currentMovementSpeed);
            }
        }

        // Edge Scrolling
        if (_moveWithEdgeScrolling)
        {

            // Move Right
            if (Input.mousePosition.x > Screen.width - _edgeSize)
            {
                newPosition += (transform.right * _currentMovementSpeed);
                ChangeCursor(CursorArrow.RIGHT);
                isCursorSet = true;
            }

            // Move Left
            else if (Input.mousePosition.x < _edgeSize)
            {
                newPosition += (transform.right * -_currentMovementSpeed);
                ChangeCursor(CursorArrow.LEFT);
                isCursorSet = true;
            }

            // Move Up
            else if (Input.mousePosition.y > Screen.height - _edgeSize)
            {
                newPosition += (transform.forward * _currentMovementSpeed);
                ChangeCursor(CursorArrow.UP);
                isCursorSet = true;
            }

            // Move Down
            else if (Input.mousePosition.y < _edgeSize)
            {
                newPosition += (transform.forward * -_currentMovementSpeed);
                ChangeCursor(CursorArrow.DOWN);
                isCursorSet = true;
            }
            else
            {
                if (isCursorSet)
                {
                    ChangeCursor(CursorArrow.DEFAULT);
                    isCursorSet = false;
                }
            }
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _movementSensitivity);

        Cursor.lockState = CursorLockMode.Confined; // If we have an extra monitor we don't want to exit screen bounds
    }
    private void CameraController_OnEscapeStarted()
    {
        _escapeButtonStarted = true;
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



    private void HandleMouseDragInput()
    {
        if (Input.GetMouseButtonDown(2) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(2) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
    }
}
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public static CameraController Instance {get; private set;}
    [SerializeField] private float _rotateSpeed = 60f;
    [SerializeField] private float _moveSpeed = 60f;
    [SerializeField] private float _flySpeed = 50f;
    [SerializeField] private float _minFlyHeight = 20f;
    [SerializeField] private float _maxFlyHeight = 50f;
    private float _moveBaseMultiplier = 1f;
    private float _moveAcceleration = 2f;

    private void Start()
    {

    }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        HandleMovementAndRotation();

    } 
    private void FixedUpdate()
    {
    } 
    private void HandleMovementAndRotation()
    {
        transform.Rotate(Vector3.up, GameInput.Instance.GetCameraRotationDirection() * _rotateSpeed * GetAccelerationIfActive() * Time.deltaTime );
        transform.Translate(GameInput.Instance.GetMoveDirection() * _moveSpeed * GetAccelerationIfActive()* Time.deltaTime );
        transform.position += transform.up * GameInput.Instance.GetFlyDirection() * _flySpeed * GetAccelerationIfActive() * Time.deltaTime;

        Vector3 posBordering = transform.position;
        posBordering.y = Mathf.Clamp(posBordering.y, _minFlyHeight, _maxFlyHeight);
        transform.position = posBordering;
    }
    private float GetAccelerationIfActive()
    {
        if(GameInput.Instance.GetIsAccelerationActive())
            return _moveAcceleration;
        return _moveBaseMultiplier;
    }
}

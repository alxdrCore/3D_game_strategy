using UnityEngine;
using System;

[RequireComponent(typeof(SphereCollider))]
public class ChaseController : MonoBehaviour
{
    public event Action<Transform> OnEnemyEnterChaseTrigger;
    public event Action<Transform> OnEnemyExitChaseTrigger;
    private SphereCollider _chaseCollider;
    [SerializeField] private float _chaseDistance = 8f;
    private void Awake()
    {
        _chaseCollider = GetComponent<SphereCollider>();
    }
    private void Start()
    {
        _chaseCollider.radius = _chaseDistance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            OnEnemyEnterChaseTrigger?.Invoke(other.transform);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            OnEnemyExitChaseTrigger?.Invoke(other.transform);
        }

    }
    
}

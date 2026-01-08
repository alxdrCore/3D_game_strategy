using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AttackController : MonoBehaviour
{
    private SphereCollider _attackCollider;
    [SerializeField] private float _attackDistance = 3f;
    public event Action<Transform> OnEnemyEnterAttackTrigger;
    public event Action<Transform> OnEnemyExitAttackTrigger;

    private void Start()
    {
        _attackCollider = GetComponent<SphereCollider>();
        _attackCollider.radius = _attackDistance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
            OnEnemyEnterAttackTrigger?.Invoke(other.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
            OnEnemyExitAttackTrigger?.Invoke(other.transform);
    }
}

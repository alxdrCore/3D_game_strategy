using System;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public event Action<Transform> OnEnemyEnterAttackZone;
    public event Action<Transform> OnEnemyExitAttackZone;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
            OnEnemyEnterAttackZone?.Invoke(other.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
            OnEnemyExitAttackZone?.Invoke(other.transform);
    }
}

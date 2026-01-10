using UnityEngine;
using System;

public class ChaseController : MonoBehaviour
{
    public event Action<Transform> OnEnemyEnterChaseZone;
    public event Action<Transform> OnEnemyExitChaseZone;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            OnEnemyEnterChaseZone?.Invoke(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy") )
            OnEnemyExitChaseZone?.Invoke(other.transform);
    }
    
}

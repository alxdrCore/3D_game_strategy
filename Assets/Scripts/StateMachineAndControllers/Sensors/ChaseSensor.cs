using UnityEngine;
using System;
using System.Collections.Generic;

public class ChaseSensor : MonoBehaviour
{
    public List<Transform> enemiesToChase = new();
    public event Action<Transform> OnEnemyEnterChaseZone;
    public event Action<Transform> OnEnemyExitChaseZone;
    [SerializeField] private float _chaseDistance = 8f;

    private void Start()
    {
        GetComponent<SphereCollider>().radius = _chaseDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            OnEnemyEnterChaseZone?.Invoke(other.transform);
        }
        if (IsInChaseRange(other.transform))
            return;
        enemiesToChase.Add(other.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            OnEnemyExitChaseZone?.Invoke(other.transform);
            RemoveEnemyFromChaseList(other.transform);
        }
    }
    public bool IsInChaseRange(Transform target)
    {
        return enemiesToChase.Contains(target);
    }
    public void SetChaseDistance(float chaseDistance)
    {
        GetComponent<SphereCollider>().radius = chaseDistance;
    }
    public bool HasEnemiesTochase()
    {
        return enemiesToChase.Count > 0;
    }
    public Transform GetTargetToChase()
    {
        //Should be complexed logic of getting nearest enemy or smth
        return enemiesToChase.Count > 0 ? enemiesToChase[0] : null;
    }
    private void RemoveEnemyFromChaseList(Transform enemy)
    {
        // If removable enemy is not target to attack
        if (!enemiesToChase.Contains(enemy))
            return;
        enemiesToChase.Remove(enemy);
    }
}

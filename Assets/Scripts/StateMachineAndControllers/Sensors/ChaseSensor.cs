using UnityEngine;
using System;
using System.Collections.Generic;

public class ChaseSensor : MonoBehaviour
{
    public List<Transform> enemiesToChase = new();
    [SerializeField] private float _chaseDistance = 8f;

    private void Start()
    {
        GetComponent<SphereCollider>().radius = _chaseDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (IsInChaseList(other.transform))
                return;
            enemiesToChase.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            RemoveEnemyFromChaseList(other.transform);
        }
    }
    public bool IsInChaseList(Transform target)
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
        if (!IsInChaseList(enemy))
            return;
        enemiesToChase.Remove(enemy);
    }
}

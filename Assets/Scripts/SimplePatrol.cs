using UnityEngine;

public class SimplePatrol : MonoBehaviour
{
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] private float _speed = 5.0f; // Adjust the speed of movement

    private bool movingForward = true;
    private float timer = 0.0f;
    private float switchDirectionTime = 5.0f; // Time to switch direction

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= switchDirectionTime)
        {
            movingForward = !movingForward;
            timer = 0.0f;
        }

        if (movingForward)
        {
            _enemyTransform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }
        else
        {
            _enemyTransform.Translate(Vector3.back * _speed * Time.deltaTime);
        }
    }
}
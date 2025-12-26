using System.Collections;
using UnityEngine;

public class Rain : MonoBehaviour
{

    [SerializeField] private Light _directionalLight;
    [SerializeField, HideInInspector] private ParticleSystem _ps;
    private bool _isRaining;
    private bool _startLightHandling;

    private void Start()
    {
        StartCoroutine(Weather());
    }
    private void Update()
    {
        HandleLightIntensity();
    }
    private void HandleLightIntensity()
    {
        if(_isRaining && _directionalLight.intensity > 0.25f)
                _directionalLight.intensity -= 0.1f * Time.deltaTime;
        else if (!_isRaining && _directionalLight.intensity < 0.5f)
                _directionalLight.intensity += 0.1f * Time.deltaTime;
    }

    IEnumerator Weather()
    {
        while(true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(20f,51f));
            _isRaining = !_isRaining;
            if(_isRaining)
                _ps.Stop();
            else
                _ps.Play();
        }
    }
    private void OnValidate()
    {
        if (_ps == null)
            _ps = GetComponent<ParticleSystem>();
    }
}

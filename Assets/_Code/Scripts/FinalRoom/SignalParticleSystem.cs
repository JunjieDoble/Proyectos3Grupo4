using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SignalParticleSystem : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Stop();
    }

    public void Activate()
    {
        _particleSystem.Play();
    }

    public void Deactivate()
    {
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}

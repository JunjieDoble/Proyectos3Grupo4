using _Code.Scripts.Bases;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SignalDevice : MonoBehaviour
{
    private SignalVFX _signalVFX;
    private SignalParticleSystem _signalParticleSystem;

    private void Start()
    {
        _signalVFX = GetComponentInChildren<SignalVFX>();
        _signalParticleSystem = GetComponentInChildren<SignalParticleSystem>();

        if (_signalVFX == null || _signalParticleSystem == null )
        {
            Debug.LogError("SignalDevice is missing required components!");
        }
    }

    public void OnSignalParticleStart()
    {
        _signalParticleSystem.Activate();
    }

    public void OnSignalVFXStart()
    {
        _signalVFX.Activate();
    }
}

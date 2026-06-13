using _Code.Scripts.Bases;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SignalDevice : Activable
{
    private Animator _animator;
    private Animator _roofAnimator;
    private Animator _hologramAnimator;
    private SignalVFX _signalVFX;
    private SignalParticleSystem _signalParticleSystem;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _roofAnimator = GameObject.Find("final_techo").GetComponent<Animator>();
        _hologramAnimator = GameObject.Find("final_hologram").GetComponent<Animator>();
        _signalVFX = GetComponentInChildren<SignalVFX>();
        _signalParticleSystem = GetComponentInChildren<SignalParticleSystem>();

        if (_signalVFX == null || _animator == null || _roofAnimator == null || _hologramAnimator == null || _signalParticleSystem == null )
        {
            Debug.LogError("SignalDevice is missing required components!");
        }

        // Temporal para testing, remove this later
        //_animator.Play("send_signal", 0, 0f);
        //_roofAnimator.Play("open_roof", 0, 0f);
        //_hologramAnimator.Play("hologram_on", 0, 0f);
    }

    public override void ActivatorUpdate()
    {
        if (IsActive())
        {
            Activate();
        }
    }

    public void Activate()
    {
        _animator.Play("send_signal", 0, 0f);
        _roofAnimator.Play("open_roof", 0, 0f);
        _hologramAnimator.Play("hologram_on", 0, 0f);
    }

    public void OnSignalAnimationFinished()
    {
        _signalParticleSystem.Activate();
        
        StartCoroutine(StartSignalVFX());
    }

    private IEnumerator StartSignalVFX()
    {
        yield return new WaitForSeconds(4f);

        //_signalParticleSystem.Deactivate();
        _signalVFX.Activate();
    }
}

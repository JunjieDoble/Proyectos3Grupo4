using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SignalDevice : MonoBehaviour
{
    private Animator _animator;
    private Animator _roofAnimator;
    private SignalVFX _signalVFX;
    private SignalParticleSystem _signalParticleSystem;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _roofAnimator = GameObject.Find("final_techo").GetComponent<Animator>();
        _signalVFX = GetComponentInChildren<SignalVFX>();
        _signalParticleSystem = GetComponentInChildren<SignalParticleSystem>();

        if (_signalVFX == null || _animator == null || _roofAnimator == null || _signalParticleSystem == null )
        {
            Debug.LogError("SignalDevice is missing required components! Please ensure it has an Animator, SignalVFX, and SignalParticleSystem.");
        }

        // Temporal para testing, remove this later
        _animator.Play("send_signal", 0, 0f);
        _roofAnimator.Play("open_roof", 0, 0f);
    }

    public void Activate()
    {
        _animator.Play("send_signal", 0, 0f);
        _roofAnimator.Play("open_roof", 0, 0f);
    }

    public void OnSignalAnimationFinished()
    {
        Debug.Log("Signal animation finished!");

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

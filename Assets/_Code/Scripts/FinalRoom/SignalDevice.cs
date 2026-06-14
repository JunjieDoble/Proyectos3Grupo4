using _Code.Scripts.Bases;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SignalDevice : MonoBehaviour
{
    [SerializeField] private Material readyToActivateMaterial;
    [SerializeField] private Material inactivatedMaterial;
    [SerializeField] private GameObject squareHologram;

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

    public void ActivateSignalDevice()
    {
        squareHologram.GetComponent<MeshRenderer>().material = readyToActivateMaterial;

        TextMeshPro[] components = squareHologram.GetComponentsInChildren<TextMeshPro>();
        foreach (var component in components)
        {
            component.text = "Active";
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

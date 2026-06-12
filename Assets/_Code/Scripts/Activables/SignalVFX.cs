using _Code.Scripts.Bases;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class SignalVFX : MonoBehaviour
{
    private VisualEffect _vfx;

    private void Start()
    {
        _vfx = GetComponent<VisualEffect>();
    }

    public void Activate()
    {
        _vfx.SendEvent("OnStartBeam");    
    }
}

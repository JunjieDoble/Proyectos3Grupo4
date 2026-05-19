using _Code.Scripts.Bases;
using UnityEngine;

public class Charger : Activable
{
    private float _charge;
    
    void Start() => UpdateVisual();
    
    public override void ActivatorUpdate()
    {
        var activeActivators = activators.FindAll(a => a.IsActive);
        _charge = activeActivators.Count / (float)activators.Count;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (_charge < 0.1f) transform.localScale = new Vector3(1, 0.1f, 1);
        else transform.localScale = new Vector3(1, _charge, 1);
    }
}

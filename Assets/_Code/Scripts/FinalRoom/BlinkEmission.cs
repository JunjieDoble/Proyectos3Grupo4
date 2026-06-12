using UnityEngine;

public class BlinkEmission : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float intensity = 5f;

    private Material _material;
    private Color _baseColor;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;

        _baseColor = _material.GetColor("_EmissionColor");
    }

    private void Update()
    {
        float pulse = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;

        _material.SetColor(
            "_EmissionColor",
            _baseColor * (pulse * intensity)
        );
    }
}

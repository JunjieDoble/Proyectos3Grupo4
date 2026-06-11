using _Code.Scripts.Bases;
using System.Collections;
using UnityEngine;

public class FinalRayo : Activable
{
    private LineRenderer lineRenderer;
    private Coroutine secuenciaRayoCoroutine;   

    [Header("Configuración de Tiempos")]
    public float tiempoCarga = 2f;
    public float duracionDisparo = 4f;
    public float rangoMaximo = 50f;

    [Header("Configuración de Grosor")]
    public float grosorCarga = 0.5f;
    public float grosorDisparo = 0.5f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.cyan;
        lineRenderer.useWorldSpace = true;

        if (secuenciaRayoCoroutine == null)
            secuenciaRayoCoroutine = StartCoroutine(SecuenciaRayo());
    }

    IEnumerator SecuenciaRayo()
    {
        lineRenderer.enabled = true;

        // --- FASE 1: CARGA ---
        float tiempoPasado = 0f;
        while (tiempoPasado < tiempoCarga)
        {
            ActualizarPosicionesRayo();

            // Define el grosor delgado de carga
            lineRenderer.startWidth = grosorCarga;
            lineRenderer.endWidth = grosorCarga;

            tiempoPasado += Time.deltaTime;
            yield return null; // Espera al siguiente frame
        }

        // --- FASE 2: DISPARO (Rayo Grueso) ---
        tiempoPasado = 0f;
        while (tiempoPasado < duracionDisparo)
        {
            ActualizarPosicionesRayo();

            // Cambia instantáneamente al grosor de disparo configurado
            lineRenderer.startWidth = grosorDisparo;
            lineRenderer.endWidth = grosorDisparo;

            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        // --- FASE 3: APAGADO ---
        lineRenderer.enabled = false;

        secuenciaRayoCoroutine = null;
    }

    void ActualizarPosicionesRayo()
    {
        RaycastHit hit;
        lineRenderer.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out hit, rangoMaximo))
        {
            lineRenderer.SetPosition(1, hit.point);
            Debug.Log("Rayo impactó en: " + hit.collider.name);
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + (transform.forward * rangoMaximo));
        }
    }
}

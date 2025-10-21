using System;
using System.Collections;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class EscuchoYEncuentroManager : MonoBehaviour
{
    [Header("Spawn inicial del jugador")]
    [SerializeField] private Transform startSpawnPoint;

    //[Header("Texto de prueba")]
    //[SerializeField] TextMeshProUGUI texto;

    [Header("Jugador")]
    [SerializeField] XROrigin playerRig;


    public GameObject[] panelesPalabras; // Array con los paneles de cada palabra
    public AudioSource audioSource;      // AudioSource para reproducir sonidos
    public AudioClip[] clipsAudio;       // Clips de audio para cada palabra

    public int indiceActual = 0;

    private Coroutine reproducirRutina;

    private void Awake()
    {
        // Tepea el jugador a la posicion inicial donde debe aparecer
        if (playerRig != null && startSpawnPoint != null)
        {
            playerRig.MoveCameraToWorldLocation(startSpawnPoint.position);
            playerRig.MatchOriginUpCameraForward(startSpawnPoint.up, startSpawnPoint.forward); // opcional, para rotación
        }
    }

    void Start()
    {
        //level lvl = levels[nivelActual];

        // Mostrar solo el primer panel y ocultar el resto
        for (int i = 0; i < panelesPalabras.Length; i++)
        {
            
            panelesPalabras[i].SetActive(i == 0);
        }
        StartCoroutine(ReproducirAudioActual(5));
    }

    // Método que se llama desde los botones de opciones con el parámetro del índice elegido
    public void SeleccionarOpcion(bool esCorrecta)
    {
        if (esCorrecta)
        {
            // Opción correcta: desactivar panel actual y avanzar al siguiente
            panelesPalabras[indiceActual].SetActive(false);
            indiceActual++;

            if (indiceActual < panelesPalabras.Length)
            {
                panelesPalabras[indiceActual].SetActive(true);
                StartCoroutine(ReproducirAudioActual(5));
            }
            else
            {
                // Se terminaron las palabras
                Debug.Log("Actividad finalizada");
                // Aquí puedes poner lógica para mostrar resultados o finalizar actividad
            }
        }
        else
        {
            // Opción incorrecta: bloquear botón (puede ser manejado en cada botón)
            Debug.Log("Opción incorrecta, el botón debe deshabilitarse (handle en UI)");
        }
    }

    // Reproducir el audio de la palabra actual
    public IEnumerator ReproducirAudioActual(float delay)
    {

        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        if (indiceActual < clipsAudio.Length && clipsAudio[indiceActual] != null)
        {
            audioSource.clip = clipsAudio[indiceActual];
            audioSource.Play();
        }
    }
}


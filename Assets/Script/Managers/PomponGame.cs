using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PomponGame : MonoBehaviour
{
    private List<Pompon> Totales;
    private List<Pompon> pompones;
    /*Estás listan guardan los "pompones" que se van agregando a las canastas, una lista es para los que son
    grandes y otro para los pequeños*/

    [Header("Textos del juego")]
    public TextMeshProUGUI TextoBig;
    public TextMeshProUGUI TextoSmall;
    public TextMeshProUGUI TextoTitulo;
    /*Estos objetos son los "objetos de Texto" que muestran mensajes dentro del VR.*/

    [Header("Spawn inicial del jugador")]
    [SerializeField] private Transform startSpawnPoint;

    [Header("Jugador")]
    [SerializeField] XROrigin playerRig;

    private void Start()
    {
        pompones = new List<Pompon>();
        Totales = new List<Pompon>();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("SmallPompon"))
            Totales.Add(go.GetComponent<Pompon>());

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("BigPompon"))
            Totales.Add(go.GetComponent<Pompon>());

        pompones = new List<Pompon>();
        /* Busca en la escena completa los objetos que tengan los tags mencionados y los guarda en un array temporal */

        // Tepea el jugador a la posicion inicial donde debe aparecer
        if (playerRig != null && startSpawnPoint != null)
        {
            playerRig.MoveCameraToWorldLocation(startSpawnPoint.position);
            playerRig.MatchOriginUpCameraForward(startSpawnPoint.up, startSpawnPoint.forward); // opcional, para rotación
        }
    }

    public void RegistrarPompon(Pompon pompon, string tipoCanasta)
    {
        if (pompon.GetSize() == tipoCanasta)
        {
            pompon.setCorrecta(true);
            pompon.GetComponent<XRGrabInteractable>().enabled = false;
        }
        else
        {
            pompon.setCorrecta(false);
            pompon.GetComponent<XRGrabInteractable>().enabled = false;
        }

        pompones.Add(pompon);

        JuegoTerminada();
    }

    private void JuegoTerminada()
    {
        if(pompones.Count == Totales.Count)
        {
            /* 
             Codigo para poder pasar de escena
             */

            TextoTitulo.text = "¡Felicidades, lo has completado!";
        }
    }
}

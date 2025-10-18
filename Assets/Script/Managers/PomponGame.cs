using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PomponGame : MonoBehaviour
{
    private List<Pompon> Spompon;
    private List<Pompon> Bpompon;
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
        Spompon = new List<Pompon>();
        Bpompon = new List<Pompon>();
        pompones = new List<Pompon>();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("SmallPompon"))
            Spompon.Add(go.GetComponent<Pompon>());

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("BigPompon"))
            Bpompon.Add(go.GetComponent<Pompon>());

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
            if (tipoCanasta == "Big")
            {
                Bpompon.Add(pompon);
            }
            else
            {
                Spompon.Add(pompon);
            }
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
        if(pompones.Count == Bpompon.Count + Spompon.Count)
        {
            /* 
             Codigo para poder pasar de escena
             */

            TextoTitulo.text = "¡Felicidades, lo has completado!";
        }
    }

    /*
    IEnumerator MostrarTexto(bool correcto,float duracion, string size)
    {
        if (correcto)
        {
            if (size == "Big")
            {
                TextoBig.text = "¡Correcto!";
            }
            else 
            {
                TextoSmall.text = "¡Correcto!";
            }
            
            
        }
        else
        {
            TextoSmall.text = "¡Te has equivocado!";
            TextoBig.text = "¡Te has equivocado!";
        }
        yield return new WaitForSeconds(duracion); 
        // Esta sección de aquí sirve para que despues de un timpo de que se haya colocado el texto en correcto o incorrecto 
        // el texto que esta "por defecto" vuelva a mostrarse
        TextoSmall.text = "Canasta de pompones chicos";
        TextoBig.text = "Canasta de pompones grandes";
    }*/

}

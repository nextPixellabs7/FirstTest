using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using Unity.XR.Oculus;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

public class CoupleGameManager : MonoBehaviour
{
    [Header("Sockets")]
    [SerializeField] SocketAutosnapGate[] gates; // arrastra todos los sockets con el script

    [Header("UI")]
    public TextMeshProUGUI textoFin;

    [Header("Opciones")]
    public float delay = 0.4f;

    List<CardFather> encontradas = new List<CardFather>();
    CardFather[] TodasLasCartas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return null;

        foreach(var g in gates)
        {
            if (g)
            {
                g.OpenGate();
            }
        }

        TodasLasCartas = GameObject.FindObjectsByType<CardFather>(FindObjectsSortMode.None);
    }

    public void CartaDadaVuelta_FromSelect(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        var comp = args.interactableObject as UnityEngine.Component;
        if (!comp) return;
        /*
        var socket = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor;
        if (socket != null)
        {
            var cardT = comp.transform;
            var at = socket.attachTransform != null ? socket.attachTransform : socket.transform;

            // Si tienes Rigidbody, evita que la fisica deshaga el asiento
            var rb = cardT.GetComponentInParent<Rigidbody>();
            if (rb && !rb.isKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true; // opcional si usas cartas �paradas�
            }

            // Reubica y reorienta a la pose del attach del socket
            cardT.SetPositionAndRotation(at.position, at.rotation);
        }*/

        var card = comp.GetComponentInParent<CardFather>();
        if (card != null)
            CartaDadaVuelta(card);
    }


    public void CartaDadaVuelta(CardFather card)
    {
        if (card.GetCorrecta()) return;
        if (!card.IsFaceUp()) return;

        // Evita el duplicado en la lista
        if (!encontradas.Contains(card))
        {
            encontradas.Add(card);
        }

        if (encontradas.Count == 2)
        {
            StartCoroutine(CompararPareja());
        }
    }

    IEnumerator CompararPareja()
    {
        var c1 = encontradas[0];
        var c2 = encontradas[1];


        // Bloquear cartas hasta que se compare
        var snapshot = new List<CardFather>(encontradas);
        encontradas.Clear();

        // **Clave**: si están en un socket, expulsarlas ANTES de decidir/animar
        /*EjectFromAnySocket(c1);
        EjectFromAnySocket(c2);*/

        // Se verifica que son el mismo tipo de carta
        if (c1.GetIDCard() == c2.GetIDCard())
        {
            c1.BloquearEncontrada();
            c2.BloquearEncontrada();
        }
        else
        {
            yield return new WaitForSeconds(delay);

            // Esto hara que las cartas no giren indefinidamente
            /*var layerInHand = InteractionLayerMask.GetMask("CardInHand");
            var layerActive = InteractionLayerMask.GetMask("CardActive");

            StartCoroutine(NoSocketCooldown(c1, 0.30f, layerInHand, layerActive));
            StartCoroutine(NoSocketCooldown(c2, 0.30f, layerInHand, layerActive));*/

            yield return c1.GirarHaciaAbajo();
            yield return c2.GirarHaciaAbajo();
        }

        JuegoCompletado();

    }

    void JuegoCompletado()
    {
        int cantidadCorrectas = 0;
        
        foreach (var c in TodasLasCartas)
        {
            if (c && c.GetCorrecta())
            {
                cantidadCorrectas++;
            }
        }

        if (cantidadCorrectas == TodasLasCartas.Length && textoFin != null)
        {
            textoFin.text = "Finalizaste la actividad";
        }
    }

    // Modificadores de comportamiento, no de logica
    /*
    void EjectFromAnySocket(CardFather card)
    {
        var grab = card.GetComponent<XRGrabInteractable>();
        var im = FindAnyObjectByType<XRInteractionManager>();
        if (grab == null || im == null) return;

        // Si algún interactor que la selecciona es un socket, forzamos SelectExit con la API nueva (interfaces)
        foreach (var sel in grab.interactorsSelecting)
        {
            if (sel is XRSocketInteractor sock)
            {
                im.SelectExit((IXRSelectInteractor)sock, (IXRSelectInteractable)grab);
            }
        }
    }

    System.Collections.IEnumerator NoSocketCooldown(CardFather card, float seconds,
    InteractionLayerMask layerInHand, InteractionLayerMask layerActive)
    {
        var grab = card.GetComponent<XRGrabInteractable>();
        if (grab == null) yield break;

        // Durante el cooldown, la carta está en una capa que los sockets NO aceptan
        var oldLayers = grab.interactionLayers;
        grab.interactionLayers = layerInHand;

        // (Opcional) apaga collider un instante para evitar hover/impulsos
        var col = card.GetComponentInChildren<Collider>();
        bool hadCol = col && col.enabled;
        if (hadCol) col.enabled = false;

        yield return new WaitForSeconds(seconds);

        if (hadCol) col.enabled = true;
        grab.interactionLayers = layerActive;
    }*/

}

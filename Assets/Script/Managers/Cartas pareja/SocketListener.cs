using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketListener : MonoBehaviour
{
    public CoupleGameManager gameManager;
    XRSocketInteractor socket;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    void OnEnable()
    {
        socket.selectEntered.AddListener(OnSelectEntered);
    }

    void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnSelectEntered);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        var card = args.interactableObject.transform.GetComponent<CardFather>();
        if (card == null || card.GetCorrecta()) return;

        // Dar un frame para que XR fije pose en el attach point del socket
        StartCoroutine(HandlePlaced(card));
    }

    IEnumerator HandlePlaced(CardFather card)
    {
        yield return null; // esperar 1 frame

        // Si cayo boca abajo, revela (gira a boca arriba).
        if (card.IsFaceDown())
            yield return card.GirarHaciaArriba();

        // Se informa al GameManager que hay una carta boca arriba en un socket
        if (gameManager) gameManager.CartaDadaVuelta(card);
    }
}

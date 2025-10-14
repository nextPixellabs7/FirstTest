using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(BoxCollider))]
public class SocketAutosnapGate : MonoBehaviour
{
    [SerializeField] XRSocketInteractor socket;
    [SerializeField] XRInteractionManager interactionManager; // arrastralo o se busca solo
    [Tooltip("Si true, al abrir la compuerta ignora lo que ya estaba dentro hasta que salga y re-entre.")]
    [SerializeField] bool ignoreExistingOnOpen = true;

    private readonly HashSet<IXRSelectInteractable> blocked = new();
    private BoxCollider trigger;
    private bool gateOpen = false;

    void Reset()
    {
        if (!socket) socket = GetComponent<XRSocketInteractor>();
    }

    void Awake()
    {
        trigger = GetComponent<BoxCollider>();
        if (!interactionManager) interactionManager = FindAnyObjectByType<XRInteractionManager>();
        if (trigger) trigger.isTrigger = true;
    }

    void OnEnable()
    {
        if (!socket) return;
        socket.selectEntered.AddListener(OnSelectEntered);
    }

    void OnDisable()
    {
        if (!socket) return;
        socket.selectEntered.RemoveListener(OnSelectEntered);
    }

    public void OpenGate()
    {
        gateOpen = true;
        if (ignoreExistingOnOpen && trigger)
        {
            blocked.Clear();

            // Detecta todo lo que esta dentro del trigger
            var bounds = trigger.bounds;
            var center = bounds.center;
            var halfExtents = bounds.extents;

            // Consulta todo lo que esta adentro
            var hits = Physics.OverlapBox(center, halfExtents, trigger.transform.rotation, ~0, QueryTriggerInteraction.Collide);
            foreach (var h in hits)
            {
                var interactable = h.GetComponentInParent<XRGrabInteractable>();
                if (interactable != null)
                    blocked.Add(interactable);
            }
        }
    }

    public void CloseGate()
    {
        gateOpen = false;
        blocked.Clear();
    }

    void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<XRGrabInteractable>();
        if (interactable != null) blocked.Remove(interactable);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!gateOpen) // Si aun no abre compuerta -> cancela todo
        {
            SafeCancel(args);
            return;
        }

        if (blocked.Contains(args.interactableObject))
        {
            // Estaba dentro al abrir la compuerta -> cancela, debe salir y re-entrar
            SafeCancel(args);
        }
    }

    private void SafeCancel(SelectEnterEventArgs args)
    {
        if (interactionManager && socket && args.interactableObject != null)
        {
            interactionManager.SelectExit(socket, args.interactableObject);
        }
    }
}

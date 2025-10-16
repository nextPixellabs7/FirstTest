using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public enum CardState { FaceDown, FaceUp, Matched, Animating }

public class Card : MonoBehaviour
{
    [Header("Config")]
    public int pairId;                  // ID para emparejar
    public Transform pivot;             // Eje de giro (hijo)
    public GameObject front;            // Cara
    public GameObject back;             // Reverso
    public float flipDuration = 0.25f;  // Velocidad de giro (rápida en VR)
    public AnimationCurve flipCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Refs")]
    public XRGrabInteractable grab;

    public CardState State { get; private set; } = CardState.FaceDown;

    bool _isGrabbed;
    GameManager _gm;

    void Awake()
    {
        if (!grab) grab = GetComponent<XRGrabInteractable>();
        _gm = FindObjectOfType<GameManager>();
        SetVisual(0f); // inicia boca abajo
        // Suscribirse a eventos de XRI
        grab.activated.AddListener(OnActivate);
        grab.selectEntered.AddListener(_ => _isGrabbed = true);
        grab.selectExited.AddListener(_ => _isGrabbed = false);
    }

    void OnDestroy()
    {
        // Limpieza de eventos
        if (grab != null)
        {
            grab.activated.RemoveListener(OnActivate);
            grab.selectEntered.RemoveAllListeners();
            grab.selectExited.RemoveAllListeners();
        }
    }

    void OnActivate(ActivateEventArgs _)
    {
        if (!_isGrabbed) return;
        if (State == CardState.Matched || State == CardState.Animating) return;

        // Toggle
        if (State == CardState.FaceDown) FlipUp();
        else if (State == CardState.FaceUp) FlipDown();
    }

    public void FlipUp(bool notify = true)
    {
        if (State != CardState.FaceDown) return;
        StartCoroutine(CoFlip(0f, 180f, CardState.FaceUp, notify));
    }

    public void FlipDown()
    {
        if (State != CardState.FaceUp) return;
        StartCoroutine(CoFlip(180f, 0f, CardState.FaceDown, false));
    }

    public void LockAsMatched()
    {
        State = CardState.Matched;
        // Deshabilitar interaccion para que no se use más
        grab.enabled = false;
        var col = GetComponent<Collider>();
        if (col) col.enabled = false;
    }

    IEnumerator CoFlip(float from, float to, CardState endState, bool notify)
    {
        State = CardState.Animating;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / flipDuration;
            float a = Mathf.Lerp(from, to, flipCurve.Evaluate(Mathf.Clamp01(t)));
            SetVisual(a);
            yield return null;
        }
        SetVisual(to);
        State = endState;

        if (notify && endState == CardState.FaceUp)
            _gm?.OnCardRevealed(this);
    }

    void SetVisual(float yAngle)
    {
        if (pivot) pivot.localEulerAngles = new Vector3(0f, yAngle, 0f);

        // Alterna visibilidad (opcional) para que no se vea la cara “al reves”
        bool faceUp = yAngle >= 90f;
        if (front) front.SetActive(faceUp);
        if (back) back.SetActive(!faceUp);
    }
}

using System.Collections;
using UnityEngine;
using static UnityEngine.GridBrushBase;


public enum FlipAxis { X, Z }

public class CardFather : MonoBehaviour
{
    // Datos para funcionamiento
    [Header("Identidad")]
    [SerializeField] int id;
    [SerializeField] bool correcta = false;

    // Datos para animacion
    [Header("Giro / Orientaci�n")]
    public FlipAxis flipAxis = FlipAxis.Z;
    [Range(1f, 45f)] public float faceToleranceDeg = 12f;
    public float DuracionGiro = 0.25f;

    // Datos para girar
    [Tooltip("Angulo 'boca abajo' en el eje elegido (habitualmente 0)")]
    public float faceDownAngle = 0f;
    [Tooltip("Angulo 'boca arriba' en el eje elegido (habitualmente 180)")]
    public float faceUpAngle = 180f;

    // Componentes
    Rigidbody rb;
    Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }


    // Getters y Setters
    public int GetIDCard() => id;
    public void SetIDCard(int v) => id = v;
    public bool GetCorrecta() => correcta;
    public void SetCorrecta(bool val) => correcta = val;


    /// Esta boca arriba?
    public bool IsFaceUp()
    {
        float angle = GetAxisAngle();
        float d = Mathf.Abs(Mathf.DeltaAngle(angle, faceUpAngle));
        return d <= faceToleranceDeg;
    }


    /// Esta boca abajo?
    public bool IsFaceDown()
    {
        float angle = GetAxisAngle();
        float d = Mathf.Abs(Mathf.DeltaAngle(angle, faceDownAngle));
        return d <= faceToleranceDeg;
    }


    /// Angulo actual en el eje elegido
    float GetAxisAngle()
    {
        Vector3 e = transform.localEulerAngles;
        return (flipAxis == FlipAxis.X) ? e.x : e.z;
    }


    /// Girar boca arriba
    public IEnumerator GirarHaciaArriba()
    {
        yield return RotateToAxisAngle(faceUpAngle, DuracionGiro);
    }


    /// Girar boca abajo
    public IEnumerator GirarHaciaAbajo()
    {
        yield return RotateToAxisAngle(faceDownAngle, DuracionGiro);
    }


    IEnumerator RotateToAxisAngle(float targetAngle, float duration)
    {
        // Desactivar interaccion para evitar interferencias
        bool hadKinematic = rb ? rb.isKinematic : false;
        if (rb) rb.isKinematic = true;
        if (col) col.enabled = false;

        Vector3 start = transform.localEulerAngles;
        Vector3 end = start;

        if (flipAxis == FlipAxis.X) end.x = targetAngle;
        else end.z = targetAngle;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, duration);
            Vector3 step = Vector3.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t));
            transform.localEulerAngles = step;
            yield return null;
        }

        if (col) col.enabled = true;
        if (rb) rb.isKinematic = hadKinematic;
    }


    /// Bloquea la carta al emparejar
    public void BloquearEncontrada()
    {
        correcta = true;
        if (rb) rb.isKinematic = true;

        var grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab) grab.enabled = false;

        if (col) col.enabled = false;
    }

}
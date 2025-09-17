using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class CoupleGame : MonoBehaviour
{
    private GameObject[] cartas;
    public TextMeshProUGUI texto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cartas = GameObject.FindGameObjectsWithTag("CardGrab");
    }

    // Update is called once per frame
    void Update()
    {
        VerificarVolteadas();
        JuegoCompletado();
    }

    // This function verifys if the card is flipped
    void VerificarVolteadas()
    {
        List<CardFather> volteadas = new List<CardFather>();

        foreach (GameObject go in cartas)
        {
            CardFather carta = go.GetComponent<CardFather>();
            if (carta == null) continue;

            bool girada = carta.RevisarGiro();

            if (girada)
            {
                volteadas.Add(carta);
            }
        }

        if (volteadas.Count == 2)
        {
            RevisarParejas(volteadas);
        }
    }

    void RevisarParejas(List<CardFather> volteadas)
    {
        CardFather c1 = volteadas[0];
        CardFather c2 = volteadas[1];

        if (c1.GetCorrecta() == false && c2.GetCorrecta() == false)
        {
            if (c1.GetIDCard() == c2.GetIDCard())
            {
                c1.GetComponent<Rigidbody>().isKinematic = true;
                c2.GetComponent<Rigidbody>().isKinematic = true;
                c1.GetComponent<Collider>().enabled = false;
                c2.GetComponent<Collider>().enabled = false;
                c1.SetCorrecta(true);
                c2.SetCorrecta(true);
            }
            else
            {
                StartCoroutine(GirarCarta(c1, 1f));
                StartCoroutine(GirarCarta(c2, 1f));
            }
        }
    }

    void JuegoCompletado()
    {
        int correctas = cartas
        .Select(go => go.GetComponent<CardFather>())
        .Where(c => c != null && c.GetCorrecta())
        .Count();

        if (correctas == cartas.Length)
        {
            texto.text = "¡Finalizaste la actividad!";
        }
    }

    IEnumerator GirarCarta(CardFather carta, float duracion)
    {
        Collider col = carta.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = carta.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Quaternion inicio = carta.transform.rotation;
        Quaternion fin = inicio * Quaternion.Euler(0f, 0f, 0f);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duracion;
            carta.transform.rotation = Quaternion.Slerp(inicio, fin, t);
            yield return null;
        }

        if (col != null) col.enabled = true;
        carta.GetComponent<Rigidbody>().isKinematic = false;
    }
}

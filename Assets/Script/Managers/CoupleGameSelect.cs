using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;

public class CoupleGameSelect : MonoBehaviour
{
    private GameObject[] cartas;
    public TextMeshProUGUI texto;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cartas = GameObject.FindGameObjectsWithTag("CardSelect");
    }

    // Funcion que sera ejecutada si se selecciona la carta
    public void CartaSeleccionada(SelectEnterEventArgs args)
    {
        GameObject cartaGO = args.interactableObject.transform.gameObject;

        // Se gira la carta
        StartCoroutine(GirarCartaYVerificar(cartaGO.GetComponent<CardFather>(), 1f));
    }

    // Esta carta verifica si existen cartas volteadas
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
                c1.GetComponent<Collider>().enabled = false;
                c2.GetComponent<Collider>().enabled = false;
                c1.SetCorrecta(true);
                c2.SetCorrecta(true);

                bool termino = JuegoCompletado(); // Verifica si el juego se completo

                if(termino)
                {
                    texto.text = "¡Completaste el juego!";
                }
                else
                {
                    texto.text = "¡Encontraste una pareja!";
                }
                
            }
            else
            {
                StartCoroutine(GirarCartaYVerificar(c1, 1f));
                StartCoroutine(GirarCartaYVerificar(c2, 1f));
            }
        }
    }

    bool JuegoCompletado()
    {
        int correctas = 0;

        foreach (GameObject go in cartas)
        {
            CardFather carta = go.GetComponent<CardFather>();
            if (carta == null) continue;

            bool correcta = carta.GetCorrecta();

            if (correcta)
            {
                correctas++;
            }
        }

        if (correctas == cartas.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator GirarCartaYVerificar(CardFather carta, float duracion)
    {

        if (carta == null) yield break;

        Collider col = carta.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Quaternion inicio = carta.transform.rotation;
        Quaternion fin = inicio * Quaternion.Euler(180f, 0f, 0f);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duracion;
            carta.transform.rotation = Quaternion.Slerp(inicio, fin, t);
            yield return null;
        }

        // Verifica si es que la carta esta volteada y la figura mirando hacia arriba
        if (carta.RevisarGiro())
        {
            VerificarVolteadas(); // Si esta volteada verifica si existen cartas ya volteadas
        }
        else
        {
            if (col != null) col.enabled = true;
        }
    }
}

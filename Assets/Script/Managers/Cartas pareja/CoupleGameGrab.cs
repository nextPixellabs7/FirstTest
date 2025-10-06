using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEditor.Experimental.GraphView;

public class CoupleGame : MonoBehaviour
{

    [Header("UI")]
    public TextMeshProUGUI textoFin;

    [Header("Opciones")]
    public float delay = 0.4f; // pausa antes de voltear de regreso

    List<CardFather> encontradas = new List<CardFather>();
    CardFather[] TodasLasCartas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TodasLasCartas = GameObject.FindObjectsByType<CardFather>(FindObjectsSortMode.None);
    }

    public void CartaDadaVuelta(CardFather card)
    {
        if (card.GetCorrecta()) return;
        if (!card.IsFaceUp()) return; // esto es para evitar errores

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

        // Se verifica que son el mismo tipo de carta
        if (c1.GetIDCard() == c2.GetIDCard())
        {
            c1.BloquearEncontrada();
            c2.BloquearEncontrada();
        }
        else
        {
            yield return new WaitForSeconds(delay);
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
}

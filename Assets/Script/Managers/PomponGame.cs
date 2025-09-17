using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PomponGame : MonoBehaviour
{
    private List<Pompon> SmallPompones = new List<Pompon>();
    private List<Pompon> BigPompones = new List<Pompon>();
    public TextMeshProUGUI TextoBig;
    public TextMeshProUGUI TextoSmall;
    public TextMeshProUGUI TextoTitulo;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Pompon pompon = other.GetComponent<Pompon>();
        

        if (pompon.GetSize() == "Small" && gameObject.CompareTag("CanastaChica"))
        {
            pompon.setCorrecta(true);
            StartCoroutine(MostrarTexto(pompon.estaCorrecta(), 5f));
            other.GetComponent<Collider>().enabled = false;
            SmallPompones.Add(pompon);
            CanastaTerminada(pompon.GetSize());
        }
        else if(pompon.GetSize() == "Big" && gameObject.CompareTag("CanastaGrande"))
        {
            pompon.setCorrecta(true);
            StartCoroutine(MostrarTexto(pompon.estaCorrecta(), 5f));
            other.GetComponent<Collider>().enabled = false;
            BigPompones.Add(pompon);
            CanastaTerminada(pompon.GetSize());
        }
        else
        {
            MostrarTexto(pompon.estaCorrecta() ,5f);
        }
    }

    private void CanastaTerminada(string size)
    {
        int correctas = 0;

        if (size == "Big")
        {
            foreach(Pompon p in BigPompones)
            {
                correctas++;;
            }
        }
        else if(size == "Small")
        {
            foreach(Pompon p in SmallPompones)
            {
                correctas++;
            }
        }

        int Todos = BigPompones.Count + SmallPompones.Count;

        if(correctas == Todos)
        {
            TextoTitulo.text = "¡Felicidades, lo has completado!";
        }
    }

    IEnumerator MostrarTexto(bool correcto,float duracion)
    {


        if (correcto)
        {
            if (duracion < 5f)
            {
                TextoSmall.text = "¡Correcto!";
                TextoBig.text = "¡Correcto!";
            }
        }
        else
        {
            if (duracion < 5f)
            {
                TextoSmall.text = "¡Te has equivocado!";
                TextoBig.text = "¡Te has equivocado!";
            }
        }
        yield return null;
    }

}

using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PomponGame : MonoBehaviour
{
    private List<Pompon> SmallPompones = new List<Pompon>();
    private List<Pompon> BigPompones = new List<Pompon>();

    public TextMeshProUGUI TextoBig;
    public TextMeshProUGUI TextoSmall;
    public TextMeshProUGUI TextoTitulo;

    private void Start()
    {
        GameObject[] Bpompon = GameObject.FindGameObjectsWithTag("BigPompon");
        GameObject[] Spompon = GameObject.FindGameObjectsWithTag("SmallPompon");
    }

    private void OnTriggerEnter(Collider other)
    {
        Pompon pompon = other.GetComponent<Pompon>();
        if (pompon == null) return;

        if (pompon.GetSize() == "Small" && gameObject.CompareTag("CanastaChica"))
        {
            pompon.setCorrecta(true);
            StartCoroutine(MostrarTexto(pompon.estaCorrecta(), 5f, pompon.GetSize()));
            other.GetComponent<XRGrabInteractable>().enabled = false;
            SmallPompones.Add(pompon);
        }
        else if(pompon.GetSize() == "Big" && gameObject.CompareTag("CanastaGrande"))
        {
            pompon.setCorrecta(true);
            StartCoroutine(MostrarTexto(pompon.estaCorrecta(), 5f, pompon.GetSize()));
            other.GetComponent<XRGrabInteractable>().enabled = false;
            BigPompones.Add(pompon);
        }
        else
        {
            MostrarTexto(pompon.estaCorrecta() ,5f, pompon.GetSize());
        }


        CanastaTerminada();
    }

    private void CanastaTerminada()
    {
        if(12 == BigPompones.Count + SmallPompones.Count)
        {
            TextoTitulo.text = "¡Felicidades, lo has completado!";
        }
    }

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
        TextoSmall.text = "Canasta de pompones chicos";
        TextoBig.text = "Canasta de pompones grandes";
    }

}

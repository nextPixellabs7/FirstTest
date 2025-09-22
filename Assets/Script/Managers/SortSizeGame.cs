using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SortSizeGame : MonoBehaviour
{

    private XRSocketInteractor socket;


    public TextMeshProUGUI textoL;
    public TextMeshProUGUI textoM;
    public TextMeshProUGUI textoS;
        

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();

        socket.selectEntered.AddListener(OnObjectSelected);

    }

    public void OnObjectSelected(SelectEnterEventArgs other)
    {

        GameObject obj = other.interactableObject.transform.gameObject;

        // Verificar si el objeto corresponde a su zona
        if (this.tag == "CubeZone" && obj.tag != "Cubo") { Reject(other); return; }
        if (this.tag == "CircleZone" && obj.tag != "Esfera") { Reject(other); return; }
        if (this.tag == "CylinderZone" && obj.tag != "Cilindro") { Reject(other); return; }

        ObjectSort figura = obj.GetComponent<ObjectSort>();
        DimensionZona dimension = GetComponent<DimensionZona>();

        if (figura == null || dimension == null || (int)figura.size != (int)dimension.capacidad)
        {
            switch(dimension.capacidad)
            {
                case DimensionZona.CapacidadZona.Chico:
                    textoS.text = "¡Te has equivocado!";
                    break;

                case DimensionZona.CapacidadZona.Mediano:
                    textoM.text = "¡Te has equivocado!";
                    break;

                case DimensionZona.CapacidadZona.Grande:
                    textoL.text = "¡Te has equivocado!";
                    break;
            }
        }

        switch (dimension.capacidad)
        {
            case DimensionZona.CapacidadZona.Chico:
                textoS.text = "¡Correcto!";
                figura.GetComponent<XRGrabInteractable>().enabled = false;
                break;

            case DimensionZona.CapacidadZona.Mediano:
                textoM.text = "¡Correcto!";
                figura.GetComponent<XRGrabInteractable>().enabled = false;
                break;

            case DimensionZona.CapacidadZona.Grande:
                textoL.text = "¡Correcto!";
                figura.GetComponent<XRGrabInteractable>().enabled = false;
                break;
        }

    }

    public void Reject(SelectEnterEventArgs obj)
    {
        socket.interactionManager.SelectExit(socket, obj.interactableObject);
    }

}

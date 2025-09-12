using TMPro;
using UnityEngine;

public class CartaLookingUp : MonoBehaviour
{
    [Range(0f, 30f)]
    public float toleranciaGrados = 10f;
    public string carta;
    private bool lookingUp;
    public TextMeshProUGUI texto;

    void Start()
    {
        lookingUp = false;   
    }

    // Update is called once per frame
    void Update()
    {

        float angulo = Vector3.Angle(transform.up, Vector3.up);

        if(angulo <= toleranciaGrados)
        {
            texto.text = "Una carta ha sido dado vuelta";
        }
        else
        {
            texto.text = "";
        }


    }
}

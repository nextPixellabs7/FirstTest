using TMPro;
using UnityEngine;

public class CrossCard : MonoBehaviour
{

    [Range(0f, 30f)]
    public float toleranciaGrados = 10f;
    private bool lookingUp;
    public TextMeshProUGUI texto;
    private int ID = 1;

    public int GetIDCard() => ID;

    public void SetIDCard(int idCard) => ID = idCard;

    public bool IsFlipped => lookingUp;

    void Start()
    {
        lookingUp = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool RevisarGiro()
    {
        float angulo = Vector3.Angle(transform.up, Vector3.up);

        if (angulo <= toleranciaGrados)
        {
            lookingUp = true;
        }
        else
        {
            texto.text = "";
            lookingUp = false;
        }

        return lookingUp;
    }
}

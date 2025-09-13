using UnityEngine;

public class CoupleGame : MonoBehaviour
{

    private int IDCarta;
    private bool estaVolteada = false;
    private GameObject[] cartas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cartas = GameObject.FindGameObjectsWithTag("Card");
    }

    // Update is called once per frame
    void Update()
    {
        int volteadas = 0;

        foreach (var carta in cartas)
        {
            var check = carta.GetComponent<CheckCard>();
            var cross = carta.GetComponent<CrossCard>();

            bool girada = false;
            int id = - 1;

            if (check != null)
            {
                girada = check.RevisarGiro();
                id = check.GetIDCard();
            }
            else if(cross != null)
            {
                girada = cross.RevisarGiro();
                id = cross.GetIDCard();
            }

            if (girada)
            {
                volteadas++;
                Debug.Log($"Carta ID {id} esta volteda");
            }
        }

        if (volteadas >= 2)
        {
           
        }

    }
}

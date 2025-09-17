using UnityEngine;

public class CardFather : MonoBehaviour
{
    [Range(0f, 30f)]
    public float toleranciaGrados = 10f;

    protected int ID = 1;
    protected bool correcta = false;

    public bool GetCorrecta() => correcta;
    public void SetCorrecta(bool correcta) => this.correcta = correcta;

    public int GetIDCard() => ID;
    public void SetIDCard(int idCard) => ID = idCard;

    public bool MirandoArriba = false;

    public bool RevisarGiro()
    {
        float cosTolerancia = Mathf.Cos(toleranciaGrados * Mathf.Deg2Rad); 
        if (Vector3.Dot(transform.up, Vector3.up) <= cosTolerancia) 
        {
            MirandoArriba = true;
        } 
        else 
        {
            MirandoArriba = false; 
        }
        return MirandoArriba;
    }
}
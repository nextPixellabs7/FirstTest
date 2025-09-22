using UnityEngine;

public class ObjectSort : MonoBehaviour
{

    public enum tamanho
    {
        Chico,
        Mediano,
        Grande
    }

    public tamanho size;

    public tamanho GetSize() => size;

}

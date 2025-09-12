using UnityEngine;
using TMPro;

public class UserLogIn : MonoBehaviour
{
    public TMP_InputField nombre;
    public GameManager GM;
    public TextMeshProUGUI bienvenido;
    private string newName;

    public void setName()
    {
        newName = nombre.text;
        GM.SetText(newName);
        bienvenido.text= ("Bienvenido\n" + GM.GetText());
    }
}

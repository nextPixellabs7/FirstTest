using UnityEngine;
using TMPro;

public class UserLogIn : MonoBehaviour
{
    public TMP_InputField nombre;
    public GameManager GM;
    public TextMeshProUGUI bienvenido;
    private string name;

    public void setName()
    {
        name = nombre.text;
        GM.SetText(name);
        bienvenido.text= ("Bienvenido\n" + GM.GetText());
    }
}

using UnityEngine;

public class CheckCard : MonoBehaviour
{
    private int ID = 2;

    public int GetIDCard() 
    { 
        return ID;
    }

    public void SetIDCard(int idCard)
    {
        this.ID = idCard;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

    public GameObject text;
    public GameObject[] hover;

    public void setMenu(string controller)
    {
        Color color = Color.white;
        //Debug.Log("Controller: " + controller);
        if (controller == "1P")
            color = new Color(124 / 255f, 0, 0);
        else if (controller == "2P")
            color = new Color(8 / 255f, 25/255f, 112/255f);

        text.GetComponent<Image>().color = color;

        for (int i = 0; i < hover.Length; i++)
        {
            hover[i].GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
            hover[i].GetComponent<Options>().setPlayer(controller);
        }
    }

    public void setMenuGameOver(string controller)
    {
        Color color = Color.white;
        
        if (controller == "1P")
            color = new Color(124 / 255f, 0, 0);
        else if (controller == "2P")
            color = new Color(8 / 255f, 25 / 255f, 112 / 255f);
         
        text.GetComponent<Image>().color = color;

        for (int i = 0; i < hover.Length; i++)      
            hover[i].GetComponent<Image>().color = color;
        
    }

    
}

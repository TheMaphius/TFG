using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Combo : MonoBehaviour {

    private int combo = 1;

    private Sprite[] number;
    private Image[] digit;

    private Engine engine;

    void Start()
    {
        int childs = transform.childCount - 1;
        digit = new Image[childs];

        for (int i = 0; i < digit.Length; i++)
            digit[i] = transform.GetChild(i).GetComponent<Image>();

        // Initialize a Sprite with images [0-9] "," "+".
        number = Resources.LoadAll<Sprite>("Textures/Score/black_score");
        engine = GameObject.FindGameObjectWithTag("Engine").GetComponent<Engine>();
    }

    
    public int getCombo()
    {
        return this.combo;
    }

    
    public void setCombo(int combo)
    {
        this.combo += combo;
        ComboNumber();
    }


    public void ComboBreak()
    {
        this.combo = 1;
        ComboNumber();
    }


    private void ComboNumber()
    {
        string str_combo = this.combo.ToString();
        int length;

        if (this.combo < 10)
            str_combo = "0" + str_combo;

        length = str_combo.Length;

        for(int i = 0; i < length; i++)
        {
            int num = int.Parse(str_combo.Substring((length - i) - 1, 1));
            digit[i].overrideSprite = number[num];
        }
    }
}

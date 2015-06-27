using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Points : MonoBehaviour {

    public enum enumColor // Selectable type of color.
    {
        black,
        red,
        blue
    };

    public enumColor color;

    private Sprite[] number;
    private Transform[] points_digit;

    // Use this for initialization
	void Awake () {

        int childs = transform.childCount;
        points_digit = new Transform[childs];

        // Get the child components of the GameObject.
        for (int i = 0; i < childs; i++)
            points_digit[i] = transform.GetChild(i);

	}

    public void setZombiePoints(int zombie_points, string color)
    {

        string points = zombie_points.ToString();//CalculatePoints(score, zombie_parts);
        int length = points.Length;
        
        Image character;
        Color char_color;


        // Initialize a Sprite with images [0-9] "," "+".
        number = Resources.LoadAll<Sprite>("Textures/Score/" + color + "_score");

        if (length > 3)
        {
            for (int i = 0; i < length; i++)
            {
                int digit = int.Parse(points.Substring((length - i) - 1, 1));
                character = points_digit[i].GetComponent<Image>();
                char_color = character.color;
                char_color.a = 1f;
                character.overrideSprite = number[digit];
                character.color = char_color;
            }

            // This part is only for the '.' and '+'
            character = points_digit[4].GetComponent<Image>(); // <-- Dot
            char_color = character.color;
            char_color.a = 1f;
            character.overrideSprite = number[10];
            character.color = char_color;

            character = points_digit[5].GetComponent<Image>(); // <-- Plus
            character.overrideSprite = number[11];
            character.color = char_color;
            
        }
        else
        {
            for (int i = 0; i < length; i++)
            {
                int digit = int.Parse(points.Substring((length - i) - 1, 1));
                character = points_digit[i].GetComponent<Image>();
                char_color = character.color;
                char_color.a = 1f;
                character.overrideSprite = number[digit];
                character.color = char_color;
            }

            // This part is only for the '+'
            character = points_digit[5].GetComponent<Image>();
            char_color = character.color;
            char_color.a = 1f;
            character.overrideSprite = number[11];
            character.color = char_color;

            points_digit[5].position = new Vector3(points_digit[4].position.x*1.4f, points_digit[5].position.y, 0);
            
        }
        
    }
	
	
}

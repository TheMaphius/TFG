/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeEffect : MonoBehaviour {

    public enum enumFade // Selectable type of platform.
    {
        Text,
        Image
    };

    public enumFade fade_type;
 
    public float alpha;
    public int direction;		// Direction -1 = Fade in
    public float fadeSpeed;

    public bool repeat;

    private Text text;
    private Image image;

    private Color color;

	// Use this for initialization
	void Start () 
    {
        switch (fade_type.ToString())
        {
            case "Text":
                text = gameObject.GetComponent<Text>();
                break;
            case "Image":
                image = gameObject.GetComponent<Image>();
                break;
        }
        
        setFade(direction);
	}
	
	// Update is called once per frame
	void Update () {
        Fade();
	}


    /// <summary>
    /// Method fade effect.
    /// </summary>
    void Fade()
    {
        alpha += direction * fadeSpeed * Time.deltaTime;		    // We decrease or increase alpha depends direction.
        alpha = Mathf.Clamp01(this.alpha);							// Values between 0 and 1.

        switch (fade_type.ToString())
        {
            case "Text":
                color = text.color;
                text.color = new Color(color.r, color.g, color.b, alpha);
                break;
            case "Image":
                color = image.color;
                image.color = new Color(color.r, color.g, color.b, alpha);
                break;
        }

        if (repeat)
        {
            if (alpha == 0)
                setFade(1);
            else if (alpha == 1)
                setFade(-1);
        }
    }


    /// <summary>
    /// Set fade direction. 1 Fade out -1 Fade in.
    /// </summary>
    /// <param name="direction">Fade direction</param>
    public void setFade(int direction)
    {
        this.direction = direction;
    }

    public void setSpeed(float speed)
    {
        this.fadeSpeed = speed;
    }

}

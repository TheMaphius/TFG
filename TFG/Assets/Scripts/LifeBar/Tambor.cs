using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tambor : MonoBehaviour {

    public Sprite[] texture;

    private Transform[] bullet;
    private Image image;

    private Combo combo;
    private bool isSpecial = true;

    private short index;

	// Use this for initialization
	void Start () {

        int childs = transform.childCount;
        bullet = new Transform[childs];

        for (int i = 0; i < childs; i++)
            bullet[i] = transform.GetChild(i);
	
	}

    public void RotateTambor(string type, Combo combo)
    {
        int idx = index % 6;

        image = bullet[idx].GetComponent<Image>();
        Color color = image.color;
        color.a = 1f;
        image.color = color;

        switch (type)
        {
            case "normal":
                image.overrideSprite = texture[0];
                isSpecial = false;
                break;
            case "special":
                image.overrideSprite = texture[1];
                break;
            default:
                break;
        }

        index++;
        
        this.combo = combo;
        
        StartCoroutine(Delay(60, Vector3.back, 3));
        
    }


    public void ResetTambor()
    {
        int idx = index;
        int rotation = 60 * idx;
        
        //Debug.Log("index: " + index);
        for (int i = 0; i < idx; i++)
        {
            try
            {
                if ((image = bullet[i].GetComponent<Image>()) != null)
                {
                    image.overrideSprite = null;
                    Color color = image.color;
                    color.a = 0f;
                    image.color = color;
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                Debug.Log("Error super random.");
            }
            
        }

        index = 0;
        isSpecial = true;

        if(gameObject.activeInHierarchy)
            StartCoroutine(Delay(rotation, Vector3.forward, 6));
    }


    private IEnumerator Delay(int rotation, Vector3 direction, int step)
    {
        
        for (int rot = 1; rot <= rotation; rot += step)
        {
            transform.Rotate(direction * step);
            yield return 0;
        }

        if (index == 6)
        {
            combo.setCombo((isSpecial) ? 2 : 1);
            ResetTambor();
        }

    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LanguageGame : MonoBehaviour {

    public Sprite[] lang;
    private Image img;

	// Use this for initialization
	void Start () 
    {
        UpdateLanguage();
	}

    public void UpdateLanguage()
    {
        string language = PlayerPrefs.GetString("language");
        img = GetComponent<Image>();

        switch (language)
        {
            case "Spanish":
                img.overrideSprite = lang[0];
                break;
            case "English":
                img.overrideSprite = lang[1];
                break;
            case "Japanese":
                img.overrideSprite = lang[2];
                break;
            default:
                img.overrideSprite = lang[1];
                break;
        }
    }

    public void setLanguage(int idx)
    {
        img.overrideSprite = lang[idx];
    }

    void OnEnable()
    {
        UpdateLanguage();
    }
	
}

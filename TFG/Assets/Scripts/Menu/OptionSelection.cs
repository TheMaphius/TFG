using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionSelection : MonoBehaviour {

    public Sprite[] options;
    public bool isDifficulty;
    public bool isContinues;
    public bool isMute;
    public bool isGraphics;
    public bool isLanguages;

    private Image image;
    private int idx;

	// Use this for initialization
	void Start () 
    {
        InitStats();
	}

    public void InitStats()
    {
        if (isDifficulty)
            idx = PlayerPrefs.GetInt("difficulty");
        else if (isContinues)
            idx = PlayerPrefs.GetInt("continue");
        else if (isMute)
        {
            idx = PlayerPrefs.GetInt("sound");

            Debug.Log("Mute idx: " + idx);
        }
        else if (isGraphics)
            idx = PlayerPrefs.GetInt("graphics");
        else if (isLanguages)
        {
            string language = PlayerPrefs.GetString("language");

            switch (language)
            {
                case "Spanish":
                    idx = 0;
                    break;
                case "English":
                    idx = 1;
                    break;
                case "Japanese":
                    idx = 2;
                    break;
            }
        }

        image = GetComponent<Image>();
        image.overrideSprite = options[idx];
    }

	public void Increase()
    {
        idx++;

        if (idx < 0)
            idx = options.Length - 1;
        else if (idx > (options.Length - 1))
            idx = 0;

        image.overrideSprite = options[idx];
        
        UpdateStats();
    }

    public void Decrease()
    {
        idx--;

        if (idx < 0)
            idx = options.Length - 1;
        else if (idx > (options.Length - 1))
            idx = 0;

        image.overrideSprite = options[idx];
        UpdateStats();
    }

    public void UpdateStats()
    {

        if (isDifficulty)
            PlayerPrefs.SetInt("difficulty", idx);
        else if (isContinues)
            PlayerPrefs.SetInt("continue", idx);
        else if (isMute)
        {
            PlayerPrefs.SetInt("sound", idx);
            AudioSource[] audio = FindObjectsOfType<AudioSource>();
            Debug.Log("Waht value: " + idx);
            for (int i = 0; i < audio.Length; i++)
            {
                audio[i].mute = (idx == 0) ? true : false;
            }
        }
        else if (isGraphics)
        {

            int aliasing;
            
            switch (idx)
            {
                case 0:
                    aliasing = 0;
                    break;
                case 1:
                    aliasing = 2;
                    break;
                case 2:
                    aliasing = 4;
                    break;
                case 3:
                    aliasing = 8;
                    break;
                default:
                    aliasing = 0;
                    break;

            }

            PlayerPrefs.SetInt("graphics", idx);
            QualitySettings.antiAliasing = aliasing;
        }
        else if (isLanguages)
        {
            string language = "";

            switch (idx)
            {
                case 0:
                    language = "Spanish";
                    break;
                case 1:
                    language = "English";
                    break;
                case 2:
                    language = "Japanese";
                    break;
            }

            PlayerPrefs.SetString("language", language);

            LanguageGame[] lang = FindObjectsOfType<LanguageGame>();

            for (int i = 0; i < lang.Length; i++)
                lang[i].setLanguage(idx);
            

        }
    }

    void OnEnable()
    {
        InitStats();
    }

}

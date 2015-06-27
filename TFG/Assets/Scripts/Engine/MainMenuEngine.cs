using UnityEngine;
using System.Collections;

public class MainMenuEngine : MonoBehaviour {

    public GameObject mainmenu;
    public GameObject options;
    public GameObject option_menu;
    public GameObject option_default;
    public GameObject game_settings;
    public GameObject control_settings;

    public AudioEngine audio_engine;

    public FadeEffect fade;

    private IEnumerator coroutine = null;

    private int idx;
    private string language;

    private bool pressedOption = false;

	// Use this for initialization
	void Start () 
    {
        audio_engine.MainAudio();

        language = PlayerPrefs.GetString("language");

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
	
	// Update is called once per frame
	void Update () 
    {
        /*if(coroutine != null)
            Debug.Log("Coroutine state: " + coroutine);
        else
            Debug.Log("Coroutine null.");*/
	}

    public void getSelection()
    { 
    }

    public void setCoroutine(IEnumerator coroutine)
    {
        this.coroutine = coroutine;
    }

    public IEnumerator getCoroutine()
    {
        return coroutine;
    }

    public void setPressedOption(bool pressed)
    {
        this.pressedOption = pressed;
    }

    public bool getPressedOption()
    {
        return pressedOption;
    }

    public void activateOptions(bool active)
    {
        if (active)
        {
            audio_engine.Stop();
            audio_engine.MainAudio();
        }
        else
        {
            audio_engine.Stop();
            audio_engine.OptionsAudio();
        }

        mainmenu.SetActive(active);
        options.SetActive(!active);
        setPressedOption(false);
    }

    public void activateOptionsSettings(bool active)
    {
        option_menu.SetActive(active);
        game_settings.SetActive(!active);
        setPressedOption(false);
    }

    public void activateOptionsController(bool active)
    {
        option_menu.SetActive(active);
        control_settings.SetActive(!active);
        setPressedOption(false);
    }

    public void activateOptionsDefault(bool active)
    {
        option_menu.SetActive(!active);
        option_default.SetActive(active);
        setPressedOption(false);
    }

    public void DefaultSettings()
    {
        PlayerPrefs.SetInt("difficulty", 2);
        PlayerPrefs.SetInt("continue", 3);
        PlayerPrefs.SetInt("sound", 1);
        PlayerPrefs.SetInt("graphics", 0);
        PlayerPrefs.SetString("language", Application.systemLanguage.ToString());

        AudioSource[] audio = FindObjectsOfType<AudioSource>();

        for (int i = 0; i < audio.Length; i++)
            audio[i].mute = false;

        QualitySettings.antiAliasing = 0;
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {

        string aux = PlayerPrefs.GetString("language");
        

        switch (aux)
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

        LanguageGame[] lang = FindObjectsOfType<LanguageGame>();

        for (int i = 0; i < lang.Length; i++)
            lang[i].setLanguage(idx);
        
    }

    public FadeEffect getFade()
    {
        return fade;
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("player2", 1);
    }

}

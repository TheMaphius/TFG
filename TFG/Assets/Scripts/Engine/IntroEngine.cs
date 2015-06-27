using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroEngine : MonoBehaviour {

	// Use this for initialization
	void Awake () 
    {
        Screen.showCursor = false;

        if (PlayerPrefs.GetInt("difficulty") == 0)
        {
            //Debug.Log("Parameter difficulty is null");
            PlayerPrefs.SetInt("difficulty", 2);
        }

        if (PlayerPrefs.GetInt("continue") == 0)
        {
            //Debug.Log("Parameter continue is null");
            PlayerPrefs.SetInt("continue", 3);
        }

        if (PlayerPrefs.GetInt("sound") == 0)
        {
            //Debug.Log("Parameter sound is null");
            PlayerPrefs.SetInt("sound", 1);
        }

        if (PlayerPrefs.GetInt("graphics") == 0)
        {
            //Debug.Log("Parameter graphics is null");
            PlayerPrefs.SetInt("graphics", 0);
        }
        
        if (PlayerPrefs.GetString("language") == "")
        {
            //Debug.Log("Parameter language is null");
            PlayerPrefs.SetString("language", Application.systemLanguage.ToString());
        }

        PlayerPrefs.SetInt("player2", 1);
        PlayerPrefs.SetInt("Level", 0);
        PlayerPrefs.SetString("loading", "");

        StartCoroutine(Delay());
	
	}

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        Application.LoadLevel(1);

    }
	
	
}

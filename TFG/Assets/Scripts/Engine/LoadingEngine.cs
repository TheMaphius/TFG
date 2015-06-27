using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingEngine : MonoBehaviour {

    public GameObject splash_screen;
    public GameObject[] loading;

    private AsyncOperation async;

    //private int level;
    //private string loading_splash;

    private Sprite[] nums;

	// Use this for initialization
	void Start () 
    {
        int level = PlayerPrefs.GetInt("Level");
        string language = PlayerPrefs.GetString("language");
        string loading_splash = PlayerPrefs.GetString("loading");

        string str_language = "";

        if(language == "Spanish") str_language = "sp"; 
        else if(language == "English") str_language = "en"; 
        else if(language == "Japanese") str_language = "jp";

        Sprite splash = Resources.Load<Sprite>("Textures/Menu/Loading/loading_screen_" + loading_splash + "_" + str_language);
        splash_screen.GetComponent<Image>().overrideSprite = splash;

        nums = Resources.LoadAll<Sprite>("Textures/Text/loading_num");
        
        StartCoroutine(loadLevel((level)));
	}

    void Update()
    {
        if (async != null)
        {
            int load = Mathf.FloorToInt(async.progress * 100f);
            setLoading(load);

            if (async.progress >= .9f)
            {
                setLoading(100);
                async.allowSceneActivation = true;
            }
        }
    }

    private void setLoading(int load)
    {
        string str_load = load.ToString();
        int size = load.ToString().Length;

        for (int i = 0; i < size; i++)
        { 
            int num = int.Parse(str_load.Substring(i, 1));
            
            loading[(size-1) - i].GetComponent<Image>().overrideSprite = nums[num];
        }
    }

    private IEnumerator loadLevel(int level)
    {
        yield return new WaitForSeconds(1);
        async = Application.LoadLevelAsync(level);
        async.allowSceneActivation = false;

    }


}

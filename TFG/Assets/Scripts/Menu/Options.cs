using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Options : MonoBehaviour {

    private const float reference_width = 716f;

    public GameObject[] pointers;
    public AudioClip[] audio_button;

    public float delay;

    public bool isMenu;
    public bool isOption;

    private Engine engine;
    private PauseEngine pause;
    private MainMenuEngine menu;
    private LevelEngine level;
    private AreaClearEngine clear;

    private Image image;
    private Color color;
    private RectTransform objectRectTransform;

    private float xMax;
    private float xMin;
    private float yMax;
    private float yMin;

    private string controller;
    private bool isPause = false;
    private bool isHover = false;
    private bool[] playerHover;

    void Start()
    {

        GameObject obj = GameObject.FindGameObjectWithTag("Engine");
        engine = obj.GetComponent<Engine>();
        pause = obj.GetComponent<PauseEngine>();
        menu = obj.GetComponent<MainMenuEngine>();
        level = obj.GetComponent<LevelEngine>();
        clear = obj.GetComponent<AreaClearEngine>();

        objectRectTransform = gameObject.GetComponent<RectTransform>();                // This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
        float width = (Screen.width * objectRectTransform.rect.width) / reference_width;
        float height = objectRectTransform.rect.height;

        xMax = (width * .65f);
        xMin = (width * -.65f);
        yMax = (height * .2f);
        yMin = (height * -.2f);

        playerHover = new bool[2];

        gameObject.AddComponent<AudioSource>();
        CheckAudio();
    }

    void Update()
    {
        if (!isMenu && !isOption)
            HoverPlayer();
        else if (isMenu)
            HoverMenu();
        else if (isOption)
            HoverOption();
    }


    private void HoverPlayer()
    {
        
        int i = (controller == "1P") ? i = 0 : i = 1;

        if (pointers[i] != null)
        {
            Vector3 position = pointers[i].transform.position;

            if (position.x <= (transform.position.x + xMax) && position.x >= (transform.position.x + xMin) && position.y <= (transform.position.y + yMax) && position.y >= (transform.position.y + yMin))
            {
                if (!isHover)
                {
                    image = GetComponent<Image>();
                    color = image.color;
                    image.color = new Color(color.r, color.g, color.b, 1f);
                    isHover = true;
                    audio.PlayOneShot(audio_button[0]);
                }
            }
            else
            {
                if (isHover)
                {
                    image = GetComponent<Image>();
                    color = image.color;
                    image.color = new Color(color.r, color.g, color.b, 0f);
                    isHover = false;
                }
            }
        }
    }

    private void HoverMenu()
    {
        //int i = (controller == "1P") ? i = 0 : i = 1;

        for (int i = 0; i < 2; i++)
        {
            Vector3 position = pointers[i].transform.position;
            //Debug.Log("x: " + (transform.position.x + xMax));
            if (position.x <= (transform.position.x + xMax) && position.x >= (transform.position.x + xMin) && position.y <= (transform.position.y + yMax) && position.y >= (transform.position.y + yMin))
            {

                if (!playerHover[i])
                {
                    playerHover[i] = true;
                    GetComponent<AudioSource>().PlayOneShot(audio_button[0]);

                    image = GetComponent<Image>();
                    color = image.color;
                    image.color = (i == 0) ? new Color(124 / 255f, 0, 0, 1f) : new Color(8 / 255f, 25 / 255f, 112 / 255f, 1f);
                }
            }
            else
            {
                if (playerHover[i])
                {
                    image = GetComponent<Image>();
                    color = image.color;
                    image.color = new Color(color.r, color.g, color.b, 0f);
                    playerHover[i] = false;
                }
            }
        }
        
    }


    private void HoverOption()
    {
        //int i = (controller == "1P") ? i = 0 : i = 1;

        for (int i = 0; i < 2; i++)
        {
            Vector3 position = pointers[i].transform.position;
            //Debug.Log("x: " + (transform.position.x + xMax));
            if (position.x <= (transform.position.x + xMax) && position.x >= (transform.position.x + xMin) && position.y <= (transform.position.y + yMax) && position.y >= (transform.position.y + yMin))
            {

                if (!playerHover[i])
                {
                    playerHover[i] = true;
                    audio.PlayOneShot(audio_button[0]);

                    image = GetComponent<Image>();
                    color = image.color;
                    image.color = (i == 0) ? new Color(124 / 255f, 0, 0, 1f) : new Color(8 / 255f, 25 / 255f, 112 / 255f, 1f);
                }
            }
            else
            {
                if (playerHover[i])
                {
                    image = GetComponent<Image>();
                    color = image.color;
                    image.color = new Color(255, 160 /255f, 2 / 255f, 1f);
                    playerHover[i] = false;
                }
            }
        }

    }

    public void ExecuteOption(string controller, string option)
    {

        Debug.Log("Option: " + option + " Controller: " + controller);
        if (pause.getPlayerPause() == controller)
        {
            audio.PlayOneShot(audio_button[1]);
            Debug.Log("Option: " + option);
            switch (option)
            {
                case "continue":
                    pause.setPause(controller);
                    break;
                case "restart":
                    //engine.getLevel.
                    Time.timeScale = 1f;
                    PlayerPrefs.SetInt("Level", engine.getLevel());
                    Application.LoadLevel(2);
                    break;
                case "controls":
                    pause.getPauseMenu().SetActive(false);
                    pause.getControlMenu().SetActive(true);
                    pause.getControlMenu().GetComponent<Menu>().setMenu(controller);
                    break;
                case "back":
                    Debug.Log("Pressed back controller " + controller);
                    pause.getControlMenu().SetActive(false);
                    pause.getPauseMenu().SetActive(true);
                    break;
                case "yes":
                    Time.timeScale = 1f;
                    Application.LoadLevel(1);
                    break;
                case "no":
                    pause.getExitMenu().SetActive(false);
                    pause.getPauseMenu().SetActive(true);
                    break;
                case "exit":
                    pause.getPauseMenu().SetActive(false);
                    pause.getExitMenu().SetActive(true);
                    pause.getExitMenu().GetComponent<Menu>().setMenu(controller);
                    break;
            }
        }
        else if (engine.getActivePlayers() == 0/* && option == "exit"*/)
        {
            switch (option)
            {
                case "exit":
                    Application.LoadLevel(1);
                    break;
                case "retry":
                    Application.LoadLevel(engine.getLevel());
                    break;
                default:
                    Application.LoadLevel(1);
                    break;
            }
            //Application.LoadLevel(1);
            //Application.Quit();   
        }
        else if (clear.getClear())
        {
            switch (option)
            {
                case "continue_nextlevel":
                    level.DoFade();
                    StartCoroutine(Delay());
                    //Application.LoadLevelAsync(4);
                    break;
                default:
                    level.DoFade();
                    StartCoroutine(Delay());
                    break;
            }
        }
        else
            audio.PlayOneShot(audio_button[2]);

    }

    public void ExecuteOptionAreaClear(string option)
    {
        if (PlayerPrefs.GetInt("sound") == 1)
            audio.PlayOneShot(audio_button[1]);
        
        Debug.Log("Option: " + option);
        
        switch (option)
        {
            case "continue_nextlevel":
                Debug.Log("Cargo el nivel");
                //Application.LoadLevel(5);
                break;
        }
        
    }


   public void OptionMainMenu(MainMenuEngine engine, string option)
    {

        OptionSelection selector;

        switch (option)
        {
            case "story":
                PlayerPrefs.SetInt("Level", 3);
                PlayerPrefs.SetString("loading", "history");
                Application.LoadLevel(2);
                break;
            case "survivor":
                PlayerPrefs.SetInt("Level", 4);
                PlayerPrefs.SetString("loading", "survivor");
                Application.LoadLevel(2);
                break;
            case "multiplayer":
                break;
            case "options":
                //menu.setPressedOption(true);
                menu.setCoroutine(null);
                menu.activateOptions(false);
                Debug.Log("Estoi en options");
                break;
            case "exit":
                Application.Quit();
                break;
            case "exit_options":
                //menu.setPressedOption(true);
                menu.fade.setFade(-1);
                menu.fade.setSpeed(0.5f);
                menu.setCoroutine(null);
                menu.activateOptions(true);
                menu.activateOptionsDefault(false);
                break;
            case "game_settings":
                //menu.setPressedOption(true);
                menu.activateOptionsSettings(false);
                break;
            case "control_settings":
                //menu.setPressedOption(true);
                menu.activateOptionsController(false);
                break;
            case "default":
                //menu.setPressedOption(true);
                menu.activateOptionsDefault(true);
                break;
            case "yes_default":
                menu.DefaultSettings();
                menu.activateOptionsDefault(false);
                break;
            case "no_default":
                //menu.setPressedOption(true);
                menu.activateOptionsDefault(false);
                break;
            case "less_difficulty":
                selector = transform.parent.GetComponent<OptionSelection>();
                selector.Decrease();
                break;
            case "more_difficulty":
                selector = transform.parent.GetComponent<OptionSelection>();
                selector.Increase();
                break;
            case "less_continue":
                selector = transform.parent.GetComponent<OptionSelection>();
                selector.Decrease();
                break;
            case "more_continue":
                selector = transform.parent.GetComponent<OptionSelection>();
                selector.Increase();
                break;
            case "sound":
                selector = transform.parent.GetComponent<OptionSelection>();
                selector.Decrease();
                break;
            case "less_graphics":
                selector = transform.parent.GetComponent<OptionSelection>();
                selector.Decrease();
                break;
            case "more_graphics":
                selector = transform.parent.GetComponent<OptionSelection>();
                selector.Increase();
                break;
            case "less_language":
                selector = transform.parent.GetComponent<OptionSelection>();
                selector.Decrease();
                break;
            case "more_language":
                selector = transform.parent.GetComponent<OptionSelection>();
                selector.Increase();
                break;
            case "back_options_game":
                //menu.setPressedOption(true);
                menu.activateOptionsSettings(true);
                break;
            case "back_options_controller":
                //menu.setPressedOption(true);
                menu.activateOptionsController(true);
                break;
        }

    }

   public void ExecuteOptionPath(string option)
   {

       switch (option)
       {
           case "left":
               level.setDirectionNode(option);
               level.MoveNode();
               break;
           case "right":
               level.setDirectionNode(option);
               level.MoveNode();
               break;
       }
   }

    public IEnumerator ExecuteOptionMainMenu(GameObject obj, string option, float delay)
    {
        int size = obj.transform.childCount;

        if (!menu.getPressedOption())
        {
            menu.setPressedOption(true);
            for (int i = 0; i < size; i++)
            {
                obj.transform.GetChild(i).gameObject.SetActive(true);
                audio.PlayOneShot(audio_button[1]);
                yield return new WaitForSeconds(delay);
            }

            if (option != "multiplayer")
            {
                Debug.Log("Ejecuto el Fade");
                menu.fade.setFade(1);
                menu.fade.setSpeed(1f);


                yield return new WaitForSeconds(this.delay);
                OptionMainMenu(menu, option);

                for (int i = 0; i < size; i++)
                    obj.transform.GetChild(i).gameObject.SetActive(false);
            }

           menu.setPressedOption(false);

        }
    
    }


    public void setPlayer(string controller)
    {
        this.controller = controller;
    }

    private void CheckAudio()
    {
        bool isMute = (PlayerPrefs.GetInt("sound") == 0) ? true : false;

        AudioSource[] audio = FindObjectsOfType<AudioSource>();

        for (int i = 0; i < audio.Length; i++)
            audio[i].mute = isMute;
    }


    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        Application.LoadLevelAsync(5);
    }

    void OnEnable()
    {
        CheckAudio(); 
    }

}

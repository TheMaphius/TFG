using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextContinue : MonoBehaviour {

    public string controller;
    public GameObject obj_credit;
    
    private GameObject obj_engine;
    private Engine engine;
    private GameOverEngine gameover;
    private Image image;
    private Image img_credit;
    private Sprite[] numbers;
    private Sprite[] img_gameover;
    //private Text text;
    private int credits;
    private bool dead = false;

	// Use this for initialization
	void Start () {
        obj_engine = GameObject.FindGameObjectWithTag("Engine");
        engine = obj_engine.GetComponent<Engine>();
        gameover = obj_engine.GetComponent<GameOverEngine>();
        
        image = GetComponent<Image>();
        numbers = Resources.LoadAll<Sprite>("Textures/Text/credit_num");
        img_credit = gameObject.transform.GetChild(0).GetComponent<Image>();
        img_gameover = Resources.LoadAll<Sprite>("Textures/Text/press_gameover");

        credits = engine.getCredits();
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*
         *  int idx;

            switch (PlayerPrefs.GetString("language"))
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
                default:
                    idx = 1;
                    break;
            }
            image.overrideSprite = img_gameover[idx];
         * 
         */
        if(!dead)
        {
            //Debug.Log("Soy el controller " + controller + " credits " + engine.getCredits());
            if (engine.getCredits() == 0)
            {
                
                int idx;

                switch (PlayerPrefs.GetString("language"))
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
                    default:
                        idx = 1;
                        break;
                }
                dead = true;
                Debug.Log("Index GO: " + idx);
                image.overrideSprite = img_gameover[idx];
                obj_credit.SetActive(false);
            }
            else if (gameover.getPlayerGameOver(controller))
            {
                //text.text = "GAME OVER";
                int idx;

                switch (PlayerPrefs.GetString("language"))
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
                    default:
                        idx = 1;
                        break;
                }
                dead = true;
                Debug.Log("Index GO: " + engine.getCredits());
                engine.setCredits(-engine.getCredits());
                image.overrideSprite = img_gameover[idx];
                obj_credit.SetActive(false);
            }
            else if (engine.getCredits() != credits && engine.getCredits() > 0)
            {
                Debug.Log("Entrare aki???");
                credits = engine.getCredits();
                img_credit.overrideSprite = numbers[credits];
                //text.text = "Press PAUSE to continue credit(s) " + credits;
            }
            else if (!gameover.getPlayerAlive(controller))
                img_credit.overrideSprite = numbers[credits];
                //text.text = "Press PAUSE to continue credit(s) " + credits;
        }
	}
}

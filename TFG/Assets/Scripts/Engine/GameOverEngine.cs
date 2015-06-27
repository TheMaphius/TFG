using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameOverEngine : MonoBehaviour {

    public GameObject gameover;
    public GameObject[] gameover_player;
    public GameObject[] player;
    public GameObject[] health;

    public GameObject area_clear;

    public AudioEngine audio_engine;

    private int num_players;
    private int[] idx_countdown;

    private Engine engine;
    private Sprite[] numbers;
    private IEnumerator[] coroutine;
    private GameObject[] obj_gameover;
    
    private bool isGameOver = false;
    private Dictionary<string, bool> player_alive;
    private Dictionary<string, bool> player_over;


    void Awake()
    {
        player_alive = new Dictionary<string, bool>();
        player_alive.Add("1P", true);
        player_alive.Add("2P", true);

        player_over = new Dictionary<string, bool>();
        player_over.Add("1P", false);
        player_over.Add("2P", false);

    }


	// Use this for initialization
	void Start () 
    {
        coroutine = new IEnumerator[2];
        idx_countdown = new int[2];

        for (int i = 0; i < idx_countdown.Length; i++)
            idx_countdown[i] = 9;

        numbers = Resources.LoadAll<Sprite>("Textures/Menu/GameOver/gameover_numbers");
        engine = GetComponent<Engine>();
        num_players = engine.getActivePlayers();
        engine.setPlayerAlive(player_alive);
	}

    void Update()
    {
        int players = engine.getActivePlayers();

        if (players != num_players)
        {
            //Debug.Log("Players: " + players);
            num_players = engine.getActivePlayers();

            if (players > 0)
            {
                // DO SOMETHING.
                for (int i = 0; i < 2; i++)
                {
                    string controller = (i == 0) ? controller = "1P" : controller = "2P";

                    if (coroutine[i] != null && !player_over[controller])
                    {
                        StopCoroutine(coroutine[i]);

                        if (gameover.activeInHierarchy)
                            gameover.SetActive(false);
                        else
                            for (int j = 0; j < gameover_player.Length; j++)
                                if (gameover_player[j].activeInHierarchy)
                                    gameover_player[j].SetActive(false);

                        if (engine.getCredits() > 0 && !player_over[controller])
                        {
                            GameContinue(i, controller);
                        }
                        else
                        {
                            GameOver(i, controller);
                        }
                    }
                }

                
            }
            /*else
                Debug.Log("Hola que ase");*/

        }
        
    }

    public void Continue(string controller)
    {
        int target = (controller == "1P") ? target = 0 : target = 1;

        idx_countdown[target] = 9;

        if (engine.getCredits() > 0)
        {
            StopCoroutine(coroutine[target]);
            coroutine[target] = null;
            player_over[controller] = false;
            engine.setCredits(-1);

            if (engine.getActivePlayers() < 2)
                gameover.SetActive(false);
            else
                gameover_player[target].SetActive(false);

            activateElements(target, true);

            player_alive[controller] = true;
            engine.setPlayerAlive(player_alive);
            health[target].GetComponent<Image>().fillAmount = 1f;
        }
        else
            GameOver(target, controller);
    }

    public void GameContinue(int target, string controller)
    {
        GameObject countdown = null;

        if (engine.getActivePlayers() < 2)
        {
            
            int size = gameover.transform.childCount;

            gameover.GetComponent<Menu>().setMenuGameOver(controller);
            obj_gameover = new GameObject[size];
            
            for (int i = 0; i < size; i++)
            {
                obj_gameover[i] = gameover.transform.GetChild(i).gameObject;

                if (obj_gameover[i].transform.name == "countdown")
                    countdown = obj_gameover[i];
                if (obj_gameover[i].transform.name == "gameover")
                    obj_gameover[i].SetActive(false);
            }
            gameover.SetActive(true);
            StartCoroutine(coroutine[target] = CountDown(idx_countdown[target], target, controller, countdown.GetComponent<Image>(), numbers));
        }
        else
        {
            int size = gameover.transform.childCount;

            gameover_player[target].GetComponent<Menu>().setMenuGameOver(controller);
            obj_gameover = new GameObject[size];

            for (int i = 0; i < size; i++)
            {
                obj_gameover[i] = gameover_player[target].transform.GetChild(i).gameObject;

                if (obj_gameover[i].transform.name == "countdown")
                    countdown = obj_gameover[i];
                if (obj_gameover[i].transform.name == "gameover")
                    obj_gameover[i].SetActive(false);
            }

            StartCoroutine(coroutine[target] = CountDown(idx_countdown[target], target, controller, countdown.GetComponent<Image>(), numbers));
            gameover_player[target].SetActive(true);
        }
    }

    void GameOver(int target, string controller)
    {

        GameObject obj = null;

        if (engine.getActivePlayers() < 2/* && engine.getCredits() > 0*/)
        {
            
            int size = gameover.transform.childCount;
            
            gameover.GetComponent<Menu>().setMenuGameOver(controller);
            obj_gameover = new GameObject[size];

            for (int i = 0; i < size; i++)
            {
                obj_gameover[i] = gameover.transform.GetChild(i).gameObject;

                if (obj_gameover[i].transform.name == "gameover")
                {
                    obj = obj_gameover[i];
                    obj_gameover[i].SetActive(true);
                }
                else
                    obj_gameover[i].SetActive(false);
            }

            gameover.SetActive(true);
            StartCoroutine(FadeGameOver(controller, target, obj));
            /*player_over[controller] = true;
            engine.setActivePlayers(-1);*/

        }
        else if(engine.getActivePlayers() > 1/* && engine.getCredits() > 0*/)
        {
            int size = gameover.transform.childCount;

            gameover_player[target].GetComponent<Menu>().setMenuGameOver(controller);
            obj_gameover = new GameObject[size];

            for (int i = 0; i < size; i++)
            {
                obj_gameover[i] = gameover_player[target].transform.GetChild(i).gameObject;

                if (obj_gameover[i].transform.name == "gameover")
                {
                    obj = obj_gameover[i];
                    obj_gameover[i].SetActive(true);
                }
                else
                    obj_gameover[i].SetActive(false);
            }

            gameover_player[target].SetActive(true);
            StartCoroutine(FadeGameOver(controller, target, obj));
            /*player_over[controller] = true;
            engine.setActivePlayers(-1);*/
        }

    }

    public void setGameOver(bool gameover)
    {
        isGameOver = gameover;
    }

    public bool getGameOver()
    {
        return isGameOver;
    }

    public void InsertCoin(int player)
    {
        string controller = (player + 1) + "P";
        //player_over[controller] = true;
        //Debug.Log("Credits: " + engine.getCredits());
        //activateElements(player, false);

        if (!player_over[controller])
        {
            player_alive[controller] = false;
            engine.setPlayerAlive(player_alive);
            //player_over[controller] = true;
            activateElements(player, false);

            if (engine.getCredits() > 0)
                GameContinue(player, controller);
            else
                GameOver(player, controller);
        }
    }

    public bool getPlayerAlive(string controller)
    {
        return player_alive[controller];
    }

    public bool getPlayerGameOver(string controller)
    {
        return player_over[controller];
    }

    void activateElements(int target, bool active)
    {

        int size = player[target].transform.childCount;

        for (int i = 0; i < player[target].transform.childCount; i++)
        {
            GameObject obj = player[target].transform.GetChild(i).gameObject;
            if (obj.name == "Continue")
                obj.SetActive(!active);
            else if (obj.name == "Reload" && active == true)
                obj.SetActive(!active);
            else if (obj.name != "Pointer_" + (target + 1) + "P")
                obj.SetActive(active);

        }
    }

    private IEnumerator CountDown(int begin, int target, string controller, Image countdown, Sprite[] number)
    {

        for (int i = begin; i >= 0; i--)
        {
            if (engine.getCredits() > 0)
            {
                idx_countdown[target] = i;
                countdown.overrideSprite = number[i];
                yield return new WaitForSeconds(1f);
            }
            else
                break;

        }
        player_over[controller] = true;
        idx_countdown[target] = 0;
        GameOver(target, controller);
    }

    private IEnumerator FadeGameOver(string controller, int target, GameObject text_gameover)
    {
        yield return new WaitForSeconds(2f);
        text_gameover.SetActive(false);
        GameObject.FindGameObjectWithTag("Avatar_" + controller).SetActive(false);
        //Debug.Log("GameOverEngine " + controller);
        GameObject obj = player[target];

        int size = obj.transform.childCount;

        for (int i = 0; i < size; i++)
        
            if (obj.transform.GetChild(i).name == "Continue")
                obj.transform.GetChild(i).gameObject.SetActive(true);
            
            else   
                obj.transform.GetChild(i).gameObject.SetActive(false);
         
        engine.setActivePlayers(-1);

        if (engine.getActivePlayers() == 0)
        {
            area_clear.SetActive(true);
            GetComponent<AreaClearEngine>().UpdateStats();
            audio_engine.Stop();
            audio_engine.GameOverAudio();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SurvivorEngine : Engine {

    private enum enumDirection // Directions of Player.
    {
        Front = 0,
        Right = 1,
        Back = 2,
        Left = 3,
    };

    public AudioEngine audio_engine;

    public GameObject panel_horde;
    public GameObject[] horde_digit;

    private Sprite[] numbers;

    private enumDirection direction;
    private AudioClip[] direction_audio;

    private int points_area;
    private GameObject[] points;        // Points reborn zombies.
    private Dictionary<string, GameObject> scene_points;

    private GameObject[] zombie;       

    private int player = 0;             // Number of players.
    private int credits = 0;
    private Dictionary<string, bool> player_alive;
    private Dictionary<string, int> player_score;
    private Dictionary<string, int> player_kills;
    private Dictionary<string, int> player_shoot;
    private Dictionary<string, int> player_touch;
    private Dictionary<string, int> player_combo;

    private int horde = 1;              // Number of horde.
    private int area_change = 0;            // Parameter that especify when change the actual area;

    private int zombies = 0;            // Number of zombie defeats.
    private int zombies_area = 0;       // Number of zombies per direction.
    private int zombies_horde = 0;      // Zombies per round.


    private enumDirection last_direction;
    private int zombies_ingame = 0;
    private int zombies_area_defeat = 0;
    private int zombies_horde_defeat = 0;

    private float difficulty;           // Difficulty of the level.

    // Parameters to calculate the number of zombies per area and horde.
    private int last_random;
    private float exp = 1;


    // Level id.
    private int level = 4;

    private GameOverEngine gameover;

    /// <summary>
    /// Use this for priority initialization.
    /// </summary>
    void Awake()
    {
        //camera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        points = GameObject.FindGameObjectsWithTag("Points");
        
        zombie = new GameObject[6];
        zombie[0] = Resources.Load<GameObject>("Prefabs/Enemies/Zombies/zombie_cop");
        zombie[1] = Resources.Load<GameObject>("Prefabs/Enemies/Zombies/zombie_girl_01");
        zombie[2] = Resources.Load<GameObject>("Prefabs/Enemies/Zombies/zombie_girl_02");
        zombie[3] = Resources.Load<GameObject>("Prefabs/Enemies/Zombies/zombie_litle_girl");
        zombie[4] = Resources.Load<GameObject>("Prefabs/Enemies/Zombies/zombie_man_01");
        zombie[5] = Resources.Load<GameObject>("Prefabs/Enemies/Zombies/zombie_man_02");

        direction_audio = new AudioClip[4];
        direction_audio[0] = Resources.Load<AudioClip>("Music/Scene/Survivor/turn_front");
        direction_audio[1] = Resources.Load<AudioClip>("Music/Scene/Survivor/turn_right");
        direction_audio[2] = Resources.Load<AudioClip>("Music/Scene/Survivor/turn_back");
        direction_audio[3] = Resources.Load<AudioClip>("Music/Scene/Survivor/turn_left");

        numbers = Resources.LoadAll<Sprite>("Textures/Menu/AreaClear/numbers_round");

        player_score = new Dictionary<string, int>();
        player_score.Add("1P", 0);
        player_score.Add("2P", 0);

        player_kills = new Dictionary<string, int>();
        player_kills.Add("1P", 0);
        player_kills.Add("2P", 0);

        player_shoot = new Dictionary<string, int>();
        player_shoot.Add("1P", 0);
        player_shoot.Add("2P", 0);

        player_touch = new Dictionary<string, int>();
        player_touch.Add("1P", 0);
        player_touch.Add("2P", 0);

        player_combo = new Dictionary<string, int>();
        player_combo.Add("1P", 0);
        player_combo.Add("2P", 0);
    }


    void Start()
    {
        audio_engine.BackgroundAudio();
        scene_points = new Dictionary<string, GameObject>();

        for (int i = 0; i < points.Length; i++)
            scene_points[points[i].transform.name] = points[i];

        points_area = (int)points.Length / 4;

        //Debug.Log("!!!Points Area: " + points_area);

        direction = enumDirection.Front;
        last_direction = direction;
        difficulty = 1;//PlayerPrefs.GetInt("difficulty");
        //Debug.Log("Direction: " + direction);
        StartCoroutine(HordeDelay());

        
        /*foreach (var point in scene_points)
        {
            Debug.Log(point.Key + ":" + point.Value);
        }*/

    }


    /*void Update()
    {

        Debug.Log("Active Players: " + player);
    }*/
    

    public override int getLevel()
    {
        return level;
    }


    /***************************************************************************/
    /****                   Engine methods of Survivor                     *****/
    /***************************************************************************/

    void InitializeHorde()
    {

        byte random = (byte)Random.Range(5, 11);
        last_random = random;

        zombies_horde = Mathf.RoundToInt(random * Mathf.Exp(difficulty));

        /*Debug.Log("IH Random: " + random);
        Debug.Log("Horde: " + horde + " Zombies: " + zombies_horde);*/

        InitializeArea();
    }

    void InitializeArea()
    {
        int area = (zombies_horde - area_change) + 1;
        /*Debug.Log("Area Left: " + (area - 1));
        Debug.Log("Zombies_horde: " + zombies_horde);
        Debug.Log("Last_Area_Change: " + area_change);*/
        int change = Random.Range(1, area);
        area_change += change;
        /*Debug.Log("Change: " + change);
        Debug.Log("Area Change: " + area_change);*/
        //direction = (enumDirection)(Random.Range(0, 4));
        //Debug.Log("Direction: " + direction);
        setZombiesArea(change);
   
        List<int> list_points = new List<int>();
        int num_zombies;

        if (change >= points_area)
            num_zombies = points_area;
        else
            num_zombies = change % points_area;

        int init = points_area * (int)direction;
        int end = (points_area * (int)direction) + points_area;

        for (int i = init; i < end; i++)
            list_points.Add(i);

        /*Debug.Log("Points area: " + points_area);
        Debug.Log("List Points: " + list_points.Count);*/
        for (int i = 0; i < num_zombies; i++)
        {
            int random_point = Random.Range(0, list_points.Count);
            //Debug.Log("IA Random_points: " + random_point);
            //int random_area_point = random_point + (points_area * (int)direction);
            StartCoroutine(generateEnemy(list_points[random_point]));
            list_points.RemoveAt(random_point);
        }

        
    }


    /// <summary>
    /// Setter the difficulty of round.
    /// </summary>
    void setDifficulty(float difficulty)
    {
        this.difficulty = difficulty;
    }


    /// <summary>
    /// Getter the difficulty value.
    /// </summary>
    /// <returns>Value of difficulty</returns>
    public override float getDifficulty()
    {
        return this.difficulty;
    }


    /// <summary>
    /// Setter the number of round.
    /// </summary>
    public void setHorde()
    {
        horde++;
    }


    /// <summary>
    /// Getter the number of round.
    /// </summary>
    /// <returns>Value of round</returns>
    public int getHorde()
    {
        return horde;
    }


    /// <summary>
    /// Setter the credits of players. Add 'continues' to actual player.
    /// </summary> 
    /// <param name="controller">Parameter with the name of player.</param>
    public override void setCredits(int credits)
    {
        this.credits += credits;
        //Debug.Log("Player: " + controller + "has " + credits + " credits.");
    }


    /// <summary>
    /// Getter the number of credits of player.
    /// </summary>
    /// <param name="controller">Controller of player.</param>
    /// <returns>Number of credits</returns>
    public override int getCredits()
    {
        return credits;
    }


    /***************************************************************************/
    /****                    Engine methods of Players                     *****/
    /***************************************************************************/


    /// <summary>
    /// Setter the number of players activated. 
    /// Calculate the number of player are activated in the scene
    /// </summary> 
    /// <param name="active">Parameter with number +1 or -1.</param>
    public override void setActivePlayers(int active)
    {
        player += active;
    }


    /// <summary>
    /// Getter the number of active player.
    /// </summary>
    /// <returns>Player active</returns>
    public override int getActivePlayers()
    {
        return player;
    }

    public override void setPlayerAlive(Dictionary<string,bool> player_alive)
    {
        this.player_alive = player_alive;
    }

    public override Dictionary<string, bool> getPlayerAlive()
    {
        return player_alive;
    }

    public override void setPlayerScore(string controller, int score)
    {
        this.player_score[controller] += score;
    }

    public override int getPlayerScore(string controller)
    {
        return this.player_score[controller];
    }

    public override void setZombiePlayerKills(string controller)
    {
        this.player_kills[controller]++;
    }

    public override int getZombiePlayerKills(string controller)
    {
        return this.player_kills[controller];
    }

    public override void setPlayerShoot(string controller)
    {
        this.player_shoot[controller]++;
    }

    public override int getPlayerShoot(string controller)
    {
        return this.player_shoot[controller];
    }

    public override void setPlayerTouch(string controller)
    {
        this.player_touch[controller]++;
    }

    public override int getPlayerTouch(string controller)
    {
        return this.player_touch[controller];
    }

    public override void setPlayerCombo(string controller, int combo)
    {
        if (this.player_combo[controller] < combo)
            this.player_combo[controller] = combo;
    }

    public override int getPlayerCombo(string controller)
    {
        return this.player_combo[controller];
    }



    /***************************************************************************/
    /****                    Engine methods of Enemies                     *****/
    /***************************************************************************/


    public override void ZombieEngine(int zombie)
    {

        //setZombiesDefeat();
        zombies++;
        zombies_horde_defeat++;
        zombies_area_defeat++;

        //setZombiesArea(zombie);
        //setZombiesRound(zombie);
        /*Debug.Log("Zombies defeat: " + zombies);
        Debug.Log("Area Zombies Defeat: " + zombies_area_defeat);
        Debug.Log("***Zombies Area: " + zombies_area);
        Debug.Log("***Zombies ingame: " + zombies_ingame);
        Debug.Log("***Zombies Horde: " + zombies_horde);
        Debug.Log("***Zombies Horde defeat: " + zombies_horde_defeat);*/
        Debug.Log("Zombies defeat: " + zombies);

        if (/*zombie == -1 && */(zombies_ingame < zombies_area))
        {
            //Debug.Log("Vamos a generar un nuevo zombie");
            int random_point = Random.Range(0, points_area);
            int random_area_point = random_point + (points_area * (int)direction);
            //Debug.Log("Genero el zombie en el point: " + random_area_point);
            StartCoroutine(generateEnemy(random_area_point));
        }
        else 
        {
            //Debug.Log("Finish Area.");
            /*Debug.Log("Zombie_horde: " + actual_zombies_area + " Actual_area: " + actual_zombies_area);
            if (zombies_horde == actual_zombies_horde)
            {
                Debug.Log("Finish Horde.");
                actual_zombies_area = 0;
                actual_zombies_horde = 0;
                //InitializeHorde();
            }
            else*/
            if (zombies_horde == zombies_horde_defeat)
            {
                Debug.Log("Finish Horde");
                area_change = 0;
                zombies_ingame = 0;
                zombies_area_defeat = 0;
                zombies_horde_defeat = 0;
                this.difficulty += .5f;
                setHorde();
                StartCoroutine(HordeDelay());
            }
            else if (zombies_area == zombies_area_defeat)
            {
                //zombies = 0;
                zombies_ingame = 0;
                zombies_area_defeat = 0;
                Debug.Log("Finish Area.");
                StartCoroutine(DirectionDelay());
                //actual_zombies_area = 0;
                //InitializeArea();
            }

            
        }


       /* if (zombies_horde == actual_zombies_horde)
        {
            Debug.Log("Finish Horde.");
            actual_zombies_area = 0;
            actual_zombies_horde = 0;
            //InitializeHorde();
        }
        else if (zombies_area == actual_zombies_area)
        {
            Debug.Log("Finish Area.");
            actual_zombies_area = 0;
            //InitializeArea();
        }*/
    }

    /// <summary>
    /// Setter the number of enemies. 
    /// Generate a enemy if the value pass is negative.
    /// </summary> 
    /// <param name="enemy">Parameter with number +1 or -1.</param>
    public void setZombiesDefeat()
    {
        zombies++;  
    }

    public void setZombiesArea(int zombie)
    {
        this.zombies_area = zombie;
    }

    public void setZombiesRound(int zombie)
    {
        this.zombies_horde = zombie;
    }


    /// <summary>
    /// Getter the number of enemies on the scene.
    /// </summary>
    /// <returns>Number of enemies</returns>
    public int getNumZombiesDefeat()
    {
        return zombies;
    }


    public int getZombiesArea()
    {
        return zombies_area;
    }


    public int getZombiesHorde()
    {
        return zombies_horde;
    }

    /// <summary>
    /// Method that generate a new enemy when the game needs.
    /// </summary>
    private IEnumerator generateEnemy(int random_point)
    {
        int random_zombie = Random.Range(0, zombie.Length);
        //Debug.Log("RandPoint: " + random_point);
        string point = "Point" + random_point;//(random_point + (points_area * (int)direction));

        if (zombies_area_defeat < area_change)
        {
            GameObject enemy = Instantiate(zombie[random_zombie]) as GameObject;
            zombies_ingame++;
            //Debug.Log("*cccc**Point: " + point);
            enemy.transform.position = scene_points[point].transform.position;

            switch (direction.ToString())
            {
                case "Left":
                    enemy.transform.rotation = Quaternion.Euler(new Vector3(0, 0.5f));
                    //enemy.transform.Rotate(new Vector3(0, 270, 0));
                    break;
                case "Back":
                    enemy.transform.rotation = Quaternion.Euler(Vector3.zero);
                    //enemy.transform.Rotate(Vector3.zero);
                    break;
                case "Right":
                    enemy.transform.rotation = Quaternion.Euler(new Vector3(0, 0.25f));
                    //enemy.transform.Rotate(new Vector3(0, 90, 0));
                    break;
            }


            //actual_zombies_horde++;
            //actual_zombies_area++;
            //Debug.Log("AZH: " + zombies_horde_defeat + " AZA: " + zombies_area_defeat);
        }
        else { Debug.Log("Sigo entrando!!!!!!!!!!!!!!!!!!"); }

        yield return 0;

    }


    private IEnumerator HordeDelay()
    {
        int size = panel_horde.transform.childCount;
        Image image;

        if (horde < 10)
        {
            image = horde_digit[0].GetComponent<Image>();
            image.overrideSprite = numbers[horde];
        }
        else
        {
            image = horde_digit[0].GetComponent<Image>();
            image.overrideSprite = numbers[horde%10];
            image = horde_digit[1].GetComponent<Image>();
            image.overrideSprite = numbers[horde / 10];
        }

        FadeEffect fade;
        
        panel_horde.GetComponent<FadeEffect>().setFade(1);
        horde_digit[0].GetComponent<FadeEffect>().setFade(1);
        horde_digit[1].GetComponent<FadeEffect>().setFade(1);
        //fade.setFade(1);

        /*for (int i = 0; i < size; i++)
        {
            fade = panel_horde.transform.GetChild(i).GetComponent<FadeEffect>();
            fade.setFade(1);
        }*/

        yield return new WaitForSeconds(5f);

        panel_horde.GetComponent<FadeEffect>().setFade(-1);
        horde_digit[0].GetComponent<FadeEffect>().setFade(-1);
        horde_digit[1].GetComponent<FadeEffect>().setFade(-1);
        /*fade = panel_horde.GetComponent<FadeEffect>();
        fade.setFade(1);

        for (int i = 0; i < size; i++)
        {
            fade = panel_horde.transform.GetChild(i).GetComponent<FadeEffect>();
            fade.setFade(-1);
        }*/

        InitializeHorde();
        yield return new WaitForSeconds(1f);       
    }


    private IEnumerator DirectionDelay()
    {

        yield return new WaitForSeconds(2f);
        //StartCoroutine(generateEnemy());
        List<int> occupied_direction = new List<int>();

        for (int i = 0; i < 4; i++)
            occupied_direction.Add(i);

        occupied_direction.RemoveAt((int)direction);
        //Debug.Log("Direction ocuppied: " + occupied_direction.Count);
        int idx = Random.Range(0, occupied_direction.Count);
        last_direction = direction;
        direction = (enumDirection)occupied_direction[idx];
        //direction = (enumDirection)0;

        InitializeArea();
        //Debug.Log("New Direction: " + direction);
        audio.PlayOneShot(direction_audio[(int)direction]);

        int init_pos = (int)last_direction * 90;
        int end_pos = (int)direction * 90;
        int diff = end_pos - init_pos;
        //Debug.Log("init: " + init_pos + " end: " + end_pos + " diff: " + diff);

        int step = 3;

        if (diff == 270)
        {
            //Debug.Log("!!!!!!!!Entro aki left");
            for (int i = init_pos; i <= 90; i += step)
            {
                //Debug.Log("!!!!!!!!Entro aki left");
                Camera.main.transform.rotation = Quaternion.Euler(Vector3.up * -i);
                yield return 0;
            }
        }
        else if (diff == -270)
            for (int i = init_pos; i <= 360; i += step)
            {
                Camera.main.transform.rotation = Quaternion.Euler(Vector3.up * i);
                yield return 0;
            }
        else if (diff > 0)

            for (int i = init_pos; i <= end_pos; i += step)
            {
                Camera.main.transform.rotation = Quaternion.Euler(Vector3.up * i);
                yield return 0;
            }
        else if (diff < 0)
            for (int i = init_pos; i >= end_pos; i -= step)
            {
                Camera.main.transform.rotation = Quaternion.Euler(Vector3.up * i);
                yield return 0;
            }
 
        yield return 0;

    }



    /// <summary>
    /// When GameObject is destroying delete de info of PlayerPrefs.
    /// </summary>
    void OnApplicationQuit()
    {
        //PlayerPrefs.DeleteAll();
    }
}

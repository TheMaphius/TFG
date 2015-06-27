using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelEngine : Engine
{

    public AudioEngine audio_engine;
    public GameObject path;
    public Image panel_survivor;
    public GameObject area_clear;
    public AudioClip[] audios;
    public GameObject fade;
    public GameObject bosslife;

    private SelectPathEngine path_engine;

    private int idx_point = 0;  // <== Node_init
    private string str_point;
    //private GameObject[] points;
    private GameObject actual_point;
    private Dictionary<string, int> horda_zombies;

    private GameObject nodes;
    private GameObject actual_node;
    private GameObject aux_node;
    //private ArrayList node_list;

    private int offset = 3;             // <== Se puede eliminar.
    private string direction = "";          // <== Node direction.

    private int level = 3;
    private int player;             // Number of players.
    private int difficulty;
    private int credits;
    private Dictionary<string, bool> player_alive;
    private Dictionary<string, int> player_score;
    private Dictionary<string, int> player_kills;
    private Dictionary<string, int> player_shoot;
    private Dictionary<string, int> player_touch;
    private Dictionary<string, int> player_combo;

    private Dictionary<string, GameObject> list_points;

    private bool audioInit = false;

    //void Awake()
    //{
        /*nodes = GameObject.FindGameObjectWithTag("Node");

        list_points = new Dictionary<string, GameObject>();

        foreach (GameObject entry in GameObject.FindGameObjectsWithTag("Horda"))
            list_points[entry.name] = entry;
        

        horda_zombies = new Dictionary<string, int>();

        foreach (KeyValuePair<string, GameObject> entry in list_points)
            horda_zombies[entry.Key] = entry.Value.transform.childCount;

        actual_point = list_points["Point_1"];*/

    //}

    // Use this for initialization
    void Start()
    {

        path_engine = GetComponent<SelectPathEngine>();
        credits = PlayerPrefs.GetInt("continue");
        difficulty = PlayerPrefs.GetInt("difficulty");

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

        //bosslife.SetActive(false);
        audio_engine.BackgroundAudio();
        StartCoroutine(InitializeLevel());
        //StartCoroutine(NextNode());       <== Poner esto en Initialize Level01.
    }

    // Update is called once per frame
    void Update()
    {

    }




    public override int getLevel()
    {
        return level;
    }

    /// <summary>
    /// Getter the difficulty value.
    /// </summary>
    /// <returns>Value of difficulty</returns>
    public override float getDifficulty()
    {
        return this.difficulty;
    }

    public override void setCredits(int credits)
    {
        this.credits += credits;
    }

    public override int getCredits()
    {
        return this.credits;
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

    public override void setPlayerAlive(Dictionary<string, bool> player_alive)
    {
        this.player_alive = player_alive;
    }

    public override Dictionary<string, bool> getPlayerAlive()
    {
        return player_alive;
    }

    public override void setPlayerScore(string controller, int score)
    {
        this.player_score[controller] = score;
        Debug.Log("Player: " + controller + " Score: " + score);
    }

    public override int getPlayerScore(string controller)
    {
        return this.player_score[controller];
    }

    public override void setZombiePlayerKills(string controller)
    {
        this.player_kills[controller]++;
        Debug.Log("Player : " + controller + " Kills: " + this.player_kills[controller]);
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
        Debug.Log("Player : " + controller + " Combo: " + this.player_combo[controller]);
    }

    public override int getPlayerCombo(string controller)
    {
        return this.player_combo[controller];
    }


    /***************************************************************************/
    /****                    Engine methods and Enemies                     *****/
    /***************************************************************************/

    public void setPoint()
    {
        /*Debug.Log("What is the actual point: " + actual_point.name);
        Debug.Log("Actual Point: " + actual_point);*/
        //Debug.Log("Actual Node: " + actual_node.name);
        int size = actual_point.transform.childCount;
        Debug.Log("Size: " + actual_point.name);
        for (int i = 0; i < size; i++)
        {
            GameObject child_obj = actual_point.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject;

            child_obj.GetComponent<LoopAnimation>().enabled = false;

            if (actual_node.name != "Node_" + idx_point + "_survivor"
                && actual_node.name != "Node_" + idx_point + "_" + direction + "_survivor")
            {
                child_obj.GetComponent<AI>().enabled = true;
                child_obj.GetComponent<Behaviour>().enabled = true;

            }
            else
            {
                StartCoroutine(showPanel());
                SurvivorAI survivor = child_obj.GetComponent<SurvivorAI>();
                if (survivor != null) survivor.enabled = true;
                ZombieSurvivor zombie = child_obj.GetComponent<ZombieSurvivor>();
                if (zombie != null) zombie.enabled = true;
            }

            child_obj.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;

        }

    }

    public override void ZombieEngine(int zombie)
    {
        horda_zombies[str_point] += zombie;
        Debug.Log("Point " + str_point + " zombies: " + horda_zombies[str_point]);
        if (horda_zombies[str_point] == 0)
        {
            Debug.Log("Clear Area");

            /*for (int i = 0; i < points.Length; i++)
            
                if (points[i].name == "Point_" + (idx_point + offset))
                    points[i].SetActive(true);*/

            //StartCoroutine(hideNode());
            StartCoroutine(NextNode());
            //Camera.main.GetComponent<SplineController>().setSpeed(50);
            //Camera.main.GetComponent<SplineController>().setSpeed(50);

        }

    }

    public void MoveNode()
    {
        Camera.main.GetComponent<SplineController>().setSpeed(50);
        //Debug.Log("adafafasfa " + actual_node.name);
        aux_node.GetComponent<SplineNode>().setAction(0);
    }

    private IEnumerator NextNode()
    {
        int aux_point = idx_point;
        string other_dir = (direction == "left") ? "right" : "left";

        aux_node = actual_node;
        idx_point++;

        Debug.Log("Desde Next node : " + actual_point.name);
        foreach (KeyValuePair<string, GameObject> entry in list_points)
        {
            if (entry.Key != ("Point:" + idx_point + "_" + other_dir))
            {
                if (entry.Key == ("Point_" + idx_point) || entry.Key == ("Point_" + idx_point + "_" + direction)
                    || entry.Key == ("Node_" + idx_point + "_" + direction + "_End"))
                {
                    actual_point = entry.Value;
                    str_point = entry.Key;
                }
            }
        }

        StartCoroutine(ZombieInstance());
        /*for (int i = 0; i < points.Length; i++)
            if (points[i] != null)
            {
                if (points[i].name == "Point_" + idx_point || points[i].name == "Point_" + idx_point + "_" + direction)
                {
                    actual_point = points[i];
                    str_point = points[i].name;
                }
            }*/

        int childs = nodes.transform.childCount;

        for (int j = 0; j < childs; j++)
        {
            GameObject obj = nodes.transform.GetChild(j).gameObject;
            /*if (obj.name == "Node_" + idx_point || obj.name == "Node_" + idx_point + "_left"
                || obj.name == "Node_" + idx_point + "_right" || obj.name == "Node_" + idx_point + "_path"
                || obj.name == "Node_" + idx_point + "_survivor")*/
            if (obj.name == "Node_" + idx_point || obj.name == "Node_" + idx_point + "_" + direction
                || obj.name == "Node_" + idx_point + "_path" || obj.name == "Node_" + idx_point + "_survivor"
                || obj.name == "Node_" + idx_point + "_" + direction + "_survivor" || obj.name == "Node_" + idx_point + "_" + direction + "_End"
                || obj.name == ("Node_" + idx_point + "_End"))
            {
                actual_node = obj;
                break;
            }
        }

        if (!audioInit)
        {
            audioInit = true;
            audio.PlayOneShot(audios[1]);
            yield return new WaitForSeconds(audios[1].length);
        }
        else
            yield return new WaitForSeconds(2f);
        
        Debug.Log("***Desde Next node : " + "Node_" + idx_point + "_" + direction + "_End");
        Debug.Log("****Actual Node Point: " + actual_node.name);
        Debug.Log("****Actual Point: " + actual_point.name);
        if (idx_point > 1)
        {

            if (aux_node.name == "Node_" + aux_point + "_path")
            {
                path.SetActive(true);
                path_engine.setSelection(true);
                // Debug.Log("Me espero hasta que seleccione direccion");
            }
            else if (aux_node.name == ("Node_" + aux_point + "_survivor") || aux_node.name == ("Node_" + idx_point + "_" + direction + "_survivor"))
            {
                //Debug.Log("Entro akissaf");
                //StartCoroutine(showPanel());
            }
            else if (actual_node.name == ("Node_" + idx_point + "_" + direction + "_End") || actual_node.name == ("Node_" + idx_point + "_End"))
            {
                Debug.Log("aux_node: " + aux_node);
                area_clear.SetActive(true);
                audio_engine.Stop();
                audio_engine.GameOverAudio();
                GetComponent<AreaClearEngine>().setAreaClear(true);
                GetComponent<AreaClearEngine>().UpdateStats();
            }
            else
            {
                MoveNode();
            }
        }
        else
            Camera.main.GetComponent<SplineController>().setGo(true);

        //Debug.Log("Next point: " + idx_point);

    }

    private IEnumerator hideNode()
    {
        int aux = idx_point;

        foreach (KeyValuePair<string, GameObject> entry in list_points)
        {
            if (list_points[("Point_" + aux)] || list_points[("Point_" + idx_point + "_" + direction)])
            {
                yield return new WaitForSeconds(5f);
                entry.Value.SetActive(false);
                break;
            }
        }
        /*for (int i = 0; i < points.Length; i++)
        {
            //Debug.Log("Point " + (points[i].name != "Point_1" || points[i].name != "Point_2" || points[i].name != "Point_3"));
            if (points[i].name == "Point_" + aux)
            {
                yield return new WaitForSeconds(5f);
                points[i].SetActive(false);
                break;
            }
        }*/
    }

    private IEnumerator showPanel()
    {
        panel_survivor.GetComponent<FadeEffect>().setFade(1);
        yield return new WaitForSeconds(1);
        panel_survivor.GetComponent<FadeEffect>().setFade(-1);
    }

    public void setDirectionNode(string direction)
    {
        this.direction = direction;
        
        GameObject[] obj = GameObject.FindGameObjectsWithTag("BossPoints");
        
        for (int i = 0; i < obj.Length; i++)
            if (obj[i].name != "Boss_Points_" + direction)
                Destroy(obj[i]);

        findPoint("Point_" + idx_point + "_" + direction);
        findNode("Node_" + idx_point + "_" + direction);
        path_engine.setSelection(false);
        path.SetActive(false);
        StartCoroutine(destroyPoint());
    }

    private IEnumerator destroyPoint()
    {
        string other_dir = (direction == "left") ? "right" : "left";

        /*for (int i = 0; i < points.Length; i++)
        {
            int size = points[i].name.Length;
            string ext = "";

            if (other_dir == "left")
                ext = points[i].name.Substring((size - 4), 4);
            else
                ext = points[i].name.Substring((size - 5), 5);
            Debug.Log("Point Name; " + points[i].name + " Other Point_" + i + "_" + other_dir);
            if (ext == other_dir)
            {
                Debug.Log("***Voi a destruir: " + points[i].name);
                Destroy(points[i].gameObject);
            }
        }*/

        for (int i = 1; i < list_points.Count; i++)
        {
            if (list_points.ContainsKey("Point_" + i + "_" + other_dir))
                Destroy(list_points[("Point_" + i + "_" + other_dir)].gameObject);
        }

        yield return new WaitForSeconds(0);
    }

    private IEnumerator ZombieInstance()
    {
        int idx = offset + idx_point;

        if (list_points.ContainsKey("Point_" + idx))
        {
            int child = list_points[("Point_" + idx)].transform.childCount;
            GameObject obj = list_points[("Point_" + idx)].gameObject;

            for (int i = 0; i < child; i++)
            {
                obj.transform.GetChild(i).gameObject.GetComponent<EnemyInstance>().ZombieInstance();
                yield return new WaitForSeconds(.25f);
            }
            
        }

        if (direction == "")
        {
            if (list_points.ContainsKey("Point_" + idx + "_left"))
            {
                int child = list_points[("Point_" + idx + "_left")].transform.childCount;
                GameObject obj = list_points[("Point_" + idx + "_left")].gameObject;

                for (int i = 0; i < child; i++)
                {
                    // AKI *********************
                    //list_points[("Point_" + idx + "_left")].transform.GetChild(i).GetComponent<Animator>().enabled = true;
                    //list_points[("Point_" + idx + "_left")].transform.GetChild(i).GetComponent<LoopAnimation>().enabled = true;
                    //GameObject obj = list_points[("Point_" + idx + "_left")].transform.GetChild(i).gameObject;
                    obj.transform.GetChild(i).gameObject.GetComponent<EnemyInstance>().ZombieInstance();
                    yield return new WaitForSeconds(.25f);
                    //yield return null;
                }
            }

            if (list_points.ContainsKey("Point_" + idx + "_right"))
            {
                int child = list_points[("Point_" + idx + "_right")].transform.childCount;
                GameObject obj = list_points[("Point_" + idx + "_right")].gameObject;

                for (int i = 0; i < child; i++)
                {
                    //GameObject obj = list_points[("Point_" + idx + "_left")].transform.GetChild(i).gameObject;
                    obj.transform.GetChild(i).gameObject.GetComponent<EnemyInstance>().ZombieInstance();
                    yield return new WaitForSeconds(.25f);
                    //list_points[("Point_" + idx + "_right")].transform.GetChild(i).GetComponent<Animator>().enabled = true;
                    //list_points[("Point_" + idx + "_right")].transform.GetChild(i).GetComponent<LoopAnimation>().enabled = true;
                    //yield return new WaitForSeconds(0);
                }
            }
        }
        else
        {
            Debug.Log("Estoi en el Point_" + idx + "_" + direction);
            if (list_points.ContainsKey("Point_" + idx + "_" + direction))
            {
                int child = list_points[("Point_" + idx + "_" + direction)].transform.childCount;
                GameObject obj = list_points[("Point_" + idx + "_" + direction)].gameObject;
                Debug.Log("***Estoi en el Point_" + idx + "_" + direction);
                for (int i = 0; i < child; i++)
                {
                    //GameObject obj = list_points[("Point_" + idx + "_" + direction)].transform.GetChild(0).transform.GetChild(i).gameObject;
                    obj.transform.GetChild(i).gameObject.GetComponent<EnemyInstance>().ZombieInstance();
                    yield return new WaitForSeconds(.25f);
                    /*list_points[("Point_" + idx + "_" + direction)].transform.GetChild(i).GetComponent<Animator>().enabled = true;
                    list_points[("Point_" + idx + "_" + direction)].transform.GetChild(i).GetComponent<LoopAnimation>().enabled = true;*/
                    
                }
            }
            /*if (list_points.ContainsKey("Point_" + idx + "_" + direction))
                list_points[("Point_" + idx + "_" + direction)].SetActive(true);*/
        }

    }

    /*private IEnumerator ZombieInstance()
    {
        int size = actual_point.transform.childCount;

        for (int i = 0; i < size; i++)
        {
            GameObject obj = actual_point.transform.GetChild(i).gameObject;
            obj.GetComponent<EnemyInstance>().ZombieInstance();
        }

        yield return null;
    }*/

    private IEnumerator InitializeLevel()
    {
        yield return new WaitForSeconds(1f);
        audio.PlayOneShot(audios[0]);
        yield return new WaitForSeconds(audios[0].length);

        nodes = GameObject.FindGameObjectWithTag("Node");

        list_points = new Dictionary<string, GameObject>();

        foreach (GameObject entry in GameObject.FindGameObjectsWithTag("Horda"))
            list_points[entry.name] = entry;


        horda_zombies = new Dictionary<string, int>();

        foreach (KeyValuePair<string, GameObject> entry in list_points)
            horda_zombies[entry.Key] = entry.Value.transform.childCount;

        actual_point = list_points["Point_1"];

        for (int i = 1; i < (offset + 1); i++)
        {
            if (list_points.ContainsKey(("Point_" + i)))
            {
                int size = list_points[("Point_" + i)].transform.childCount;

                for (int j = 0; j < size; j++)
                {
                    GameObject obj = list_points[("Point_" + i)].transform.GetChild(j).gameObject;
                    obj.GetComponent<EnemyInstance>().ZombieInstance();
                }
                  //list_points[("Point_" + i + "_left")].SetActive(false);
            }
        }
        Debug.Log("Todo OK!!");
        StartCoroutine(NextNode()); 
         
    }

    public void findPoint(string point)
    {

        actual_point = list_points[point];
        str_point = point;
        Debug.Log("Desde Find point : " + str_point);

        /*for (int i = 0; i < points.Length; i++)
            if (points[i].name == point)
            {
                actual_point = points[i];
                str_point = point;
                break;
            }*/
    }

    public void findNode(string node)
    {
        int childs = nodes.transform.childCount;

        for (int j = 0; j < childs; j++)
        {
            GameObject obj = nodes.transform.GetChild(j).gameObject;
            if (obj.name == node)
            {
                actual_node = obj;
                SplineNode nextNode = aux_node.GetComponent<SplineNode>();
                nextNode.setNext(actual_node.GetComponent<SplineNode>());
                break;
            }
        }
    }

    public GameObject getBossLife()
    {
        return this.bosslife;
    }
    private void CheckAudio()
    {
        bool isMute = (PlayerPrefs.GetInt("sound") == 0) ? true : false;

        AudioSource[] audio = FindObjectsOfType<AudioSource>();

        for (int i = 0; i < audio.Length; i++)
            audio[i].mute = isMute;
    }

    public void DoFade()
    {
        fade.GetComponent<FadeEffect>().setFade(1);
        fade.GetComponent<FadeEffect>().setSpeed(1);
    }

    void OnEnable()
    {
        CheckAudio();
    }

}

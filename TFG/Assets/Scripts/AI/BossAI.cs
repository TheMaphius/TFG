using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BossAI : AI {

    private GameObject boss_nodes;
    public float node_distance;
    
    private AudioEngine audio_engine;

    private IEnumerator coroutine = null;

    private Dictionary<string, GameObject> list_points;

    private Engine engine;
    private Animator anim;

    private Transform _transform;
    private Vector3 direction;
    private float currentDistance;
    private int node;

    private LevelEngine level;
    private GameObject lifebar;

    private int num_players;
    private string controller;
    private Transform player;
    private GameObject[] player_life;
    private GameObject[] player_avatar;

    private BossBehaviour behaviour;
    private string laststate;

    private bool isAttack = false;
    private bool isBossDead = false;
    private bool isBossCancel = false; 

    void Awake()
    {
        list_points = new Dictionary<string, GameObject>();
        engine = GameObject.FindGameObjectWithTag("Engine").GetComponent<Engine>();
        level = GameObject.FindGameObjectWithTag("Engine").GetComponent<LevelEngine>();
        audio_engine = GameObject.FindGameObjectWithTag("AudioEngine").GetComponent<AudioEngine>();
        lifebar = level.getBossLife();
        behaviour = GetComponent<BossBehaviour>();
        num_players = engine.getActivePlayers();
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {

        lifebar.SetActive(true);
        audio_engine.Stop();
        audio_engine.BossAudio();

        boss_nodes = GameObject.FindGameObjectWithTag("BossPoints");

        int childs = boss_nodes.transform.childCount;

        for (int i = 0; i < childs; i++)
        {
            GameObject obj = boss_nodes.transform.GetChild(i).gameObject;
            list_points[obj.name] = obj;
        }

        target = list_points["Boss_point_1"].transform;
        node = 1;

        player_life = new GameObject[2];
        player_life[0] = GameObject.FindGameObjectWithTag("LifeBar_1P");
        player_life[1] = GameObject.FindGameObjectWithTag("LifeBar_2P");

        player_avatar = new GameObject[2];
        player_avatar[0] = GameObject.FindGameObjectWithTag("Avatar_1P");
        player_avatar[1] = GameObject.FindGameObjectWithTag("Avatar_2P");

        behaviour = GetComponent<BossBehaviour>();

        

        anim.SetBool(state = "run", true);
        laststate = state;



        _transform = transform;
    }

    void Update()
    {
        IA();
        DeadBoss();
        //setCancelState();
    }

    private void setCancelState()
    {
        if (behaviour.isCancel() && !isBossDead && !isBossCancel)
        {
            isBossCancel = true;
            Debug.Log("xxxxxxxxCuandtas veces entros");

            if (coroutine != null)
            {
                Debug.Log("Cancelo rutina " + state);
                anim.SetBool(state, false);
                anim.SetBool(state = "hit", true);
                StopCoroutine(coroutine);
                
                laststate = state;
                
            }//StopAllCoroutines();
            anim.SetBool("attack", false);
            //anim.SetBool("attack", false);
            //anim.SetBool("run", false);
            
            
            StartCoroutine(coroutine = CancelBar());
            
        }
    }

    private void DeadBoss()
    {
        if (behaviour.isDead() && !isBossDead)
        {
            isBossDead = true;
            anim.SetBool("run", false);
            anim.SetBool("attack", false);
            anim.SetBool(state = "death", true);
            laststate = state;
            behaviour.ZombieDead(controller);
            Destroy(lifebar, 1f);
            Destroy(gameObject, 10f);
            
        }
    }

    public override void IA()
    {
        int node_level = node / 3;

        if (!isBossDead)
        {
            if (!isAttack)
            {
                currentDistance = Vector3.Distance(target.position, _transform.position);
                //Debug.Log("Distance: " + currentDistance);
            }

            if (currentDistance >= node_distance)
            {
                direction = target.position - _transform.position;
                direction.y = 0;
                _transform.rotation = Quaternion.Slerp(_transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                        rotation * Time.deltaTime);

                _transform.position += _transform.forward * (speed * 2.5f) * Time.deltaTime;
            }
            else if (node_level != 2)
            {
                node_level++;
                node = Random.Range(node_level * 3, (node_level + 1) * 3);
                Debug.Log("new node: " + node);
                target = list_points[("Boss_point_" + node)].transform;
            }
            else
            {
                if (!isAttack)
                {
                    isAttack = true;
                    Debug.Log("Me paro");
                    anim.SetBool(state = "run", false);
                    Debug.Log("Te pego");
                    StartCoroutine(coroutine = attackPlayer());
                }

                //Debug.Log("xxxEntro akixxx");

                if (player != null)
                {
                    direction = player.position - _transform.position;
                    direction.y = 0;
                    _transform.rotation = Quaternion.Slerp(_transform.rotation,
                                                            Quaternion.LookRotation(direction),
                                                            (rotation * 2) * Time.deltaTime);
                }

            }
        }
    }

    public override void StateMachine()
    {
        throw new System.NotImplementedException();
    }

    public override void HitState(RaycastHit hit, string player)
    {
        this.controller = player;

        engine.setPlayerTouch(player);

        GameObject bloodEffect = Instantiate(Resources.Load<GameObject>("Particles/Blood")) as GameObject;
        bloodEffect.transform.position = hit.point;

        setCancelState();

    }

    public override void setAIState(string ai_state)
    {
        
    }

    public override void setTimeAttack(float time_attack)
    {
       
    }

    public override void setTimeRun(float time_run)
    {
        
    }

    private IEnumerator attackPlayer()
    {

        int players = engine.getActivePlayers();
        anim.SetBool(state = "attack", true);
        laststate = state;
        Debug.Log("How many players: " + players);
        //if (players != num_players)
        //{
        LifeBar life = null;

        if(player_life[0] == null)
            player_life[0] = GameObject.FindGameObjectWithTag("LifeBar_1P");
        if (player_life[1] == null)
            player_life[1] = GameObject.FindGameObjectWithTag("LifeBar_2P");

        if (player_avatar[0] == null)
            player_avatar[0] = GameObject.FindGameObjectWithTag("Avatar_1P");
        if (player_life[1] == null)
            player_avatar[1] = GameObject.FindGameObjectWithTag("Avatar_2P");

        //Debug.Log("///////// " + player_life[0].name + " " + player_life[1].name);
        yield return new WaitForSeconds(1f);

        if (players == 2)
            {
                switch(node)
                {
                    case 6:
                        life = player_life[1].GetComponent<LifeBar>();
                        life.getHealth().setLifeBar(-.2f, 1);
                        player = player_avatar[1].transform;
                    break;
                    case 7:
                    int rand = Random.Range(0, 2);
                        life = player_life[rand].GetComponent<LifeBar>();
                        life.getHealth().setLifeBar(-.2f, rand);
                        player = player_avatar[rand].transform;
                        //target = (player_avatar[0].transform - player_avatar[1].transform);
                    break;
                    case 8:
                        life = player_life[0].GetComponent<LifeBar>();
                        life.getHealth().setLifeBar(-.2f, 0);
                        player = player_avatar[0].transform;
                    break;
                }

            }
            else if (players == 1)
            {
                if (player_life[0] != null)
                {
                    life = player_life[0].GetComponent<LifeBar>();
                    life.getHealth().setLifeBar(20f, 0);
                }
                else 
                {
                    life = player_life[1].GetComponent<LifeBar>();
                    life.getHealth().setLifeBar(20f, 1);
                }

                if (player_life[0] != null)
                {
                    player_avatar[0] = GameObject.FindGameObjectWithTag("Avatar_1P");
                    player = player_avatar[0].transform;
                }
                else
                {
                    player_avatar[1] = GameObject.FindGameObjectWithTag("Avatar_2P");
                    player = player_avatar[1].transform;
                }

            }


           // num_players = players;
        //}

        yield return new WaitForSeconds(1.5f);
        anim.SetBool(laststate, false);
        anim.SetBool(state = "run", true);
        laststate = state;
        node = Random.Range(0, 3);
        Debug.Log("new node: " + node);
        target = list_points[("Boss_point_" + node)].transform;
        isAttack = false;
    }

    private IEnumerator CancelBar()
    {
        
        player = null;
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("attack", false);
        anim.SetBool(state = "hit", false);
        laststate = state;
        node = Random.Range(0, 3);
        Debug.Log("new node: " + node);
        target = list_points[("Boss_point_" + node)].transform;
        anim.SetBool(state = "run", true);
        isBossCancel = false;
        isAttack = false;
     
    }
}

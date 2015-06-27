/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using System.Collections;

public class ZombieDogAI : AI {

    private Engine engine;

    private Animator anim;
    private string laststate = "";
    private bool animated = false;

    private float difficulty;

    // Timers to execute animations.
    private float time_run = 2f;
    private float time_attack = 2f;
    
    private int idx_attack;     // <-- Index to the last attack state.

    private IEnumerator coroutine;
    private Transform _transform;
    private Vector3 direction;
    private float currentDistance;
    //private bool nearEnemy = false;


    // STATES ANIMATION
    private string[] pose_state = { "stand_01", "stand_02", "bark", "eat", "dig" };
    private string[] attack_state = { "attack_01", "attack_02", "jump" };
    private string[] hit_state = { "hit_01", "hit_02", "hit_air" };
    private string[] walk_state = { "walk", "run_01", "run_02" };

    // TIME ANIMATION
    private float[] attack_time = { 1.83f, 2f, 1f};

    private ZombieDogBehaviour behaviour;

    private string target_zombie = "";
    private bool[] zombie_parts; //0: Head 
                                 //1: Body

    private string player = "";
    private int num_players;


    /// <summary>
    /// Use this for priority initialization.
    /// </summary>
    void Awake()
    {
        engine = GameObject.FindGameObjectWithTag("Engine").GetComponent<Engine>();
    }


    /// <summary>
    /// Use this for initialization.
    /// </summary>
    void Start()
    {
        anim = this.transform.GetComponent<Animator>();
        behaviour = this.transform.GetComponent<ZombieDogBehaviour>();
        int idx = Random.Range(0, pose_state.Length);

        if (state == "")
            state = pose_state[idx];

        if (target == null)
        {
            int random = Random.Range(1, engine.getActivePlayers());
            behaviour.setZombieTarget(random - 1);
            target = GameObject.FindGameObjectWithTag("Avatar_" + random + "P").transform;  
        }

        _transform = transform;
        currentDistance = Mathf.Infinity;

        if (engine.GetType() == typeof(SurvivorEngine))
        {
            StartCoroutine(UpdateDifficulty());
            Debug.Log("We're in the Survivor Level");
        }
        else
        {
            float zombie_attack = 0;

            switch ((int)engine.getDifficulty())
            {
                case 1:
                    this.speed *= 1f;
                    zombie_attack = behaviour.getZombieAttack() * .5f;
                    behaviour.setZombieAttack(zombie_attack);
                    break;
                case 2:
                    this.speed *= 1f;
                    zombie_attack = behaviour.getZombieAttack() * 1;
                    behaviour.setZombieAttack(zombie_attack);
                    break;
                case 3:
                    this.speed *= 2f;
                    zombie_attack = behaviour.getZombieAttack() * 2;
                    behaviour.setZombieAttack(zombie_attack);
                    break;
                case 4:
                    this.speed *= 3f;
                    zombie_attack = behaviour.getZombieAttack() * 3;
                    behaviour.setZombieAttack(zombie_attack);
                    break;
                case 5:
                    this.speed *= 4f;
                    zombie_attack = behaviour.getZombieAttack() * 6;
                    behaviour.setZombieAttack(zombie_attack);
                    break;
                default:
                    this.speed *= 2f;
                    zombie_attack = behaviour.getZombieAttack() * 3;
                    behaviour.setZombieAttack(zombie_attack);
                    break;
            }
            UpdateStats();
        }
    }


    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        //UpdateStats();
        //UpdateDifficulty();
        CheckPlayers();
        StateMachine();
        IA();
    }

    private IEnumerator UpdateDifficulty()
    {
        float zombie_attack = 0;
        float aux = 0/*engine.getDifficulty()*/;

        for (; ; )
        {
            if (aux != engine.getDifficulty())
            {
                Debug.Log("GetDifficulty: " + engine.getDifficulty());
                switch ((int)engine.getDifficulty())
                {
                    case 1:
                        this.speed *= 0.5f;
                        zombie_attack = behaviour.getZombieAttack() * 1;
                        behaviour.setZombieAttack(zombie_attack);
                        break;
                    case 2:
                        this.speed *= 1f;
                        zombie_attack = behaviour.getZombieAttack() * 2;
                        behaviour.setZombieAttack(zombie_attack);
                        break;
                    case 3:
                        this.speed *= 1.5f;
                        zombie_attack = behaviour.getZombieAttack() * 3;
                        behaviour.setZombieAttack(zombie_attack);
                        break;
                    case 4:
                        this.speed *= 5f;
                        zombie_attack = behaviour.getZombieAttack() * 4;
                        behaviour.setZombieAttack(zombie_attack);
                        break;
                    case 5:
                        Debug.Log("Voi a entrar aki???");
                        this.speed *= 8f;
                        zombie_attack = behaviour.getZombieAttack() * 8;
                        behaviour.setZombieAttack(zombie_attack);
                        break;
                }
                Debug.Log("Attack: " + zombie_attack);
                Debug.Log("Speed: " + this.speed);
                Debug.Log("Aux: " + aux + " Difficult: " + engine.getDifficulty());
                aux = engine.getDifficulty();
                Debug.Log("Afeter Aux: " + aux + " Difficult: " + engine.getDifficulty());
                UpdateStats();
            }

            yield return new WaitForSeconds(1f);
        }

    }

    void UpdateStats()
    {

        difficulty = engine.getDifficulty();

        if (time_run > 0.5f)
            this.time_run /= difficulty;
        else
            time_attack = 0.5f;

        if (time_attack > 0.5f)
            this.time_attack /= difficulty;
        else
            time_attack = 0.5f;



    }

    void CheckPlayers()
    {
        int players = engine.getActivePlayers();

        if (players != num_players)
        {
            GameObject avatar = null;
            //Debug.Log("Player: " + players + " Num: " + num_players);
            //Debug.Log("ZombieAI entro 1");
            if (players == 2)
            {
                float random = Random.Range(1, engine.getActivePlayers() + 1);
                avatar = GameObject.FindGameObjectWithTag("Avatar_" + random + "P");
                target_zombie = random + "P";
                behaviour.setZombieTarget((int)random - 1);
                //Debug.Log("ZombieAI entro 2");
            }
            else if (players == 1)
            {
                avatar = GameObject.FindGameObjectWithTag("Avatar_1P");
                if (avatar != null)
                {
                    target_zombie = "1P";
                    behaviour.setZombieTarget(0);
                    Debug.Log("ZombieAI entro 4");
                }
                else if ((avatar = GameObject.FindGameObjectWithTag("Avatar_2P")) != null)
                {
                    //avatar = GameObject.FindGameObjectWithTag("Avatar_2P");
                    target_zombie = "2P";
                    behaviour.setZombieTarget(1);
                    Debug.Log("ZombieAI entro 5");
                }
                Debug.Log("ZombieAI entro 3");
            }

            if (avatar != null)
                target = avatar.transform;

            num_players = players;
        }
    }

    
    /// <summary>
    /// Play a animation state in the State Machine.
    /// </summary>
    public override void StateMachine()
    {

        if (!animated)
        {
            int idx_pose;
            int idx_walk;
            //int random = Random.Range(0, 2);

            switch (state)
            {
                case "stand_01":
                case "stand_02":
                case "eat":
                case "bark":
                case "dig":

                    //int idx_pose = Random.Range(0, pose.Length);

                    anim.SetBool(state, true);


                    //Debug.Log(random);

                    if (laststate != "")
                        anim.SetBool(laststate, false);
                    else
                    {
                        idx_walk = Random.Range(0, walk_state.Length);
                        //Debug.Log("Walk dog " + walk_state[idx_walk]);
                        StartCoroutine(coroutine = (Delay(walk_state[idx_walk], time_run)));
                    }

                    if (currentDistance < distance)
                    {
                        // ATTACK
                        idx_attack = Random.Range(0, attack_state.Length);
                        StartCoroutine(coroutine = (Delay(attack_state[idx_attack], time_attack)));
                        /*zombie_parts = behaviour.getZombieParts();
                        Debug.Log("Estoi cerca del player.");
                        if (zombie_parts[1] && zombie_parts[2])
                            StartCoroutine(coroutine = (Delay((random == 0) ? "attack" : "bite", 2f)));
                        else if (!zombie_parts[2])
                            StartCoroutine(coroutine = (Delay("bite", 2f)));
                        else if (!zombie_parts[1])
                            StartCoroutine(coroutine = (Delay("attack", 2f)));*/
                    }

                    laststate = state;
                    animated = true;

                    //StartCoroutine(TriggerAnimatorState("idle"));
                    //state = "walk";
                    break;
                case "walk": 
                case "run_01": 
                case "run_02":

                    //Debug.Log("WALK State: " + state + " LastState: " + laststate);
                    
                    anim.SetBool(laststate, false);
                    anim.SetBool(state, true);

                    laststate = state;
                    animated = true;

                    break;
                case "attack_01":
                case "attack_02":
                case "jump":
                    
                    Debug.Log("Dog ATTACK State: " + state + " LastState: " + laststate);

                    
                    anim.SetBool(laststate, false);
                    //anim.SetBool(state, true);
                    if (engine.getPlayerAlive()[target_zombie])
                        anim.SetBool(state, true);

                    idx_pose = Random.Range(0, 3);
                    coroutine = Delay(pose_state[idx_pose], attack_time[idx_attack]);
                    StartCoroutine(coroutine);

                    laststate = state;
                    animated = true;
                    break;
                /*case "crawl":
                    Debug.Log("CRAWL State: " + state + " LastState: " + laststate);
                    // StopCoroutine(laststate);
                    anim.SetBool(state, true);

                    //int random = Random.Range(0, 2);
                    //Debug.Log(random);

                    if (laststate != "" && laststate != "dead")
                        anim.SetBool(laststate, false);
                    else
                        StartCoroutine(coroutine = (Delay("crawl", Mathf.Infinity)));

                    laststate = "crawl";
                    animated = true;
                    break;
                case "walk":
                    Debug.Log("WALK State: " + state + " LastState: " + laststate);
                    //if(laststate != "walk")
                    anim.SetBool(laststate, false);
                    anim.SetBool(state, true);

                    laststate = "walk";
                    animated = true;

                    break;
                case "run":
                    Debug.Log("RUN State: " + state + " LastState: " + laststate);
                    //if(laststate != "run")
                    anim.SetBool(laststate, false);
                    anim.SetBool(state, true);

                    laststate = "run";
                    animated = true;
                    break;
                case "attack":
                    //Debug.Log("ATTACK State: " + state + " LastState: " + laststate);
                    if (laststate != "attack")
                        anim.SetBool(laststate, false);
                    anim.SetBool(state, true);

                    coroutine = Delay("wait", 2.617f);
                    StartCoroutine(coroutine);

                    laststate = "attack";
                    animated = true;
                    break;
                case "bite":
                    //Debug.Log("BITE State: " + state + " LastState: " + laststate);
                    if (laststate != "bite")
                        anim.SetBool(laststate, false);
                    anim.SetBool(state, true);

                    coroutine = Delay("wait", 4.167f);
                    StartCoroutine(coroutine);

                    laststate = "bite";
                    animated = true;
                    break;
                case "eat":

                    anim.SetBool(state, true);

                    if (laststate != "")
                        anim.SetBool(laststate, false);
                    else
                        StartCoroutine(coroutine = (Delay("eat_awake", 3.217f)));

                    laststate = "eat";
                    animated = true;

                    break;
                case "eat_awake":

                    anim.SetBool(state, true);
                    anim.SetBool(laststate, false);

                    coroutine = Delay((random == 0) ? "walk" : "run", 2.717f);
                    StartCoroutine(coroutine);

                    laststate = "eat_awake";
                    animated = true;

                    break;
                case "eat_down":

                    anim.SetBool(state, true);

                    if (laststate != "")
                        anim.SetBool(laststate, false);
                    else
                        StartCoroutine(coroutine = (Delay("eat", .983f)));

                    laststate = "eat_down";
                    animated = true;

                    break;*/
                case "hit_01":
                case "hit_02":
                case "hit_air":
                    //Debug.Log("HIT State: " + state + " LastState: " + laststate);
                    //if(laststate != "hit")
                    /*anim.SetBool(laststate, false);
                    if (zombie_parts[0] && zombie_parts[3])
                    {*/
                    anim.SetBool(laststate, false);
                   // Debug.Log("LastState: " + laststate + " is " + anim.GetBool(laststate));
                    anim.SetBool(state, true);
                    //Debug.Log("Entro primero en hit SM");

                    if (currentDistance > distance)
                    {
                        //Debug.Log("Entro??? para correr");
                        coroutine = Delay("run_02", .25f);
                    }
                    else
                    {
                        idx_pose = Random.Range(0, 3);
                        coroutine = Delay(pose_state[idx_pose], (state == "hit_air") ? 1f : .25f);
                    }

                    laststate = state;
                    /*if (laststate == "crawl")
                    {

                        state = "crawl";
                        Debug.Log("HIT State: " + state + " LastState: " + laststate);
                        coroutine = Delay("crawl", Mathf.Infinity);
                    }
                    else if (laststate == "eat" && (!zombie_parts[4] || !zombie_parts[5]))
                    {
                        state = "crawl";
                        Debug.Log("HIT State: " + state + " LastState: " + laststate);
                    }
                    else if (laststate == "eat" && !zombie_parts[0])
                    {
                        state = "eat";
                        Debug.Log("HIT State: " + state + " LastState: " + laststate);
                    }
                    else
                    {*/
                        //state = laststate;
                        //coroutine = Delay("run", .8f);
                    //}

                    StartCoroutine(coroutine);
                    

                    break;
                case "dead":
                    /*Debug.Log("Entro en el estado de DEAD");
                    Debug.Log("State: " + state);
                    Debug.Log("Last State " + laststate);*/
                    //anim.SetBool(state, true);

                    //if (laststate != "")
                    StopCoroutine(coroutine);
                    anim.SetBool(laststate, false);

                    int idx_death = Random.Range(1, 4);
                    //state = "death_0" + idx_death;
                    anim.SetBool("death_0" + idx_death, true);
                    

                    //Debug.Log("State: " + state);
                    behaviour.ZombieDead(player);
                    StartCoroutine(Delay("dead", Mathf.Infinity));

                    //animated = true;
                    Destroy(gameObject, 10f);
                    break;
                /*case "death_01":
                case "death_02":
                    Debug.Log("Entro en el estado de DEAD");
                    Debug.Log("State: " + state);
                    Debug.Log("Last State " + laststate);
                    //anim.SetBool(state, true);

                    //if (laststate != "")
                    StopCoroutine(coroutine);
                    anim.SetBool(laststate, false);
                    anim.SetBool(state, true);
                    //else
                    StartCoroutine(coroutine = (Delay(state, 3.333f)));

                    animated = true;
                    Destroy(gameObject, 5f);

                    break;
                case "death_crawl":
                    Debug.Log("Entro en el estado de DEAD CRAWL");
                    Debug.Log("State: " + state);
                    Debug.Log("Last State " + laststate);
                    StopCoroutine(coroutine);
                    anim.SetBool(laststate, false);
                    anim.SetBool(state, true);
                    //else
                    StartCoroutine(coroutine = (Delay(state, 3.333f)));

                    animated = true;
                    Destroy(gameObject, 5f);

                    break;*/
                default:
                    // DO SOME ANIMATION DEFAULT.
                    break;
            }

        }

    }


    /// <summary>
    /// Control the AI of zombies.
    /// </summary>
    public override void IA()
    {
        switch (state)
        {
            
            case "walk":
            case "run_01":
            case "run_02":

                currentDistance = Vector3.Distance(target.position, _transform.position);
                
                if (currentDistance >= distance)
                {
                    direction = target.position - _transform.position;
                    direction.y = 0;
                    _transform.rotation = Quaternion.Slerp(_transform.rotation,
                                                            Quaternion.LookRotation(direction),
                                                            rotation * Time.deltaTime);

                    _transform.position += _transform.forward * ((state == "walk") ? speed : speed * 2.5f) * Time.deltaTime;
                }
                else
                {
                    laststate = state;
                    state = pose_state[Random.Range(0, 3)];
                    animated = false;
                    //nearEnemy = true;
                    //Debug.Log("Paso al estado de wait");
                }

                break;

            //case "eat":
                /*zombie_parts = behaviour.getZombieParts();
                if (!zombie_parts[0] || !zombie_parts[3])
                {
                    anim.SetBool("hit", false);
                    laststate = state;
                    Debug.Log("LastState: " + laststate);
                    state = "dead";
                    animated = false;
                    Debug.Log("State CRAWL en AI: " + state);
                }*/
                //break;

            case "hit_01":
            case "hit_02":
            case "hit_air":

                if (!zombie_parts[0] || !zombie_parts[1])
                {
                    StopCoroutine(coroutine);
                    //Debug.Log("*****:" + laststate);
                    laststate = state;
                    state = "dead";
                    animated = false;

                }

               break;
            default:
                //Debug.Log("Entro en el default con el estado " + state + " cuyo estado anterior es " + laststate);
                break;
        }
    }


    /// <summary>
    /// Play a animation Hit when the character shoot and generate blood particles.
    /// </summary>
    /// <param name="hit">Parameter with the info of RaycastHit.</param>
    /// <param name="player">Parameter with the name of player.</param>
    public override void HitState(RaycastHit hit, string player)
    {

        this.player = player;
        zombie_parts = behaviour.getZombieParts();
        
        engine.setPlayerTouch(player);
        
        if (!zombie_parts[0])
            Debug.Log("Cabeza rebentada.");

        if (state != "dead")
        {
            if (state == "jump")
                state = "hit_air";
            else
                state = hit_state[Random.Range(0, 1)];

            if (coroutine != null)
            {
                // This condition prevents the freeze of the animation 'hit'
                if (laststate != "hit")
                {
                    // Stop the laststate coroutine.
                    StopCoroutine(coroutine);
                    animated = false;
                }
            }
        }

        GameObject bloodEffect = Instantiate(Resources.Load<GameObject>("Particles/Blood")) as GameObject;
        bloodEffect.transform.position = hit.point;

    }


    public override void setAIState(string ai_state)
    {
        this.state = ai_state;
    }


    /// <summary>
    /// Setter timer_run to execute more faster or lower the animation.
    /// </summary>
    /// <param name="time_run">Parameter with the value of time that enemy run.</param>
    public override void setTimeRun(float time_run)
    {
        this.time_run = time_run;
    }


    /// <summary>
    /// Setter timer_attack to execute more faster or lower the animations to attack.
    /// </summary>
    /// <param name="time_attack">Parameter with the value of time that enemy attack.</param>
    public override void setTimeAttack(float time_attack)
    {
        this.time_attack = time_attack;
    }


    /// <summary>
    /// Delay method that waits in a current state and assign the next.
    /// </summary>
    /// <param name="nextstate">Parameter with the name of the next state.</param>
    /// <param name="delay">Parameter with the value of time delay.</param>
    /// <returns>Returns null.</returns>
    private IEnumerator Delay(string nextstate, float delay)
    {

        animated = true;
        //state = nextstate;
        //Debug.Log("State: " + state + " Time: " + anim.GetNextAnimatorStateInfo(0).length);

        //for (float time = delay; time > 0; time -= Time.deltaTime)
        //{
            yield return new WaitForSeconds(delay);
        //}

        //Debug.Log("Time: " + delay);
        state = nextstate;
        animated = false;

    }


    /// <summary>
    /// When GameObject is destroying if exist any coroutine executing stop them.
    /// </summary>
    void OnDestroy()
    {
        // Free all coroutines.
        StopAllCoroutines();
    }
}

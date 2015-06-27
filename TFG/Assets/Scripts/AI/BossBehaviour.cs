using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossBehaviour : Behaviour {

    public GameObject bosslife;
    private BossLifeBar health;

    private LifeBar[] player_life;
    private string player;

    private Engine engine;
    private Transform parent;
    private IEnumerator coroutine;

    private GameObject zombiePoints;
    private GameObject poolBlood;
    private GameObject clawBlood;

    private Vector3 box;    // Calculate the object box.
    private Bounds bound;
	
    // Use this for initialization
	void Start () 
    {
        engine = GameObject.FindGameObjectWithTag("Engine").GetComponent<Engine>();
        parent = GameObject.FindGameObjectWithTag("ZombieAttack").transform;

        player_life = new LifeBar[2];

        GameObject life;
        life = GameObject.FindGameObjectWithTag("LifeBar_1P");
        player_life[0] = (life != null) ? life.GetComponent<LifeBar>() : null;

        life = GameObject.FindGameObjectWithTag("LifeBar_2P");
        player_life[1] = (life != null) ? life.GetComponent<LifeBar>() : null;

        zombiePoints = Resources.Load<GameObject>("Prefabs/LifeBar/Points");
        poolBlood = Resources.Load<GameObject>("Particles/PoolBlood");
        clawBlood = Resources.Load<GameObject>("Prefabs/Effects/BloodyClaw");

        bound = parts[0].transform.collider.bounds;

        bosslife = GameObject.FindGameObjectWithTag("BossLife");
        health = bosslife.GetComponent<BossLifeBar>();
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}


    public override float getZombieAttack()
    {
        throw new System.NotImplementedException();
    }

    public override bool[] getZombieParts()
    {
        throw new System.NotImplementedException();
    }

    public override void setPlayerTarget(int target)
    {
        throw new System.NotImplementedException();
    }

    public override void setZombieAttack(float attack)
    {
        throw new System.NotImplementedException();
    }

    public override void setZombieTarget(int target)
    {
        throw new System.NotImplementedException();
    }

    public override void ZombieDamage(string zombie_part, float damage)
    {

        health.setBarCancel(damage / 10);
    }

    /// <summary>
    /// Method that take damage to the player that AI choose.
    /// </summary>
    /// <param name="type_attack">Name of attack.</param>
    public void ZombieAttack(string type_attack)
    {

        if (type_attack == "claw")
            StartCoroutine(coroutine = BloodyClaw());
        
    }


    /// <summary>
    /// Method that check when zombie die and assign the scores and effects.
    /// </summary>
    /// <param name="player">Name of player that beat the zombie.</param>
    public override void ZombieDead(string player)
    {
        this.player = player;

        GameObject obj = GameObject.FindGameObjectWithTag("LifeBar_1P");
        
        if (obj != null)
            player_life[0] = obj.GetComponent<LifeBar>();

        obj = GameObject.FindGameObjectWithTag("LifeBar_2P");

        if (obj != null)
            player_life[1] = obj.GetComponent<LifeBar>();

        //Debug.Log("Player 1: " + player_life[0].name + " Player 2: " + player_life[1].name);
        for (int i = 0; i < player_life.Length; i++)
        {
            if (player_life[i] != null)
            {
                player_life[i].getTambor().RotateTambor("special", player_life[i].getCombo());

                ZombiePoints(i);
                engine.setZombiePlayerKills((i + 1) + "P");
                engine.setPlayerCombo(player, player_life[i].getCombo().getCombo());
            }
        }
        
        engine.ZombieEngine(-1);

    }


    /// <summary>
    /// Effect of blood when zombie es dead.
    /// </summary>
    public void ZombieDeadEffect()
    {
        GameObject BloodPoolEffect = Instantiate(poolBlood) as GameObject;

        if (parts.Length > 1)
            BloodPoolEffect.transform.SetParent(parts[3].transform, true);
        else
            BloodPoolEffect.transform.SetParent(parts[0].transform, true);

        BloodPoolEffect.transform.position = transform.position;
        StartCoroutine(BloodyPool(BloodPoolEffect, 2.5f));
    }


    /// <summary>
    /// Method that assign the zombie points when is dead.
    /// </summary>
    /// <param name="idx">Index of the player lifebar.</param>
    public void ZombiePoints(int idx)
    {
        float x = bound.center.x;
        float y = bound.max.y;
        float z = bound.center.z;
        //string player = (idx + 1) + "P";

        Vector3 position = Camera.main.WorldToScreenPoint(new Vector3(x, y, z));

        GameObject points = Instantiate(zombiePoints) as GameObject;

        int zombie_points = 0;
        int size = score.Length;

        if (zombie_points == 0)
            zombie_points += score[0];
        /*for (int i = size - 1; i > 0; i--)
            if (!zombie_parts[i])
                zombie_points += score[i];

        if (zombie_points == 0)
            zombie_points += score[0];
        else if (!zombie_parts[0])
            zombie_points += (int)(0.1f * score[0]);*/

        for (int i = 0; i < player_life.Length; i++)
        {
            int combo = 1;

            if (player_life[i] != null)
                player_life[i].getCombo().getCombo();

            zombie_points *= combo;

            if(this.player == "1P")
                points.GetComponent<Points>().setZombiePoints(zombie_points, "red");
            else if(this.player == "2P")
                points.GetComponent<Points>().setZombiePoints(zombie_points, "blue");
            
            Transform parent = GameObject.FindGameObjectWithTag("Canvas").transform;

            float offset = position.y * .1f;
            points.transform.position = new Vector2(position.x, offset + position.y);
            points.transform.SetParent(parent.transform, true);

            if (player_life[i] != null)
            {
                player_life[i].getScore().SumPoints(zombie_points);
                engine.setPlayerScore(((i + 1) + "P"), player_life[idx].getScore().getScore());
            }
        }

        Destroy(points, 0.5f);
    }

    /// <summary>
    /// Effect of bloody claws on the screen.
    /// </summary>
    private IEnumerator BloodyClaw()
    {
        if (parts[0] != null)
        {
            Vector3 arm_position = Camera.main.WorldToScreenPoint(parts[0].transform.position);
            if (clawBlood != null)
            {
                GameObject claw = Instantiate(clawBlood) as GameObject;
                claw.transform.SetParent(parent, true);
                claw.transform.position = arm_position;
                //claw.transform.localScale = new Vector3(1f, 1f, 1f);

                Image image = claw.GetComponent<Image>();
                float alpha = image.fillAmount;

                while (alpha < 1)
                {
                    image.fillAmount += 0.05f;
                    alpha = image.fillAmount;
                    yield return 0;
                }

                Destroy(claw.gameObject, .5f);
            }
        }
    }


    /// <summary>
    /// Delay of effect 'BloodyPool'.
    /// </summary>
    /// <param name="effect">GameObject with the effect.</param>
    /// <param name="delay">Value with the time of the effect.</param>
    private IEnumerator BloodyPool(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        effect.GetComponent<DestroyParticles>().Stop();
    }

    public bool isCancel()
    {
        return health.isCancelBar();
    }

    public bool isDead()
    {
        return health.isDead();
    }

    void OnEnable()
    {
        //bosslife = GameObject.FindGameObjectWithTag("BossLife");
        //health = bosslife.GetComponent<BossLifeBar>();
    }

    /// <summary>
    /// When GameObject is destroying if exist any coroutine executing stop them.
    /// </summary>
    void OnDestroy()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

}

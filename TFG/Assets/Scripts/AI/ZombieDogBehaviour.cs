/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ZombieDogBehaviour : Behaviour {

    private LifeBar[] player_life;

    private Engine engine;
    private Transform parent;
    private IEnumerator coroutine;

    private bool[] zombie_parts = { true, true };
    private int target;

    private GameObject zombiePoints;
    private GameObject poolBlood;
    private GameObject clawBlood;


    private Vector3 box;    // Calculate the object box.

    /// <summary>
    /// Use this for priority initialization.
    /// </summary>
    void Awake()
    {
        engine = GameObject.FindGameObjectWithTag("Engine").GetComponent<Engine>();
        parent = GameObject.FindGameObjectWithTag("ZombieAttack").transform;

        player_life = new LifeBar[2];

        GameObject life;
        life = GameObject.FindGameObjectWithTag("LifeBar_1P");
        player_life[0] = (life != null) ? life.GetComponent<LifeBar>() : null;

        life = GameObject.FindGameObjectWithTag("LifeBar_2P");
        player_life[1] = (life != null) ? life.GetComponent<LifeBar>() : null;

        // Add +1 enemy of total.
        //engine.setNumEnemies(1);
    }


    /// <summary>
    /// Use this for initialization.
    /// </summary>
    void Start()
    {
        zombiePoints = Resources.Load<GameObject>("Prefabs/LifeBar/Points");
        poolBlood = Resources.Load<GameObject>("Particles/PoolBlood");
        clawBlood = Resources.Load<GameObject>("Prefabs/Effects/BloodyClaw");
    }


    /// <summary>
    /// Method that take damage a part of zombie beat.
    /// </summary>
    /// <param name="zombie_part">Parameter with the name of the part beaten.</param>
    /// <param name="damage">Parameter with the damage value.</param>
    public override void ZombieDamage(string zombie_part, float damage)
    {
        switch (zombie_part)
        {
            case "Head":
                life[0] -= damage;
                break;
            case "Body":
                life[1] -= damage;
                break;
        }

        for (int i = 0; i < parts.Length; i++)
        {
            if (life[i] <= 0)
            {
                zombie_parts[i] = false;
                if (parts[i] != null && parts[i].name != "Body")
                    if (parts[0] != null && life[1] >= 0)
                        Destroy(parts[i], .2f);
                    
            }
        }                 
    }


    /// <summary>
    /// Method that take damage to the player that AI choose.
    /// </summary>
    /// <param name="type_attack">Name of attack.</param>
    public void ZombieAttack(string type_attack)
    {
        float damage = attack / 100;
        string controller = (target + 1) + "P";

        if (type_attack == "claw")
            StartCoroutine(coroutine = BloodyClaw());
        else if (type_attack == "bite")
            StartCoroutine(coroutine = BloodyClaw());

        /*Debug.Log("Soy un perro y ataco a " + target);
        player_life[target].getHealth().setLifeBar(-damage, target);
        player_life[target].getTambor().ResetTambor();
        player_life[target].getCombo().ComboBreak();*/

        if (player_life[target] == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("LifeBar_" + controller);
            if (obj != null)
            {
                player_life[target] = obj.GetComponent<LifeBar>();
                player_life[target].getHealth().setLifeBar(-damage, target);
                player_life[target].getTambor().ResetTambor();
                player_life[target].getCombo().ComboBreak();
            }
        }
        else
        {
            player_life[target].getHealth().setLifeBar(-damage, target);
            player_life[target].getTambor().ResetTambor();
            player_life[target].getCombo().ComboBreak();
        }

    }


    /// <summary>
    /// Method that check when zombie die and assign the scores and effects.
    /// </summary>
    /// <param name="player">Name of player that beat the zombie.</param>
    public override void ZombieDead(string player)
    {
        int idx;

        if (player.Equals("2P") && player_life[1] == null)
            player_life[1] = GameObject.FindGameObjectWithTag("LifeBar_2P").GetComponent<LifeBar>();

        idx = (player.Equals("1P")) ? 0 : 1;

        if (!zombie_parts[0])
            player_life[idx].getTambor().RotateTambor("special", player_life[idx].getCombo());
        else if (!zombie_parts[1])
            player_life[idx].getTambor().RotateTambor("normal", player_life[idx].getCombo());

        ZombiePoints(idx);
        //player_life[idx].getCombo().setCombo(1);

        // Delete a enemy of total.
        engine.setZombiePlayerKills(player);
        engine.setPlayerCombo(player, player_life[idx].getCombo().getCombo());
        engine.ZombieEngine(-1);

    }


    /// <summary>
    /// Effect of blood when zombie es dead.
    /// </summary>
    public void ZombieDeadEffect()
    {
        GameObject BloodPoolEffect = Instantiate(poolBlood) as GameObject;

        BloodPoolEffect.transform.SetParent(parts[1].transform, true);
        BloodPoolEffect.transform.position = transform.position;

        StartCoroutine(BloodyPool(BloodPoolEffect, 2.5f));
    }


    /// <summary>
    /// Method that assign the zombie points when is dead.
    /// </summary>
    /// <param name="idx">Index of the player lifebar.</param>
    public void ZombiePoints(int idx)
    {
        Bounds bound = parts[0].transform.collider.bounds;
        float x = bound.center.x;
        float y = bound.max.y;
        float z = bound.center.z;

        Vector3 position = Camera.main.WorldToScreenPoint(new Vector3(x, y, z));

        GameObject points = Instantiate(zombiePoints) as GameObject;
     
        int zombie_points = 0;
        int size = score.Length;

        for (int i = size - 1; i > 0; i--)
            if (!zombie_parts[i])
                zombie_points += score[i];

        if (zombie_points == 0)
            zombie_points += score[0];
        else if (!zombie_parts[0])
            zombie_points += (int)(0.1f * score[0]);

        int combo = player_life[idx].getCombo().getCombo();
        zombie_points *= combo;

        points.GetComponent<Points>().setZombiePoints(zombie_points, (idx == 0) ? "red" : "blue");

        Transform parent = GameObject.FindGameObjectWithTag("Canvas").transform;

        float offset = position.y * .1f;
        points.transform.position = new Vector2(position.x, offset + position.y);
        points.transform.SetParent(parent.transform, true);

        player_life[idx].getScore().SumPoints(zombie_points);

        Destroy(points, 0.5f);
    }


    /// <summary>
    /// Getter wich say is the part of zombie is beaten.
    /// </summary>
    public override bool[] getZombieParts()
    {
        return zombie_parts;
    }

    /// <summary>
    /// Setter wich the target that zombie follow.
    /// </summary>
    /// <param name="target">Index of the player target.</param>
    public override void setZombieTarget(int target)
    {
        this.target = target;
    }


    /// <summary>
    /// Setter of zombie attak.
    /// </summary>
    /// <param name="attack">Value of zombie attack.</param>
    public override void setZombieAttack(float attack)
    {
        this.attack = attack;
    }

    public override float getZombieAttack()
    {
        return this.attack;
    }


    public override void setPlayerTarget(int target)
    {
        this.target = target;
    }

    /// <summary>
    /// Effect of bloody claws on the screen.
    /// </summary>
    private IEnumerator BloodyClaw()
    {

        Vector3 arm_position = Camera.main.WorldToScreenPoint(parts[1].transform.position);
        Vector3 position = new Vector3(arm_position.x, Screen.height * .5f, arm_position.z); 

        GameObject claw = Instantiate(clawBlood) as GameObject;
        claw.transform.SetParent(parent, true);
        claw.transform.position = position;
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


    /// <summary>
    /// When GameObject is destroying if exist any coroutine executing stop them.
    /// </summary>
    void OnDestroy()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

}

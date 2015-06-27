using UnityEngine;
using System.Collections;

public class ZombieSurvivor : AI {

   
    //public string state;
    //public GameObject survivor;

    //public float speed;
    //private int rotation = 2;

    private LevelEngine engine;

    private Animator anim;

    private Transform _transform;
    private float currentDistance;

    private bool isDead = false;
    private bool isAttack = false;

    // Use this for initialization
	void Start () 
    {
        engine = GameObject.FindGameObjectWithTag("Engine").GetComponent<LevelEngine>();
        anim = GetComponent<Animator>();
        anim.SetBool(state, true);
        _transform = transform;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(!isDead)
            killSurvivor();
	}

    void killSurvivor()
    {
        if(target == null)
            target = GameObject.FindGameObjectWithTag("Survivor").transform;

        currentDistance = Vector3.Distance(target.transform.position, _transform.position);
        //Debug.Log("Current Distance " + currentDistance);
        if (currentDistance > 8)
        {
            Vector3 direction;
            direction = target.transform.position - _transform.position;
            direction.y = 0;
            _transform.rotation = Quaternion.Slerp(_transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    rotation * Time.deltaTime);
            _transform.position += _transform.forward * speed * Time.deltaTime;
        }
        else
        {
            isDead = true;
            StartCoroutine(attack());
        }
    }

    public override void HitState(RaycastHit hit, string player)
    {
        if (!isDead)
        {
            isDead = true;
            //Debug.Log("/////////Tocado*******");
            anim.SetBool(state, false);
            anim.SetBool("wait", false);
            anim.SetBool("attack", false);
            GetComponent<Behaviour>().enabled = true;
            anim.SetBool("death_01", true);

            if (!isAttack)
                engine.ZombieEngine(-2);
            else
                engine.ZombieEngine(-1);

            Destroy(gameObject, 20f);
        }

    }

    public override void IA()
    {
        Debug.Log("Function not implemented");
    }

    public override void setTimeAttack(float time_attack)
    {
        Debug.Log("Function not implemented");
    }

    public override void setTimeRun(float time_run)
    {
        Debug.Log("Function not implemented");
    }

    public override void StateMachine()
    {
        Debug.Log("Function not implemented");
    }

    public override void setAIState(string ai_state)
    {
        Debug.Log("Function not implemented");
    }

    private IEnumerator attack()
    {
        Debug.Log("Wait");

        isAttack = true;
        anim.SetBool(state, false);
        
       /*try
        {*/
        Debug.Log("Te ataco");
        target.GetComponent<SurvivorAI>().setGoal(true);
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(.8f);
        target.GetComponent<SurvivorAI>().Dead();
        
        /*}
        catch(System.ArgumentException ae)
        {
           Debug.Log("Capturada la exception " + ae.Message);
        }*/

        yield return new WaitForSeconds(0.8f);
        //engine.ZombieEngine(-1);
        anim.SetBool("attack", false);
        GetComponent<AI>().enabled = true;
        GetComponent<Behaviour>().enabled = true;
        GetComponent<ZombieSurvivor>().enabled = false;

    }

    public void setStats(string state, float time, float speed, float rotation, float distance)
    {
        target = GameObject.FindGameObjectWithTag("Survivor").transform;
        this.state = state;
        this.timeState = time;
        this.speed = speed;
        this.rotation = rotation;
        this.distance = distance;
    }

}

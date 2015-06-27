using UnityEngine;
using System.Collections;

public class SurvivorAI : MonoBehaviour {

    public string state;
    public GameObject goal;

    public bool isMan;

    private AudioClip audio_help;
    private AudioClip audio_die;

    //public GameObject zombie;
    public float speed;
    private int rotation = 2;

    private LevelEngine engine;

    private Animator anim;

    private Transform _transform;
    private float currentDistance;

    private bool isGoal = false;
    private bool isDead = false;

    // Use this for initialization
	void Start () 
    {
        engine = GameObject.FindGameObjectWithTag("Engine").GetComponent<LevelEngine>();
        audio_help = Resources.Load<AudioClip>("Music/Voice/" + ((isMan == true) ? "help_man" : "help_girl"));
        audio_die = Resources.Load<AudioClip>("Music/Voice/" + ((isMan == true) ? "die_man" : "die_girl"));
        anim = GetComponent<Animator>();
        anim.SetBool(state, true);
        _transform = transform;
        audio.PlayOneShot(audio_help);
	}

    void Update()
    {
        if(!isGoal)
            goToGoal();
    }

    void goToGoal()
    {

        currentDistance = Vector3.Distance(goal.transform.position, _transform.position);
        Debug.Log("Current distance: " + currentDistance);
        if (currentDistance > 5)
        {
            Vector3 direction;
            direction = goal.transform.position - _transform.position;
            direction.y = 0;
            _transform.rotation = Quaternion.Slerp(_transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    rotation * Time.deltaTime);
            _transform.position += _transform.forward * speed * Time.deltaTime;
        }
        else
        {
            isGoal = true;
            Debug.Log("Destroy");
            Destroy(gameObject);
        }
    }

    public void setGoalSurvivor(GameObject obj)
    {
        this.goal = obj;
    }

    public void setGoal(bool goal)
    {
        this.isGoal = goal;
        anim.SetBool(state, false);
        anim.SetBool("wait", true);
    }

    public void Dead()
    {
        engine.ZombieEngine(-1);
        anim.SetBool("wait", false);
        audio.PlayOneShot(audio_die);
        anim.SetBool("death", true);
        Destroy(gameObject, 20f);
    }
	
}

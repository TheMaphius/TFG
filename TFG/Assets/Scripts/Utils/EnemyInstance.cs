using UnityEngine;
using System.Collections;

public class EnemyInstance : MonoBehaviour {

    public enum enumDifficulty // Selectable type of platform.
    {
        very_easy = 0,
        easy = 1,
        normal = 2,
        hard = 3,
        extreme = 4
    };

    public enumDifficulty difficulty_type;

    public GameObject enemy;
    public Vector3 orientation;
    public string loop_state;
    public string ai_state;
    public bool isSurvivor;
    public GameObject goal_survivor;

    private int difficulty;

    void Awake()
    {
        difficulty = PlayerPrefs.GetInt("difficulty");
        
        if ((int)difficulty_type > difficulty)
            Destroy(gameObject);
    }
    /*void Start()
    {
        Debug.Log("Difficulty: " + PlayerPrefs.GetInt("difficulty"));
    }*/

    public void ZombieInstance()
    {
        GameObject zombie = Instantiate(enemy) as GameObject;

        if (!isSurvivor)
        {
            zombie.GetComponent<LoopAnimation>().setState(loop_state);
            zombie.GetComponent<AI>().setAIState(ai_state);
        }
        else
        {
            zombie.GetComponent<LoopAnimation>().setState(loop_state);
            SurvivorAI survivor = zombie.GetComponent<SurvivorAI>();

            Debug.Log("Survivor: " + (survivor == null));
            if (survivor != null)
                survivor.setGoalSurvivor(goal_survivor);
            else
            {
                Debug.Log("Zombie survivor enabled.");
                zombie.AddComponent<ZombieSurvivor>();
                zombie.GetComponent<ZombieSurvivor>().setStats(loop_state, 1, 12, 2, 5.5f);
                zombie.GetComponent<ZombieSurvivor>().enabled = false;
                
            }
        }
        
        zombie.transform.SetParent(this.transform, true);
        zombie.transform.position = transform.position;
        zombie.transform.rotation = Quaternion.Euler(orientation);

    }
}

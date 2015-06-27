using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeBar : MonoBehaviour {

    
    //public Transform[] lifeBar;    // Element 0: LifeBar; Element 1: Tambor
    //private Transform[] bullet;

    //private Image lifeBarImage;
    //public Sprite[] texture;
    public string player;
    //private short index;
    public Health health;
    public Score score;
    public Tambor tambor;
    public Combo combo;
    //private short rotation = 0;

    // Use this for initialization
    void Start()
    {
        /*int idx = (player.Equals("1P")) ? 0 : 1;
        Debug.Log("player: " + player);
        Debug.Log("Idx: " + idx);*/
        

        /*health = GameObject.FindGameObjectsWithTag("Health")[idx].GetComponent<Health>();
        score = GameObject.FindGameObjectWithTag("Score_" + player).GetComponent<Score>();
        tambor = GameObject.FindGameObjectsWithTag("Tambor")[idx].GetComponent<Tambor>();
        combo = GameObject.FindGameObjectWithTag("Combo_" + player).GetComponent<Combo>();*/
        
    }

    public Health getHealth()
    {
        return health;
    }

    public Score getScore()
    {
        return score;
    }

    public Tambor getTambor()
    {
        return tambor;
    }

    public Combo getCombo()
    {
        return combo;
    }

}

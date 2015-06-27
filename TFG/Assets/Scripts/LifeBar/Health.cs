using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {

    private GameObject engine;
    private GameOverEngine gameover;
    private Image health;
    private bool isDead = false;

	// Use this for initialization
	void Start () 
    {
        engine = GameObject.FindGameObjectWithTag("Engine");
        gameover = engine.GetComponent<GameOverEngine>();
        health = transform.GetComponent<Image>();
	}


    public void setLifeBar(float damage, int target)
    {
        health.fillAmount += damage;
        //Debug.Log("Actual Life: " + health.fillAmount);
        if (health.fillAmount <= 0 && !isDead)
        {

            //string controller = (target + 1) + "P";
            isDead = true;
            //gameover.Continue(target, controller);
            gameover.InsertCoin(target);
        }
        else if(health.fillAmount > 0)
            isDead = false;
        
    }


    public float getLifeBar()
    {
        return health.fillAmount;
    }

}

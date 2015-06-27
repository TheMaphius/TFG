using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AreaClearEngine : MonoBehaviour {

    public GameObject results_1P;
    public GameObject results_2P;

    private Engine engine;

    private bool isClear = false;

	// Use this for initialization
	void Start () 
    {
        engine = GetComponent<Engine>();
	}

    public void UpdateStats()
    {

        float touch = 0f;
        float shoot = 0f;
        float accuracy = 0f;

        results_1P.transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + engine.getPlayerScore("1P");
        results_1P.transform.GetChild(1).gameObject.GetComponent<Text>().text = "" + engine.getZombiePlayerKills("1P");

        
        if (engine.getPlayerShoot("1P") != 0)
        {

            touch = engine.getPlayerTouch("1P");
            shoot = engine.getPlayerShoot("1P");
            accuracy = Round(((float)touch / (float)shoot) * 100f, 2);
            results_1P.transform.GetChild(2).gameObject.GetComponent<Text>().text = "" + accuracy + "%";
        }
        else
            results_1P.transform.GetChild(2).gameObject.GetComponent<Text>().text = "0%";

        results_1P.transform.GetChild(3).gameObject.GetComponent<Text>().text = "" + engine.getPlayerCombo("1P");

        results_2P.transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + engine.getPlayerScore("2P");
        results_2P.transform.GetChild(1).gameObject.GetComponent<Text>().text = "" + engine.getZombiePlayerKills("2P");


        if (engine.getPlayerShoot("2P") != 0)
        {
            touch = engine.getPlayerTouch("2P");
            shoot = engine.getPlayerShoot("2P");
            accuracy = Round(((float)touch / (float)shoot) * 100f, 2);
            results_2P.transform.GetChild(2).gameObject.GetComponent<Text>().text = "" + accuracy + "%";
        }
        else
            results_2P.transform.GetChild(2).gameObject.GetComponent<Text>().text = "0%";

        results_2P.transform.GetChild(3).gameObject.GetComponent<Text>().text = "" + engine.getPlayerCombo("2P");
        
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    public void setAreaClear(bool clear)
    {
        this.isClear = clear;
    }

    public bool getClear()
    {
        return this.isClear;
    }
	

}

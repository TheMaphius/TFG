/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEngine : MonoBehaviour {

    private GameObject[] points;
    private int round = 0;
    private int player = 0;

    private float difficulty;

    private GameObject[] zombies;
    private int enemies = 0;


    /// <summary>
    /// Use this for priority initialization.
    /// </summary>
	void Awake () 
    {
        /*Debug.Log("Estoi en GameEngine");
        points = GameObject.FindGameObjectsWithTag("Points");
        zombies = new GameObject[2];
        zombies[0] = Resources.Load<GameObject>("Prefabs/Enemies/Zombies/zombie_cop");
        zombies[1] = Resources.Load<GameObject>("Prefabs/Enemies/ZombieDog/zombie_dog");
        Debug.Log("Tengo " + points.Length + " puntos");*/
	}


    /// <summary>
    /// Use this for initialization.
    /// </summary>
    void Start()
    {
        difficulty = PlayerPrefs.GetFloat("Difficulty");  
    }


    /// <summary>
    /// Getter the difficulty value.
    /// </summary>
    /// <returns>Value of difficulty</returns>
    public float getDifficulty()
    {
        return difficulty;
    }


    /***************************************************************************/
    /****                     Engine methods of Rounds                     *****/
    /***************************************************************************/


    /// <summary>
    /// Setter the number of round.
    /// </summary>
    public void setRound()
    {
        round++;
    }


    /// <summary>
    /// Getter the number of round.
    /// </summary>
    /// <returns>Value of round</returns>
    public int getRound()
    {
        return round;
    }


    /***************************************************************************/
    /****                    Engine methods of Players                     *****/
    /***************************************************************************/


    /// <summary>
    /// Setter the number of players. Add 'continues' to actual player.
    /// </summary> 
    /// <param name="controller">Parameter with the name of player.</param>
    public void setNumPlayers(string controller)
    {
        PlayerPrefs.SetInt(controller, 3);
        //Debug.Log("Player: " + controller + "has 3 credits.");
    }


    /// <summary>
    /// Setter the number of players activated. 
    /// Calculate the number of player are activated in the scene
    /// </summary> 
    /// <param name="active">Parameter with number +1 or -1.</param>
    public void setActivePlayers(int active)
    {
        player += active;
    }


    /// <summary>
    /// Getter the number of active player.
    /// </summary>
    /// <returns>Player active</returns>
    public int getActivePlayers()
    {
        return player;
    }


    /// <summary>
    /// Getter the number of credits of player.
    /// </summary>
    /// <param name="controller">Controller of player.</param>
    /// <returns>Number of credits</returns>
    public int getNumCredits(string controller)
    {
        return PlayerPrefs.GetInt(controller);
    }

    /***************************************************************************/
    /****                    Engine methods of Enemies                     *****/
    /***************************************************************************/


    /// <summary>
    /// Setter the number of enemies. 
    /// Generate a enemy if the value pass is negative.
    /// </summary> 
    /// <param name="enemy">Parameter with number +1 or -1.</param>
    public void setNumEnemies(int enemy)
    {
        enemies += enemy;

        if (enemy == -1)
            StartCoroutine(generateEnemy());
    }


    /// <summary>
    /// Getter the number of enemies on the scene.
    /// </summary>
    /// <returns>Number of enemies</returns>
    public int getEnemies()
    {
        return enemies;
    }


    /// <summary>
    /// Method that generate a new enemy when the game needs.
    /// </summary>
    private IEnumerator generateEnemy()
    {
        int random;
        
        random = Random.Range(0, zombies.Length);
        GameObject enemy = Instantiate(zombies[0]) as GameObject;
        random = Random.Range(0, points.Length);
        enemy.transform.position = points[random].transform.position;

        yield return 0;


    }

    /// <summary>
    /// When GameObject is destroying delete de info of PlayerPrefs.
    /// </summary>
    void OnApplicationQuit()
    {
        //PlayerPrefs.DeleteAll();
    }
}

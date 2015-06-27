using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class GameController : MonoBehaviour {

    private static List<string> playerList;
    private static byte controllers;

    private GameObject player;
    private Transform _transform;

    private Transform bullet;
    //private Bullet b;

    void Start()
    {
        /*player = GameObject.FindGameObjectWithTag("1P");
        _transform = player.transform;


        bullet = Instantiate(Resources.Load<Transform>("Prefabs/Bullet")) as Transform;
        bullet.parent = _transform;
        bullet.position = new Vector3(Screen.width*0.25f, Screen.height*.25f);*/
        
 
    }
	// Use this for initialization
	/*void Awake () {
	    playerList = new List<string>();
	}

    void Start() {
        controllers = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static int PlayerController(){

        if (playerList.Capacity > 0)
            playerList.Add("joystick " + (playerList.Capacity + 1));
        else
            playerList.Add("joystick 1");

        return 0;
    }*/
}

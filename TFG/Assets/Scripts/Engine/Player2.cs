/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player2 : MonoBehaviour {

    public enum enumPlatform // Selectable type of platform.
    {
        PC,
        PS3,
        Wii
    };

    public enumPlatform joystick;
    public GameObject avatar;
    public GameObject pointer;

    public string controller;
    public int wii_controller;
    public bool isMenu;
    /*public GameObject life;
    public GameObject pointer;
    public GameObject reload;
    public GameObject grenades;*/
    public GameObject player;

    //private GameEngine engine;
    private Engine engine; 
    private PS3 ps3;
    private Wiimote wiimote;



    /// <summary>
    /// Use this for priority initialization.
    /// </summary>
    void Awake()
    {
        if(!isMenu)
            engine = GameObject.FindGameObjectWithTag("Engine").GetComponent<Engine>();
    }


    /// <summary>
    /// Use this for initialization.
    /// </summary>
	void Start () 
    {
        if (joystick.ToString().Equals("PS3"))
            ps3 = new PS3();

        if (joystick.ToString().Equals("Wii"))
        {
            wiimote = new Wiimote();
            //wiimote.connectWiimote();
        }

        if (PlayerPrefs.GetInt("player2") == 0)
        {
            activateElements(true);
            Destroy(gameObject);
            
            // Set the army for the second controller
            if (!isMenu)
            {
                Controller controller = pointer.GetComponent<Controller>();
                controller.setFirstWeapon("Revolver");
                controller.setSecondWeapon("SMG");
                controller.setPlayer("2P");
            }

        }
	}


    /// <summary>
    /// Update is called once per frame.
    /// </summary>
	void Update () 
    {
        //Debug.Log("Wiimote pressed HOME: " + wiimote.pressedWiimoteButtonHome(wii_controller));
        //Debug.Log("Wiimote pressed B: " + wiimote.pressedWiimoteButtonB(wii_controller));

        if (isMenu)
        {
            if (joystick.ToString().Equals("PC"))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {

                    //PlayerPrefs.SetInt("P2", 0);
                    PlayerPrefs.SetInt("player2", 0);
                    activateElements(true);
                    Destroy(gameObject);


                }
            }
            else if (joystick.ToString().Equals("PS3"))
            {

                if (ps3.isPS3Start("joystick 2"))
                {

                    //PlayerPrefs.SetInt("P2", 0);
                    PlayerPrefs.SetInt("player2", 0);
                    activateElements(true);
                    Destroy(gameObject);

                }
            }
            else if (joystick.ToString().Equals("Wii"))
            {
                Debug.Log("Entro 1");
                if (wiimote.pressedWiimoteButtonHome(wii_controller))
                {
                    Debug.Log("Entro 2");
                    //PlayerPrefs.SetInt("P2", 0);
                    PlayerPrefs.SetInt("player2", 0);
                    activateElements(true);
                    Destroy(gameObject);

                }
            }
        }
        else if (engine.getActivePlayers() != 0)
        {
            if (joystick.ToString().Equals("PC"))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PlayerPrefs.SetInt("player2", 0);
                    activateElements(true);

                    // Set the army for the second controller
                    MouseController controller = pointer.GetComponent<MouseController>();
                    controller.setFirstWeapon("Revolver");
                    controller.setSecondWeapon("SMG");
                    controller.setPlayer("2P");

                    Destroy(gameObject);

                }
            }
            else if (joystick.ToString().Equals("PS3"))
            {
                Debug.Log("Pulso START " + ps3.isPS3Start("joystick 2"));
                if (ps3.isPS3Start("joystick 2"))
                {
                    PlayerPrefs.SetInt("player2", 0);
                    activateElements(true);

                    PS3Controller controller = pointer.GetComponent<PS3Controller>();
                    controller.setPause(true);      // <-- Set pause to avoid the pause controller.
                    controller.setFirstWeapon("Revolver");
                    controller.setSecondWeapon("SMG");
                    controller.setPlayer("2P");

                    Destroy(gameObject);
                }
            }
            else if (joystick.ToString().Equals("Wii"))
            {

                if (wiimote.pressedWiimoteButtonHome(wii_controller))
                {

                    //PlayerPrefs.SetInt("P2", 0);
                    PlayerPrefs.SetInt("player2", 0);
                    activateElements(true);
                    Destroy(gameObject);

                }
            }
        }
        else
        {
            int childs = player.transform.childCount;

            player.SetActive(true);

            for (int i = 0; i < childs; i++)
            {
                GameObject obj = player.transform.GetChild(i).gameObject;
                if (obj.name != "Continue")
                    obj.SetActive(false);
                else
                    obj.SetActive(true);
            }

            Destroy(gameObject);
        }
	}

    public void activateElements(bool activate)
    {
        /*life.SetActive(activate);
        pointer.SetActive(activate);
        reload.SetActive(activate);
        grenades.SetActive(activate);*/

        player.SetActive(activate);
        pointer.SetActive(activate);
        //Debug.Log("****Activado******");
        //pointer = GameObject.FindGameObjectWithTag("Pointer_" + controller);
        
        if (!isMenu)
            avatar.SetActive(activate);
    }


}

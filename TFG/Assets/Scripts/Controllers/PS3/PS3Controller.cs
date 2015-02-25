/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PS3Controller : MonoBehaviour {

    private PS3 controller;
    private Weapon gun;

    private RectTransform target_box;
    private Image targetTexture;

    private Transform _transform;

    private string controller_id;

    public int sensibility = 20;

    private int targetWidth;
    private int targetHeight;

    // Events states
    private bool pressedUp = false;
    private bool pressedDown = false;
    private bool shoot = false;
    private bool reload = false;
    private bool grenade = false;

    // Debug
    public bool debug = true;
    public bool returnCenter = true;
    public bool sixaxis = false;
    public int joystick = 0;

	// Use this for initialization
	void Start () {

        controller = new PS3();
        for (int i = 0; i < Input.GetJoystickNames().Length; i++ )
            Debug.Log("Controllers: " + Input.GetJoystickNames()[i]);
        controller_id = "joystick " + joystick;// + controller.getCountControllers();
        Debug.Log("Controllers: " + Input.GetJoystickNames().Length);
        Debug.Log("Controller ID: " + controller_id);

        //gun = AddComponent<HandGun>();
        gameObject.AddComponent<HandGun>();
        gun = GetComponent<HandGun>();
        gun.weaponInit();

        this.targetTexture = GetComponent<Image>();
        this.targetTexture.overrideSprite = gun.pointerTexture;

        this.targetWidth = targetTexture.mainTexture.width;
        this.targetHeight = targetTexture.mainTexture.height;

        this.target_box = GetComponent<RectTransform>();
        /*this.target_box.position = new Vector3(Screen.width, Screen.height, 0);
        //this.target_box.sizeDelta = new Vector2(targetWidth, targetHeight);
        //this.target_box.sizeDelta = new Vector2(Screen.width, Screen.height);
        this.target_box.anchorMin = new Vector2(.5f, .5f);
        this.target_box.anchorMax = new Vector2(.5f, .5f);
        this.target_box.pivot = new Vector2(.5f, .5f);*/

        _transform = this.target_box;

        

	}
	
	// Update is called once per frame
	void Update () {

        if (!sixaxis)
            movementStick();
        else
            movementSixaxis();

        eventControllerKeys();
	}

    void OnGUI() {

        if(debug)
        { 
            GUI.Label(new Rect(10, 10, 150, 25), "X pressed: " + controller.isPS3Cross(controller_id));
            GUI.Label(new Rect(10, 30, 150, 25), "[] pressed: " + controller.isPS3Square(controller_id));
            GUI.Label(new Rect(10, 50, 150, 25), "/_\\ pressed: " + controller.isPS3Triangle(controller_id));
            GUI.Label(new Rect(10, 70, 150, 25), "O pressed: " + controller.isPS3Circle(controller_id));
            GUI.Label(new Rect(10, 90, 150, 25), "L1 pressed: " + controller.isPS3L1(controller_id));
            GUI.Label(new Rect(10, 110, 150, 25), "R1 pressed: " + controller.isPS3R1(controller_id));
            GUI.Label(new Rect(10, 130, 150, 25), "L2 pressed: " + controller.isPS3L2(controller_id));
            GUI.Label(new Rect(10, 150, 150, 25), "R2 pressed: " + controller.isPS3R2(controller_id));
            GUI.Label(new Rect(10, 170, 150, 25), "SELECT pressed: " + controller.isPS3Select(controller_id));
            GUI.Label(new Rect(10, 190, 150, 25), "L3 pressed: " + controller.isPS3L3(controller_id));
            GUI.Label(new Rect(10, 210, 150, 25), "R3 pressed: " + controller.isPS3R3(controller_id));
            GUI.Label(new Rect(10, 230, 150, 25), "START pressed: " + controller.isPS3Start(controller_id));
            GUI.Label(new Rect(10, 250, 150, 25), "HOME pressed: " + controller.isPS3Home(controller_id));
            GUI.Label(new Rect(10, 270, 150, 25), "UP pressed: " + controller.isPS3Up(controller_id));
            GUI.Label(new Rect(10, 290, 150, 25), "LEFT pressed: " + controller.isPS3Left(controller_id));
            GUI.Label(new Rect(10, 310, 150, 25), "DOWN pressed: " + controller.isPS3Down(controller_id));
            GUI.Label(new Rect(10, 330, 150, 25), "RIGHT pressed: " + controller.isPS3Right(controller_id));
            GUI.Label(new Rect(10, 350, 150, 25), "STICK-X: " + controller.getPS3LeftStickX(controller_id));
            GUI.Label(new Rect(10, 370, 150, 25), "STICK-Y: " + controller.getPS3LeftStickY(controller_id));
            GUI.Label(new Rect(10, 390, 150, 25), "STICK+X: " + controller.getPS3RightStickX(controller_id));
            GUI.Label(new Rect(10, 410, 150, 25), "STICK+Y: " + controller.getPS3RightStickY(controller_id));
            GUI.Label(new Rect(10, 430, 250, 25), "SIXAXIS-X: " + controller.getPS3SixaxisX(controller_id));
            GUI.Label(new Rect(10, 450, 250, 25), "SIXAXIS-Y: " + controller.getPS3SixaxisY(controller_id));
            GUI.Label(new Rect(10, 470, 400, 25), "Num. Controllers: " + controller.getCountControllers());
        }

    }

    private void movementStick()
    {

        float x = _transform.position.x;
        float y = _transform.position.y;

        if (returnCenter)
        {
            x = Mathf.Clamp((controller.getPS3LeftStickX(controller_id) * Screen.width * .5f) + Screen.width * .5f, 0, Screen.width);
            y = Mathf.Clamp((controller.getPS3LeftStickY(controller_id) * Screen.height * .5f) + Screen.height * .5f, 0, Screen.height);
        }
        else
        {
            x = Mathf.Clamp(x + (controller.getPS3LeftStickX(controller_id) * sensibility) /*+ Screen.width * .5f*/, 0, Screen.width);
            y = Mathf.Clamp(y + (controller.getPS3LeftStickY(controller_id) * sensibility) /*+ Screen.height * .5f*/, 0, Screen.height);
       
        }
        
        _transform.position = new Vector3(x, y, 0);
    }

    private void movementSixaxis() 
    {
        float x = _transform.position.x;
        float y = _transform.position.y;

        if (returnCenter)
        {
            x = Mathf.Clamp((controller.getPS3SixaxisX(controller_id) * Screen.width * .5f) + Screen.width * .5f, 0, Screen.width);
            y = Mathf.Clamp((controller.getPS3SixaxisY(controller_id) * Screen.height * .5f) + Screen.height * .5f, 0, Screen.height);
        }
        else
        {
            x = Mathf.Clamp(x + (controller.getPS3SixaxisX(controller_id) * sensibility) /*+ Screen.width * .5f*/, 0, Screen.width);
            y = Mathf.Clamp(y + (controller.getPS3SixaxisY(controller_id) * sensibility) /*+ Screen.height * .5f*/, 0, Screen.height);

        }
        

        _transform.position = new Vector3(x, y, 0);

    }

    

    private void eventControllerKeys() {

        // Event ButtonUp pressed and released. Used to set the sensibility of the controller.
        if (controller.isPS3Up(controller_id))
        {
            if (!pressedUp)
            {
                sensibility = Mathf.Clamp(sensibility + 5, 10 , 100);
                pressedUp = true;
            }

        }else if (!(controller.isPS3Up(controller_id)))
            pressedUp = false;

        // Event ButtonDown pressed and released. Used to set the sensibility of the controller.
        if (controller.isPS3Down(controller_id)){
            if (!pressedDown)
            {
                sensibility = Mathf.Clamp(sensibility - 5, 10, 100);
                pressedDown = true;
            }
        }else if (!controller.isPS3Down(controller_id))
            pressedDown = false;

        // Event ButtonL1 pressed and released. Used to shoot weapon.
        if (controller.isPS3L1(controller_id))
        {
            if (!grenade)
            {
                //Do something
                Debug.Log("Grenadeeee!!!");
                grenade = true;
            }
        }
        else if (!controller.isPS3L1(controller_id))
            grenade = false;

        // Event R1 pressed and released. Used to throw a grenade.
        if (controller.isPS3R1(controller_id))
        {
            if (!shoot)
            {
                //Do something
                gun.weaponFire(_transform.position);
                Debug.Log("Shoot!!!");
                shoot = true;
            }
        }
        else if (!controller.isPS3R1(controller_id))
            shoot = false;

        // Event R2 pressed and released. Used to reload weapon.
        if (controller.isPS3R2(controller_id))
        {
            if (!reload)
            {
                //Do something
                gun.weaponReload();
                Debug.Log("Reload!!!");
                reload = true;
            }
        }
        else if (!controller.isPS3R2(controller_id))
            reload = false;

    }

}

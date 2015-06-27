/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PS3Controller : Controller {

    [Tooltip("Name of first weapon.")]
    public string firstWeapon;
    [Tooltip("Name of second weapon.")]
    public string secondWeapon;
    [Tooltip("Show a Debug controller.")]
    public bool debug = true;
    [Tooltip("Pointer return automatically to center.")]
    public bool returnCenter = true;
    [Tooltip("Enable sixaxis.")]
    public bool sixaxis = false;
    [Tooltip("sensibility of controller.")]
    public int sensibility = 20;
    [Tooltip("Controller number.")]
    public int joystick = 0;

    private GameObject obj;

    private Engine engine;
    private PauseEngine pause;
    private GameOverEngine gameover;
    private SelectPathEngine path;
    private AreaClearEngine clear;

    private PS3 ps3;
    private string controller;
    private string controller_id;

    private Image pointer;

    private string weapon;
    private Weapon firstGun;
    private Weapon secondGun;
    //private bool shoot = false; <-- Echar un ojo

    //private GameObject reload;

    //private GameObject[] avatar;
    private GameObject avatar;
    private AvatarController ac;
    //private Animation[] anim;
    private Animation anim;
    private List<string> anim_action;

    private Transform _transform;

    // Events states
    private bool pressedUp = false;
    private bool pressedDown = false;
    private bool pressedHOME = false;
    private bool isShoot = false;
    private bool isReload = false;
    private bool grenade = false;
    private bool change_weapon = false;
    private bool isPaused = false;

    private bool GamePause = false;

    /// <summary>
    /// Load the 'Engine' scene and get the controller and assign the weapons to the player.
    /// </summary>
    void Awake()
    {


        obj = GameObject.FindGameObjectWithTag("Engine");
        engine = obj.GetComponent<Engine>();
        pause = obj.GetComponent<PauseEngine>();
        gameover = obj.GetComponent<GameOverEngine>();
        path = obj.GetComponent<SelectPathEngine>();
        clear = obj.GetComponent<AreaClearEngine>();
        //engine.setNumPlayers(controller);
        
        /*controller = joystick + "P";
        controller_id = "joystick " + joystick;
        
        Debug.Log("ps3 Controller: " + controller);
        
        obj = GameObject.FindGameObjectWithTag("Engine");
        engine = obj.GetComponent<Engine>();
        //engine.setNumPlayers(controller);
        engine.setActivePlayers(1);

        setPlayer(controller);*/
    }

	// Use this for initialization
	void Start () {

        Screen.showCursor = false;

        controller = joystick + "P";
        controller_id = "joystick " + joystick;
        engine.setActivePlayers(1);
        setPlayer(controller);

        ps3 = new PS3();

        for (int i = 0; i < Input.GetJoystickNames().Length; i++ )
            Debug.Log("***Controllers: " + Input.GetJoystickNames()[i]);

        /*reload = GameObject.FindGameObjectWithTag("Reload_" + controller);
        reload.SetActive(false);*/

        pointer = gameObject.GetComponent<Image>();
        pointer.color = (this.controller == "1P") ? new Color(.651f, 0, 0) : new Color(0, 0, .651f);

        this.avatar = GameObject.FindGameObjectWithTag("Avatar_" + controller);

        ac = avatar.GetComponent<AvatarController>();
        ac.setPlayer(controller);
        ac.setLifeBar(controller);
        //ac.setReloadPanel(reload);

        this.addWeapon(firstWeapon, secondWeapon);
        weapon = firstWeapon;

        this.anim = avatar.GetComponent<Animation>();
        this.anim_action = new List<string>();

        foreach (AnimationState action in anim)
            anim_action.Add(action.name);

        anim[anim_action[0]].speed = firstGun.reload_speed;
        anim[anim_action[1]].speed = secondGun.reload_speed;

        gameObject.AddComponent<AudioSource>();
        _transform = transform;

	}
	
	// Update is called once per frame
	void Update () {

        if (!GamePause)
        {
            if (!sixaxis)
                movementStick();
            else
                movementSixaxis();

            eventControllerKeys();
        }
	}

    void OnGUI() {

        if(debug)
        {
            GUI.Label(new Rect(10, 10, 150, 25), "X pressed: " + ps3.isPS3Cross(controller_id));
            GUI.Label(new Rect(10, 30, 150, 25), "[] pressed: " + ps3.isPS3Square(controller_id));
            GUI.Label(new Rect(10, 50, 150, 25), "/_\\ pressed: " + ps3.isPS3Triangle(controller_id));
            GUI.Label(new Rect(10, 70, 150, 25), "O pressed: " + ps3.isPS3Circle(controller_id));
            GUI.Label(new Rect(10, 90, 150, 25), "L1 pressed: " + ps3.isPS3L1(controller_id));
            GUI.Label(new Rect(10, 110, 150, 25), "R1 pressed: " + ps3.isPS3R1(controller_id));
            GUI.Label(new Rect(10, 130, 150, 25), "L2 pressed: " + ps3.isPS3L2(controller_id));
            GUI.Label(new Rect(10, 150, 150, 25), "R2 pressed: " + ps3.isPS3R2(controller_id));
            GUI.Label(new Rect(10, 170, 150, 25), "SELECT pressed: " + ps3.isPS3Select(controller_id));
            GUI.Label(new Rect(10, 190, 150, 25), "L3 pressed: " + ps3.isPS3L3(controller_id));
            GUI.Label(new Rect(10, 210, 150, 25), "R3 pressed: " + ps3.isPS3R3(controller_id));
            GUI.Label(new Rect(10, 230, 150, 25), "START pressed: " + ps3.isPS3Start(controller_id));
            GUI.Label(new Rect(10, 250, 150, 25), "HOME pressed: " + ps3.isPS3Home(controller_id));
            GUI.Label(new Rect(10, 270, 150, 25), "UP pressed: " + ps3.isPS3Up(controller_id));
            GUI.Label(new Rect(10, 290, 150, 25), "LEFT pressed: " + ps3.isPS3Left(controller_id));
            GUI.Label(new Rect(10, 310, 150, 25), "DOWN pressed: " + ps3.isPS3Down(controller_id));
            GUI.Label(new Rect(10, 330, 150, 25), "RIGHT pressed: " + ps3.isPS3Right(controller_id));
            GUI.Label(new Rect(10, 350, 150, 25), "STICK-X: " + ps3.getPS3LeftStickX(controller_id));
            GUI.Label(new Rect(10, 370, 150, 25), "STICK-Y: " + ps3.getPS3LeftStickY(controller_id));
            GUI.Label(new Rect(10, 390, 150, 25), "STICK+X: " + ps3.getPS3RightStickX(controller_id));
            GUI.Label(new Rect(10, 410, 150, 25), "STICK+Y: " + ps3.getPS3RightStickY(controller_id));
            GUI.Label(new Rect(10, 430, 250, 25), "SIXAXIS-X: " + ps3.getPS3SixaxisX(controller_id));
            GUI.Label(new Rect(10, 450, 250, 25), "SIXAXIS-Y: " + ps3.getPS3SixaxisY(controller_id));
            GUI.Label(new Rect(10, 470, 400, 25), "Num. Controllers: " + ps3.getCountControllers());
        }

    }

    private void movementStick()
    {

        float x = _transform.position.x;
        float y = _transform.position.y;

        if (returnCenter)
        {
            x = Mathf.Clamp((ps3.getPS3LeftStickX(controller_id) * Screen.width * .5f) + Screen.width * .5f, 0, Screen.width);
            y = Mathf.Clamp((ps3.getPS3LeftStickY(controller_id) * Screen.height * .5f) + Screen.height * .5f, 0, Screen.height);
        }
        else
        {
            x = Mathf.Clamp(x + (ps3.getPS3LeftStickX(controller_id) * sensibility), 0, Screen.width);
            y = Mathf.Clamp(y + (ps3.getPS3LeftStickY(controller_id) * sensibility), 0, Screen.height);     
        }
        
        _transform.position = new Vector3(x, y, 0);
    }

    private void movementSixaxis() 
    {
        float x = _transform.position.x;
        float y = _transform.position.y;

        if (returnCenter)
        {
            x = Mathf.Clamp((ps3.getPS3SixaxisX(controller_id) * Screen.width * .5f) + Screen.width * .5f, 0, Screen.width);
            y = Mathf.Clamp((ps3.getPS3SixaxisY(controller_id) * Screen.height * .5f) + Screen.height * .5f, 0, Screen.height);
        }
        else
        {
            x = Mathf.Clamp(x + (ps3.getPS3SixaxisX(controller_id) * sensibility) /*+ Screen.width * .5f*/, 0, Screen.width);
            y = Mathf.Clamp(y - (ps3.getPS3SixaxisY(controller_id) * sensibility) /*+ Screen.height * .5f*/, 0, Screen.height);

        }
        

        _transform.position = new Vector3(x, y, 0);

    }

    

    private void eventControllerKeys() {

        bool isClear = clear.getClear();
        bool isPause = pause.getPause();
        bool isAlive = gameover.getPlayerAlive(controller);
        bool isGameOver = gameover.getPlayerGameOver(controller);
        bool isSelectPath = (path != null) ? path.isSelection() : false;

        /**************************************************************/
        /****                 UP --> SENSIBILITY +                *****/
        /**************************************************************/

        // Event ButtonUp pressed and released. Used to set the sensibility of the controller.
        if (ps3.isPS3Up(controller_id))
        {
            if (!pressedUp)
            {
                sensibility = Mathf.Clamp(sensibility + 5, 10 , 100);
                pressedUp = true;
            }

        }
        else if (!(ps3.isPS3Up(controller_id)))
            pressedUp = false;


        /**************************************************************/
        /****                DOWN --> SENSIBILITY -               *****/
        /**************************************************************/

        // Event ButtonDown pressed and released. Used to set the sensibility of the controller.
        if (ps3.isPS3Down(controller_id))
        {
            if (!pressedDown)
            {
                sensibility = Mathf.Clamp(sensibility - 5, 10, 100);
                pressedDown = true;
            }
        }
        else if (!ps3.isPS3Down(controller_id))
            pressedDown = false;


        if (ps3.isPS3Home(controller_id))
        {
            if (!pressedHOME)
            {
                sixaxis = !sixaxis;
                pressedHOME = true;
            }
        }
        else if (!ps3.isPS3Home(controller_id))
            pressedHOME = false;

        if (!isClear && !isPause && isAlive && !isSelectPath)
        {
            /**************************************************************/
            /****                    L1 --> GRENADE                   *****/
            /**************************************************************/

            // Event ButtonL1 pressed and released. Used to shoot weapon.
            if (ps3.isPS3L1(controller_id))
            {
                if (!grenade)
                {
                    //Do something
                    Debug.Log("Grenadeeee!!!");
                    grenade = true;
                }
            }
            else if (!ps3.isPS3L1(controller_id))
                grenade = false;


            /**************************************************************/
            /****               L2 --> CHANGE WEAPON                  *****/
            /**************************************************************/

            // Event ButtonL2 pressed and released. Used to shoot weapon.
            if (ps3.isPS3L2(controller_id))
            {
                if (!change_weapon)
                {
                    //Do something
                    switchWeapon();
                    Debug.Log("Change Weapon!!!");
                    change_weapon = true;
                }
            }
            else if (!ps3.isPS3L2(controller_id))
                change_weapon = false;


            /**************************************************************/
            /****                    R1 --> SHOOT                     *****/
            /**************************************************************/

            // Event R1 pressed and released. Used to throw a grenade.
            if (!isShoot && weapon.Equals("SMG") && ps3.isPS3R1(controller_id))
            {
                isShoot = true;
                secondGun.weaponFire(_transform.position);
                StartCoroutine(Delay(getWeapon().shoot_speed));
            }
            else if (ps3.isPS3R1(controller_id))
            {
                if (!isShoot)
                {
                    //Do something
                    //gun.weaponFire(_transform.position);
                    if (firstGun.enabled)
                        firstGun.weaponFire(_transform.position);
                    else if (secondGun.enabled)
                        secondGun.weaponFire(_transform.position);

                    Debug.Log("Shoot!!!");
                    isShoot = true;
                }
            }
            else if (!ps3.isPS3R1(controller_id))
                isShoot = false;


            /**************************************************************/
            /****                    R2 --> RELOAD                    *****/
            /**************************************************************/


            // Event R2 pressed and released. Used to reload weapon.
            if (ps3.isPS3R2(controller_id))
            {
                if (!isReload)
                {
                    //Do something
                    if (firstGun.enabled)
                    {
                        firstGun.setReload(true);
                        anim.Play(anim_action[0]);
                    }
                    else if (secondGun.enabled)
                    {
                        secondGun.setReload(true);
                        anim.Play(anim_action[1]);
                    }

                    //gun.weaponReload(true);
                    Debug.Log("Reload!!!");
                    isReload = true;
                }
            }
            else if (!ps3.isPS3R2(controller_id))
                isReload = false;
        }
        else
        {
            if (ps3.isPS3R1(controller_id))
            {
                if (!isShoot)
                {
                    isShoot = true;
                    PointerEventData pointer = new PointerEventData(EventSystem.current);
                    pointer.position = _transform.position;

                    List<RaycastResult> raycastResults = new List<RaycastResult>();
 
                    EventSystem.current.RaycastAll(pointer, raycastResults);

                    if (raycastResults.Count > 0)
                    {
                        for (int i = 0; i < raycastResults.Count; i++)
                        {
                            string option = raycastResults[i].gameObject.transform.name;
                            Options op = raycastResults[i].gameObject.GetComponent<Options>();
                            if (op != null)
                                if (!isSelectPath)
                                    op.ExecuteOption(controller, option);
                                else
                                    op.ExecuteOptionPath(option);
                        }
                    }                   
                }
            }
            else if (!ps3.isPS3R1(controller_id))
                isShoot = false;
        }


        /**************************************************************/
        /****                  START --> PAUSE                    *****/
        /**************************************************************/

        // Event ButtonL2 pressed and released. Used to shoot weapon.
        if (ps3.isPS3Start(controller_id))
        {
            if (!isPaused)
            {
                /*if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                    //GamePause = false;
                }
                else
                {
                    Time.timeScale = 0;
                    //GamePause = true;
                }*/
                if (!isGameOver && !isAlive)
                    gameover.Continue(controller);
                else if (!isGameOver)
                    pause.setPause(controller);

                Debug.Log("PAUSE GAME!!!");
                isPaused = true;
            }
        }
        else if (!ps3.isPS3Start(controller_id))
            isPaused = false;

    }


    /// <summary>
    /// Assign the weapons player.
    /// </summary>
    /// <param name="firstWeapon">Name of the first weapon</param>
    /// <param name="secondWeapon">Name of the second weapon</param>
    void addWeapon(string firstWeapon, string secondWeapon)
    {
        Debug.Log("IsTrue? " + firstWeapon.Equals("Handgun"));
        if (firstWeapon.Equals("Handgun"))
        {
            gameObject.AddComponent<Handgun>();
            firstGun = GetComponent<Handgun>();
            firstGun.weaponInit(ac);
        }

        if (firstWeapon.Equals("Revolver"))
        {
            gameObject.AddComponent<Revolver>();
            firstGun = GetComponent<Revolver>();
            firstGun.weaponInit(ac);
        }

        if (secondWeapon.Equals("Shotgun"))
        {
            gameObject.AddComponent<Shotgun>();
            secondGun = GetComponent<Shotgun>();
            secondGun.enabled = false;
        }

        if (secondWeapon.Equals("SMG"))
        {
            gameObject.AddComponent<SMG>();
            secondGun = GetComponent<SMG>();
            secondGun.enabled = false;
        }

    }


    /// <summary>
    /// Change weapon.
    /// </summary>
    public void switchWeapon()
    {
        if (firstGun.enabled)
        {
            firstGun.weaponEmpty();
            firstGun.enabled = false;
        }
        else
        {
            firstGun.setReload(true);
            anim.Play(anim_action[0]);
            firstGun.enabled = true;
            firstGun.weaponInit(ac);
            weapon = firstWeapon;
        }

        if (secondGun.enabled)
        {
            secondGun.weaponEmpty();
            secondGun.enabled = false;
        }
        else
        {
            secondGun.setReload(true);
            anim.Play(anim_action[1]);
            secondGun.enabled = true;
            secondGun.weaponInit(ac);
            weapon = secondWeapon;
        }

        Debug.Log("Your current weapon is a " + weapon);
    }


    public override void setFirstWeapon(string weapon)
    {
        this.firstWeapon = weapon;
    }


    public override void setSecondWeapon(string weapon)
    {
        this.secondWeapon = weapon;
    }

    /// <summary>
    /// Get the current weapon.
    /// </summary>
    /// <returns>Returns a object type Weapon.</returns>
    public override Weapon getWeapon()
    {
        if (secondGun.enabled)
            return secondGun;

        return firstGun;

    }


    public override void setPlayer(string player)
    {
        this.controller = player;
    }


    public string getPlayer()
    {
        return this.controller;
    }

    public void setPause(bool pause)
    {
        this.isPaused = pause;
    }


    private IEnumerator Delay(float delay)
    {

        for (float time = delay; time > 0; time -= Time.deltaTime)
        {
            yield return 0;
        }

        isShoot = false;

    }

    void OnDestroy()
    {
        // Free all coroutines.
        StopAllCoroutines();
    }

}

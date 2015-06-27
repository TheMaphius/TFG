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
using System.Runtime.InteropServices;

public class WiimoteController : Controller {

    [Tooltip("Name of first weapon.")]
    public string firstWeapon;
    [Tooltip("Name of second weapon.")]
    public string secondWeapon;
    [Tooltip("sensibility of controller.")]
    public int sensibility;
    [Tooltip("Controller number.")]
    public int joystick;

    private GameObject obj;

    private Engine engine;
    private PauseEngine pause;
    private GameOverEngine gameover;
    private SelectPathEngine path;
    private AreaClearEngine clear;

    private Wiimote wiimote;
    private string controller;

    //private int accuracy = 5;

    private bool isAvailable;
    private bool isNunchuck;
    private bool isIR;

    private Image pointer;
    private int widthPointer;
    private int heightPointer;

    private string weapon;
    private Weapon firstGun;
    private Weapon secondGun;
    //private bool shoot = false; <-- Echar un ojo

    //private GameObject reload;

    //private GameObject[] avatar;
    private GameObject avatar;
    private AvatarController ac;
    private Animation anim;
    private List<string> anim_action;

    private Transform _transform;

    // Events states
    private bool pressedUp = false;
    private bool pressedDown = false;
    private bool isShoot = false;
    private bool isReload = false;
    private bool isGrenade = false;
    private bool isAdjust = false;
    private bool change_weapon = false;
    private bool isPaused = false;
    private bool GamePause = false;

    /// <summary>
    /// Load the 'Engine' scene and get the controller and assign the weapons to the player.
    /// </summary>
    void Awake()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 70;    
        
        obj = GameObject.FindGameObjectWithTag("Engine");
        engine = obj.GetComponent<Engine>();
        pause = obj.GetComponent<PauseEngine>();
        gameover = obj.GetComponent<GameOverEngine>();
        path = obj.GetComponent<SelectPathEngine>();
        clear = obj.GetComponent<AreaClearEngine>();
    }

    // Use this for initialization
    void Start()
    {
        
        Screen.showCursor = false;

        controller = (joystick + 1) + "P";
        engine.setActivePlayers(1);
        setPlayer(controller);

        pointer = gameObject.GetComponent<Image>();
        pointer.color = (this.controller == "1P") ? new Color(.651f, 0, 0) : new Color(0, 0, .651f);

        widthPointer = GetComponent<Image>().mainTexture.width;
        heightPointer = GetComponent<Image>().mainTexture.height;

        this.avatar = GameObject.FindGameObjectWithTag("Avatar_" + controller);

        ac = avatar.GetComponent<AvatarController>();
        ac.setPlayer(controller);
        ac.setLifeBar(controller);

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

        wiimote = new Wiimote();
        wiimote.connectWiimote();

    }

    void Update()
    {
        Controller();
    }



    void Controller()
    {
        bool isClear = clear.getClear();
        bool isPause = pause.getPause();
        bool isAlive = gameover.getPlayerAlive(controller);
        bool isGameOver = gameover.getPlayerGameOver(controller);
        bool isSelectPath = (path != null) ? path.isSelection() : false;

        this.isAvailable = wiimote.isWiimoteAvailable(joystick);
        this.isNunchuck = wiimote.isWiimoteNunchuck(joystick);
        this.isIR = wiimote.isIREnabled(joystick);

        if (isAvailable)
        {
            if (isIR)
            {
                directionIR();
            }
            else if (isNunchuck) 
            {
                Stick();
            }
            else 
            {
                //DPad();
                //Accelerometer();
                DPad();
                
            }


            if (!isClear && !isPause && isAlive && !isSelectPath)
            {

                /**************************************************************/
                /****                    B  --> SHOOT                     *****/
                /**************************************************************/

                // Event B pressed and released. Used to throw a grenade.
                if (!isShoot && weapon.Equals("SMG") && (wiimote.pressedWiimoteButtonB(joystick) || wiimote.pressedNunchuckButtonZ(joystick)))
                {
                    Shoot();
                    secondGun.weaponFire(_transform.position);
                    StartCoroutine(Delay(getWeapon().shoot_speed));
                }
                else if ((wiimote.pressedWiimoteButtonB(joystick) || wiimote.pressedNunchuckButtonZ(joystick)))
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
                        Shoot();
                    }
                }
                else if ((!wiimote.pressedWiimoteButtonB(joystick) && !wiimote.pressedNunchuckButtonZ(joystick)))
                    isShoot = false;

                /*if (wiimote.pressedWiimoteButtonB(joystick) || wiimote.pressedNunchuckButtonZ(joystick))
                    Shoot();
                else isShoot = false;*/

                /**************************************************************/
                /****                    A  --> GRENADE                   *****/
                /**************************************************************/

                // Event Button A pressed and released. Used to shoot weapon.
                if (wiimote.pressedWiimoteButtonA(joystick) || wiimote.pressedNunchuckButtonC(joystick))
                    Grenade();
                else isGrenade = false;

                /**************************************************************/
                /****               '+' or '-' --> ADJUST                 *****/
                /**************************************************************/

                // Adjust Sensibility Dpad
                if (wiimote.pressedWiimoteButtonMinus(joystick))
                    AdjustAccuracy(-1);
                else if (wiimote.pressedWiimoteButtonPlus(joystick))
                    AdjustAccuracy(1);
                else isAdjust = false;

                Reload();


                /**************************************************************/
                /****               1 --> CHANGE WEAPON                  *****/
                /**************************************************************/
                
                // Event Button 1 pressed and released. Used to shoot weapon.
                if (wiimote.pressedWiimoteButton1(joystick))
                {
                    if (!change_weapon)
                    {
                        //Do something
                        switchWeapon();
                        Debug.Log("Change Weapon!!!");
                        change_weapon = true;
                    }
                }
                else if (!wiimote.pressedWiimoteButton1(joystick))
                    change_weapon = false;


                /**************************************************************/
                /****                    R1 --> SHOOT                     *****/
                /**************************************************************/

                // Event R1 pressed and released. Used to throw a grenade.
               /* if (!isShoot && weapon.Equals("SMG") && ps3.isPS3R1(controller_id))
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
                    isShoot = false;*/


                
            }
            else
            {
                if ((wiimote.pressedWiimoteButtonB(joystick) || wiimote.pressedNunchuckButtonZ(joystick)))
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
                else if (!(wiimote.pressedWiimoteButtonB(joystick) || wiimote.pressedNunchuckButtonZ(joystick)))
                    isShoot = false;
            }
            
        }

        /**************************************************************/
        /****                  START --> PAUSE                    *****/
        /**************************************************************/

        // Event ButtonL2 pressed and released. Used to shoot weapon.
        if (wiimote.pressedWiimoteButtonHome(joystick))
        {
            if (!isPaused)
            {
                if (!isGameOver && !isAlive)
                    gameover.Continue(controller);
                else if (!isGameOver)
                    pause.setPause(controller);

                Debug.Log("PAUSE GAME!!!");
                isPaused = true;
            }
        }
        else if (!wiimote.pressedWiimoteButtonHome(joystick))
            isPaused = false;

    }

    // Method that controls directional pad direction.
    private void DPad() 
    {
        /*if (wiimote.pressedWiimoteUp(player))
            this.target_box.y = Mathf.Clamp(this.target_box.y - sensibility, (this.targetTexture.height * -.5f), Screen.height);

        if (wiimote.pressedWiimoteDown(player))
            this.target_box.y = Mathf.Clamp(this.target_box.y + sensibility, 0, Screen.height - (this.targetTexture.height * .5f));

        if (wiimote.pressedWiimoteLeft(player))
            this.target_box.x = Mathf.Clamp(this.target_box.x - sensibility, (this.targetTexture.width * -.5f), Screen.width + (this.targetTexture.width * -.5f));

        if (wiimote.pressedWiimoteRight(player))
            this.target_box.x = Mathf.Clamp(this.target_box.x + sensibility, (this.targetTexture.width * -.5f), Screen.width + (this.targetTexture.width * -.5f));
        */

        float x = _transform.position.x;
        float y = _transform.position.y;

        if (wiimote.pressedWiimoteLeft(joystick))
            x = Mathf.Clamp(_transform.position.x - (sensibility * 5), 0, Screen.width + (widthPointer * -.5f));

        else if (wiimote.pressedWiimoteRight(joystick))
            x = Mathf.Clamp(_transform.position.x + (sensibility * 5), 0, Screen.width);

        if (wiimote.pressedWiimoteUp(joystick))
            y = Mathf.Clamp(_transform.position.y + (sensibility * 5), (heightPointer * -.5f), Screen.height);

        else if (wiimote.pressedWiimoteDown(joystick))
            y = Mathf.Clamp(_transform.position.y - (sensibility * 5), 0, Screen.height);

        _transform.position = new Vector3(x, y, 0);

    }

    // Method that controls stick direction.
    private void Stick()
    {

        /*if (wiimote.getWiimoteNunchuckX(player) <= 120)
            this.target_box.x = Mathf.Clamp(this.target_box.x - sensibility, (this.targetTexture.width * -.5f), Screen.width);
        
        if (wiimote.getWiimoteNunchuckX(player) >= 150)
            this.target_box.x = Mathf.Clamp(this.target_box.x + sensibility, 0, Screen.width - (this.targetTexture.width * .5f));


        if (wiimote.getWiimoteNunchuckY(player) <= 120)
            this.target_box.y = Mathf.Clamp(this.target_box.y + sensibility, (this.targetTexture.height * -.5f), Screen.height + (this.targetTexture.height * -.5f));
        
        if (wiimote.getWiimoteNunchuckY(player) >= 145)
            this.target_box.y = Mathf.Clamp(this.target_box.y - sensibility, (this.targetTexture.height * -.5f), Screen.height + (this.targetTexture.height * -.5f));
        */
        float x = _transform.position.x;
        float y = _transform.position.y;

        if (wiimote.getWiimoteNunchuckX(joystick) <= 120)
            x = Mathf.Clamp(_transform.position.x - (sensibility * 5), 0, Screen.width + (widthPointer * -.5f));

        else if (wiimote.getWiimoteNunchuckX(joystick) >= 150)
            x = Mathf.Clamp(_transform.position.x + (sensibility * 5), 0, Screen.width);

        if (wiimote.getWiimoteNunchuckY(joystick) <= 120)
            y = Mathf.Clamp(_transform.position.y - (sensibility * 5), 0, Screen.height);

        else if (wiimote.getWiimoteNunchuckY(joystick) >= 145)
            y = Mathf.Clamp(_transform.position.y + (sensibility * 5), 0, Screen.height);

        _transform.position = new Vector3(x, y, 0);
    }

    private void Accelerometer()
    {
        float x = _transform.position.x;
        float y = _transform.position.y;

        if (wiimote.getWiimoteRoll(joystick) <= -10)
            x = Mathf.Clamp(_transform.position.x - (sensibility * 5), 0, Screen.width + (widthPointer * -.5f));

        else if (wiimote.getWiimoteRoll(joystick) >= 10)
            x = Mathf.Clamp(_transform.position.x + (sensibility * 5), 0, Screen.width);

        if (wiimote.getWiimotePitch(joystick) <= -5)
            y = Mathf.Clamp(_transform.position.y + (sensibility * 5), 0, Screen.height);

        else if (wiimote.getWiimotePitch(joystick) >= 5)
            y = Mathf.Clamp(_transform.position.y - (sensibility * 5), 0, Screen.height);

        _transform.position = new Vector3(x, y, 0);
    }

    private void directionIR()
    {
        float x = _transform.position.x;
        float y = _transform.position.y;

        float irX = wiimote.getWiiMoteIRX(joystick);
        float irY = wiimote.getWiiMoteIRY(joystick);
        
        if( irX != -100)
            x = Mathf.Clamp((wiimote.getWiiMoteIRX(joystick) * Screen.width * .5f) + Screen.width * .5f, 0, Screen.width);
        
        if(irY != -100)
            y = Mathf.Clamp((wiimote.getWiiMoteIRY(joystick) * Screen.height * .5f) + Screen.height * .5f, 0, Screen.height);

        _transform.position = new Vector3(x, y, 0);

    }

    // Call method Shoot into the screen.
    private void Shoot()
    {
        if (!isShoot)
        {
            isShoot = true;
            wiimote.shakeWiimote(joystick, 0.075f);
            print("Shoot");
        }
    }

    // Call method throw Grenade.
    private void Grenade()
    {
        if (!isGrenade)
        {
            isGrenade = true;
            print("Throw grenade");
        }
        
    }

    private void AdjustAccuracy(int i) 
    {

        if (!isAdjust)
        {
            if (i < 0 && (sensibility + i) != 0)
                sensibility--;
            else if (i > 0 && (sensibility + i) != 11)
                sensibility++;

            isAdjust = true;
            print("Accuracy: " + sensibility);
        }
    }

    //float min = 122;
    //float max = 122;
    // Call method 'Reload' bullets.
    private void Reload()
    {
        byte reload = (byte)wiimote.getWiimoteAccelerometer(joystick).y;

        if (reload >= 175)
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
                StartCoroutine(Reload(1));
            }
            print("Reload: " + reload);
        }
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

    private IEnumerator Reload(float delay)
    {
        yield return new WaitForSeconds(delay);
        isReload = false;
    }

    void OnDestroy()
    {
        // Free all coroutines.
        StopAllCoroutines();
    }

    void OnApplicationQuit()
    {
        wiimote.disconnectWiimote();
    }
}

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

public class MouseController : Controller {

    [Tooltip("Name of first weapon.")]
    public string firstWeapon;
    [Tooltip("Name of second weapon.")]
    public string secondWeapon;
    [Tooltip("Player controller 1P or 2P.")]
    public string controller;
    [Tooltip("Set speed controller 2P.")]
    public float controller_speed = 20f;

    private Engine engine;
    private PauseEngine pause;
    private GameOverEngine gameover;
    private SelectPathEngine path;
    private AreaClearEngine clear;

    private Image pointer;

    private string weapon;
    private Weapon firstGun;
    private Weapon secondGun;
    private bool shoot = false;

    //private GameObject reload;

    //private GameObject[] avatar;
    private GameObject avatar;
    private AvatarController ac;
    //private Animation[] anim;
    private Animation anim;
    private List<string> anim_action;

    private Transform _transform;

    private int wheel = 0;
    private bool rollWheel = false;

    /// <summary>
    /// Load the 'Engine' scene and get the controller and assign the weapons to the player.
    /// </summary>
    void Awake() 
    {
        setPlayer(controller);
        
        GameObject obj = GameObject.FindGameObjectWithTag("Engine");
        
        engine = obj.GetComponent<Engine>();
        pause = obj.GetComponent<PauseEngine>();
        gameover = obj.GetComponent<GameOverEngine>();
        path = obj.GetComponent<SelectPathEngine>();
        clear = obj.GetComponent<AreaClearEngine>();

        //engine.setPlayerCredits(controller);
        engine.setActivePlayers(1);

    }


	/// <summary>
    /// Get the player avatar and load the animations and audios.
    /// </summary>
    void Start () 
    {

        Screen.showCursor = false;

        //reload = GameObject.FindGameObjectWithTag("Reload_" + controller);
        //reload.SetActive(false);

        pointer = gameObject.GetComponent<Image>();
        pointer.color = (this.controller == "1P") ? new Color(.651f, 0, 0) : new Color(0, 0, .651f);
        
        this.avatar = GameObject.FindGameObjectWithTag("Avatar_" + controller);

        ac = avatar.GetComponent<AvatarController>();
        //Debug.Log("Name: " + avatar.name);
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
        audio.mute = (PlayerPrefs.GetInt("sound") == 0) ? true : false;
        _transform = transform;

	}


    /// <summary>
    /// Engine of the PC controller.
    /// </summary>
	void Update () 
    {

        if (this.controller.Equals("1P"))
            mouseController();
        else if (this.controller.Equals("2P"))
            KeyController();

        
        
	}


    void mouseController()
    {
        bool isClear = clear.getClear();
        bool isPause = pause.getPause();
        bool isAlive = gameover.getPlayerAlive(controller);
        bool isGameOver = gameover.getPlayerGameOver(controller);
        bool isSelectPath = (path != null) ? path.isSelection() : false;

        _transform.position = Input.mousePosition;

        /*if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
         {*/
             //Debug.Log("Wheel: " + Input.GetAxis("Mouse ScrollWheel"));
        //}
        //isClear = true;
        if (!isClear && !isPause && isAlive && !isSelectPath)
        {
            
            if (!shoot && weapon.Equals("SMG") && Input.GetMouseButton(0))
            {
                shoot = true;
                engine.setPlayerShoot(controller);
                secondGun.weaponFire(Input.mousePosition);
                StartCoroutine(Delay(getWeapon().shoot_speed));
            }
            else if (Input.GetMouseButtonDown(0))
            {
                engine.setPlayerShoot(controller);
                if (firstGun.enabled)
                    firstGun.weaponFire(Input.mousePosition);
                else if (secondGun.enabled)
                    secondGun.weaponFire(Input.mousePosition);

            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (firstGun.enabled)
                {
                    firstGun.setReload(true);
                    //Debug.Log("Reload desde MouseController");
                    anim.Play(anim_action[0]);

                }
                else if (secondGun.enabled)
                {
                    secondGun.setReload(true);
                    anim.Play(anim_action[1]);
                }
            }
            else if (!rollWheel && Input.GetAxis("Mouse ScrollWheel") > 0) // Switch Weapon.
            {
                rollWheel = true;
                switchWeapon();
                Debug.Log("Change Weapon.");
                StartCoroutine(WheelDelay(1.5f));         
            }
            else if (!rollWheel && Input.GetAxis("Mouse ScrollWheel") < 0) // Throw Grenade.
            {
                rollWheel = true;
                //switchWeapon(); 
                Debug.Log("Throw Grenade");
                StartCoroutine(WheelDelay(1.5f));              
            }
        }
        else 
        {
            
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();

            if (Input.GetMouseButtonDown(0))
            {
                EventSystem.current.RaycastAll(pointer, raycastResults);

                if (raycastResults.Count > 0)
                {
                    for (int i = 0; i < raycastResults.Count; i++)
                    {
                        string option = raycastResults[i].gameObject.transform.name;
                        Options op = raycastResults[i].gameObject.GetComponent<Options>();
                        if (op != null)
                            if (!isSelectPath/* && !isAlive*/)
                                op.ExecuteOption(controller, option);
                            else/* if (isSelectPath)*/
                                op.ExecuteOptionPath(option);
                            /*else if (isAlive)
                                op.ExecuteOptionAreaClear(option);*/

                    }
                }
            }
        }

        
        if (Input.GetMouseButtonDown(2))
        {
            if (!isGameOver && !isAlive)
                gameover.Continue(controller);
            else if(!isGameOver)
                pause.setPause(controller);
            //switchWeapon();
        }
    }


    void KeyController()
    {
        bool isClear = clear.getClear();
        bool isPause = pause.getPause();
        bool isAlive = gameover.getPlayerAlive(controller);
        bool isGameOver = gameover.getPlayerGameOver(controller);
        bool isSelectPath = (path != null) ? path.isSelection() : false;

        /*if (Input.GetKey(KeyCode.W))
            _transform.position += Vector3.up * 5f;
        else if (Input.GetKey(KeyCode.S))
            _transform.position += Vector3.down * 5f;
        
        if (Input.GetKey(KeyCode.A))
            _transform.position += Vector3.left * 5f;
        else if (Input.GetKey(KeyCode.D))
            _transform.position += Vector3.right * 5f;*/
        float x = transform.position.x;
        float y = transform.position.y;

        //x = Mathf.Clamp(x + (ps3.getPS3LeftStickX(controller_id) * sensibility), 0, Screen.width);
        //y = Mathf.Clamp(y + (ps3.getPS3LeftStickY(controller_id) * sensibility), 0, Screen.height); 

        if (Input.GetKey(KeyCode.W))
            y = Mathf.Clamp((y + controller_speed), 0, Screen.height);
        //_transform.position += Vector3.up * 10f;
        else if (Input.GetKey(KeyCode.S))
            y = Mathf.Clamp((y - controller_speed), 0, Screen.height);
        //_transform.position += Vector3.down * 10f;

        if (Input.GetKey(KeyCode.A))
            x = Mathf.Clamp((x - controller_speed), 0, Screen.width);
        //_transform.position += Vector3.left * 10f;
        else if (Input.GetKey(KeyCode.D))
            x = Mathf.Clamp((x + controller_speed), 0, Screen.width);
        //_transform.position += Vector3.right * 10f;

        _transform.position = new Vector3(x, y, 0);

        if (!isClear && !isPause && isAlive && !isSelectPath)
        {
            if (!shoot && weapon.Equals("SMG") && Input.GetKey(KeyCode.Return))
            {
                shoot = true;
                engine.setPlayerShoot(controller);
                secondGun.weaponFire(_transform.position);
                StartCoroutine(Delay(getWeapon().shoot_speed));
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                engine.setPlayerShoot(controller);
                if (firstGun.enabled)
                    firstGun.weaponFire(_transform.position);
                else if (secondGun.enabled)
                    secondGun.weaponFire(_transform.position);

            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                anim.Play(anim_action[2]);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
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
            }

            if (Input.GetKeyDown("c"))
                switchWeapon();

        }
        else 
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = _transform.position;

            List<RaycastResult> raycastResults = new List<RaycastResult>();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                EventSystem.current.RaycastAll(pointer, raycastResults);

                if (raycastResults.Count > 0)
                {
                    for (int i = 0; i < raycastResults.Count; i++)
                    {
                        string option = raycastResults[i].gameObject.transform.name;
                        Options op = raycastResults[i].gameObject.GetComponent<Options>();
                        if (op != null)
                            if (!isSelectPath/* && !isAlive*/)
                                op.ExecuteOption(controller, option);
                            else/* if (isSelectPath)*/
                                op.ExecuteOptionPath(option);
                            

                    }
                }
            }
        }

        // PAUSE Menu Player 2
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            /*if (Time.timeScale == 0)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;*/
            Debug.Log("What is this controller: " + controller);
            if (!isGameOver && !isAlive)
                gameover.Continue(controller);
            else if (!isGameOver)
                pause.setPause(controller);
        }

    }


    /// <summary>
    /// Assign the weapons player.
    /// </summary>
    /// <param name="firstWeapon">Name of the first weapon</param>
    /// <param name="secondWeapon">Name of the second weapon</param>
    void addWeapon(string firstWeapon, string secondWeapon) 
    {
        //Debug.Log("IsTrue? " + firstWeapon.Equals("Handgun"));
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

        //Debug.Log("Your current weapon is a " + weapon );
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

    private IEnumerator Delay(float delay)
    {
 
        /*for (float time = delay; time > 0; time -= Time.deltaTime)
        {
            yield return 0;
        }*/
        yield return new WaitForSeconds(delay);
        shoot = false;

    }


    private IEnumerator WheelDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        rollWheel = false;

    }

    void OnDestroy()
    {
        // Free all coroutines.
        StopAllCoroutines();
    }

}

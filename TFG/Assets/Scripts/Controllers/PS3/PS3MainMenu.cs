using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PS3MainMenu : MonoBehaviour {

    [Tooltip("Pointer return automatically to center.")]
    public bool returnCenter = true;
    [Tooltip("Enable sixaxis.")]
    public bool sixaxis = false;
    [Tooltip("sensibility of controller.")]
    public int sensibility = 20;
    [Tooltip("Controller number.")]
    public int joystick;

    private PS3 ps3;
    private string controller;
    private string controller_id;

    private GameObject obj;
    private MainMenuEngine engine;
    private Transform _transform;
    private Image pointer;

    // Events states
    private bool pressedUp = false;
    private bool pressedDown = false;
    private bool isShoot = false;
    private bool isReload = false;
    private bool grenade = false;
    private bool change_weapon = false;
    private bool isPaused = false;

	// Use this for initialization
	void Start () 
    {
        obj = GameObject.FindGameObjectWithTag("Engine");

        if (obj != null)
            engine = obj.GetComponent<MainMenuEngine>();

        _transform = transform;

        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            Debug.Log(Input.GetJoystickNames()[i]);

        controller = joystick + "P";
        controller_id = "joystick " + joystick;

        pointer = gameObject.GetComponent<Image>();
        pointer.color = (this.controller == "1P") ? new Color(.651f, 0, 0) : new Color(0, 0, .651f);
        
        ps3 = new PS3();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        if (!sixaxis)
            movementStick();
        else
            movementSixaxis();

        eventControllerKeys();
            
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
            y = Mathf.Clamp(y + (ps3.getPS3SixaxisY(controller_id) * sensibility) /*+ Screen.height * .5f*/, 0, Screen.height);

        }


        _transform.position = new Vector3(x, y, 0);

    }



    private void eventControllerKeys()
    {

        /**************************************************************/
        /****                 UP --> SENSIBILITY +                *****/
        /**************************************************************/

        // Event ButtonUp pressed and released. Used to set the sensibility of the controller.
        if (ps3.isPS3Up(controller_id))
        {
            if (!pressedUp)
            {
                sensibility = Mathf.Clamp(sensibility + 5, 10, 100);
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


        /**************************************************************/
        /****                    R1 --> SHOOT                     *****/
        /**************************************************************/

        // Event R1 pressed and released. Used to throw a grenade.
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
                        Debug.Log("Option: " + option);
                        Options op = raycastResults[i].gameObject.GetComponent<Options>();
                        if (op != null)
                        {
                            IEnumerator coroutine = engine.getCoroutine();

                            if (coroutine == null)
                                StartCoroutine(coroutine = op.ExecuteOptionMainMenu(raycastResults[i].gameObject, option, 0.2f));

                        }
                    }
                }  
            }
        }
        else if (!ps3.isPS3R1(controller_id))
            isShoot = false;


    }
}

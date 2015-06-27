using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class WiiMainMenu : MonoBehaviour
{

    [Tooltip("sensibility of controller.")]
    public int sensibility = 20;
    [Tooltip("Controller number.")]
    public int joystick = 0;

    private Wiimote wiimote;
    private string controller;

    private GameObject obj;
    private MainMenuEngine engine;
    private Transform _transform;

    private Image pointer;
    private int widthPointer;
    private int heightPointer;

    // Events states
    private bool isAvailable;
    private bool isNunchuck;
    private bool isIR;

    private bool pressedUp = false;
    private bool pressedDown = false;
    private bool isShoot = false;

    // Use this for initialization
    void Start()
    {
        obj = GameObject.FindGameObjectWithTag("Engine");

        if (obj != null)
            engine = obj.GetComponent<MainMenuEngine>();

        _transform = transform;

        controller = (joystick + 1) + "P";

        pointer = gameObject.GetComponent<Image>();
        pointer.color = (this.controller == "1P") ? new Color(.651f, 0, 0) : new Color(0, 0, .651f);

        wiimote = new Wiimote();
        wiimote.connectWiimote();
    }

    void Update()
    {
        Controller();
        /*Debug.Log("Wiimote pressed HOME: " + wiimote.pressedWiimoteButtonHome(joystick));
        Debug.Log("Wiimote pressed B: " + wiimote.pressedWiimoteButtonB(joystick));*/
    }



    void Controller()
    {

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
                DPad();

            }
        }


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
        else if (!(wiimote.pressedWiimoteButtonB(joystick) || wiimote.pressedNunchuckButtonZ(joystick)))
            isShoot = false;
    }



    private void DPad()
    {

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


    private void directionIR()
    {
        float x = _transform.position.x;
        float y = _transform.position.y;

        float irX = wiimote.getWiiMoteIRX(joystick);
        float irY = wiimote.getWiiMoteIRY(joystick);

        if (irX != -100)
            x = Mathf.Clamp((wiimote.getWiiMoteIRX(joystick) * Screen.width * .5f) + Screen.width * .5f, 0, Screen.width);

        if (irY != -100)
            y = Mathf.Clamp((wiimote.getWiiMoteIRY(joystick) * Screen.height * .5f) + Screen.height * .5f, 0, Screen.height);

        _transform.position = new Vector3(x, y, 0);

    }

    void OnApplicationQuit()
    {
        wiimote.disconnectWiimote();
    }
}

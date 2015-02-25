/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;

public class WiimoteController : MonoBehaviour {

    private Wiimote wiimote;

    private byte player;

    private bool isAvailable;
    private bool isNunchuck;

    //private Rect target_box;
    //public Texture2D targetTexture;
    private RectTransform target_box;
    private Image targetTexture;

    private int width;
    private int height;

    private Transform _transform;
    private int accuracy = 5;

    private bool shoot = false;
    private bool grenade = false;
    private bool adjust = false;

    //
    // Event Messages
    //

    /*void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 70;        
    }*/

    // Use this for initialization
    void Start()
    {
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 70;
        connectWiimote();
        wiimote = new Wiimote();
        wiimote.connectWiimote();
        this.addPlayer();
        this.targetTexture = GetComponent<Image>();
        this.width = targetTexture.mainTexture.width;
        this.height = targetTexture.mainTexture.height;

        this.target_box = GetComponent<RectTransform>();
        this.target_box.position = new Vector3(Screen.width, Screen.height, 0);
        //this.target_box.position = new Vector3(-1*((Screen.width * .5f) - (widht * .5f)), -1*((Screen.height * .5f) - (heigth * .5f)));
        this.target_box.sizeDelta = new Vector2(width, height);
        this.target_box.anchorMin = new Vector2(0, 0);
        this.target_box.anchorMax = new Vector2(0, 0);
        this.target_box.pivot = new Vector2(.5f, .5f);

        _transform = this.target_box;
        //this.isAvailable = wiimote.isWiimoteAvailable(player);
        //this.isNunchack = wiimote.isWiimoteNunchack(player);

        /*this.target_box = new Rect(Screen.width * .5f - this.targetTexture.width * .5f,
            Screen.height * .5f - this.targetTexture.height * .5f,
            this.targetTexture.width,
            this.targetTexture.height);*/

    }

    // Update is called once per frame
    void FixedUpdate() {
        Controller();
        Debug.Log("Sensor X: " + wiimote.getWiiMoteIRX(player) + " Sensor Y: " + wiimote.getWiiMoteIRY(player));
        
    }



    void Controller()
    {

        this.isAvailable = wiimote.isWiimoteAvailable(player);
        this.isNunchuck = wiimote.isWiimoteNunchuck(player);

        if (isAvailable)
        {
            if (!isNunchuck) 
            { 
                DPad();
            }
            else 
            {
                //DPad();
                //Accelerometer();
                Stick();
                //directionIR();
            }

            if (wiimote.pressedWiimoteButtonB(player) || wiimote.pressedNunchuckButtonZ(player))
                Shoot();
            else shoot = false;

            if (wiimote.pressedWiimoteButtonA(player) || wiimote.pressedNunchuckButtonC(player))
                Grenade();
            else grenade = false;

            if (wiimote.pressedWiimoteButtonMinus(player))
                AdjustAccuracy(-1);
            else if (wiimote.pressedWiimoteButtonPlus(player))
                AdjustAccuracy(1);
            else adjust = false;

            Reload();
            
        }

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

        if (wiimote.pressedWiimoteLeft(player))
            x = Mathf.Clamp(_transform.position.x - (accuracy * 5), 0, Screen.width + (width * -.5f));

        else if (wiimote.pressedWiimoteRight(player))
            x = Mathf.Clamp(_transform.position.x + (accuracy * 5), 0, Screen.width);

        if (wiimote.pressedWiimoteUp(player))
            y = Mathf.Clamp(_transform.position.y + (accuracy * 5), (height * -.5f), Screen.height);
        
        else if (wiimote.pressedWiimoteDown(player))
            y = Mathf.Clamp(_transform.position.y - (accuracy * 5), 0, Screen.height);

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

        if (wiimote.getWiimoteNunchuckX(player) <= 120)
            x = Mathf.Clamp(_transform.position.x - (accuracy * 5), 0, Screen.width + (width * -.5f));

        else if (wiimote.getWiimoteNunchuckX(player) >= 150)
            x = Mathf.Clamp(_transform.position.x + (accuracy * 5), 0, Screen.width);

        if (wiimote.getWiimoteNunchuckY(player) <= 120)
            y = Mathf.Clamp(_transform.position.y - (accuracy * 5), 0, Screen.height);

        else if (wiimote.getWiimoteNunchuckY(player) >= 145)
            y = Mathf.Clamp(_transform.position.y + (accuracy * 5), 0, Screen.height);

        _transform.position = new Vector3(x, y, 0);
    }

    private void Accelerometer()
    {
        //print("Roll: " + wiimote.getWiimoteRoll(player));
        /*if (wiimote.getWiimoteRoll(player) <= -10)
            this.target_box.x = Mathf.Clamp(this.target_box.x - sensibility, (this.targetTexture.width * -.5f), Screen.width);
        else if (wiimote.getWiimoteRoll(player) >= 10)
            this.target_box.x = Mathf.Clamp(this.target_box.x + sensibility, 0, Screen.width - (this.targetTexture.width * .5f));


        if (wiimote.getWiimotePitch(player) < -5)
            this.target_box.y = Mathf.Clamp(this.target_box.y - sensibility, (this.targetTexture.height * -.5f), Screen.height + (this.targetTexture.height * -.5f));
        else if (wiimote.getWiimotePitch(player) > 5)
            this.target_box.y = Mathf.Clamp(this.target_box.y + sensibility, (this.targetTexture.height * -.5f), Screen.height + (this.targetTexture.height * -.5f));
        */

        float x = _transform.position.x;
        float y = _transform.position.y;

        if (wiimote.getWiimoteRoll(player) <= -10)
            x = Mathf.Clamp(_transform.position.x - (accuracy * 5), 0, Screen.width + (width * -.5f));

        else if (wiimote.getWiimoteRoll(player) >= 10)
            x = Mathf.Clamp(_transform.position.x + (accuracy * 5), 0, Screen.width);

        if (wiimote.getWiimotePitch(player) <= -5)
            y = Mathf.Clamp(_transform.position.y + (accuracy * 5), 0, Screen.height);

        else if (wiimote.getWiimotePitch(player) >= 5)
            y = Mathf.Clamp(_transform.position.y - (accuracy * 5), 0, Screen.height);

        _transform.position = new Vector3(x, y, 0);
    }

    private void directionIR()
    {
        float x = _transform.position.x;
        float y = _transform.position.y;

        float irX = wiimote.getWiiMoteIRX(player);
        float irY = wiimote.getWiiMoteIRY(player);
        
        if( irX != -100)
            x = Mathf.Clamp((wiimote.getWiiMoteIRX(player) * Screen.width * .5f) + Screen.width * .5f, 0, Screen.width);
        
        if(irY != -100)
            y = Mathf.Clamp((wiimote.getWiiMoteIRY(player) * Screen.height * .5f) + Screen.height * .5f, 0, Screen.height);

        _transform.position = new Vector3(x, y, 0);

    }

    // Call method Shoot into the screen.
    private void Shoot()
    {
        if (!shoot)
        {
            shoot = true;
            wiimote.shakeWiimote(player, 0.075f);
            print("Shoot");
        }
    }

    // Call method throw Grenade.
    private void Grenade()
    {
        if (!grenade)
        {
            grenade = true;
            print("Throw grenade");
        }
        
    }

    private void AdjustAccuracy(int i) 
    {

        if (!adjust)
        {
            if (i < 0 && (accuracy + i) != 0)
                accuracy--;
            else if (i > 0 && (accuracy + i) != 11)
                accuracy++;

            adjust = true;
            print("Accuracy: " + accuracy);
        }
    }

    //float min = 122;
    //float max = 122;
    // Call method 'Reload' bullets.
    private void Reload()
    {
        //min = Mathf.Min(min, wiimote.getWiimoteAccelerometer(player).x);
        //max = Mathf.Max(max, wiimote.getWiimoteAccelerometer(player).x);
        //print("min: " + min + " max:" + max);
        //print("X: " + wiimote.getWiimoteAccelerometer(player).x);
        byte reload = (byte)wiimote.getWiimoteAccelerometer(player).y;
        if (reload >= 200)
        {
            print("Reload: " + reload);
        }
    }

    // Assign player to controller.
    private void addPlayer() 
    {

        if (wiimote.getCountWiimotes() != 0)
        {
            this.player = (byte)(wiimote.getCountWiimotes() - 1);
            print("Player " + (this.player + 1) + " connected.");
        }
        else
            print("Controller not connected.");
    }

    [DllImport("UniWii")]
    private static extern void wiimote_start();

    public void connectWiimote()
    {
        wiimote_start();
    }

    [DllImport("UniWii")]
    private static extern int wiimote_count();

    public int getCountWiimotes()
    {
        return wiimote_count();
    }

    void OnGUI()
    {

        GUI.Label(new Rect(10, 10, 150, 25), "WiiMote controllers: " + getCountWiimotes());
        GUI.Label(new Rect(10, 30, 150, 25), "WiiMote connected: " + wiimote.isWiimoteAvailable(1));

        /*target_box.Set(this.target_box.x, this.target_box.y, this.targetTexture.width, this.targetTexture.height);
        GUI.DrawTexture(target_box, this.targetTexture);
        //GUI.DrawTexture(new Rect(this.target_box.x, this.target_box.y, this.targetTexture.width, this.targetTexture.height), this.targetTexture);
        GUI.depth = 100;*/



    }

    void OnApplicationQuit()
    {
        wiimote.disconnectWiimote();
    }
}

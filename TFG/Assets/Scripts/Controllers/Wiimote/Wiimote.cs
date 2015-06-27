/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class Wiimote {


    /***************************************************************************/
    /****                        WIIMOTE METHODS                           *****/
    /***************************************************************************/

    //
    // Connect wiimote with the game.
    //

    [DllImport("UniWii")]
    private static extern void wiimote_start();

    public void connectWiimote()
    {
        wiimote_start();
    }

    //
    // Disconnect wiimote with the game.
    //

    [DllImport("UniWii")]
    private static extern void wiimote_stop();

    public void disconnectWiimote()
    {
        wiimote_stop();
    }

    //
    // Return the number of wiimotes connected.
    //

    [DllImport("UniWii")]
    private static extern int wiimote_count();

    public int getCountWiimotes()
    {
        return wiimote_count();
    }

    //
    // Returns boolean is wiimote is enabled.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern bool wiimote_available(int wiimote);

    public bool isWiimoteAvailable(int wiimote)
    {
        return wiimote_available(wiimote);
    }

    //
    // Returns boolean is wiimote IR is enabled.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern bool wiimote_enableIR(int wiimote);

    public bool isWiimoteIRAvailable(int wiimote)
    {
        return wiimote_enableIR(wiimote);
    }

    [DllImport("UniWii")]
    private static extern bool wiimote_isIRenabled(int which);

    public bool isIREnabled(int wiimote)
    {
        return wiimote_isIRenabled(wiimote);
    }


    //
    // Return boolean is wiimote has a nunchack enabled.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern bool wiimote_isExpansionPortEnabled(int which);

    public bool isWiimoteNunchuck(int wiimote)
    {
        return wiimote_isExpansionPortEnabled(wiimote);
    }

    //
    // Method shake wiimote controller according the time you put.
    // Parameter: Index of wiimote and time in seconds.
    //

    [DllImport("UniWii")]
    private static extern void wiimote_rumble(int wiimote, float duration);

    public void shakeWiimote(int wiimote, float seconds)
    {
        wiimote_rumble(wiimote, seconds);
    }

    //
    // Return float with the Pitch value.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern double wiimote_getBatteryLevel(int wiimote);

    public double getBatteryWiimote(int wiimote)
    {
        return wiimote_getBatteryLevel(wiimote);
    }


    /***************************************************************************/
    /****                        WIIMOTE BUTTONS                           *****/
    /***************************************************************************/

    //
    // Return bool if up is pressed.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern bool wiimote_getButtonUp(int which);

    public bool pressedWiimoteUp(int wiimote)
    {
        return wiimote_getButtonUp(wiimote);
    }

    //
    // Return bool if left is pressed.
    // Parameter: Index of wiimote.
    //


    [DllImport("UniWii")]
    private static extern bool wiimote_getButtonLeft(int which);

    public bool pressedWiimoteLeft(int wiimote)
    {
        return wiimote_getButtonLeft(wiimote);
    }

    //
    // Return bool if right is pressed.
    // Parameter: Index of wiimote.
    //


    [DllImport("UniWii")]
    private static extern bool wiimote_getButtonRight(int which);

    public bool pressedWiimoteRight(int wiimote)
    {
        return wiimote_getButtonRight(wiimote);
    }

    //
    // Return bool if down is pressed.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern bool wiimote_getButtonDown(int which);

    public bool pressedWiimoteDown(int wiimote)
    {
        return wiimote_getButtonDown(wiimote);
    }

    [DllImport("UniWii")]
    private static extern bool wiimote_getButton1(int which);
    public bool pressedWiimoteButton1(int wiimote)
    {
        return wiimote_getButton1(wiimote);
    }

    [DllImport("UniWii")]
    private static extern bool wiimote_getButton2(int which);
    public bool pressedWiimoteButton2(int wiimote)
    {
        return wiimote_getButton2(wiimote);
    }
    //
    // Return bool if button A is pressed.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern bool wiimote_getButtonA(int wiimote);

    public bool pressedWiimoteButtonA(int wiimote)
    {
        return wiimote_getButtonA(wiimote);
    }

    //
    // Return bool if button B is pressed.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern bool wiimote_getButtonB(int wiimote);

    public bool pressedWiimoteButtonB(int wiimote)
    {
        return wiimote_getButtonB(wiimote);
    }

    //
    // Return bool if + is pressed.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern bool wiimote_getButtonPlus(int which);

    public bool pressedWiimoteButtonPlus(int which)
    {
        return wiimote_getButtonPlus(which);
    }

    //
    // Return bool if HOME is pressed.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern bool wiimote_getButtonHome(int which);

    public bool pressedWiimoteButtonHome(int which)
    {
        return wiimote_getButtonHome(which);
    }

    //
    // Return bool if - is pressed.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern bool wiimote_getButtonMinus(int which);

    public bool pressedWiimoteButtonMinus(int which)
    {
        return wiimote_getButtonMinus(which);
    }
 

    /***************************************************************************/
    /****                 WIIMOTE ACCELEROMETER & AXIS                     *****/
    /***************************************************************************/


    //
    // Return Vector3 with the x, y, z accelerometer.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern byte wiimote_getAccX(int which);
    [DllImport("UniWii")]
    private static extern byte wiimote_getAccY(int which);
    [DllImport("UniWii")]
    private static extern byte wiimote_getAccZ(int which);

    public Vector3 getWiimoteAccelerometer(int wiimote)
    {
        return new Vector3(wiimote_getAccX(wiimote), wiimote_getAccY(wiimote), wiimote_getAccZ(wiimote));
    }

    //
    // Return float with the Roll value.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern float wiimote_getRoll(int which);

    public float getWiimoteRoll(int wiimote)
    {
        return wiimote_getRoll(wiimote);
    }

    //
    // Return float with the Pitch value.
    // Parameter: Index of wiimote.
    //

    [DllImport("UniWii")]
    private static extern float wiimote_getPitch(int which);

    public float getWiimotePitch(int wiimote)
    {
        return wiimote_getPitch(wiimote);
    }

    //
    // Return float with the Yaw value.
    // Parameter: Index of wiimote.
    //


    [DllImport("UniWii")]
    private static extern float wiimote_getYaw(int which);

    public float getWiimoteYaw(int wiimote)
    {
        return wiimote_getYaw(wiimote);
    }


    /***************************************************************************/
    /****                      NUNCHUCK METHODS                            *****/
    /***************************************************************************/


    [DllImport("UniWii")]
    private static extern byte wiimote_getNunchuckStickX(int wiimote);

    public byte getWiimoteNunchuckX(int wiimote)
    {
        return wiimote_getNunchuckStickX(wiimote);
    }

    [DllImport("UniWii")]
    private static extern byte wiimote_getNunchuckStickY(int wiimote);

    public byte getWiimoteNunchuckY(int wiimote)
    {
        return wiimote_getNunchuckStickY(wiimote);
    }

    [DllImport("UniWii")]
    private static extern bool wiimote_getButtonNunchuckC(int wiimote);

    public bool pressedNunchuckButtonC(int wiimote)
    {
        return wiimote_getButtonNunchuckC(wiimote);
    }

    [DllImport("UniWii")]
    private static extern bool wiimote_getButtonNunchuckZ(int which);

    public bool pressedNunchuckButtonZ(int wiimote)
    {
        return wiimote_getButtonNunchuckZ(wiimote);
    }

    /***************************************************************************/
    /****                      IR SENSOR METHODS                           *****/
    /***************************************************************************/

    [DllImport("UniWii")]
    private static extern float wiimote_getIrX(int which);

    public float getWiiMoteIRX(int wiimote)
    {
        return wiimote_getIrX(wiimote);
    }

    [DllImport("UniWii")]
    private static extern float wiimote_getIrY(int which);

    public float getWiiMoteIRY(int wiimote)
    {
        return wiimote_getIrY(wiimote);
    }
}

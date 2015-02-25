/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using System.Collections;

public class PS3 {

    //
    // Return the number of controllers there're connected.
    //
    public int getCountControllers() 
    {
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            if (Input.GetJoystickNames()[i].GetHashCode() < 0)
                return Input.GetJoystickNames().Length - 1;

        return Input.GetJoystickNames().Length;
    }

    //
    // Return boolean is X button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3Cross(string controller)
    {
        return Input.GetButton(controller + " button X");
    }

    //
    // Return boolean is [] button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3Square(string controller)
    {
        return Input.GetButton(controller + " button []");
    }

    //
    // Return boolean is /_\\ button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3Triangle(string controller)
    {
        return Input.GetButton(controller + " button /_\\");
    }

    //
    // Return boolean is O button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3Circle(string controller)
    {
        return Input.GetButton(controller + " button O");
    }

    //
    // Return boolean is L1 button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3L1(string controller)
    {
        return Input.GetButton(controller + " button L1");
    }

    //
    // Return boolean is R1 button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3R1(string controller)
    {
        return Input.GetButton(controller + " button R1");
    }

    //
    // Return boolean is L2 button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3L2(string controller)
    {
        return Input.GetButton(controller + " button L2");
    }

    //
    // Return boolean is R2 button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3R2(string controller)
    {
        return Input.GetButton(controller + " button R2");
    }

    //
    // Return boolean is L3 button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3L3(string controller)
    {
        return Input.GetButton(controller + " button L3");
    }

    //
    // Return boolean is R3 button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3R3(string controller)
    {
        return Input.GetButton(controller + " button R3");
    }

    //
    // Return boolean is START button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3Start(string controller)
    {
        return Input.GetButton(controller + " button START");
    }

    //
    // Return boolean is SELECT button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3Select(string controller)
    {
        return Input.GetButton(controller + " button SELECT");
    }

    //
    // Return boolean is HOME button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3Home(string controller)
    {
        return Input.GetButton(controller + " button HOME");
    }

    //
    // Return boolean is UP button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3Up(string controller)
    {
        return Input.GetButton(controller + " button UP");
    }

    //
    // Return boolean is LEFT button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3Left(string controller)
    {
        return Input.GetButton(controller + " button LEFT");
    }

    //
    // Return boolean is DOWN button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3Down(string controller)
    {
        return Input.GetButton(controller + " button DOWN");
    }

    //
    // Return boolean is RIGHT button is pressed.
    // Parameter: joystick + number id.
    //
    public bool isPS3Right(string controller)
    {
        return Input.GetButton(controller + " button RIGHT");
    }

    //
    // Return float with the value of left stick direction axis X.
    // Parameter: joystick + number id.
    //
    public float getPS3LeftStickX(string controller)
    {
        return Input.GetAxisRaw(controller + " STICK-X");
    }

    //
    // Return float with the value of left stick direction axis Y.
    // Parameter: joystick + number id.
    //
    public float getPS3LeftStickY(string controller)
    {
        return Input.GetAxisRaw(controller + " STICK-Y");
    }

    //
    // Return float with the value of right stick direction axis X.
    // Parameter: joystick + number id.
    //
    public float getPS3RightStickX(string controller)
    {
        return  Input.GetAxisRaw(controller + " STICK+X");
    }

    //
    // Return float with the value of right stick direction axis Y.
    // Parameter: joystick + number id.
    //
    public float getPS3RightStickY(string controller)
    {
        return Input.GetAxisRaw(controller + " STICK+Y");
    }

    //
    // Return float with the value of sixaxis X direction.
    // Parameter: joystick + number id.
    //
    public float getPS3SixaxisX(string controller)
    {
        return Input.GetAxisRaw(controller + " SIXAXIS X");
    }

    //
    // Return float with the value of sixaxis Y direction.
    // Parameter: joystick + number id.
    //
    public float getPS3SixaxisY(string controller)
    {
        return Input.GetAxisRaw(controller + " SIXAXIS Y");
    }

}
/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using System.Collections;

public abstract class Controller : MonoBehaviour {

    public enum enumController
    {
        PC,
        PS3,
        XBOX,
        WiiMote
    }

    public abstract Weapon getWeapon();
    public abstract void setFirstWeapon(string weapon);
    public abstract void setSecondWeapon(string weapon);
    public abstract void setPlayer(string player);
}

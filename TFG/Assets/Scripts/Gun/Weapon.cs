/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour 
{

	public enum enumWeapon
    {
        HandGun,
        Revolver,
        Shotgun,
        SMG
    }

    public Sprite pointerTexture;
    public GameObject sparkParticles;
    public Sprite bulletTexture_P1;
    public Sprite bulletTexture_P2;
    public AudioClip shootAudio;
    public float firepower;
    public float shoot_speed;
    public float reload_speed;
    public byte capacity;

    public bool reload = false;

    public abstract void weaponInit(AvatarController controller);
    public abstract void weaponFire(Vector3 vec);
    public abstract void weaponReload(bool reload);
    public abstract void weaponEmpty();
    public abstract void setReload(bool reload);

}

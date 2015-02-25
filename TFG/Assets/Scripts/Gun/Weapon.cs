using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

	public enum Gun
    {
        HandGun,
        Shotgun,
        MachineGun
    }

    public Sprite pointerTexture;
    public Sprite shotTexture;
    public Sprite bulletTexture;
    public byte damage;
    public float reload;
    public byte capacity;

    public abstract void weaponInit();
    public abstract void weaponFire(Vector3 vec);
    public abstract void weaponReload();
}

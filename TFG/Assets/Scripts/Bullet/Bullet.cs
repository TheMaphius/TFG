using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Bullet : MonoBehaviour 
{

    public enum enumBullet
    {
        HandGun,
        Revolver,
        Shotgun,
        SMG
    }

    public abstract void BulletShoot();
    public abstract void setBulletPosition(Vector2 vec);
    public abstract Vector2 getBulletPosition();
    public abstract void setBulletTexture(Sprite texture);
    public abstract void setBulletActivate(bool activate);
    public abstract void deleteBullet();

}

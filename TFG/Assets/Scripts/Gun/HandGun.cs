using UnityEngine;
using System.Collections;

public class HandGun : Weapon {

    private GameObject player;
    private Transform _transform;

    private Transform[] bullets;
    private Bullet b;

    private int bullet = 0;

    public HandGun()
    {

        this.pointerTexture = Resources.Load<Sprite>("Textures/Pointers/target");      
        //this.shotTexture = Resources.Load<Sprite>("Textures/Pointers/target");	
        this.bulletTexture = Resources.Load<Sprite>("Textures/Bullets/bullet2");	
        this.damage = 1;
        this.reload = 1.5f;
        this.capacity = 9;

    }

    public override void weaponInit()
    {

        bullets = new Transform[capacity];
        player = GameObject.FindGameObjectWithTag("1P");
        _transform = player.transform;

        for (int i = capacity - 1; i >= bullet; i--)
        {
            bullets[i] = Instantiate(Resources.Load<Transform>("Prefabs/Bullet")) as Transform;
            b = bullets[i].GetComponent<Bullet>();

            bullets[i].name = "bullet_" + i;
            bullets[i].SetParent(_transform, true);
            bullets[i].position = new Vector2(10 + b.getBulletWidth() * .5f, Screen.height * 0.65f - (i * b.getBulletHeight() * .55f));
                //b.getBulletPosition();
            
            b.setBulletPosition(bullets[i].position);
            Debug.Log("Position 2: " + b.getBulletPosition());
        }

        bullet = 0;
    }

    public override void weaponFire(Vector3 vec)
    {
        Ray ray = Camera.main.ScreenPointToRay(vec);
        RaycastHit hit;

        if (bullet < capacity)
        {
            bullets[bullet].GetComponent<Bullet>().setBulletShoot(true);
            //bullets[bullet] = null;
            bullet++;
        }

        if (Physics.Raycast(ray, out hit, 100))
        {

            Debug.Log(hit.transform.name);
            /*if (bullet < capacity)
            {
                bullets[bullet].GetComponent<Bullet>().setBulletShoot(true);
                bullets[bullet] = null;
                bullet++;
            }*/
        }
    }
    
    public override void weaponReload()
    {

        for (int i = 0; i < bullet; i++)
        {
            //bullets[i].GetComponent<Bullet>().deleteBullet();
            b = bullets[i].GetComponent<Bullet>();
            b.setBulletShoot(false);
            bullets[i].position = b.getBulletPosition();
            bullets[i].rotation = Quaternion.Euler(Vector3.zero);
            //b.gameObject.SetActive(true);
        }

       bullet = 0;

    }

}

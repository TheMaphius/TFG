/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using System.Collections;

public class Shotgun : Weapon{

    private string controller;
    private GameObject player;
    private Transform _transform;
    private AvatarController avatar;

    private LifeBar life;

    private Transform magazine;
    private Transform[] bullets;
    private ShotgunBullet shotgun_bullet;

    private int bullet = 0;

    private GameObject reloadPanel;
    private AudioClip reloadAudio;

    /// <summary>
    /// Load the Shotgun properties.
    /// </summary>
    public Shotgun()
    {
        this.pointerTexture = Resources.Load<Sprite>("Textures/Pointers/aim_shotgun");
        this.sparkParticles = Resources.Load<GameObject>("Particles/handgun_sparks");
        this.bulletTexture_P1 = Resources.Load<Sprite>("Textures/Bullets/shotgun_bullet_red");
        this.bulletTexture_P2 = Resources.Load<Sprite>("Textures/Bullets/shotgun_bullet_blue");
        this.shootAudio = Resources.Load<AudioClip>("Music/Weapons/shotgun_shoot_v2");
        this.reloadAudio = Resources.Load<AudioClip>("Music/Effects/reload_01");
        this.firepower = 3;
        this.reload_speed = 1f;
        this.capacity = 6;
    }

    /// <summary>
    /// Initialize the weapon.
    /// </summary>
    /// <param name="controller">Controller ID</param>
    public override void weaponInit(AvatarController controller)
    {
        this.avatar = controller;
        this.controller = controller.getPlayer();

        this.avatar.setPointer(this.pointerTexture);

        Transform parent = GameObject.FindGameObjectWithTag("Ammo_" + this.controller).transform;

        // Load the Prefab Bullets in the corner of the screen.
        magazine = Instantiate(Resources.Load<Transform>("Prefabs/Bullets/MagShotgun")) as Transform;
        magazine.transform.SetParent(parent.transform, true);
        magazine.transform.localScale = new Vector3(.9f, .9f, .9f);

        float height = Screen.height;

        if (this.controller.Equals("1P"))
            magazine.transform.position = new Vector2(0, height - (height * 0.08f));
        else if (this.controller.Equals("2P"))
        {
            magazine.transform.Rotate(new Vector3(0f, 180f, 0f));
            magazine.transform.position = new Vector2(Screen.width, height - (height * 0.08f));
        }

        // Get the num of childs of the GameObject and save the bullets
        int childs = magazine.childCount;
        bullets = new Transform[childs];

        for (int i = 0; i < childs; i++)
        {
            bullets[i] = magazine.GetChild((childs - 1) - i);
            shotgun_bullet = bullets[i].GetComponent<ShotgunBullet>();
            shotgun_bullet.setBulletTexture(this.controller.Equals("1P") ? bulletTexture_P1 : bulletTexture_P2);
            shotgun_bullet.setBulletPosition(bullets[i].position);
        }

        reloadPanel = this.avatar.getReloadPanel();

        bullet = 0;
    }

    public override void weaponFire(Vector3 vec)
    {
       
        if (!isReloading())
        {

            Ray ray = Camera.main.ScreenPointToRay(vec);
            RaycastHit hit;

            // Each fire put shoot false of every bullet.
            if (bullet < capacity)
            {
                bullets[bullet].GetComponent<ShotgunBullet>().BulletShoot();
                audio.PlayOneShot(this.shootAudio);

                //Vector3 point = Input.mousePosition;
                //point.z = 13.5f; // <-- Coordinates where you want to set the sparks respect to the camera.
                vec.z = 13.5f;

                // Set instation of shoot effect and set color.
                GameObject sparksEffect = Instantiate(this.sparkParticles) as GameObject;
                ParticleAnimator colorParticle = sparksEffect.GetComponent<ParticleAnimator>();
                Color[] modifiedColors = null;

                if (this.controller.Equals("1P"))
                    modifiedColors = new Color[5] { Color.red, Color.red, Color.white, Color.red, Color.red };
                else if (this.controller.Equals("2P"))
                    modifiedColors = new Color[5] { Color.blue, Color.blue, Color.white, Color.blue, Color.blue };

                colorParticle.colorAnimation = modifiedColors;
                sparksEffect.transform.position = Camera.main.ScreenToWorldPoint(vec);

                bullet++;

                // Throw a ray in the direction of the pointer
                if (Physics.Raycast(ray, out hit, 100))
                {
                    // Get the layer of enemies.
                    string layer = LayerMask.LayerToName(hit.transform.gameObject.layer);
                    Transform parent = hit.transform.parent;
                    
                    if (layer.Equals("Zombie"))
                    {
                        string zombie_part = hit.transform.name;

                        // Loop to get the parent object of the current object.
                        while (parent.tag != "Enemy" && parent.tag != null)
                        {
                            parent = parent.transform.parent;
                        }

                        bool ai = parent.gameObject.GetComponent<AI>().enabled;

                        if (ai)
                        {
                            parent.gameObject.GetComponent<AI>().HitState(hit, this.controller);
                            parent.gameObject.GetComponent<Behaviour>().ZombieDamage(zombie_part, this.firepower);
                        }
                        else if (parent.gameObject.GetComponent<ZombieSurvivor>() != null)
                        {
                            parent.gameObject.GetComponent<ZombieSurvivor>().HitState(hit, this.controller);
                        }

                    }
                    else
                    {
                        // If we miss the shoot or shoot any thing that isn't it a zombie reset combo.
                        avatar.getLifeBar().getTambor().ResetTambor();
                        avatar.getLifeBar().getCombo().ComboBreak();
                    }
                }
            }

            if (bullet == capacity)
            {
                // If magazine is empty show the reload panel and play audio.
                if (!reloadPanel.activeInHierarchy)
                {

                    reloadPanel.SetActive(true);
                    audio.PlayOneShot(reloadAudio);
                }
                else
                {
                    // If panel is active play only the audio.
                    audio.PlayOneShot(reloadAudio);
                }

            }
        }
    }
    
    public override void weaponReload(bool reload)
    {

        for (int i = 0; i < bullet; i++)
        {
            shotgun_bullet = bullets[i].GetComponent<ShotgunBullet>();
            bullets[i].position = shotgun_bullet.getBulletPosition();
            bullets[i].rotation = this.controller.Equals("1P") ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(new Vector3(0, 180, 0));
        }

        setReload(reload);
        bullet = 0;

    }

    public override void weaponEmpty()
    {
        for (int i = 0; i < capacity; i++)
            Destroy(bullets[i].gameObject);

        Destroy(magazine.gameObject);
        bullet = 0;
    }

    public override void setReload(bool reload)
    {
        this.reload = reload;
    }

    private bool isReloading()
    {
        return this.reload;
    }

}

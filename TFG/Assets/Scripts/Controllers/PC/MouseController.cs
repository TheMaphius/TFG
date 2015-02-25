using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseController : MonoBehaviour {

    private Weapon gun;

    private Transform _transform;
    private RectTransform rect;
    private Image targetTexture;

    private int widht;
    private int heigth;

	// Use this for initialization
	void Start () {

        Screen.showCursor = false;

        gameObject.AddComponent<HandGun>();
        gun = GetComponent<HandGun>();
        gun.weaponInit();

        this.targetTexture = GetComponent<Image>();
        this.widht = targetTexture.mainTexture.width;
        this.heigth = targetTexture.mainTexture.height;

        this.rect = GetComponent<RectTransform>();
        this.rect.position = new Vector3(Screen.width, Screen.height, 0);
        this.rect.sizeDelta = new Vector2(widht, heigth);
        this.rect.anchorMin = new Vector2(0, 0);
        this.rect.anchorMax = new Vector2(0, 0);
        this.rect.pivot = new Vector2(.5f, .5f);

        _transform = rect;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        _transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            gun.weaponFire(Input.mousePosition);
            
        }
        else if (Input.GetMouseButtonDown(2))
            print("Throw Grenade.");
        else if (Input.GetMouseButtonDown(1))
        {
            gun.weaponReload();
        }
        
	}

    void OnMouseDown()
    {
        /*if (Input.GetMouseButton(0))
        {
            print("Shoot");
            //gun.weaponFire(Input.mousePosition);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            print("Reload");
        }*/
        print("Shoot");
    }


}

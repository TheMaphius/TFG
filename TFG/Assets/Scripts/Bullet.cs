using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bullet : MonoBehaviour {

    private const short reference_width = 1366;
    private const short reference_height = 598;

    private Transform _transform;
    private RectTransform rect;
    private Image bullet;
    public Sprite texture;

    private float width = 82;
    private float height = 56;
    private float setWidth;
    private float setHeight;
    private Vector2 bulletOrigin;

    private byte animation;
    public bool shoot = false;

    void Awake()
    {
        setWidth = setBulletWidth(width);
        setHeight = setBulletHeight(height);

        rect = GetComponent<RectTransform>();
        bullet = GetComponent<Image>();
        _transform = rect.transform;
    }

	// Use this for initialization
	void Start () {

        rect.sizeDelta = new Vector2(setWidth, setHeight);
        bullet.overrideSprite = texture;

        animation = (byte)Random.Range(0, 3);
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        //position.sizeDelta = new Vector2(Screen.width, Screen.height);
        if (shoot)
        {
            //Debug.Log(animation);
            switch (animation)
            {
                case 0:
                    animationBullet1();
                    break;
                case 1:
                    animationBullet2();
                    break;
                case 2:
                    animationBullet3();
                    break;
                default:
                    break;
            }
            

        }
	
	}

    private void animationBullet1()
    {

        float x = _transform.position.x;
        float y = _transform.position.y;

        if (y < Screen.height * -.5f)
        {
            //Debug.Log("Position: " + getBulletPosition());
            //gameObject.SetActive(false);
            //this.shoot = false;
        }
        else
        {
            _transform.position = new Vector3(x - 2, y - 10, 0);
            _transform.Rotate(Vector3.forward * 15);
        }
    
    }

    private void animationBullet2()
    {

        float x = _transform.position.x;
        float y = _transform.position.y;

        if (y < Screen.height * -.5f)
        {
            //Debug.Log("Position: " + getBulletPosition());
            //gameObject.SetActive(false);
            //this.shoot = false;
        }
        else
        {
            _transform.position = new Vector3(x + 2, y - 10, 0);
            _transform.Rotate(Vector3.back * 15);
        }

    }

    private void animationBullet3()
    {

        float x = _transform.position.x;
        float y = _transform.position.y;

        if (x + bullet.flexibleWidth < Screen.width*-.5f)
        {
            //Debug.Log("Position: " + getBulletPosition());
            //gameObject.SetActive(false);
            //this.shoot = false;
        }
        else
        {
            _transform.position = new Vector3(x - 10, y - 2, 0);
            _transform.Rotate(Vector3.forward * 15);
        }
    }

    public void setBulletPosition(Vector2 vec)
    {
        Debug.Log("Set Position: " + getBulletPosition());
        _transform.position = vec;
        _transform.Rotate(Vector3.zero);
        bulletOrigin = vec;
    }

    public Vector2 getBulletPosition()
    {
        return bulletOrigin;//_transform.position;
    }

    public void setBulletTexture(Sprite bullet)
    {
        this.bullet.overrideSprite = bullet;
    }

    private float setBulletWidth(float width)
    {
        return ((Screen.width * width) / (reference_width * 1.0f));
    }

    public float getBulletWidth()
    {
        return setWidth;//width;//bullet.mainTexture.width;
    }

    private float setBulletHeight(float height)
    {
        return ((Screen.height * height) / (reference_height * 1.0f));
    }

    public float getBulletHeight()
    {
        return setHeight;//height; //bullet.mainTexture.height;
    }

    public void setBulletShoot(bool shoot)
    {
        this.shoot = shoot;
    }

    public void setBulletActivate(bool active)
    {
        gameObject.SetActive(active);
    }

    public void deleteBullet()
    {
        Destroy(gameObject);
    }
}

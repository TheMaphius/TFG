/***************************************************************************/
/**** AUTOR:    Joshua García Palacios                                 *****/
/**** PROGRAMA:  TFG                                                   *****/
/**** VERSIO:    1.0                                                   *****/
/***************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShotgunBullet : Bullet
{

    private Transform _transform;
    private Vector2 origin;
    private Image bullet;

    private byte rand_animation;

    private IEnumerator coroutine;

    /// <summary>
    /// Get the transform property and the Image component of the UI.
    /// </summary>
    void Awake()
    {
        _transform = transform;
        bullet = transform.GetComponent<Image>();
    }

    /// <summary>
    /// Method that start a random animation of bullet.
    /// </summary>
    public override void BulletShoot()
    {
        rand_animation = (byte)Random.Range(0, 3);

        switch (rand_animation)
        {
            case 0:
                StartCoroutine(coroutine = animationBullet1());
                break;
            case 1:
                StartCoroutine(coroutine = animationBullet2());
                break;
            case 2:
                StartCoroutine(coroutine = animationBullet3());
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Coroutine with the animation of bullet falling down with a litle degree to the left.
    /// </summary>
    private IEnumerator animationBullet1()
    {

        float x = _transform.position.x;
        float y = _transform.position.y;

        float limit = Screen.height * -.5f;

        while (y > limit)
        {
            _transform.position = new Vector3(x - 2, y - 10, 0);
            _transform.Rotate(Vector3.forward * 15);
            x = _transform.position.x;
            y = _transform.position.y;

            yield return 0;
        }

    }

    /// <summary>
    /// Coroutine with the animation of bullet falling down with a litle degree to the right.
    /// </summary>
    private IEnumerator animationBullet2()
    {

        float x = _transform.position.x;
        float y = _transform.position.y;

        float limit = Screen.height * -.5f;

        while (y > limit)
        {
            _transform.position = new Vector3(x + 2, y - 10, 0);
            _transform.Rotate(Vector3.forward * 15);
            x = _transform.position.x;
            y = _transform.position.y;

            yield return 0;
        }

    }

    /// <summary>
    /// Coroutine with the animation of bullet falling down with a big degree to the left or right 
    /// depends the player controller.
    /// </summary>
    private IEnumerator animationBullet3()
    {

        // This variable get the 'x' position of magazine and deduce the controller.
        float position = transform.parent.transform.position.x;

        float x = _transform.position.x;
        float y = _transform.position.y;

        float limit = Screen.width * -.5f;

        if (position < Mathf.Abs(limit))
        {
            // Player 1 --> Effect to left direction.
            while (x + bullet.flexibleWidth > limit)
            {
                _transform.position = new Vector3(x - 10, y - 2, 0);
                _transform.Rotate(Vector3.forward * 15);
                x = _transform.position.x;
                y = _transform.position.y;

                yield return 0;
            }
        }
        else
        {
            // Player 2 --> Effect to right direction.
            while (x + bullet.flexibleWidth < -3 * limit)
            {
                _transform.position = new Vector3(x + 10, y - 2, 0);
                _transform.Rotate(Vector3.forward * 15);
                x = _transform.position.x;
                y = _transform.position.y;

                yield return 0;
            }
        }
    }


    /// <summary>
    /// Set the bullet position.
    /// </summary>
    /// <param name="vec">Vector2 with the position of the bullet</param>
    public override void setBulletPosition(Vector2 vec)
    {
        _transform.position = vec;
        _transform.Rotate(Vector3.zero);
        origin = vec;
    }


    /// <summary>
    /// Return the bullet position.
    /// </summary>
    /// <returns>Returns a Vector2 with position of the bullet.</returns>
    public override Vector2 getBulletPosition()
    {

        if (coroutine != null)
            StopCoroutine(coroutine);

        return origin;
    }


    /// <summary>
    /// Set the bullet texture.
    /// </summary>
    /// <param name="texture">Texture of the bullet</param>
    public override void setBulletTexture(Sprite texture)
    {
        bullet.overrideSprite = texture;
    }


    /// <summary>
    /// Set the bullet object active.
    /// </summary>
    /// <param name="activate">Boolean true is enable object or false is disabled.</param>
    public override void setBulletActivate(bool activate)
    {
        gameObject.SetActive(activate);
    }


    /// <summary>
    /// Method delete the bullets of the screen and stop all the coroutines that are running.
    /// </summary>
    public override void deleteBullet()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        Destroy(gameObject);
    }

}

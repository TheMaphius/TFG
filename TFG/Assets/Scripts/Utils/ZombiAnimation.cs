using UnityEngine;
using System.Collections;

public class ZombiAnimation : MonoBehaviour {

    public string animation;
    public GameObject point;
    //public float depth;

    private Animator anim;
    private Vector3 origin;
    private Vector3 direction;

    private float currentDistance;

	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
        anim.SetBool(animation, true);
        origin = transform.position;
	}

    void Update()
    {

        if (animation == "walk")
        {
            currentDistance = Vector3.Distance(point.transform.position, transform.position);
            if (currentDistance > 2f)
            {
                direction = point.transform.position - transform.position;
                direction.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                        2f * Time.deltaTime);
                transform.position += (transform.forward * 5f) * Time.deltaTime;
                //currentDistance = Vector3.Distance(player.position, _transform.position);
                //Debug.Log("I'm running or walking a " + currentDistance + "m of the player.");
            }
            else
            {
                //Debug.Log("He llegado a mi objetivo");
                /*Debug.Log("Origin Before: " + origin);
                origin.z = (depth != 0) ? depth : -675f;
                Debug.Log("Origin After: " + origin);*/
                transform.position = origin;
            }
        }
    }

}

using UnityEngine;
using System.Collections;

public class ParticleDamage : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Soy la particula");
	}
	
	// Update is called once per frame
	void Update () {

        gameObject.transform.position += new Vector3(0.1f, 0, 0);
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("*********He tocado ha " + other.gameObject.name + " y soi " + this.name);

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("///////////He tocado ha " + collision.gameObject.name + " y soi " + this.name);
    }
    
    void OnParticleCollision(GameObject other) 
    {
        Debug.Log ("He tocado ha " + other.gameObject.name + " y soi " + this.name);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("*********He tocado ha " + other.gameObject.name + " y soi " + this.name);

    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("///////////He tocado ha " + collision.gameObject.name + " y soi " + this.name);
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("*********He tocado ha " + other.gameObject.name + " y soi " + this.name);

    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("///////////He tocado ha " + collision.gameObject.name + " y soi " + this.name);
    }

}

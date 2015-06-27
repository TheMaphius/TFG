using UnityEngine;
using System.Collections;

public class ObjectBehaviour : MonoBehaviour {

    public enum enumObjects // Selectable type of platform.
    {
        Barrel,
        Boxes,
        Crate_Box,
        Fire_Hydrant,
        FirstAid,
        Street_Light,
        Tire,
        Trash_Bin,
        Trash_Can,
    };

    public enumObjects object_type;
    public float force = 20f;

    private GameObject effect;
    private bool destroyed = false;

    void Start()
    {
        //water_fount = Resources.Load<GameObject>("Particles/water_fount");
    }

    public void HitObject()
    {
        switch (object_type.ToString())
        {
            case "Barrel":
                effect = Resources.Load<GameObject>("Particles/explosion");
                Explosion();
                break;
            case "Boxes":
                gameObject.rigidbody.AddForce(Vector3.forward * force, ForceMode.Impulse);
                break;
            case "Fire_Hydrant":
                effect = Resources.Load<GameObject>("Particles/water_fount");
                WaterFount();
                break;
            case "Street_Light":
                if(!destroyed)
                    LightSpark();    
                break;
            case "Tire":
                gameObject.rigidbody.AddForce(Vector3.forward * force, ForceMode.Impulse);
                break;
            case "Trash_Bin":
                gameObject.rigidbody.AddForce(Vector3.forward * force, ForceMode.Impulse);
                break;
            case "Trash_Can":
                gameObject.rigidbody.AddForce(Vector3.forward * force, ForceMode.Impulse);
                break;
        }
    }

    void Explosion()
    {
        GameObject prefab = Instantiate(effect) as GameObject;
    
        prefab.transform.position = transform.position;
        prefab.GetComponent<AudioSource>().mute = (PlayerPrefs.GetInt("sound") == 0) ? true : false;
        Destroy(gameObject, 1f);
        Destroy(prefab, 5f);
    }

    void WaterFount()
    {
        
        GameObject prefab = Instantiate(effect) as GameObject;
        prefab.transform.position = transform.position;
        Destroy(gameObject);
        prefab.GetComponent<AudioSource>().mute = (PlayerPrefs.GetInt("sound") == 0) ? true : false;
        Destroy(prefab, 8.3f);
    }

    void LightSpark()
    {
        int childs = gameObject.transform.childCount;
        Transform spark = gameObject.transform.GetChild(0);
        Transform light = gameObject.transform.GetChild(1);

        //Behaviour halo = GetComponent("Halo") as Behaviour;
        //halo.enabled = true;

        Component halo = light.GetComponent("Halo"); 
        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);


        ParticleEmitter particle = spark.GetComponent<ParticleEmitter>();
        StartCoroutine(DelaySpark(particle, 4.9f));

        destroyed = true;

    }

    public string getObjectType()
    {
        return object_type.ToString();
    }

    private IEnumerator DelaySpark(ParticleEmitter particle, float delay)
    {
        for (; ; )
        {
            particle.emit = true;
            yield return new WaitForSeconds(delay);
            particle.emit = false;
        }

    }
	
	
}

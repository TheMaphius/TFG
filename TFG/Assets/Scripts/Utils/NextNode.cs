using UnityEngine;
using System.Collections;

public class NextNode : MonoBehaviour {

 
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("El objeto " + other.name + " ha colisionado");
        other.gameObject.GetComponent<SplineController>().setGo(false);
        StartCoroutine(Delay(other));
    }

    private IEnumerator Delay(Collider other)
    {
        yield return new WaitForSeconds(3f);
        other.gameObject.GetComponent<SplineController>().setGo(true);
    }
}

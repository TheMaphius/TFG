using UnityEngine;
using System.Collections;

public class CreditsEngine : MonoBehaviour {

    private AsyncOperation async;

	// Use this for initialization
	void Start () 
    {
        StartCoroutine(loadLevel());
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (async != null)
        {
            // DO SOMETHING.
            if (async.progress >= .9f)
            {
                async.allowSceneActivation = true;
            }
        }
	
	}

    private IEnumerator loadLevel()
    {
        yield return new WaitForSeconds(5);
        async = Application.LoadLevelAsync(1);
        async.allowSceneActivation = false;

    }

}

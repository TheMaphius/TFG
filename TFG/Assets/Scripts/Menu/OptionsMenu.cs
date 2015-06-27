using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

    private const float reference_width = 716f;

    public GameObject[] pointers;
    public AudioClip[] audio_button;


    private RectTransform objectRectTransform;

    private float xMax;
    private float xMin;
    private float yMax;
    private float yMin;

    private string controller;
    private bool isPause = false;
    private bool isHover = false;
    private bool[] playerHover;

	// Use this for initialization
	void Start () 
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Engine");

        objectRectTransform = gameObject.GetComponent<RectTransform>();                // This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
        float width = (Screen.width * objectRectTransform.rect.width) / reference_width;
        float height = objectRectTransform.rect.height;
        
        xMax = (width * .65f);
        xMin = (width * -.65f);
        yMax = (height * .2f);
        yMin = (height * -.2f);

        playerHover = new bool[2];
	}
	
	// Update is called once per frame
	void Update () 
    {
        HoverMenu();
	}

    private void HoverMenu()
    {
        //int i = (controller == "1P") ? i = 0 : i = 1;

        for (int i = 0; i < 2; i++)
        {
            Vector3 position = pointers[i].transform.position;
            //Debug.Log("x: " + (transform.position.x + xMax));
            if (position.x <= (transform.position.x + xMax) && position.x >= (transform.position.x + xMin) && position.y <= (transform.position.y + yMax) && position.y >= (transform.position.y + yMin))
            {

                if (!playerHover[i])
                {
                    playerHover[i] = true;
                    audio.PlayOneShot(audio_button[0]);
                }
            }
            else
            {
                if (playerHover[i])
                {
                    playerHover[i] = false;
                }
            }
        }

    }

    public void OptionMainMenu(MainMenuEngine engine, string option)
    {


        switch (option)
        {
            case "story":
                break;
            case "survivor":
                break;
            case "multiplayer":
                break;
            case "options":
                engine.setCoroutine(null);
                //engine.activateElements(false);
                Debug.Log("Estoi en options");
                break;
            case "exit":
                break;
            case "exit_options":
                engine.fade.setFade(-1);
                engine.fade.setSpeed(0.5f);
                engine.setCoroutine(null);
                //engine.activateElements(true);
                break;
        }


    }

    public IEnumerator ExecuteOptionMainMenu(MainMenuEngine engine, GameObject obj, string option, float delay)
    {
        int size = obj.transform.childCount;

        for (int i = 0; i < size; i++)
        {
            obj.transform.GetChild(i).gameObject.SetActive(true);
            audio.PlayOneShot(audio_button[1]);
            yield return new WaitForSeconds(delay);
        }

        engine.fade.setFade(1);
        engine.fade.setSpeed(1f);

        yield return new WaitForSeconds(1f);
        OptionMainMenu(engine, option);

        for (int i = 0; i < size; i++)
            obj.transform.GetChild(i).gameObject.SetActive(false);

    }
}

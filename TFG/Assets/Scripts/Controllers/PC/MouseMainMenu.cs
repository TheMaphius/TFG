using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MouseMainMenu : MonoBehaviour {

    [Tooltip("Player controller 1P or 2P.")]
    public string controller;
    [Tooltip("Set speed controller 2P.")]
    public float controller_speed = 20f;

    private GameObject obj;
    private MainMenuEngine engine;
    private Transform _transform;
    private Image pointer;

	// Use this for initialization
	void Start () {

        obj = GameObject.FindGameObjectWithTag("Engine");
        
        if(obj != null)
            engine = obj.GetComponent<MainMenuEngine>();

        _transform = transform;

        pointer = gameObject.GetComponent<Image>();
        pointer.color = (this.controller == "1P") ? new Color(.651f, 0, 0) : new Color(0, 0, .651f);
	}

    /// <summary>
    /// Engine of the PC controller.
    /// </summary>
    void Update()
    {

        if (this.controller.Equals("1P"))
            mouseController();
        else if (this.controller.Equals("2P"))
            KeyController();

    }

    void mouseController()
    {

        _transform.position = Input.mousePosition;

        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();

        if (Input.GetMouseButtonDown(0))
        {
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 0)
            {
                for (int i = 0; i < raycastResults.Count; i++)
                {
                    string option = raycastResults[i].gameObject.transform.name;
                    Debug.Log("Option: " + option);
                    Options op = raycastResults[i].gameObject.GetComponent<Options>();
                    if (op != null)
                    {
                        IEnumerator coroutine = engine.getCoroutine();
                        
                        if (coroutine == null)
                            StartCoroutine(coroutine = op.ExecuteOptionMainMenu(raycastResults[i].gameObject, option, 0.2f));
                                                
                    }
                }
            }
        }
     
    }


    void KeyController()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        //x = Mathf.Clamp(x + (ps3.getPS3LeftStickX(controller_id) * sensibility), 0, Screen.width);
        //y = Mathf.Clamp(y + (ps3.getPS3LeftStickY(controller_id) * sensibility), 0, Screen.height); 
        
        if (Input.GetKey(KeyCode.W))
            y = Mathf.Clamp((y + controller_speed), 0, Screen.height);
            //_transform.position += Vector3.up * 10f;
        else if (Input.GetKey(KeyCode.S))
            y = Mathf.Clamp((y - controller_speed), 0, Screen.height);
            //_transform.position += Vector3.down * 10f;

        if (Input.GetKey(KeyCode.A))
            x = Mathf.Clamp((x - controller_speed), 0, Screen.width);
            //_transform.position += Vector3.left * 10f;
        else if (Input.GetKey(KeyCode.D))
            x = Mathf.Clamp((x + controller_speed), 0, Screen.width);
            //_transform.position += Vector3.right * 10f;

        _transform.position = new Vector3(x, y, 0);

        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = _transform.position;

        List<RaycastResult> raycastResults = new List<RaycastResult>();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 0)
            {
                for (int i = 0; i < raycastResults.Count; i++)
                {
                    string option = raycastResults[i].gameObject.transform.name;
                    Debug.Log("Option: " + option);
                    Options op = raycastResults[i].gameObject.GetComponent<Options>();
                    if (op != null)
                    {
                        IEnumerator coroutine = engine.getCoroutine();

                        if (coroutine == null)
                            StartCoroutine(coroutine = op.ExecuteOptionMainMenu(raycastResults[i].gameObject, option, 0.2f));

                    }
                }
            }
            /*if (raycastResults.Count > 0)
            {
                for (int i = 0; i < raycastResults.Count; i++)
                {
                    string option = raycastResults[i].gameObject.transform.name;
                    Debug.Log("Option: " + option);
                    Options op = raycastResults[i].gameObject.GetComponent<Options>();
                    if (op != null)
                        op.ExecuteOption(controller, option);

                }
            }*/
        }
        
    }

}

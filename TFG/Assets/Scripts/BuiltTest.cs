using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class BuiltTest : MonoBehaviour {

    private wiiMote wii = new wiiMote();

	// Use this for initialization
	void Start () {
        wii.start();
	}

    void OnGUI()
    {

        GUI.Label(new Rect(10, 10, 150, 25), "WiiMote controllers: " + wii.count());
        GUI.Label(new Rect(10, 30, 150, 25), "WiiMote connected: " + wii.availble(0));



    }

    void OnApplicationQuit()
    {
        wii.stop();
    }
}

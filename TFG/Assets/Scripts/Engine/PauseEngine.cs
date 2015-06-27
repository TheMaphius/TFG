using UnityEngine;
using System.Collections;

public class PauseEngine : MonoBehaviour {

    public GameObject pause_menu;
    public GameObject control_menu;
    public GameObject exit_menu;

    private Menu menu;

    private bool isPause = false;
    private string player_pause;

    void Start()
    {
        menu = pause_menu.GetComponent<Menu>();
    }

    public void setPause(string player)
    {
        if (isPause == false)
        {
            player_pause = player;
            isPause = true;
            menu.setMenu(player);
            pause_menu.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (isPause == true && player_pause == player)
        {
            player_pause = "";
            isPause = false;
            pause_menu.SetActive(false);
            control_menu.SetActive(false);
            exit_menu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public string getPlayerPause()
    {
        return player_pause;
    }

    public Menu getMenu()
    {
        return menu;
    }

    public GameObject getPauseMenu()
    {
        return pause_menu;
    }

    public bool getPause()
    {
        return isPause;
    }

    public GameObject getControlMenu()
    {
        return control_menu;
    }

    public GameObject getExitMenu()
    {
        return exit_menu;
    }

}

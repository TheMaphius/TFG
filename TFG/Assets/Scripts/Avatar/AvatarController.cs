using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AvatarController : MonoBehaviour {

    private LifeBar life;
    private GameObject weapon;
    public GameObject reload;

    private string player;

    public void setPlayer(string player)
    {
        this.player = player;
        weapon = GameObject.FindGameObjectWithTag("Pointer_" + player);
    }

    public string getPlayer()
    {
        return this.player;
    }

    public void setLifeBar(string player)
    {
        this.life = GameObject.FindGameObjectWithTag("LifeBar_" + player).GetComponent<LifeBar>();
    }

    public LifeBar getLifeBar()
    {
        return this.life;
    }

    public void setPointer(Sprite pointer)
    {
        weapon.GetComponent<Image>().sprite = pointer;
    }


    public void setReloadPanel(GameObject reload)
    {
        this.reload = reload;
    }

    public GameObject getReloadPanel()
    {
        return reload;
    }

    public void reloadWeapon()
    {
        //if (reload.activeInHierarchy)
        reload.SetActive(false);
        weapon.GetComponent<Controller>().getWeapon().weaponReload(false);
    }

}

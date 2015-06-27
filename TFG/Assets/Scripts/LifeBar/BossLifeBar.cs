using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossLifeBar : MonoBehaviour {

    public Image health_bar;
    public float health;
    public Image cancel_bar;
    public float cancel;

    private bool isCancel = false;

	// Use this for initialization
	void Start () 
    {
	
	}

    void Update()
    {

    }

    public void setBarCancel(float cancel)
    {
        if (!isCancel)
        {
            cancel_bar.fillAmount -= cancel;

            if (cancel_bar.fillAmount == 0)
            {
                isCancel = true;
                StartCoroutine(damageHealth());
                StartCoroutine(restoreCancelBar());
            }
        }

    }

    public void setCancel(float cancel)
    {
        this.cancel = cancel;
    }

    private IEnumerator damageHealth()
    {
        float aux = health_bar.fillAmount - .2f;

        while ((aux + .01f) < health_bar.fillAmount)
        {
            health_bar.fillAmount -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator restoreCancelBar()
    {
        yield return new WaitForSeconds(1f);

        if (!isDead())
        {
            while (cancel_bar.fillAmount + 0.01f < 1)
            {
                cancel_bar.fillAmount += 0.05f;
                yield return new WaitForSeconds(0.05f);
            }
        }

        isCancel = false;
    }

    public bool isCancelBar()
    {
        return isCancel;
    }

    public bool isDead()
    {
        return (health_bar.fillAmount == 0) ? true : false;
    }
}

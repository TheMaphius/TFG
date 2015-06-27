using UnityEngine;
using System.Collections;

public class LoopAnimation : MonoBehaviour {

    public string state;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
	// Use this for initialization
	void Start () 
    {      
        anim.SetBool(state, true);
	}

    public void OnDisable()
    {
        if(anim != null)
            anim.SetBool(state, false);
    }

    public void setState(string state)
    {
        this.state = state;
    }
    /*public void OnEnable()
    {
        if(anim == null)
            anim = GetComponent<Animator>();
        anim.SetBool(state, true);
    }*/
	
}

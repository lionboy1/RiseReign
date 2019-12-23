using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSeeAction : GoapAction {

	//[SerializeField] float timeBetweenAttack = 1.0f;
	   
	bool m_sawPlayer = false;

	void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();        
    }
    
    public CanSeeAction(){
	addEffect ("canSeePlayer", true);
        name = "Can see the player";
	}

	void Update()
    {
        //
    }
    
    public override void reset() {
		target = null;
	}

	public override bool isDone(){
		return m_sawPlayer;
	}

	public override bool requiresInRange(){
		return false; //the sight script will determine sight range and pass the canSee variable to
		// the checkProceduralPrecondition().
	}

	public override bool checkProceduralPrecondition(GameObject agent)
	{
		if(GetComponent<Signt>().m_canSeePlayer == true)
		{
			target = GameObject.FindWithTag("Player");
			if (target != null)
			{
				return true;
			}
		}	
		return false;
	}

	public override bool perform(GameObject agent)
	{
	    m_sawPlayer = true;
	    //return m_sawPlayer;
	    Debug.Log("Ah ketch eem!")	
            return true;
	}
}

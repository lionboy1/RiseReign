using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverWater : GoapAction {

	Animator anim;

	bool completed = false;
	float startTime = 0;
	public float workDuration = 5; // seconds
	
	public DeliverBread () {
		addPrecondition ("hasDeliveryWater", true); 
		addEffect ("doJob", true);
		name = "Deliver water";
	}

	public void Start()
	{
		anim = this.GetComponent<Animator>();
	}
	
	public override void reset ()
	{
		completed = false;
		startTime = 0;
	}
	
	public override bool isDone ()
	{
		return completed;
	}
	
	public override bool requiresInRange ()
	{
		return true; 
	}
	
	public override bool checkProceduralPrecondition (GameObject agent)
	{	
		return true;
	}
	
	public override bool perform (GameObject agent)
	{
		if (startTime == 0)
		{
			//Debug.Log("Starting: " + name);
			anim.SetBool("deliverWater", true );
			startTime = Time.time;
		}

		if (Time.time - startTime > workDuration) 
		{
			//Debug.Log("Finished: " + name);
			anim.SetBool("deliverWater", false );
			this.GetComponent<Inventory>().waterLevel += 1;
			completed = true;
		}
		return true;
	}
	
}

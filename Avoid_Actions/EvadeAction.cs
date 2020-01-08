using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeAction : GoapAction {
//attach the HidingSpotComponent to any hiding spot
    testEnemyHealth health;
    Animator anim;
    bool avoid = false;
    bool injured = false;
    float threatDistance = 20.0f;
    HidingSpotComponent targetSpot; // where we hide
	
	void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
	health = this.GetComponent<testEnemyHealth>();
    }
    
    public EvadeAction()
    {
        addPrecondition("runAway", false);
        addPrecondition("hasHealth", false);	    
	    addEffect ("runAway", true );
	//cost = 1.0f;
        name = "EvadeAction";
	}
      
    public override void reset() {
		avoid = false;
		targetSpot = null;
	}

	public override bool isDone(){
		return avoid;
	}

	public override bool requiresInRange(){
		return true;
	}

	public override bool checkProceduralPrecondition(GameObject agent)
    {
	   // find the nearest hiding spot 
		HidingSpotComponent[] spots = (HidingSpotComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(HidingSpotComponent));
		HidingSpotComponent closest = null;
		float closestDist = 0;
		
		foreach (HidingSpotComponent spot in spots) {
			if (closest == null) {
				// first one, so choose it for now
				closest = spot;
				closestDist = (spot.gameObject.transform.position - agent.transform.position).magnitude;
			} else {
				// is this one closer than the last?
				float dist = (spot.gameObject.transform.position - agent.transform.position).magnitude;
				if (dist < closestDist) {
					// we found a closer one, use it
					closest = spot;
					closestDist = dist;
				}
			}
		}
		if (closest == null)
			return false;

		targetSpot = closest;
		target = targetSpot.gameObject;
		
		return closest != null;
    }

	public override bool perform(GameObject agent)	        
    {
        anim.SetTrigger("hidingAnimation");
	    avoid = true;
	    return true;
	}        
	
}

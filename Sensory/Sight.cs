using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiseReign {

	public class Sight : MonoBehaviour
	{
		private bool m_investigate = false;//Should agent investigate?

		[SerializeField]
		private float m_investigateRange = 5;//range in which to investigate


		private Transform m_playerLastKnownPosition;//last place player was spotted.
		public bool m_canSeePlayer = false;//can the agent see the player?


		[Range(0, 359)]
		public float m_viewAngle;//FOV

		public float m_viewRadius;//The view radius.
		public LayerMask m_obstacleMask;//What are obstacles?
		public LayerMask m_targetMask;//What are possible targets?
		public List<Transform> m_visibleTargets = new List<Transform>();//Create a list to store targets and construct it.

		[SerializeField]
		private GameObject m_alertSound;//any noise that will alert ai.

		private bool m_noiseAlert = false;

		//Start Coroutine to find targets.

		private void Start()
		{
			StartCoroutine( "FindTargets", 0.2f);//find targets every 0.2 seconds.
		}

		private IEnumerator Findtargets(float delay)
		{
			while( true)
			{
				yield return new WaitForSeconds( delay);
				FindVisibleTargets();//Method to loop through targets
			}
		}

		//Now calculate distances to objects and analyze whether they are valid targets or not.

		private void FindVisibleTargets()
		{
			m_visibleTargets.Clear();//Clear list before starting a new round
			m_canSeePlayer = false;//reset seeing the player

			//Create an array of objects to spherecast collide with to check them.
			Collider[] cTargetsInViewRadius = Physics.OverlapSphere( transform.position, m_viewRadius, m_targetMask );

			//Iterate the list. of targets in the view radius.
			for ( int i = 0; i < cTargetsInViewRadius.Length; i++ )
			{
				Transform target = cTargetsInViewRadius[i].transform;//Select the target as the current iteration to analyze.
				//Calculate direction to target.
				Vector3 dirToTarget = (target.position - transform.position).normalized;//ray ends at target.

				//Now calculate the angle the target is to the transform to see if in view	

				if( Vector3.Angle(transform.forward, dirToTarget) < m_viewAngle * 0.5f )
				{
					//Calculate distance to target now.

					float distanceToTarget = Vector3.Distance( transform.position, target.position);
					if( !Physics.RayCast( transform.position, dirToTarget, distanceToTarget, m_obstacleMask))//Is the target in range and not an obstacle?
					{
						//Add the target to list if it wasn't added already.
						m_visibleTargets.Add(target);

						if( target.CompareTag("Player"))//if target is the player..
						{
							m_canSeePlayer = true;
							GetComponent<Worker>.m_interrupt = true;//Interrupt current action.
							m_playerLastKnownPosition = target;
							m_investigate = true;
						}
					}
				}
			}

			if ( !m_canSeePlayer )
			{
				if( m_investigate)
				{
					GetComponent<Worker>.m_interrupt = true;
					m_investigate = false;
					m_canSeePlayer = false;
					//m_alertNoise = false; Implement noise later.

					//Also implement later calling for reinforcements (i.e another guard can see player).
				}
				
			}
		}
	}
}		

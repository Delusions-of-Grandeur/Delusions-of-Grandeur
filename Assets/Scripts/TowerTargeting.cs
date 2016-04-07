using UnityEngine;
using System.Collections;

public class TowerTargeting : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		SearchTarget ();
	}

	void SearchTarget()
	{
		GameObject newTarget = null;
		float radius = transform.FindChild ("Range").transform.localScale.z;
		print (radius);
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
		Collider[] enemiesColliders = null;
		for (int i = 0; i < hitColliders.Length; i++) {
			if(hitColliders[i].tag == "Enemy" ){
				print ("found enemy");
			}
		}

//		for (var currentCollider : Collider in collidersInRange)
//		{
//			var currentEnemy : Enemy = currentCollider.GetComponent.<Enemy>();
//
//			if (newTarget == null)
//			{
//				newTarget = currentEnemy;
//			}
//
//			else if (newTarget.health > currentEnemy.health)
//			{
//				newTarget = currentEnemy;
//			}
//		}
//
//		if (newTarget != null)
//			currentTarget = newTarget.transform;
	}
}

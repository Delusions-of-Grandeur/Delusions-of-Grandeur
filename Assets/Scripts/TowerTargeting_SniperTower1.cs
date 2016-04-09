using UnityEngine;
using System.Collections;

public class TowerTargeting_SniperTower1 : MonoBehaviour {

	public GameObject UFO;
	Transform target;

	private bool firing = false;
	public GameObject bulletPrefab;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		SearchTarget ();
		if(target != null){
			transform.LookAt (target);
			StartCoroutine(shoot ());
		}
	}

	void SearchTarget()
	{
		Collider newTarget = null;
		float radius = transform.FindChild ("Range").transform.localScale.z;

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
		Collider[] enemiesColliders = null;

		for (int i = 0; i < hitColliders.Length; i++) {
			if(hitColliders[i].tag == "Enemy" ){
				if (newTarget == null) {
					newTarget = hitColliders [i];
				} else {
					float curTargetDis = Vector3.Distance(newTarget.bounds.center, UFO.transform.position); 
					float potentialDis = Vector3.Distance(hitColliders [i].bounds.center, UFO.transform.position); 

					if(0 < curTargetDis - potentialDis){
						newTarget = hitColliders [i];
					}
				}
			}
		}

		if(newTarget == null){
			target = null;
		} else {
			target = newTarget.transform;
		}

	}

	IEnumerator shoot()
	{
		print ("enter");
		yield return new WaitForSeconds(.1f);
		GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
		firing = false;


	}
}

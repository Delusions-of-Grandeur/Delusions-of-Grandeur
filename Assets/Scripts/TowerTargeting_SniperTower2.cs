using UnityEngine;
using System.Collections;

public class TowerTargeting_SniperTower2 : MonoBehaviour {

	public GameObject UFO;
	Transform target;
	Vector3 lastSpotToLook;

	private bool firing = false;
	public GameObject bulletPrefab;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		SearchTarget ();
		print (target);
		if (target != null) {
			print ("in range");
			transform.LookAt (target);
			StartCoroutine (shoot ());
			lastSpotToLook = target.position;
		} else {
			print ("not in range");
			transform.LookAt (lastSpotToLook);
		}
	}

	void SearchTarget()
	{
		Collider newTarget = null;
		float radius = transform.FindChild ("Range").transform.localScale.z;

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
		Collider[] enemiesColliders = null;
		int enemyCount = 0;
		for (int i = 0; i < hitColliders.Length; i++) {
			if(hitColliders[i].tag == "Enemy" ){
				enemyCount++;
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
		if(enemyCount == 0){
			target = null;
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

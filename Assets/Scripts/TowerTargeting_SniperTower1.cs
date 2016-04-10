using UnityEngine;
using System.Collections;

public class TowerTargeting_SniperTower1 : MonoBehaviour {

	public GameObject UFO;
	Transform target;
	Vector3 lastSpotToLook;

	public bool firing = false;
	public GameObject bulletPrefab;
	public GameObject sniperBase;
	public GameObject sniperGun;
	public GameObject barrel;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		target = SearchTarget ();

		print (target);
		if(target != null){
			print ("in range");

			customLookAt ();
			firing = true;
			StartCoroutine("shoot");
		} else {
			print ("not in range");
			firing = false;
			StopCoroutine("shoot");
		}
	}

	Transform SearchTarget()
	{
		Collider newTarget = null;
		float radius = transform.FindChild ("Range").transform.localScale.z*.6f*.5f;
		print (radius);

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

		return newTarget == null ? null : newTarget.transform;

	}

	void customLookAt()
	{
		Vector3 defaultBase = sniperBase.transform.eulerAngles;
		sniperBase.transform.LookAt (target);
		sniperBase.transform.eulerAngles = new Vector3 (defaultBase.x, sniperBase.transform.eulerAngles.y, defaultBase.z);

		sniperGun.transform.LookAt (target);
		sniperGun.transform.eulerAngles = sniperGun.transform.eulerAngles + 180f * Vector3.up;
		sniperGun.transform.eulerAngles = new Vector3 (-sniperGun.transform.eulerAngles.x, sniperGun.transform.eulerAngles.y, sniperGun.transform.eulerAngles.z);
	}

	IEnumerator shoot()
	{
		yield return new WaitForSeconds(.1f);
		GameObject bullet = (GameObject)Instantiate(bulletPrefab, barrel.transform.position, Quaternion.identity);
		bullet.transform.LookAt (target);

		if(target == null){
			yield return null;
		}
	}
}

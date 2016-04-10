using UnityEngine;
using System.Collections;

public class TowerTargeting_SniperTower2 : MonoBehaviour {

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
		Vector3 defaultBase = gattlingBase.transform.eulerAngles;
		gattlingBase.transform.LookAt (target);
		gattlingBase.transform.eulerAngles = new Vector3 (defaultBase.x, gattlingBase.transform.eulerAngles.y, defaultBase.z);

		gattlingGun.transform.LookAt (target);
		gattlingGun.transform.eulerAngles = gattlingGun.transform.eulerAngles + 180f * Vector3.up;
		gattlingGun.transform.eulerAngles = new Vector3 (-gattlingGun.transform.eulerAngles.x, gattlingGun.transform.eulerAngles.y, gattlingGun.transform.eulerAngles.z);
		gattlingGun2.transform.LookAt (target);
		gattlingGun2.transform.eulerAngles = gattlingGun2.transform.eulerAngles + 180f * Vector3.up;
		gattlingGun2.transform.eulerAngles = new Vector3 (-gattlingGun2.transform.eulerAngles.x, gattlingGun2.transform.eulerAngles.y, gattlingGun2.transform.eulerAngles.z);
	}

	IEnumerator shoot()
	{
		yield return new WaitForSeconds(.1f);
		barrel.transform.Rotate (0,0,360*Time.deltaTime);
		barrel2.transform.Rotate (0,0,360*Time.deltaTime);
		GameObject bullet = (GameObject)Instantiate(bulletPrefab, barrel.transform.position, Quaternion.identity);
		bullet.transform.LookAt (target);
		GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, barrel2.transform.position, Quaternion.identity);
		bullet2.transform.LookAt (target);

		if(target == null){
			yield return null;
		}
	}
}

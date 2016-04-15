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
	public GameObject barrel2;

	// Use this for initialization
	void Start () {
		UFO = GameObject.Find("flying Disk landed");
	}

	// Update is called once per frame
	void Update () {

		target = SearchTarget ();
		if(target != null){
			customLookAt ();
			if (!firing) StartCoroutine("shoot");
		}
	}

	Transform SearchTarget()
	{
		Collider newTarget = null;
		float radius = transform.FindChild ("Range").transform.localScale.z*2.0f*.5f;

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
	}

	IEnumerator shoot()
	{
		firing = true;
		yield return new WaitForSeconds(.75f);
		GameObject bullet = (GameObject)Instantiate(bulletPrefab, new Vector3(barrel.transform.position.x+.1f,barrel.transform.position.y,barrel.transform.position.z) , Quaternion.identity);
		bullet.transform.LookAt (target);
		GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, new Vector3(barrel.transform.position.x-.1f,barrel.transform.position.y,barrel.transform.position.z) , Quaternion.identity);
		bullet2.transform.LookAt (target);
		GameObject bullet3 = (GameObject)Instantiate(bulletPrefab, new Vector3(barrel2.transform.position.x+.1f,barrel2.transform.position.y,barrel2.transform.position.z) , Quaternion.identity);
		bullet3.transform.LookAt (target);
		GameObject bullet4 = (GameObject)Instantiate(bulletPrefab, new Vector3(barrel2.transform.position.x-.1f,barrel2.transform.position.y,barrel2.transform.position.z) , Quaternion.identity);
		bullet4.transform.LookAt (target);
		firing = false;
	}
}
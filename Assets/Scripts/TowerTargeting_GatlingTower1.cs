using UnityEngine;
using System.Collections;

public class TowerTargeting_GatlingTower1 : MonoBehaviour {

	public GameObject UFO;
	Transform target;
	Vector3 lastSpotToLook;

	public bool firing = false;
	public GameObject bulletPrefab;
	public GameObject gattlingBase;
	public GameObject gattlingGun;
	public GameObject barrel;

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
		float radius = transform.FindChild ("Range").transform.localScale.z*1.75f*.5f;

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
	}

	IEnumerator shoot()
	{
		firing = true;
		yield return new WaitForSeconds(.1f);
		barrel.transform.Rotate (0,0,360*Time.deltaTime);
		GameObject bullet = (GameObject)Instantiate(bulletPrefab, barrel.transform.position, Quaternion.identity);
		bullet.transform.LookAt (target);
		bullet.GetComponent<Rigidbody>().AddForce(barrel.transform.up * 1000);
		firing = false;
	}
}

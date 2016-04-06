using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public GameObject bullet;
	//public GameObject bulletHole;
	public float delayTime = 0.5f;
	private float counter = 0.5f;

	private Vector3 mousePos;
	private float zDistance = 100f;


	void FixedUpdate ()
 	{
		GameObject thePlayer = GameObject.FindWithTag("Player");
		UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl playerScript = thePlayer.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>();

		if (playerScript.aim) {
			if(Input.GetKey(KeyCode.Mouse0) && counter > delayTime)
			{
				mousePos = Input.mousePosition;
				transform.LookAt (Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, zDistance)));
				Instantiate(bullet, transform.position, transform.rotation);
				GetComponent<AudioSource>().Play();
				counter = 0;

				RaycastHit hit;
				Ray ray = new Ray(transform.position, transform.forward);
				if(Physics.Raycast(ray, out hit, zDistance))
				{
					//Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
				}
			}
			counter += Time.deltaTime;
		}

	}
}

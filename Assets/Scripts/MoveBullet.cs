using UnityEngine;
using System.Collections;

public class MoveBullet : MonoBehaviour {

	public float speed = 1f;

	void Start ()	
 	{
		Destroy(gameObject, 5f); //Delete the bullet after 5 seconds
	}
	
	void Update ()	
 	{
		transform.Translate(0, 0, speed);
	}
}

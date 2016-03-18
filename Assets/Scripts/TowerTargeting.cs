using UnityEngine;
using System.Collections;

public class TowerTargeting : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Cube_Tower_1")
			Debug.Log("child goes ouch!");
	}
}

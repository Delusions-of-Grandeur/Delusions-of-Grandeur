using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

	public bool destruct;

	public Transform[] debris;


	// Use this for initialization
	void Start () 
	{
		foreach(Transform go in debris)
		{
			go.gameObject.SetActive(false);
		}
	}

	void Update ()
	{
		if(destruct)
		{
			foreach(Transform go in debris)
			{
				go.gameObject.SetActive(true);
				go.parent = null;
			}
			Destroy(gameObject);
		}
	}
}

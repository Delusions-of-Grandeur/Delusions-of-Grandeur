using UnityEngine;
using System.Collections;

public class Disappear : MonoBehaviour {

	public float time = 5f;
	private float a = 1f;
	
	void Update ()	
 	{
		time -= Time.deltaTime;
		if(time < 0)
		{
			a -= (Time.deltaTime / 3f);
			Color textureColor = GetComponent<Renderer>().material.color;
			textureColor.a = a;
			GetComponent<Renderer>().material.color = textureColor;
			if(a < 0)
			{
				Destroy(transform.parent.gameObject);
			}
			
		}
	}
}

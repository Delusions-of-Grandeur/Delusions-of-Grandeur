using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour {

	void Update()
	{
		StartCoroutine("selfDestruct");
	}

	IEnumerator selfDestruct()
	{
		yield return new WaitForSeconds(0.3f);
		Destroy(gameObject);
	}
}

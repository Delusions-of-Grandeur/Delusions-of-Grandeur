using UnityEngine;
using System.Collections;

public class MoveBullet : MonoBehaviour {

	public float speed = 1f;

	void Start ()
 	{
        Destroy(gameObject, 7f); //Delete the bullet after 7 seconds
    }

    void Update ()
 	{
		transform.Translate(0, 0, speed);
	}

    void OnTriggerEnter(Collider other)
    {
		print (other.tag);
		if(other.tag != "Gatling1" && other.tag != "Gatling2" && other.tag != "SniperTower1" && other.tag != "SniperTower2" && other.tag!="Player"){
			Destroy(gameObject); //Delete the bullet
		}
    }
}

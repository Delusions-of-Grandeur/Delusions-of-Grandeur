using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UFO : MonoBehaviour {

    public GameObject ufo;

    public float MaxHealth;
    public float health;
    bool alive;

	public UnityEngine.UI.Slider slider;


    // Use this for initialization
    void Start () {
        health = MaxHealth;
        alive = true;
    }
	
	// Update is called once per frame
	void Update () {

	}

    void life()
    {
//        print(health);
    }

    /*
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collide");
        if (other.tag == ("Enemy"))
        {
            Debug.Log(health);
            Hurt(5);
        }
    }
    */

    public bool Hurt(float damage)
    {
        if (!alive)
            return false;//exit if dead

        health -= damage;//hurt the enemy

        if (health < 0.1)
        {
            alive = false;
            return true;
        }

        life();
		float percent =  health/MaxHealth;
		print(percent);
		slider.value = percent;
        return health < 0.1;//return if this will die
    }

}

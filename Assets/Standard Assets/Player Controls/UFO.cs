using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UFO : MonoBehaviour {

    public GameObject ufo;

    public float MaxHealth;
    public float health;
    bool alive;

	public UnityEngine.UI.Slider slider;
	public Text gameOverText;

    // Use this for initialization
    void Start () {
		gameOverText.enabled = false;
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

	void GG(){
		print ("gg");
		gameOverText.enabled = true;
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

        if (health < 0.01)
        {
            alive = false;
			GG ();
            return true;
        }
			
        life();
		float percent =  health/MaxHealth;
		print(percent);
		slider.value = percent;
        return health < 0.01;//return if this will die
    }

}

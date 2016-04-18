using UnityEngine;
using System.Collections;

public class AlienHQ : MonoBehaviour {

    public GameObject Enemy;

    public float MaxHealth;
    float health;
    bool alive;


    // Use this for initialization
    void Start () {
        health = MaxHealth;
        alive = true;
        Invoke("startSpawn", Random.Range(4, 19));
	}
	
	// Update is called once per frame
	void Update () {
	}

    void startSpawn()
    {
        InvokeRepeating("Spawn", 1, 2);
    }

    public void Spawn()
    {
        Instantiate(Enemy, transform.position, Quaternion.identity);
        SpawnController.numAlive++;
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.tag == ("Bullet"))
		{
			Hurt(20);      
		} else if (other.tag == ("BulletGatling"))
		{
			Hurt(3f);  
		} else if (other.tag == ("BulletSniper"))
		{
			Hurt(8f);
		}
    }

    bool Hurt(float damage)
    {
        if (!alive)
            return false;//exit if dead

        health -= damage;//hurt the enemy

        if (health < 0.1)
        {
            alive = false;
            Destroy(gameObject);
            return true;
        }

        return health < 0.1;//return if this will die
    }
}

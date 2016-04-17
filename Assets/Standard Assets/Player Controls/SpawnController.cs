using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

    public GameObject EnemySmall;
    public GameObject EnemyLarge;
    GameObject[] objs;
    int top;

    public static int numAlive;
    public int round;
    bool active;

	// Use this for initialization
	void Start () {

        objs = GameObject.FindGameObjectsWithTag("Spawn_Point");
        top = objs.Length;

        numAlive = 0;
        round = 0;
        active = false;

        InvokeRepeating("waveController", 0.01f, 10);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    void waveController()
    {
        if(!active)
        {
            Debug.Log("Active now = true");
            active = true;
            startWave();
        }
        else
        {
            if(numAlive == 0)
            {
                Debug.Log("End round ");
                active = false;
                round++;
            }
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    void startWave()
    {
        Debug.Log("Starting wave: " + round);
        if (round % 3 == 0 && round > 0)
        {
            for(int i = 0; i < round / 3; i++)
            {
                Invoke("SpawnLarge", Random.Range(0, 9));
            }
        }
        else
        {
            for (int i = 0; i < (1 + 2 * round); i++)
            {
                Invoke("SpawnSmall", Random.Range(0, 9));
            }
        }
    }

    public void SpawnSmall()
    {
        Instantiate(EnemySmall, RandomCircle(objs[Random.Range(0, top)].GetComponent<Transform>().position, 4), Quaternion.identity);
        numAlive++;
    }
    public void SpawnLarge()
    {
        Instantiate(EnemyLarge, RandomCircle(objs[Random.Range(0, top)].GetComponent<Transform>().position, 4), Quaternion.identity);
        numAlive++;
    }
}

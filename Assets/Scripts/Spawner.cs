using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject Enemy;
    GameObject[] objs;
    int top;

    void Start()
    {
        objs = GameObject.FindGameObjectsWithTag("Spawn_Point");
        top = objs.Length;
        InvokeRepeating("CreateObject", 0.01f, 5.0f);
    }

    void CreateObject()
    {
        Instantiate(Enemy, objs[Random.Range(0,top)].GetComponent<Transform>().position, Quaternion.identity);
    }

    void Update () {
    }
}

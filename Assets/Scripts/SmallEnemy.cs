using UnityEngine;
using System.Collections;

namespace SpawningFramework
{
    public class SmallEnemy : Enemy
    {
        void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.tag);
            if (other.tag == ("Player"))
            {
                Hurt(53);
                //              Destroy(other.gameObject);        
            }
        }
    }
}
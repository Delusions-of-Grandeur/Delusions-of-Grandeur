using UnityEngine;
using System.Collections;

namespace SpawningFramework
{
    public class SmallEnemy : Enemy
    {
        void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.tag);
            if (other.tag == ("Bullet"))
            {
                Hurt(53);
                //              Destroy(other.gameObject);        
            }
        }
    }
}
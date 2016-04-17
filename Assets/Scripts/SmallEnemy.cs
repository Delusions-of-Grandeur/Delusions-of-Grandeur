using UnityEngine;
using System.Collections;

namespace SpawningFramework
{
    public class SmallEnemy : Enemy
    {

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == ("Bullet"))
            {
                Hurt(20);
                //              Destroy(other.gameObject);        
			} else if (other.tag == ("BulletGatling"))
			{
				Hurt(3f);  
			} else if (other.tag == ("BulletSniper"))
			{
				Hurt(8f);
			}
        }
    }
}
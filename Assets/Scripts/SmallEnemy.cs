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
                Hurt(25);
                //              Destroy(other.gameObject);        
			} else if (other.tag == ("BulletGatling"))
			{
				Hurt(2f);  
			} else if (other.tag == ("BulletSniper"))
			{
				Hurt(8f);
			}
        }
    }
}
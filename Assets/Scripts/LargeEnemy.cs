﻿using UnityEngine;
using System.Collections;

namespace SpawningFramework
{


    public class LargeEnemy : Enemy
    {

        public GameObject HQ;

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

        public override void die()
        {
            Vector3 pos = transform.position;

            alive = false;
            SpawnController.numAlive--;
            Destroy(this.gameObject);
            Destroy(this);

            int dist = 8;
            pos.x+= dist;
            Instantiate(HQ, pos, Quaternion.identity);
            pos.x -= 2 * dist;
            pos.z+= dist;
            Instantiate(HQ, pos, Quaternion.identity);
            pos.z -= 2 * dist;
            Instantiate(HQ, pos, Quaternion.identity);

        }
    }
}
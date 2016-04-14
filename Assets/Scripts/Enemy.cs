using UnityEngine;
using System.Collections;

namespace SpawningFramework
{
    public class Enemy : MonoBehaviour
    {
        public float MaxHealth;
        float health;
		private NavMeshAgent nav;

        StateMachine sm;

        [HideInInspector]
        public bool alive;

        void Update()
        {
			//avoidance logic
        }

        /// Called when this enemy has been spawned
        void Start()
        {
            health = MaxHealth;
            alive = true;
			nav = GetComponent<NavMeshAgent>();
			nav.destination = GameObject.Find("flying Disk landed").transform.position;
        }

        public void life()
        {
            Debug.Log(health);
        }

        public virtual void die()
        {
			GameObject.Find ("Ethan").GetComponent<PlayerDisplay>().money += 100;

            alive = false;
            SpawnController.numAlive--;
            Destroy(this.gameObject);
            Destroy(this);
        }

        /// Hurts the enemy and returns if it dies or not
        public virtual bool Hurt(float damage)
        {
            if (!alive)
                return false;//exit if dead

            health -= damage;//hurt the enemy

            if (health < 0.1)
            {
                die();
                return true;
            }

            return health < 0.1;//return if this will die
        }


    }

}

public class StateMachine
{
    int currentState;

    public void update()
    {
        switch (currentState)
        {
            case 0:     // Default state
//			nav.destination = new Vector3(0,0,0);

                break;
            case 1:     // Attack

                break;
            case 2:     // Pursue

                break;
            case 3:     // Die

                break;
            case 4:
                break;
        }
    }

}

using UnityEngine;
using System.Collections;

namespace SpawningFramework
{
    public class Enemy : MonoBehaviour
    {
        public float MaxHealth;
        float health;
		private NavMeshAgent nav;
		public GameObject goal;

        StateMachine sm;

        [HideInInspector]
        public bool alive;

        void Update()
        {
//			nav.speed = 2f;
//			nav.destination = transform.Find("Ship").position;
			//avoidance logic
			nav.destination = goal.transform.position; 
        }

        /// Called when this enemy has been spawned
        void Start()
        {
            health = MaxHealth;
            alive = true;
			nav = GetComponent<NavMeshAgent>();
        }

        public void life()
        {
            Debug.Log(health);
        }

        public virtual void die()
        {
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
<<<<<<< 28236723540018cfa665178cc65707e5de72cf01
			
=======
//			nav.destination = new Vector3(0,0,0);
>>>>>>> nav works also fixed tower size

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

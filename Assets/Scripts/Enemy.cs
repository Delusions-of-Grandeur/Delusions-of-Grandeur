using UnityEngine;
using System.Collections;

namespace SpawningFramework
{
    public class Enemy : MonoBehaviour
    {
        public float MaxHealth;
        float health;
        private NavMeshAgent nav;

        float attackTimer = 1.5f;
        float coolDown = 1.5f;
        float attackDuration = 0.5f;

        StateMachine sm;

        [HideInInspector]
        public bool alive;

        void Update()
        {
            nav.destination = GameObject.Find("flying Disk landed").transform.position;

            if (Vector3.Distance(this.getDest(), this.transform.position) < 2)
            {
                if (attackTimer > 0)
                    attackTimer -= Time.deltaTime;
                if (attackTimer < 0)
                    attackTimer = 0;

                if (attackTimer == 0)
                {
                    Attack();
                    attackTimer = coolDown;
                }

                nav.Stop();
            }
        }

        void GoToIdle()
        {
        }

        void Attack()
        {
            GameObject.Find("flying Disk landed").GetComponent<UFO>().Hurt(5);
        }

        /// Called when this enemy has been spawned
        void Start()
        {
            health = MaxHealth;
            alive = true;
			nav = GetComponent<NavMeshAgent>();
        }

        public void Go()
        {
            nav.destination = GameObject.Find("flying Disk landed").transform.position;
        }

        public void Stop()
        {
            nav.Stop();
        }

        public Vector3 getDest()
        {
            return nav.destination;
        }

        public void life()
        {
            Debug.Log(health);
        }

        public virtual void die()
        {
            alive = false;
            SpawnController.numAlive--;
            Invoke("Delete", 7);
            //set animation to dead
        }

        void Delete()
        {
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

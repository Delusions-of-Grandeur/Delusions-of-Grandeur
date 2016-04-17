using UnityEngine;
using System.Collections;

namespace SpawningFramework
{
    public class Enemy : MonoBehaviour
    {
        public float MaxHealth;
        float health;
		private NavMeshAgent nav;
		public bool canMakeIt;

        StateMachine sm;

        [HideInInspector]
        public bool alive;

		public Transform target;
		private NavMeshPath path;
		private float elapsed = 0.0f; 

        void Update()
        { 
			//avoidance logic
			elapsed += Time.deltaTime;
			if (elapsed > 1.0f) {
				elapsed -= 1.0f;
				if (Vector3.Distance (transform.position, target.position) < 10) {
					canMakeIt = true;
				} else {
					if (NavMesh.CalculatePath (transform.position, target.position, NavMesh.AllAreas, path)) {
						canMakeIt = true;
					} else {
//						print (path.status);
						canMakeIt = false;
					}
				}
				 
			}
			for (int i = 0; i < path.corners.Length-1; i++)
				Debug.DrawLine(path.corners[i], path.corners[i+1], Color.red);
        }

        /// Called when this enemy has been spawned
        void Start()
        {
//            health = MaxHealth;
			health = 1000;
            alive = true;


			target = GameObject.Find ("flying Disk landed").transform;
			nav = GetComponent<NavMeshAgent>();
			nav.destination = target.position;
			path = new NavMeshPath();
			canMakeIt = true;
			elapsed = 0.0f;
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

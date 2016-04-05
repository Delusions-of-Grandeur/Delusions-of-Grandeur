using UnityEngine;
using System.Collections;

namespace SpawningFramework
{
    public class Enemy : MonoBehaviour
    {

        public float MaxHealth;
        float health;
        StateMachine sm;

        [HideInInspector]
        public bool alive;

        void Update()
        {
        }

        /// Called when this enemy has been spawned
        void Start()
        {
            health = MaxHealth;
            alive = true;
//            InvokeRepeating("move", 0.01f, .2f);

        }

        void move()
        {
            transform.Translate(0, 0.025f, 0);
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.tag);
            if (other.tag == ("Bullet"))
            {
                Hurt(34);
//              Destroy(other.gameObject);        
            }
        }

        /// Hurts the enemy and returns if it dies or not
        public virtual bool Hurt(float damage)
        {
            if (!alive)
                return false;//exit if dead

            health -= damage;//hurt the enemy

            if (health < 0.1)
            {
                alive = false;
                SpawnController.numAlive--;
                Destroy(this.gameObject);
                Destroy(this);
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
                // call navigation torwards ship

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
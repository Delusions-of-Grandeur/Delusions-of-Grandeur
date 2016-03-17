using UnityEngine;
using System.Collections;

namespace SpawningFramework
{
    public class Enemy : MonoBehaviour
    {

        public float MaxHealth;
        float health;
        Vector3 pos;


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
            pos = transform.position;
            
            InvokeRepeating("move", 0.01f, .2f);

        }

        void move()
        {
            transform.Translate(0, 0.025f, 0);
        }

        /// Called when the player loses
        public virtual void OnGameOver()
        {
        }

        #region Taking Damage
        /// Hurts the enemy and returns if it dies or not
        public virtual bool Hurt(float damage)
        {
            if (!alive)
                return false;//exit if dead

            health -= damage;//hurt the enemy

            if (health < 0.1)
            {
                alive = false;
                return true;
            }

            return health < 0.1;//return if this will die
        }


        #endregion
    }
}

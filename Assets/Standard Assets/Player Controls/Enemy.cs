using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

namespace SpawningFramework
{
	public class Enemy : MonoBehaviour
	{
		public float MaxHealth;
		float health;
		private NavMeshAgent nav;
		Vector3 dest;
		GameObject[] objs;

		float attackTimer = 1.5f;
		float coolDown = 1.5f;
		float attackDuration = 0.5f;

		StateMachine sm;

		public Animator anim;

		[HideInInspector]
		public bool alive;

		void Update()
		{
			nav.destination = dest;

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
			anim.SetBool ("Attack", true);
		}

		/// Called when this enemy has been spawned
		void Start()
		{
			health = MaxHealth;
			alive = true;
			nav = GetComponent<NavMeshAgent>();
			anim = GetComponent<Animator> ();
			//          dest = GameObject.Find("flying Disk landed").transform.position;

			objs = GameObject.FindGameObjectsWithTag("Waypoint");
			int top = objs.Length;
			float length = 9999f;

			for(int i = 0; i < top; i++)
			{
				float temp = Vector3.Distance(objs[i].transform.position, this.transform.position);
				if (temp < length)
				{
					length = temp;
					dest = objs[i].transform.position;
				}
			}

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
			GameObject.Find ("Ethan").GetComponent<PlayerDisplay>().money += 100;
			Collider col =  this.GetComponent<Collider> ();
			col.enabled = false;

			alive = false;
			SpawnController.numAlive--;
			anim.SetBool ("Dead", true);
			nav.Stop ();
			Invoke("Delete", 3.5f);
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

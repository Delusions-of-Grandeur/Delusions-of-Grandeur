using UnityEngine;
using System.Collections;

//this script is basically the stats for each weapon
public class WeaponControl : MonoBehaviour {

	public bool equip; //if the weapon is equiped
	public WeaponManager.WeaponType weaponType; //the weapon type

	//pretty much self explanatory
	public int MaxAmmo; 
	public int MaxClipAmmo = 30;
	public int curAmmo;
	public bool CanBurst;
	public float Kickback = 0.1f;

	public GameObject HandPosition; //where the left hand should go
	public GameObject bulletPrefab; //the bullet prefab, this is an absolete variable
	public Transform bulletSpawn; //where the bullets come from
	
	//absolete variables
	GameObject bulletSpawnGO; 
	ParticleSystem bulletPart;
	bool fireBullet;
	AudioSource audioSource;
	
	//reference to our weapon manager from the parent
	WeaponManager parentControl;

	//reference to the weapon animator
	Animator weaponAnim;
	
	//we store here the equip and unequip position/rotation
	[Header("Positions")]
	public bool hasOwner;
	public Vector3 EquipPosition;
	public Vector3 EquipRotation;
	public Vector3 UnEquipPosition;
	public Vector3 UnEquipRotation;
	//Debug Scale
	Vector3 scale;

	//on which body part should the weapon be placed if it's not equipped?
	public RestPosition restPosition;
	public enum RestPosition
	{
		RightHip,
		Waist
	}


	void Start () 
	{
		curAmmo = MaxClipAmmo; 
		
		//we don't use this anymore
		/*bulletSpawnGO = Instantiate(bulletPrefab, transform.position,Quaternion.identity) as GameObject;
		bulletSpawnGO.AddComponent<ParticleDirection>();
		bulletSpawnGO.GetComponent<ParticleDirection>().weapon = bulletSpawn;
		bulletPart = bulletSpawnGO.GetComponent<ParticleSystem>();*/

		//store our references and our scale
		audioSource = GetComponent<AudioSource>();
		weaponAnim = GetComponent<Animator>();
		scale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//the local scale is always the stored scale
		//we do this because we don't have consistent scale for all our assets and parenting/unparenting children might mess with the scale
		transform.localScale = scale;

		//if the weapon is equipped
		if(equip)
		{
			//put it to the correct position and rotation
			transform.parent = transform.GetComponentInParent<WeaponManager>().transform.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightHand);
			transform.localPosition = EquipPosition;
			transform.localRotation = Quaternion.Euler(EquipRotation);

			//we don't use this anymore but we will in the future
			/*
			if(fireBullet)
			{
				if(curAmmo > 0)
				{
					curAmmo --;
					bulletPart.Emit(1);
					audioSource.Play();

					if(weaponType == WeaponManager.WeaponType.Pistol)
					weaponAnim.SetTrigger("Fire");
				
					fireBullet = false;
				}
				else
				{
					if(MaxAmmo >= MaxClipAmmo)
					{
						curAmmo = MaxClipAmmo;
						MaxAmmo -= MaxClipAmmo;
					}
					else
					{
						curAmmo = MaxClipAmmo - (MaxClipAmmo - MaxAmmo);
						
					}

					fireBullet = false;
					Debug.Log("Reload");
				}
			}*/
		}
		else//if it's not equiped
		{
			if(hasOwner)//but it has an owner
			{
				//put it on the rest position we setted up
				switch (restPosition)
				{
					case RestPosition.RightHip:
						transform.parent = transform.GetComponentInParent<WeaponManager>().transform.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightUpperLeg);
						break;
					case RestPosition.Waist:
						transform.parent = transform.GetComponentInParent<WeaponManager>().transform.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Spine);
						break;
				}

				transform.localPosition = UnEquipPosition;
				transform.localRotation = Quaternion.Euler(UnEquipRotation);
			}
		}
	}

	//absolete function
	public void Fire()
	{
		fireBullet = true;
	}
}

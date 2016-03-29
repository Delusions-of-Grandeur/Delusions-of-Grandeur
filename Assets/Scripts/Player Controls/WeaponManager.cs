using UnityEngine;
using System.Collections;
using System.Collections.Generic; //we want to use list so we add the generic namespace

//This script is the manager for the inventory of our characters
//either it is a player or an enemy character
public class WeaponManager : MonoBehaviour {

	//setup our list of current weapons
	public List<GameObject> WeaponList = new List<GameObject>();
	
	public WeaponControl ActiveWeapon; //we store our active weapon here
	int weaponNumber = 0; //index of our weapons

	public bool aim; // if the char is aiming

	//our weapon types
	public enum WeaponType
	{
		Pistol,
		Rifle
	}

	//the current weapon type
	public WeaponType weaponType;
	
	Animator anim; //reference to our animator

	float IKweight; //our ik weight



	void Start ()
	{
		//the active weapon is taken from our weapon list's index
		ActiveWeapon = WeaponList[weaponNumber].GetComponent<WeaponControl>();
		ActiveWeapon.equip = true; //and we equip that weapon

		anim = GetComponent<Animator>(); //setup the refence to the animator

		//and for every weapon that we have on our list, we inform it that they have an owner
		//so they are placed in the correct position
		foreach(GameObject go in WeaponList)
		{
			go.GetComponent<WeaponControl>().hasOwner = true;
		}

	}
	
	void Update () 
	{
		//The ikweight is based on if we are aiming or not
		IKweight = Mathf.MoveTowards(IKweight,(aim)? 1.0f : 0.0f, Time.deltaTime * 5);

		//Again the active weapon is based on the index of the weapon list
		ActiveWeapon = WeaponList[weaponNumber].GetComponent<WeaponControl>();
		ActiveWeapon.equip = true;

		//and our weapon type is the same as the active weapon
		weaponType = ActiveWeapon.weaponType;

		//we setup the correct animations for each weapon, based on weapon type
		switch(weaponType)
		{
		case WeaponType.Pistol:
			anim.SetInteger("Weapon",0);
			break;
		case WeaponType.Rifle:
			anim.SetInteger("Weapon",1);
			break;
		}

	}

	//every IK functions from the build in IK must go inside OnAnimatorIK
	void OnAnimatorIK()
	{
		//the IK weight of the left hand is controlled by the local IKweight
		anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKweight);
		anim.SetIKRotationWeight(AvatarIKGoal.LeftHand,IKweight);

		//the Ik position for the left hand is taken by the hand placement go of the active weapon
		Vector3 pos = ActiveWeapon.HandPosition.transform.TransformPoint(Vector3.zero);
		anim.SetIKPosition(AvatarIKGoal.LeftHand,ActiveWeapon.HandPosition.transform.position);
		anim.SetIKRotation(AvatarIKGoal.LeftHand,ActiveWeapon.HandPosition.transform.rotation);
	}

	//this is an absolete function, we don't use it anymore but we might in the future
	public void FireActiveWeapon()
	{
		if(ActiveWeapon != null)
		{
			ActiveWeapon.Fire();
		}
	}

	//picks the correct weapon from the weapon list, depending if we want to ascend or descend 
	public void ChangeWeapon(bool Ascending)
	{
		if(WeaponList.Count > 1)
		{
			ActiveWeapon.equip = false;

			if(Ascending)
			{
				if(weaponNumber < WeaponList.Count - 1)
				{
					weaponNumber++;
				}
				else
				{
					weaponNumber = 0;
				}
			}
			else
			{
				if(weaponNumber > 0)
				{
					weaponNumber--;
				}
				else
				{
					weaponNumber = WeaponList.Count - 1;
				}
			}
		}
	}
}






using UnityEngine;
using System.Collections;

//This script is basically how the user controls the character
public class UserInput : MonoBehaviour {

	public bool walkByDefault = false; //If we want to walk by default
	
	private CharMove character; //reference to our character movement script
	private Transform cam; //reference to our case
	private Vector3 camForward; //stores the forward vector of the cam
	private Vector3 move; //our move vector

	public bool aim; //if we are aiming
	public float aimingWeight; //the aiming weight, helps with IK

	public bool lookInCameraDirection; // if we want the character to look at the same direction as the camera
	Vector3 lookPos; //the looking position

	Animator anim; //reference to our animator

	WeaponManager weaponManager; //reference to the weapon manager

	public bool debugShoot; //helps us debug the shooting (basically shoots the current weapon)
	WeaponManager.WeaponType weaponType; //the current weapon type we have equipped

	CapsuleCollider col; //reference to our collider
	float startHeight; //we store the starting heigh of our collider here


	//Ik stuff
	[SerializeField] public IK ik;
	[System.Serializable] public class IK
	{
		public Transform spine; //the bone where we rotate the body of our character from
		//The Z/x/y values, doesn't really matter the values here since we ovveride them depending on the weapon
		public float aimingZ = 213.46f; 
		public float aimingX = -65.93f;
		public float aimingY = 20.1f;
		//The point in the ray we do from our camera, basically how far the character looks
		public float point = 30; 
		
		public bool DebugAim; 
		//Help us debug the aim, basically makes it possible to change the current values 
		//on runtime since we are hardcoding them
	}

	//Reference to the camera
	FreeCameraLook cameraFunctions;


	// Use this for initialization
	void Start ()
	{
		//Setup our camera reference
		if(Camera.main != null)
		{
			cam = Camera.main.transform;
		}

		//and our Character Movement
		character = GetComponent<CharMove> ();
		//and our animator
		anim = GetComponent<Animator>();
		//and our weapon manager
		weaponManager = GetComponent<WeaponManager>();
		//and the collider
		col = GetComponent<CapsuleCollider>();
		//store the starting height
		startHeight = col.height;

		//And setup the reference to the FreeCameraLook, 
		//but since we already have store the camera, 
		//we can navigate to the root transform to get the component from there instead of searching for it
		cameraFunctions = Camera.main.transform.root.GetComponent<FreeCameraLook>();
		
		//Store the offset of the crosshair
		offsetCross = cameraFunctions.crosshairOffsetWiggle;
	}

	//Function that corrects the Ik depending on the current weapon type
	void CorrectIK()
	{
		weaponType = weaponManager.weaponType;

		if(!ik.DebugAim)
		{
			switch(weaponType)
			{
			case WeaponManager.WeaponType.Pistol:
				ik.aimingZ = 221.4f;
				ik.aimingX = -71.5f;
				ik.aimingY = 20.6f;
				break;
			case WeaponManager.WeaponType.Rifle:
				ik.aimingZ = 212.19f;
				ik.aimingX = -66.1f;
				ik.aimingY = 14.1f;
				break;
			}
		}
	}

	void AdditionalInput()
	{
		//if we are running
		if(anim.GetFloat("Forward")> 0.5f)
		{
			//and we press crouch
			if(Input.GetButtonDown("Crouch"))
			{
				//we slide, so tell the animator to play the slide animation
				//The transition we set it was with the Vault trigger
				anim.SetTrigger("Vault");
			}
		}
	}

	void HandleCurves()
	{
		//Get the stored curve from the animator
		float sizeCurve = anim.GetFloat("ColliderSize");
		//and the new center we want for the capsule collider
		float newYcenter = 0.3f;

		//And lerp between the original height and center and the new ones, depending on t = curve
		float lerpCenter = Mathf.Lerp(1,newYcenter,sizeCurve);
		col.center = new Vector3(0,lerpCenter,0);
		col.height = Mathf.Lerp(startHeight, 0.5f, sizeCurve);

	}

	void Update()
	{
		CorrectIK();

		if(!ik.DebugAim) //if we do not debug the aim
		aim = Input.GetMouseButton (1); //then the aim bool is controlled by the right mouse click

		//the same goes for the aim of the weapon manager
		weaponManager.aim = aim;

		//if we are aiming
		if(aim)
		{
			//And our active weapon can't burst fire
			if(!weaponManager.ActiveWeapon.CanBurst)
			{
				//and we left click
				if(Input.GetMouseButtonDown(0) || debugShoot)
				{
					//Then shoot
					anim.SetTrigger("Fire"); //play the animation to shoot
					//weaponManager.FireActiveWeapon();
					ShootRay();//Call our shooting ray, see below
					//and wiggle our crosshair and camera
					cameraFunctions.WiggleCrosshairAndCamera(weaponManager.ActiveWeapon, true);
				}
			}
			else //if it can burst fire
			{
				//then do the same as above for as long the fire mouse button is pressed
				if(Input.GetMouseButton(0) || debugShoot)
				{
					anim.SetTrigger("Fire");
					//weaponManager.FireActiveWeapon();
					ShootRay();
					cameraFunctions.WiggleCrosshairAndCamera(weaponManager.ActiveWeapon, true);
				}
			}
		}

		//Switches between our weapons, linear
		if(Input.GetAxis("Mouse ScrollWheel") <= -0.1f)
			{
				weaponManager.ChangeWeapon(false);
			}

			if(Input.GetAxis("Mouse ScrollWheel") >= 0.1f)
			{
				weaponManager.ChangeWeapon(true);
			}

		AdditionalInput();
		HandleCurves();
	}

	//the prefab for our bullets
	public GameObject bulletPrefab;

	//Shoots a ray everytime we shoot
	void ShootRay()
	{
		//find the center of the screen
		float x = Screen.width /2;
		float y = Screen.height/2;

		//and make a ray from it
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(x,y,0));
		RaycastHit hit;

		//Instantiate the bullet prefab that has a line render and store it in a variable
		GameObject go = Instantiate(bulletPrefab,transform.position,Quaternion.identity) as GameObject;
		LineRenderer line = go.GetComponent<LineRenderer>();
		
		//the first position of or "bullet" will be the bullet spawn point
		//of our active weapon, converted from local to world position
		Vector3 startPos = weaponManager.ActiveWeapon.bulletSpawn.TransformPoint(Vector3.zero);
		Vector3 endPos = Vector3.zero;

		//bit shift a layer mask
		int mask = ~(1<< 8);

		//so that our raycast collides with all the colliders in all the layers, except the one masked
		if(Physics.Raycast(ray,out hit,Mathf.Infinity,mask))
		{
			//find the distance between the bullet spawn position and the hit.point
			float distance = Vector3.Distance(weaponManager.ActiveWeapon.bulletSpawn.transform.position, hit.point);

			//and raycast everything in that direction and for that distance
			RaycastHit[] hits = Physics.RaycastAll(startPos,hit.point - startPos, distance);

			//and for every hit
			foreach(RaycastHit hit2 in hits)
			{
				//Add the logic to what happens on whatever we hit
				
				//for example, if we hit a gameobject that has a rigidbody
				if(hit2.transform.GetComponent<Rigidbody>())
				{
					//then apply the appropriate force at the correct direction
					Vector3 direction = hit2.transform.position - transform.position;
					direction = direction.normalized;
					hit2.transform.GetComponent<Rigidbody>().AddForce(direction * 200);
				}
				else if(hit2.transform.GetComponent<Destructible>())
				{
					//or if we hit an object that has a destructible component, then tell it to destruct
					hit2.transform.GetComponent<Destructible>().destruct = true;
				}
			}

			//the end position of our bullet is the hit.point
			endPos = hit.point;
		}
		else //else if the raycast didn't hit anything
		{
			//the end position will be a far away point upon the ray
			endPos = ray.GetPoint(100);
		}

		//set up the positions to the line renderer
		line.SetPosition(0,startPos);
		line.SetPosition(1,endPos);
	}


	//We do everything that has to do with IK on LateUpdate and after the animations have played to remove jittering
	void LateUpdate()
	{
		//our aiming weight smoothly becomes 0 or 1 depending if we are aiming or not, 
		aimingWeight = Mathf.MoveTowards(aimingWeight, (aim)? 1.0f : 0.0f , Time.deltaTime * 5);
		
		//the normal and aiming state of the camera, basically how much close to the player it is
		Vector3 normalState = new Vector3(0,0,-2f);
		Vector3 aimingState = new Vector3(0,0,-0.5f);
		
		//and that is lerped depending on t = aimigweight
		Vector3 pos = Vector3.Lerp(normalState,aimingState,aimingWeight);
		cam.transform.localPosition= pos;

		if(aim) //if we aim
		{
			//pass the new rotation to the IK bone
			Vector3 eulerAngleOffset = Vector3.zero;
			eulerAngleOffset = new Vector3(ik.aimingX,ik.aimingY,ik.aimingZ);

			//do a ray from the center of the camera and forward
			Ray ray = new Ray(cam.position, cam.forward);

			//find where the character should look
			Vector3 lookPosition = ray.GetPoint(ik.point);

			//and apply the rotation to the bone
			ik.spine.LookAt(lookPosition);
			ik.spine.Rotate(eulerAngleOffset, Space.Self);
		}
	}

	//our variables where we store our input and the offset of the crosshair 
	float horizontal;
	float vertical;
	float offsetCross;

	void FixedUpdate () 
	{
		//our connection with the variables and our Input
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		//basically this makes the crosshair expand depending if there is any movement
		if(horizontal < -offsetCross || horizontal > offsetCross || vertical < -offsetCross || vertical > offsetCross)
		{
			cameraFunctions.WiggleCrosshairAndCamera(weaponManager.ActiveWeapon, false);
		}

		//if we are not aiming
		if(!aim)
		{
			if(cam != null) //if there is a camera
			{
				//Take the forward vector of the camera (from its transform) and 
				// eliminate the y component
				// scale the camera forward with the mask (1, 0, 1) to eliminate y and normalize it
				camForward = Vector3.Scale(cam.forward, new Vector3(1,0,1)).normalized;

				//move input front/backward = forward direction of the camera * user input amount (vertical)
				//move input left/right = right direction of the camera * user input amount (horizontal)
				move = vertical * camForward + horizontal * cam.right;
			}
			else
			{
				//if there is not a camera, use the global forward (+z) and right (+x)
				move = vertical * Vector3.forward + horizontal * Vector3.right;
			}
		}
		else //but if we are aiming
		{
			//we pass a zero to the move input
			move = Vector3.zero;

			//we make the character look where the camera is looking
			Vector3 dir = lookPos - transform.position;
			dir.y = 0;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);

			//and we directly manipulate the animator
			//this works because we've set up from our other script
			//to take every movement in the animator and convert it to a force to be applied to the rigidbody
			anim.SetFloat("Forward",vertical);
			anim.SetFloat("Turn",horizontal);
		}

		if (move.magnitude > 1) //Make sure that the movement is normalized
			move.Normalize ();

		bool walkToggle = Input.GetKey (KeyCode.LeftShift) || aim; //check for walking input or aiming input

		//the walk multiplier determines if the character is running or walking
		//if walkByDefault is set and walkToggle is pressed
		float walkMultiplier = 1;

		if(walkByDefault) {
			if(walkToggle) {
				walkMultiplier = 1;
			} else {
				walkMultiplier = 0.5f;
			}
		} else {
			if(walkToggle) {
				walkMultiplier = 0.5f;
			} else {
				walkMultiplier = 1;
			}
		}

		//Our look position depends on if we want the character to look towards the camera or not
		lookPos = lookInCameraDirection && cam != null ? transform.position + cam.forward * 100 : transform.position + transform.forward * 100;

		//apply the multiplier to our move input
		move *= walkMultiplier;

		//pass it to our move function from our character movement script
		character.Move (move,aim,lookPos);
	}
}

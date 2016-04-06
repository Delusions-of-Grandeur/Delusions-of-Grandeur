using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent (typeof(ThirdPersonCharacter))]
	public class ThirdPersonUserControl : MonoBehaviour
	{
		private ThirdPersonCharacter m_Character;
		// A reference to the ThirdPersonCharacter on the object
		private Transform m_Cam;
		// A reference to the main camera in the scenes transform
		private Vector3 m_CamForward;
		// The current forward direction of the camera
		private Vector3 m_Move;
		private bool m_Jump;
		// the world-relative desired move direction, calculated from the camForward and user input.

		public bool aim;
		//if we are aiming
		public float aimingWeight;
		//the aiming weight, helps with IK

		public bool lookInCameraDirection;
		// if we want the character to look at the same direction as the camera
		Vector3 lookPos;
		//the looking position

		Animator anim;

		public float normalZ;
		public float aimZ;
		public float aimX;

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

		private void Start ()
		{
			// get the transform of the main camera
			if (Camera.main != null) {
				m_Cam = Camera.main.transform;
			} else {
				Debug.LogWarning (
					"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
				// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
			}

			// get the third person character ( this should never be null due to require component )
			m_Character = GetComponent<ThirdPersonCharacter> ();
			anim = GetComponent<Animator> ();


			normalZ = -1.2f;
			aimZ = 0.0f;
			aimX = 0.1f;
		}


		private void Update ()
		{
			if (!m_Jump) {
				m_Jump = CrossPlatformInputManager.GetButtonDown ("Jump");
			}
			//then the aim bool is controlled by the right mouse click
			if (Input.GetMouseButtonUp (1)) {
				aim = !aim;
			}
		}

		void LateUpdate ()
		{

			//our aiming weight smoothly becomes 0 or 1 depending if we are aiming or not,

			aimingWeight = Mathf.MoveTowards (aimingWeight, (aim) ? 1.0f : 0.0f, Time.deltaTime * 5);

			//the normal and aiming state of the camera, basically how much close to the player it is
			Vector3 normalState = new Vector3 (0, 0, normalZ);
			Vector3 aimingState = new Vector3(aimX,0,aimZ);

			//and that is lerped depending on t = aimigweight
			Vector3 pos = Vector3.Lerp (normalState, aimingState, aimingWeight);

			m_Cam.transform.localPosition = pos;

			if (aim) { //if we aim
				//pass the new rotation to the IK bone
				Vector3 eulerAngleOffset = Vector3.zero;
				//eulerAngleOffset = new Vector3 (ik.aimingX, ik.aimingY, ik.aimingZ);

				//do a ray from the center of the camera and forward
				Ray ray = new Ray (m_Cam.position, m_CamForward);

				//find where the character should look
				//Vector3 lookPosition = ray.GetPoint (ik.point);

				//and apply the rotation to the bone
				//ik.spine.LookAt (lookPosition);
				//ik.spine.Rotate (eulerAngleOffset, Space.Self);
			}
		}


			}
		}



		// Fixed update is called in sync with physics
		private void FixedUpdate ()
		{
			// read inputs
			float h = CrossPlatformInputManager.GetAxis ("Horizontal");
			float v = CrossPlatformInputManager.GetAxis ("Vertical");
			bool crouch = Input.GetKey (KeyCode.C);


			if (!aim) {
				// calculate move direction to pass to character
				if (m_Cam != null) {
					// calculate camera relative direction to move:
					m_CamForward = Vector3.Scale (m_Cam.forward, new Vector3 (1, 0, 1)).normalized;
					m_Move = v * m_CamForward + h * m_Cam.right;
				} else {
					// we use world-relative directions in the case of no main camera
					m_Move = v * Vector3.forward + h * Vector3.right;
				}
				#if !MOBILE_INPUT
				// walk speed multiplier
				if (Input.GetKey (KeyCode.LeftShift))
					m_Move *= 0.5f;
				#endif

				//Our look position depends on if we want the character to look towards the camera or not
				if (lookInCameraDirection && m_Cam != null) {
					lookPos = transform.position + m_Cam.forward * 100;
				} else {
					lookPos = transform.position + transform.forward * 100;
				}
			} else { //but if we are aiming
				//we pass a zero to the move input
				m_Move = Vector3.zero;

				//Our look position depends on if we want the character to look towards the camera or not
				if (lookInCameraDirection && m_Cam != null) {
					lookPos = transform.position + m_Cam.forward * 100;
				} else {
					lookPos = transform.position + transform.forward * 100;
				}
				//and we directly manipulate the animator
				//this works because we've set up from our other script
				//to take every movement in the animator and convert it to a force to be applied to the rigidbody
				anim.SetFloat("Forward",v);
				anim.SetFloat("Turn",h);
			}


			// pass all parameters to the character control script
			m_Character.Move (m_Move, crouch, m_Jump, aim, lookPos);
			m_Jump = false;
		}
	}
}

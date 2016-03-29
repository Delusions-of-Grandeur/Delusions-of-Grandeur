using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
  [RequireComponent(typeof (ThirdPersonCharacter))]
  public class ThirdPersonUserControl : MonoBehaviour
  {
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

    public bool aim; //if we are aiming
    public float aimingWeight; //the aiming weight, helps with IK

    public bool lookInCameraDirection; // if we want the character to look at the same direction as the camera
    Vector3 lookPos; //the looking position

    private void Start()
    {
      // get the transform of the main camera
      if (Camera.main != null)
      {
        m_Cam = Camera.main.transform;
      }
      else
      {
        Debug.LogWarning(
        "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
        // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
      }

      // get the third person character ( this should never be null due to require component )
      m_Character = GetComponent<ThirdPersonCharacter>();
    }


    private void Update()
    {
      if (!m_Jump)
      {
        m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
      }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
      // read inputs
      float h = CrossPlatformInputManager.GetAxis("Horizontal");
      float v = CrossPlatformInputManager.GetAxis("Vertical");
      bool crouch = Input.GetKey(KeyCode.C);


      if (!aim) {
        // calculate move direction to pass to character
        if (m_Cam != null)
        {
          // calculate camera relative direction to move:
          m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
          m_Move = v*m_CamForward + h*m_Cam.right;
        }
        else
        {
          // we use world-relative directions in the case of no main camera
          m_Move = v*Vector3.forward + h*Vector3.right;
        }
        #if !MOBILE_INPUT
        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
        #endif

        //Our look position depends on if we want the character to look towards the camera or not
  			if (lookInCameraDirection && m_Cam != null) {
  				lookPos = transform.position + m_Cam.forward * 100;
  			} else {
  				lookPos = transform.position + transform.forward * 100;
  			}
      } else //but if we are aiming
  		{
  			//we pass a zero to the move input
			m_Move = Vector3.zero;

  			//we make the character look where the camera is looking
  			Vector3 dir = lookPos - transform.position;
  			dir.y = 0;
  			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);

  			//and we directly manipulate the animator
  			//this works because we've set up from our other script
  			//to take every movement in the animator and convert it to a force to be applied to the rigidbody
  			// anim.SetFloat("Forward",vertical);
  			// anim.SetFloat("Turn",horizontal);
  		}


      // pass all parameters to the character control script
      m_Character.Move(m_Move, crouch, m_Jump, aim, lookPos);
      m_Jump = false;
    }
  }
}

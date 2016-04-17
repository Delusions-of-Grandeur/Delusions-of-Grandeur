﻿using UnityEngine;
using UnityEditor;

public class FreeCameraLook : Pivot {

	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float turnSpeed = 1.5f;
	[SerializeField] private float turnsmoothing = .1f;
	[SerializeField] private float tiltMax = 75f;
	[SerializeField] private float tiltMin = 45f;
	[SerializeField] private bool lockCursor = false;


	private float lookAngle;
	private float tiltAngle;

	private const float LookDistance = 100f;

	private float smoothX = 0;
	private float smoothY = 0;
	private float smoothXvelocity = 0;
	private float smoothYvelocity = 0;


	public Crosshair activeCrosshair;
	public float crosshairOffsetWiggle = 0.2f;

	public GameObject refPivot;


	protected override void Awake()
	{
		base.Awake();

		Screen.lockCursor = lockCursor;

		if(lockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		cam = GetComponentInChildren<Camera>().transform;

		pivot = cam.parent;
	}

	void Start()
	{
		//ChangeCrosshair();
		pivot.position = refPivot.gameObject.transform.position;
	}

	public void ChangeCrosshair()
	{
		//activeCrosshair = GameObject.FindGameObjectWithTag("CrosshairManager").GetComponent<CrosshairManager>().activeCrosshair;
	}

	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();

		HandleRotationMovement();

		if(lockCursor && Input.GetMouseButtonUp(0))
		{
			Screen.lockCursor = lockCursor;
		}
	}

	void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	protected override void Follow (float deltaTime)
	{
		transform.position = Vector3.Lerp(transform.position, target.position, deltaTime * moveSpeed);
	}


	float offsetX;
	float offsetY;

	void handleOffsets()
	{
		if(offsetX != 0)
		{
			offsetX = Mathf.MoveTowards(offsetX,0,Time.deltaTime);
		}

		if(offsetY != 0)
		{
			offsetY = Mathf.MoveTowards(offsetY,0,Time.deltaTime);
		}
	}



	void HandleRotationMovement()
	{
		handleOffsets();

		float x = Input.GetAxis("Mouse X") + offsetX;
		float y = Input.GetAxis("Mouse Y") + offsetY;

		if(turnsmoothing > 0)
		{
			smoothX = Mathf.SmoothDamp(smoothX,x,ref smoothXvelocity, turnsmoothing);
			smoothY = Mathf.SmoothDamp(smoothY,y,ref smoothYvelocity,turnsmoothing);
		}
		else
		{
			smoothX = x;
			smoothY = y;
		}

		lookAngle += smoothX*turnSpeed;

		transform.rotation = Quaternion.Euler(0f, lookAngle,0);

		tiltAngle -= smoothY * turnSpeed;
		tiltAngle = Mathf.Clamp(tiltAngle, - tiltMin, tiltMax);

		pivot.localRotation = Quaternion.Euler(tiltAngle,0,0);

		if(x > crosshairOffsetWiggle || x < -crosshairOffsetWiggle || y > crosshairOffsetWiggle || y <-crosshairOffsetWiggle)
		{
			//activeCrosshair.WiggleCrosshair();
		}
	}

	public void WiggleCrosshairAndCamera(WeaponControl weapon, bool shoot)
	{
		activeCrosshair.WiggleCrosshair();

		if(shoot)
		{
			offsetY = weapon.Kickback;
		}
	}





}

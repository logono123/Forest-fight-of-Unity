using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FPSController))]


public class FPSInputController : MonoBehaviour
{
	private GunHanddle gunHanddle;
	private FPSController FPSmotor;


	
	void Start(){
		Application.targetFrameRate = 60;
		MouseLock.MouseLocked = true;

	}
	void Awake ()
	{
		FPSmotor = GetComponent<FPSController> ();		
		gunHanddle = GetComponent<GunHanddle> (); 
	}

	void Update ()
	{

		FPSmotor.Aim(new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y")));
		FPSmotor.Move (new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical")));
		FPSmotor.Jump (Input.GetButton ("Jump"));
		
		if(Input.GetKey(KeyCode.LeftShift)){
			FPSmotor.Boost(1.7f);	
		}
		
		FPSmotor.Holdbreath(1);	
		
		if(Input.GetKey(KeyCode.LeftShift)){
			FPSmotor.Holdbreath(0);	
		}
		if(Input.GetButton("Fire1")){
			gunHanddle.Shoot();	
		}
		if(Input.GetButtonDown("Fire2")){
			gunHanddle.Zoom();	
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0){
			gunHanddle.ZoomAdjust(-1);	
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0){
			gunHanddle.ZoomAdjust(1);	
		}
		if(Input.GetKeyDown(KeyCode.R)){
			gunHanddle.Reload();
		}
		if(Input.GetKeyDown(KeyCode.Q)){
			gunHanddle.SwitchGun();
		}
		if(Input.GetKeyDown(KeyCode.Z)){
			gunHanddle.OffsetAdjust(new Vector2(0,-1));
		}
		if(Input.GetKeyDown(KeyCode.X)){
			gunHanddle.OffsetAdjust(new Vector2(0,1));
		}
		if(Input.GetKeyDown(KeyCode.C)){
			gunHanddle.OffsetAdjust(new Vector2(-1,0));
		}
		if(Input.GetKeyDown(KeyCode.V)){
			gunHanddle.OffsetAdjust(new Vector2(1,0));
		}

	}

}

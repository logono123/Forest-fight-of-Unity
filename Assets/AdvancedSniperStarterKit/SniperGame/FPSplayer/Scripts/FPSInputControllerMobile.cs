using UnityEngine;
using System.Collections;

public class FPSInputControllerMobile : MonoBehaviour {

	private GunHanddle gunHanddle;
	private FPSController FPSmotor;
	
	public TouchScreenVal touchMove;
	public TouchScreenVal touchAim;
	public TouchScreenVal touchShoot;
	public TouchScreenVal touchZoom;
	public TouchScreenVal touchJump;
	
	public Texture2D ImgButton;
	public float TouchSensMult = 0.05f;

	
	void Start(){
		Application.targetFrameRate = 60;
	}
	void Awake ()
	{
		FPSmotor = GetComponent<FPSController> ();		
		gunHanddle = GetComponent<GunHanddle> (); 
	}

	void Update ()
	{

		Vector2 aimdir = touchAim.OnDragDirection(true);
		FPSmotor.Aim(new Vector2(aimdir.x,-aimdir.y)*TouchSensMult);
		Vector2 touchdir = touchMove.OnTouchDirection (false);
		FPSmotor.Move (new Vector3 (touchdir.x, 0, touchdir.y));
		
		FPSmotor.Jump (Input.GetButton ("Jump"));
		
		if(touchShoot.OnTouchPress()){
			gunHanddle.Shoot();	
		}
		if(touchZoom.OnTouchRelease()){
			gunHanddle.ZoomToggle();
		}
	}
	
	
	void OnGUI(){
		
		touchMove.Draw();
		touchAim.Draw();
		touchShoot.Draw();
		touchZoom.Draw();
		
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]

public class FPSController : MonoBehaviour
{
	[HideInInspector]
	public CharacterMotor motor;
	private Vector2 mouseDirection;
	public Camera FPSCamera;
	public GameObject FPSmain;
	public Camera GunView;
	[HideInInspector]
	public float sensitivityXMult = 1;
	[HideInInspector]
	public float sensitivityYMult = 1;
	public float sensitivityX = 15;
	public float sensitivityY = 15;
	public float minimumX = -360;
	public float maximumX = 360;
	public float minimumY = -60;
	public float maximumY = 60;
	public float delayMouse = 3;
	public float noiseX = 0.1f;
	public float noiseY = 0.1f;
	public bool Noise;
	private float rotationX = 0;
	private float rotationY = 0;
	private float rotationXtemp = 0;
	private float rotationYtemp = 0;
	private Quaternion originalRotation;
	private float noisedeltaX;
	private float noisedeltaY;
	private float stunY;
	private float breathHolderValtarget = 1;
	private float breathHolderVal = 1;
	[HideInInspector]
	public Vector3 direction;
	[HideInInspector]
	public Vector3 rotationDif;
	[HideInInspector]
	public Vector3 positionDif;
	Vector3 lastRotation;
	Vector3 localCameraPositionTemp;
	Quaternion localCameraRotationTemp;
	Vector3 localCameraRotationOffset;
	
	void Start ()
	{
		if (!FPSCamera)
			FPSCamera = GetComponentInChildren<Camera> ();
		if (FPSCamera) {
			localCameraPositionTemp = FPSCamera.gameObject.transform.localPosition;
			localCameraRotationTemp = FPSCamera.gameObject.transform.localRotation;
			
		}
		

       
		
		
		motor = GetComponent<CharacterMotor> ();
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
		originalRotation = transform.localRotation;
	}
	
	public void CameraForceRotation (Vector3 axis)
	{
		localCameraRotationOffset = axis;
	}
	
	void FixedUpdate ()
	{
		float magnitude = motor.controller.velocity.magnitude * 0.5f;
		float swaySpeed = 1;
		float sizeX = 1.3f;
		float sizeY = 1.3f;
		if (magnitude > 3) {
			swaySpeed = 1.8f;
			sizeX = 2;
			sizeY = 2;
		} else {
			swaySpeed = 1;
			sizeX = 1.0f;
			sizeY = 1.0f;
			if (magnitude <= 1) {
				swaySpeed = 0;	
			}
		}
		localCameraRotationOffset = Vector3.Lerp (localCameraRotationOffset, Vector3.zero, Time.deltaTime * 3);
		float swayY = (Mathf.Cos (Time.fixedTime * 10 * swaySpeed) * 0.3f) * sizeY;
		float swayX = (Mathf.Sin (Time.fixedTime * 5 * swaySpeed) * 0.2f) * sizeX;
		FPSCamera.gameObject.transform.localPosition = Vector3.Lerp (FPSCamera.gameObject.transform.localPosition, localCameraPositionTemp + new Vector3 (swayX, swayY, 0), Time.deltaTime * 3);
		FPSCamera.gameObject.transform.localRotation = Quaternion.Lerp (FPSCamera.gameObject.transform.localRotation, Quaternion.Euler (localCameraRotationTemp.eulerAngles + localCameraRotationOffset), Time.deltaTime * 3);

	}

	public void HideGun (bool visible)
	{
		if (GunView) {
			GunView.GetComponent<Camera>().enabled = visible;	
		}
	}

	public void Boost (float mult)
	{
		motor.boostMults = mult;
	}

	public void Move (Vector3 directionVector)
	{
		direction = directionVector;
		if (directionVector != Vector3.zero) {
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			var directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
		
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min (1, directionLength);
		
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
		
			// Multiply the normalized direction vector by the modified length
			directionVector = directionVector * directionLength;
		}
		
		Quaternion rotation = transform.rotation;
		
		if (FPSmain) {
			rotation = FPSmain.transform.rotation;
		}
		Vector3 angle = rotation.eulerAngles; 
		angle.x = 0;
		angle.z = 0;
		rotation.eulerAngles = angle;
		// Apply the direction to the CharacterMotor
		motor.inputMoveDirection = rotation * directionVector;
		
		
	}

	public void Jump (bool jump)
	{
		motor.inputJump = jump;
	}
	
	public void Aim (Vector2 direction)
	{
		#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		MouseLock.MouseLocked = true;
		#endif
		mouseDirection = direction;	
		
	}
	
	void Update ()
	{
		
		motor.boostMults += (1 - motor.boostMults) * Time.deltaTime;
		stunY += (0 - stunY) / 20f;
		
		// gun sway
		if (Noise) {
			noisedeltaX += ((((Mathf.Cos (Time.time) * Random.Range (-10, 10) / 5f) * noiseX) - noisedeltaX) / 100) * Time.timeScale;
			noisedeltaY += ((((Mathf.Sin (Time.time) * Random.Range (-10, 10) / 5f) * noiseY) - noisedeltaY) / 100) * Time.timeScale;
		} else {
			noisedeltaX = 0;
			noisedeltaY = 0;
		}
		

		rotationXtemp += (mouseDirection.x * sensitivityX * sensitivityXMult) + (noisedeltaX * breathHolderVal);
		rotationYtemp += (mouseDirection.y * sensitivityY * sensitivityYMult) + (noisedeltaY * breathHolderVal);
		rotationX += ((rotationXtemp - rotationX) / delayMouse) * Time.timeScale;
		rotationY += ((rotationYtemp - rotationY) / delayMouse) * Time.timeScale;


		if (rotationX >= 360) {
			rotationX = 0;
			rotationXtemp = 0;
		}
		if (rotationX <= -360) {
			rotationX = 0;
			rotationXtemp = 0;
		}
		
		rotationX = ClampAngle (rotationX, minimumX, maximumX);
		rotationY = ClampAngle (rotationY, minimumY, maximumY);
		rotationYtemp = ClampAngle (rotationYtemp, minimumY, maximumY);
      
		Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis (rotationY + stunY, Vector3.left);
		
		if (FPSmain) {
			FPSmain.transform.localRotation = originalRotation * xQuaternion * yQuaternion;
		} else {
			transform.localRotation = originalRotation * xQuaternion * yQuaternion;
		}
		
		breathHolderVal += (breathHolderValtarget - breathHolderVal) / 10;	
		rotationDif = FPSCamera.transform.rotation.eulerAngles - lastRotation;
		positionDif = FPSCamera.transform.localPosition - positionDif;
		lastRotation = FPSCamera.transform.rotation.eulerAngles;
	}
	
	public void Holdbreath (float val)
	{
		breathHolderValtarget = val;
	}
	
	public void Stun (float val)
	{
		stunY = val;
	}

	static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360.0f)
			angle += 360.0f;

		if (angle > 360.0f)
			angle -= 360.0f;

		return Mathf.Clamp (angle, min, max);
	}
}

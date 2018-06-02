using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class AS_ActionCamera : MonoBehaviour
{
	
	public AS_ActionPreset[] ActionPresets;
	[HideInInspector]
	public float FOVTarget;
	[HideInInspector]
	public GameObject ObjectLookAt, ObjectFollowing, ObjectLookAtRoot;
	[HideInInspector]
	public Vector3 PositionHit;
	[HideInInspector]
	public Vector3 PositionLookAt;
	[HideInInspector]
	public float Raduis = 5;
	[HideInInspector]
	public float TimeDuration = 2;
	[HideInInspector]
	public float SlowTimeDuration = 2;
	public int PresetIndex = 0;
	public bool RandomIndex = true;
	[HideInInspector]
	public bool InAction;
	[HideInInspector]
	public bool Detected;
	[HideInInspector]
	public bool HitTarget;
	public float TimeChangeSpeed = 10;
	private static float initialFixedTimeStep = 1;
	private float timeTemp, slowtimeTemp;
	private bool[] cameraEnabledTemp;
	private bool[] audiolistenerEnabledTemp;
	private float timeScaleTarget = 1;
	[HideInInspector]
	public bool cameraTemp;
	[HideInInspector]
	public bool Follow = false;
	public float Length = 15;
	[HideInInspector]
	public float LengthMult = 0;
	private float lengthCast = 0;
	public float Damping = 10;
	public int IgnoreCameraLayer = 11;
	public float ColliderOffset = 1.0f;
	public Light SpotLight;
	public Camera MainCamera;
	private bool timeSetByASK;
	private float fovTemp;
	
	public AS_ActionPreset GetPresets ()
	{
		
		if (ActionPresets.Length <= 0) {
			return null;
			
		}
		AS_ActionPreset res = ActionPresets [Random.Range (0, ActionPresets.Length)];
		if (!RandomIndex) {
			if (PresetIndex >= 0 && PresetIndex < ActionPresets.Length) {
				res = ActionPresets [PresetIndex];
			}
		}
		
		return res;
	}
	
	void Start ()
	{
		for (int i=0; i<ActionPresets.Length; i++) {
			ActionPresets [i].Initialize ();	
		}
		if(GetComponent<Camera>())
			fovTemp = GetComponent<Camera>().fieldOfView;
		MainCamera = this.gameObject.GetComponent<Camera> ();
		initialFixedTimeStep = Time.fixedDeltaTime;
		cameraPosition = this.transform.position;
	}
	
	public void ActionBullet (float actionduration)
	{
		TimeDuration = actionduration;
		timeTemp = Time.realtimeSinceStartup;
		CollisionEnabled = true;
		setTarget ();
	}

	public void SetLookAtPosition (Vector3 pos)
	{
		lookAtPosition = pos;
		ObjectLookAt = null;
	}
	
	public void Slowmotion (float timescale, float slowduration)
	{
		TimeSet (timescale);
		SlowTimeDuration = slowduration;
		slowtimeTemp = Time.realtimeSinceStartup;
	}

	public void SlowmotionOnce (float timescale, float slowduration)
	{
		if (timeScaleTarget != timescale) {
			TimeSet (timescale);
			SlowTimeDuration = slowduration;
			slowtimeTemp = Time.realtimeSinceStartup;
		}
	}

	public void SlowmotionNow (float timescale, float slowduration)
	{
		TimeSet (timescale);
		Time.timeScale = timescale;
		SlowTimeDuration = slowduration;
		slowtimeTemp = Time.realtimeSinceStartup;
	}
	
	public void TimeSet (float scale)
	{
		timeSetByASK = true;
		timeScaleTarget = scale;
	}

	public void TimeSetNow (float scale)
	{
		timeSetByASK = true;
		timeScaleTarget = scale;
		Time.timeScale = scale;
	}

	private void setTarget ()
	{
		
		InAction = true;
		CameraActive ();
		this.GetComponent<Camera>().enabled = true;
		if (this.GetComponent<Camera>().gameObject.GetComponent<AudioListener> ())
			this.GetComponent<Camera>().gameObject.GetComponent<AudioListener> ().enabled = true;
	}

	private Camera[] cams;

	public void CameraActive ()
	{
		if (!cameraTemp) {
			cams = (Camera[])GameObject.FindObjectsOfType (typeof(Camera));
			audiolistenerEnabledTemp = new bool[cams.Length];
			cameraEnabledTemp = new bool[cams.Length];
			for (int i=0; i<cams.Length; i++) {
				cameraEnabledTemp [i] = cams [i].enabled;
				
				if (cams [i].gameObject.GetComponent<AudioListener> ()) {
					audiolistenerEnabledTemp [i] = cams [i].gameObject.GetComponent<AudioListener> ().enabled;
				}
				
				cams [i].enabled = false;
				if (cams [i].gameObject.GetComponent<AudioListener> ()) {
					cams [i].gameObject.GetComponent<AudioListener> ().enabled = false;
				}
			}
			//Debug.Log ("Action Cameras");
			cameraTemp = true;
		}
	}
	
	public void CameraRestore ()
	{
		if (cameraTemp) {
			cameraTemp = false;
			cams = (Camera[])GameObject.FindObjectsOfType (typeof(Camera));
			if (cameraEnabledTemp != null && cams != null) {
				if (cams.Length > 0 && cameraEnabledTemp.Length > 0 && cameraEnabledTemp.Length == cams.Length) {
					for (int i=0; i<cams.Length; i++) {
						cams [i].enabled = cameraEnabledTemp [i];	
						if (cams [i].gameObject.GetComponent<AudioListener> ()) {
							cams [i].gameObject.GetComponent<AudioListener> ().enabled = audiolistenerEnabledTemp [i];
						}
					}
				}
			}
			//Debug.Log ("Restore Cameras");
		}
	}
	
	public void ClearTarget ()
	{
		
		Follow = false;
		InAction = false;
		HitTarget = false;
		Detected = false;
		ObjectFollowing = null;
		ObjectLookAt = null;
		ObjectLookAtRoot = null;
		lengthCast = 0;
		LengthMult = 1;
		LookAtOffset = Vector3.zero;
		CameraOffset = Vector3.zero;
		ResetFOV();
		CameraRestore ();
	}
	
	public void ResetFOV(){
		FOVTarget = fovTemp;	
	}
	
	Vector3 Direction (Vector3 point1, Vector3 point2)
	{
		return (point1 - point2).normalized;
	}

	[HideInInspector]
	public Vector3 CameraOffset;
	[HideInInspector]
	public Vector3 cameraPosition, LookAtOffset, lookAtPosition;
	[HideInInspector]
	public bool CollisionEnabled = true;
	
	void cameraUpdate ()
	{
		if (onWallCollision) {
			this.transform.position = (lookAtPosition + ((-this.transform.forward) * lengthCast));
			cameraPosition = this.transform.position;
		} else {
			this.transform.position = Vector3.Lerp (this.transform.position, cameraPosition, Damping);	
		}
		if (GetComponent<Camera>()) {
			GetComponent<Camera>().fieldOfView = Mathf.Lerp (GetComponent<Camera>().fieldOfView, FOVTarget, 0.5f * Time.timeScale);	
		}
		gameObject.transform.LookAt (lookAtPosition + LookAtOffset);
		
		if (ObjectLookAt != null) {
			lookAtPosition = ObjectLookAt.transform.position;
		}
		if (Follow) {
			cameraPosition = (lookAtPosition + ((-this.transform.forward) * lengthCast)) + CameraOffset;	
		}

		lengthCast = Length * LengthMult;
	}

	[HideInInspector]
	public bool onWallCollision = false;

	void cameraCollision ()
	{
		
		onWallCollision = false;
		float distance = Vector3.Distance (lookAtPosition + LookAtOffset, this.transform.position);
				
		if (distance <= 0)
			distance = 0.01f;
		if (distance > Length)
			distance = Length;
		if (InAction) {
			RaycastHit hit;
			if (Physics.Raycast (lookAtPosition + LookAtOffset, -this.transform.forward, out hit, distance + ColliderOffset)) {
				if (hit.collider.gameObject != this.gameObject && hit.collider.gameObject.layer != IgnoreCameraLayer && !hit.collider.GetComponent<AS_BulletHiter> ()) {
					lengthCast = hit.distance;
					onWallCollision = true;
				}
			}

			if (Physics.Raycast (this.transform.position, -Vector3.up, out hit, 1)) {
				if (hit.collider.gameObject != this.gameObject && hit.collider.gameObject.layer != IgnoreCameraLayer) {
					cameraPosition.y = hit.point.y + 1;
				}
			}
		
		}
		
		if (!CollisionEnabled)
			onWallCollision = false; 
	}
	
	public void SetFOV (float target, bool blend)
	{
		FOVTarget = target;	
		if (!blend) {
			if (GetComponent<Camera>())
				GetComponent<Camera>().fieldOfView = target;
		}
	}
	
	public void SetPosition (Vector3 position, bool blend)
	{
		cameraPosition = position;
		
		if (!blend) {
			this.transform.position = cameraPosition;
		}
	}

	public void SetPositionDistance (Vector3 position, bool blend)
	{
		lengthCast = Length * LengthMult;
		cameraPosition = position + ((-this.transform.forward + CameraOffset) * lengthCast);
		
		if (!blend) {
			this.transform.position = cameraPosition;
		}
	}
	
	void FixedUpdate ()
	{
		cameraUpdate ();
		cameraCollision ();
		
	}

	public bool HaveAnotherTimeSytem;

	void TimeUpdate ()
	{
		if (timeSetByASK || !HaveAnotherTimeSytem) {
			Time.timeScale = Mathf.Lerp (Time.timeScale, timeScaleTarget, 0.5f);
			Time.fixedDeltaTime = (initialFixedTimeStep * Time.timeScale);	

			if (Time.realtimeSinceStartup >= (slowtimeTemp + SlowTimeDuration)) {
				TimeSet (1);	
				timeSetByASK = false;	
			}
		}	
	}
	
	void Update ()
	{
		TimeUpdate ();
		
		if (Time.realtimeSinceStartup >= timeTemp + TimeDuration) {
			ClearTarget ();
		}
		
		if (cameraTemp) {
			for (int i=0; i<cams.Length; i++) {
				if (cams [i] != this.GetComponent<Camera>()) {
					cams [i].enabled = false;	
				}
			}
		}
		if (MainCamera && SpotLight) {
			SpotLight.enabled = MainCamera.enabled;
		}
	}
	

	
}
